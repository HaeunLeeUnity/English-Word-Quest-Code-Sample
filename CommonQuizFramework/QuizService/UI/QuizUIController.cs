using System;
using System.Collections;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LHEPackage.Helper;
using UnityEngine.Serialization;

namespace CommonQuizFramework.QuizService
{
    public class QuizUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI wordText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField]
        private TextMeshProUGUI[] choicesButtonTexts = new TextMeshProUGUI[QuizServiceDefinition.MaxChoiceCount];

        [SerializeField] private TextMeshProUGUI stepIndicatorText;
        [SerializeField] private Button[] choicesButtons = new Button[QuizServiceDefinition.MaxChoiceCount];
        [SerializeField] private Button skipButton;
        [SerializeField] private Button memorizeCompleteButton;
        [SerializeField] private Slider memorizeWaitTimerSlider;
        [SerializeField] private TextMeshProUGUI memorizeWaitTimeText;
        [SerializeField] private Toggle wordToggle;
        [SerializeField] private Button speakTTSButton;
        [SerializeField] private Sprite normalChoiceButtonSprite;
        [SerializeField] private Sprite wrongChoiceButtonSprite;
        [SerializeField] private Sprite correctChoiceButtonSprite;
        [SerializeField] private Slider levelProgressBar;
        [SerializeField] private TextMeshProUGUI remainWordCountText;
        [SerializeField] private Slider stepProgressBar;
        [SerializeField] private TextMeshProUGUI stepProgressText;
        [SerializeField] private TextMeshProUGUI stepProgressTitleText;
        [SerializeField] private GameObject filterStateToast;
        [SerializeField] private GameObject memorizeStateToast;
        [SerializeField] private GameObject testStateToast;
        [SerializeField] private ParticleSystem correctAnswerParticle;
        [SerializeField] private ParticleSystem wrongAnswerParticle;
        [SerializeField] private TextMeshProUGUI partOfSpeechIndicator;
        [SerializeField] private Slider chanceIndicator;
        [SerializeField] private TextMeshProUGUI chanceIndicatorText;

        [SerializeField] private GameObject resultPopup;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI resultComboRecordIndicatorText;
        [SerializeField] private GameObject resultRemainQuizIndicator;
        [SerializeField] private TextMeshProUGUI resultRemainQuizIndicatorText;
        [SerializeField] private GameObject resultRemainChanceIndicator;
        [SerializeField] private TextMeshProUGUI resultRemainChanceIndicatorText;
        [SerializeField] private GameObject failedWordIndicator;
        [SerializeField] private TextMeshProUGUI failedWordIndicatorTextLeft;
        [SerializeField] private TextMeshProUGUI failedWordIndicatorTextRight;
        [SerializeField] private GameObject[] starsOff = new GameObject[3];
        [SerializeField] private GameObject[] starsOn = new GameObject[3];


        [SerializeField] private TextMeshProUGUI resultNextLevelButtonText;
        [SerializeField] private Button resultNextLevelButton;
        [SerializeField] private Button resultRebornButton;
        [SerializeField] private Button resultRetryButton;
        [SerializeField] private Button resultFailExitButton;
        [SerializeField] private Button resultClearExitButton;

        [SerializeField] private Tooltip stepTooltip;

        private Image[] _choicesButtonsImages = new Image[QuizServiceDefinition.MaxChoiceCount];
        private Image _skipButtonImage;
        private Animator[] _choicesButtonsAnimators;
        private Animator _skipButtonAnimator;
        private Animator _wordToggleAnimator;

        private SelectedLevel _nextLevel;

        private Coroutine _showChoicesButtonsCoroutine;
        private Coroutine _hideChoicesButtonsCoroutine;
        private Coroutine _setSliderValueGradualCoroutine;
        private Coroutine _showLearningStepToastCoroutine;

        private bool _choiceButtonInteractable = false;

        #region 답변 버튼 이벤트 등록

        private void Awake()
        {
            _choicesButtonsAnimators = new Animator[QuizServiceDefinition.MaxChoiceCount];
            for (var i = 0; i < QuizServiceDefinition.MaxChoiceCount; i++)
            {
                _choicesButtonsImages[i] = choicesButtons[i].GetComponent<Image>();
                _choicesButtonsAnimators[i] = choicesButtons[i].GetComponent<Animator>();
            }

            _skipButtonAnimator = skipButton.GetComponent<Animator>();
            _skipButtonImage = skipButton.GetComponent<Image>();
            _wordToggleAnimator = wordToggle.GetComponent<Animator>();
            chanceIndicator.maxValue = QuizServiceDefinition.MaxChanceCount;

            levelProgressBar.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, -(Screen.height - Screen.safeArea.yMax) - 20);

            ShowRemainChance(QuizServiceDefinition.MaxChanceCount);

            wordToggle.onValueChanged.AddListener(OnChangeWordToggle);

            void OnChangeWordToggle(bool value)
            {
                wordText.enabled = value;
                _wordToggleAnimator.SetBool("On", value);
            }
        }

        public void Reset()
        {
            HideGameResult();
            stepTooltip.gameObject.SetActive(false);

            stepIndicatorText.text = "";
            levelProgressBar.value = 0;
            remainWordCountText.text = "";

            wordText.text = "";
            descriptionText.text = "";

            // 실행중인 코루틴 중단 (순차적 버튼 표시, 슬라이더 값 변경, 토스트 메세지 출력)
            if (_showChoicesButtonsCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(_showChoicesButtonsCoroutine);
            }

            if (_hideChoicesButtonsCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(_hideChoicesButtonsCoroutine);
            }

            if (_setSliderValueGradualCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(_setSliderValueGradualCoroutine);
            }

            if (_showLearningStepToastCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(_showLearningStepToastCoroutine);
            }

            filterStateToast.SetActive(false);
            memorizeStateToast.SetActive(false);
            testStateToast.SetActive(false);

            foreach (var choicesButton in choicesButtons)
            {
                choicesButton.gameObject.SetActive(false);
            }

            skipButton.gameObject.SetActive(false);

            ShowStepProgress(LearningStep.Filter, 0);
            ShowRemainChance(QuizServiceDefinition.MaxChanceCount);
        }

        // 게임 결과 화면에서 다음 스테이지 버튼을 표시하기 위해 할당
        // 버튼 표시 유/무, 스테이지 ID 
        public void SetNextLevel(SelectedLevel nextLevel)
        {
            _nextLevel = nextLevel;
        }

        public void RegisterAnswerAction(Action<int> answerAction)
        {
            for (var i = 0; i < QuizServiceDefinition.MaxChoiceCount; i++)
            {
                var ii = i;

                choicesButtons[i].onClick.AddListener(OnClickAnswer);
                continue;

                void OnClickAnswer()
                {
                    if (!_choiceButtonInteractable) return;

                    SetInteractableChoiceButtons(false);
                    answerAction(ii);
                }
            }
        }

        public void RegisterSkipAction(Action skipAction)
        {
            skipButton.onClick.AddListener(OnClickSkip);
            memorizeCompleteButton.onClick.AddListener(OnClickSkip);
            return;

            void OnClickSkip()
            {
                if (!_choiceButtonInteractable) return;

                SetInteractableChoiceButtons(false);
                skipAction();
            }
        }

        private void SetInteractableChoiceButtons(bool interactable)
        {
            _choiceButtonInteractable = interactable;
        }

        public void RegisterRetryAction(Action onRetry)
        {
            resultRetryButton.onClick.AddListener(OnClickRetry);
            return;

            void OnClickRetry()
            {
                onRetry();
            }
        }

        public void RegisterNextLevelAction(Action onNext)
        {
            resultNextLevelButton.onClick.AddListener(OnClickNextLevel);
            return;

            void OnClickNextLevel()
            {
                onNext();
            }
        }

        public void RegisterResumeAction(Action resumeAction)
        {
            resultRebornButton.onClick.AddListener(OnClickResume);
            return;

            void OnClickResume()
            {
                resumeAction();
            }
        }

        public void RegisterExitAction(Action exitAction)
        {
            resultFailExitButton.onClick.AddListener(OnClickExit);
            resultClearExitButton.onClick.AddListener(OnClickExit);
            return;

            void OnClickExit()
            {
                exitAction();
            }
        }

        public void RegisterSpeakTTSAction(Action speakTTSAction)
        {
            speakTTSButton.onClick.AddListener(OnClickSpeakTTS);
            return;

            void OnClickSpeakTTS()
            {
                speakTTSAction();
            }
        }

        #endregion

        #region 진행 단계 표시

        public void ShowSelectedPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            partOfSpeechIndicator.text = CommonDefinition.PartOfSpeechKorean[(int)partOfSpeech];
        }

        public void ShowLearningStep(LearningStep learningStep, Action onComplete)
        {
            stepIndicatorText.text = QuizServiceDefinition.StepName[(int)learningStep];
            stepTooltip.gameObject.SetActive(true);
            stepTooltip.SetText(QuizServiceDefinition.StepDescription[(int)learningStep]);
            _setSliderValueGradualCoroutine = CoroutineManager.Instance.CoroutineStart(
                CommonUIFunction.Co_SetSliderValueGradual(levelProgressBar, (float)learningStep + 1f, 0.3f));
            _showLearningStepToastCoroutine =
                CoroutineManager.Instance.CoroutineStart(ShowLearningStepToast(learningStep, onComplete));
        }

        private IEnumerator ShowLearningStepToast(LearningStep learningStep, Action onComplete)
        {
            switch (learningStep)
            {
                case LearningStep.Filter:
                    filterStateToast.SetActive(true);
                    break;
                case LearningStep.Memorize:
                    memorizeStateToast.SetActive(true);
                    break;
                case LearningStep.Test:
                    testStateToast.SetActive(true);
                    break;
                default:
                    break;
            }

            yield return YieldInstructionCache.WaitForSeconds(2);
            ShowStepProgress(learningStep, 0);
            filterStateToast.SetActive(false);
            memorizeStateToast.SetActive(false);
            testStateToast.SetActive(false);
            onComplete?.Invoke();
        }

        public void ShowStepProgress(LearningStep learningStep, float stepProgress)
        {
            stepProgressTitleText.text =
                $"<font-weight=\"600\">{QuizServiceDefinition.StepName[(int)learningStep]} 진행도</font-weight>";
            stepProgressBar.value = stepProgress;
            stepProgressText.text = $"<font-weight=\"600\">{stepProgress:F0}%</font-weight>";
        }

        public void ShowRemainQuizCount(int remainQuizCount)
        {
            remainWordCountText.text = $"<font-weight=\"600\">{remainQuizCount}</font-weight>";
        }

        #endregion

        #region 문제 표시

        // 외우기 단계 표시
        // - 단어 + 뜻 표시
        public void ShowMemorizeView(Quiz quiz)
        {
            wordText.text = $"<font-weight=\"500\">{quiz.Word}</font-weight>";
            descriptionText.text = $"<font-weight=\"500\">{quiz.Mean}</font-weight>";
            wordText.gameObject.SetActive(true);
            wordToggle.gameObject.SetActive(true);
            speakTTSButton.gameObject.SetActive(true);
            wordToggle.isOn = true;
            memorizeCompleteButton.gameObject.SetActive(true);
            SetInteractableChoiceButtons(true);
        }

        public void HideMemorizeView()
        {
            wordText.text = "";
            wordText.gameObject.SetActive(false);
            wordToggle.gameObject.SetActive(false);
            speakTTSButton.gameObject.SetActive(false);
            memorizeCompleteButton.gameObject.SetActive(false);
        }

        public void HideQuizView()
        {
            _hideChoicesButtonsCoroutine = CoroutineManager.Instance.CoroutineStart(Co_HideChoicesButtons());
        }

        public void ShowTestResult(int? select, int answer)
        {
            if (select == answer)
            {
                correctAnswerParticle.transform.position = choicesButtons[(int)select].transform.position;

                // UIParticleSystem 버그로 인해 Stop()을 먼저 실행 후 Play()
                // https://discussions.unity.com/t/free-script-particle-systems-in-ui-screen-space-overlay/628576/84
                correctAnswerParticle.Stop();
                correctAnswerParticle.Play();
            }
            else
            {
                if (select == null)
                    wrongAnswerParticle.transform.position = skipButton.transform.position;
                else
                    wrongAnswerParticle.transform.position = choicesButtons[(int)select].transform.position;

                wrongAnswerParticle.Stop();
                wrongAnswerParticle.Play();
            }

            if (select != null)
                _choicesButtonsImages[(int)select].sprite = wrongChoiceButtonSprite;
            else
                _skipButtonImage.sprite = wrongChoiceButtonSprite;

            _choicesButtonsImages[answer].sprite = correctChoiceButtonSprite;
        }

        // [퀴즈]
        // 뜻 + 보기를 함께 표시
        public void ShowQuizAndExamples(Quiz quiz)
        {
            wordText.text = "";
            descriptionText.text = quiz.Mean;

            _showChoicesButtonsCoroutine =
                CoroutineManager.Instance.CoroutineStart(Co_ShowChoicesButtons(quiz.Choices));
        }


        private IEnumerator Co_ShowChoicesButtons(Choice[] choices)
        {
            foreach (var choicesButton in choicesButtons)
            {
                choicesButton.gameObject.SetActive(true);
            }

            skipButton.gameObject.SetActive(true);

            for (var i = 0; i < QuizServiceDefinition.MaxChoiceCount; i++)
            {
                _choicesButtonsImages[i].sprite = normalChoiceButtonSprite;
                _skipButtonImage.sprite = normalChoiceButtonSprite;
                choicesButtonTexts[i].text = choices[i].Word;
                _choicesButtonsAnimators[i].SetBool("Show", true);
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }

            _skipButtonAnimator.SetBool("Show", true);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            SetInteractableChoiceButtons(true);
        }

        private IEnumerator Co_HideChoicesButtons()
        {
            for (var i = 0; i < QuizServiceDefinition.MaxChoiceCount; i++)
            {
                _choicesButtonsAnimators[i].SetBool("Show", false);
            }

            _skipButtonAnimator.SetBool("Show", false);

            yield return YieldInstructionCache.WaitForSeconds(1);

            foreach (var choicesButton in choicesButtons) choicesButton.gameObject.SetActive(false);

            skipButton.gameObject.SetActive(false);

            wordText.text = "";
            descriptionText.text = "";
            wordText.gameObject.SetActive(false);
        }

        #endregion

        #region 남은 대기 시간 표시

        public void ShowMemorizeTime(float memorizeWaitTime)
        {
            memorizeWaitTimeText.gameObject.SetActive(true);
            memorizeWaitTimerSlider.value = memorizeWaitTime;
            memorizeWaitTimeText.text = memorizeWaitTime.ToString("F1");
        }

        public void HideMemorizeTime()
        {
            memorizeWaitTimeText.gameObject.SetActive(false);
        }

        #endregion

        #region 기회

        public void ShowRemainChance(int chance)
        {
            chanceIndicator.value = chance;
            chanceIndicatorText.text = $"<font-weight=\"600\">{chance}/{QuizServiceDefinition.MaxChanceCount}";
        }

        #endregion

        #region 결과창

        public void ShowGameResult(QuizResult quizResult)
        {
            resultPopup.SetActive(true);

            if (quizResult.IsClear)
            {
                resultTitleText.text = "<font-weight=\"500\">CLEAR";
                resultTitleText.color = new Color(0, 1, 0.65f);

                for (var i = 0; i < QuizServiceDefinition.ResultStarStandards.Length; i++)
                {
                    starsOff[i].SetActive(true);
                    if (QuizServiceDefinition.ResultStarStandards[i] <= quizResult.RemainChance)
                    {
                        starsOff[i].SetActive(false);
                        starsOn[i].SetActive(true);
                    }
                }

                if (_nextLevel != null)
                {
                    resultNextLevelButton.gameObject.SetActive(true);
                    resultNextLevelButtonText.text =
                        $"<font-weight=\\\"500\\\">다음 스테이지로 ( {_nextLevel.MajorID} - {_nextLevel.MinorID} )";
                }

                resultRemainChanceIndicator.SetActive(true);
                resultRemainChanceIndicatorText.text = $"<font-weight=\"500\">{quizResult.RemainChance.ToString()}";

                resultClearExitButton.gameObject.SetActive(true);
            }
            else
            {
                resultTitleText.text = "<font-weight=\"500\">FAIL";
                resultTitleText.color = new Color(1, 0, 0);

                failedWordIndicator.SetActive(true);

                var failedWordsLeftPage = "";
                for (var i = 0; i < 5; i++)
                {
                    if (string.IsNullOrWhiteSpace(quizResult.FailedWords[i].Item1))
                    {
                        if (i < 4)
                        {
                            failedWordsLeftPage += "\n";
                        }

                        continue;
                    }

                    failedWordsLeftPage +=
                        $"[{i}] {quizResult.FailedWords[i].Item1} : {quizResult.FailedWords[i].Item2}";

                    if (i < 4)
                    {
                        failedWordsLeftPage += "\n";
                    }
                }

                var failedWordsRightPage = "";
                for (var i = 5; i < 10; i++)
                {
                    if (string.IsNullOrWhiteSpace(quizResult.FailedWords[i].Item1))
                    {
                        if (i < 9)
                        {
                            failedWordsRightPage += "\n";
                        }

                        continue;
                    }

                    failedWordsRightPage +=
                        $"[{i}] {quizResult.FailedWords[i].Item1} : {quizResult.FailedWords[i].Item2}";

                    if (i < 9)
                    {
                        failedWordsRightPage += "\n";
                    }
                }

                failedWordIndicatorTextLeft.text = failedWordsLeftPage;
                failedWordIndicatorTextRight.text = failedWordsRightPage;

                resultRemainQuizIndicator.SetActive(true);
                resultRemainQuizIndicatorText.text = $"<font-weight=\"500\">{quizResult.RemainQuiz.ToString()}";

                resultRebornButton.gameObject.SetActive(true);
                resultRetryButton.gameObject.SetActive(true);
                resultFailExitButton.gameObject.SetActive(true);
            }

            resultComboRecordIndicatorText.text = $"<font-weight=\"500\">{quizResult.ComboRecord}";
        }


        public void HideGameResult()
        {
            resultPopup.SetActive(false);

            for (var i = 0; i < 3; i++)
            {
                starsOff[i].SetActive(false);
                starsOn[i].SetActive(false);
            }

            resultRemainChanceIndicator.SetActive(false);
            resultClearExitButton.gameObject.SetActive(false);
            failedWordIndicator.SetActive(false);

            resultRemainQuizIndicator.SetActive(false);
            resultRebornButton.gameObject.SetActive(false);
            resultRetryButton.gameObject.SetActive(false);
            resultFailExitButton.gameObject.SetActive(false);
            resultNextLevelButton.gameObject.SetActive(false);
        }

        #endregion

        private void OnDestroy()
        {
            try
            {
                if (_showChoicesButtonsCoroutine != null)
                {
                    CoroutineManager.Instance?.CoroutineStop(_showChoicesButtonsCoroutine);
                }

                if (_hideChoicesButtonsCoroutine != null)
                {
                    CoroutineManager.Instance?.CoroutineStop(_hideChoicesButtonsCoroutine);
                }

                if (_setSliderValueGradualCoroutine != null)
                {
                    CoroutineManager.Instance?.CoroutineStop(_setSliderValueGradualCoroutine);
                }

                if (_showLearningStepToastCoroutine != null)
                {
                    CoroutineManager.Instance?.CoroutineStop(_showLearningStepToastCoroutine);
                }
            }
            catch (Exception e)
            {
                LHELogger.Log("Quiz UI Controller is removed by stop the app.");
            }
        }
    }
}
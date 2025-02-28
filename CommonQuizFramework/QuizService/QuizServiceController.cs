using System;
using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.QuizService.LearningState;
using CommonQuizFramework.QuizService.TTS;
using LHEPackage.Helper;
using Newtonsoft.Json;
using UnityEngine;

namespace CommonQuizFramework.QuizService
{
    public class QuizServiceController : ISceneComponentAdapter, ILearningStateRequestHandler
    {
        private IQuizServiceRequestHandler _quizServiceRequestHandler;
        private QuizContainer _quizContainer;
        private QuizUIController _quizUIController;
        private ILearningState _currentState;
        private ChanceController _chanceController;
        private TTSController _ttsController;

        public List<Quiz> RemainQuizList
        {
            get => _quizContainer.RemainQuizList;
            set => _quizContainer.RemainQuizList = value;
        }

        public int RemainChance => _chanceController.RemainChance;
        public bool IsGameOver => _chanceController.RemainChance == 0;

        public float TTSVolume
        {
            set => _ttsController.Volume = value;
        }

        public QuizServiceController(IQuizServiceRequestHandler quizServiceRequestHandler)
        {
            _quizServiceRequestHandler = quizServiceRequestHandler;
            _chanceController = new ChanceController(this);
            _quizContainer = new QuizContainer();
            _ttsController = new TTSController();
        }

        public void RegisterUIController(QuizUIController quizUIController)
        {
            _quizUIController = quizUIController;
            _quizUIController.RegisterAnswerAction(Answer);
            _quizUIController.RegisterSkipAction(Skip);
            _quizUIController.RegisterResumeAction(ResumeQuiz);
            _quizUIController.RegisterExitAction(ExitQuiz);
            _quizUIController.RegisterSpeakTTSAction(SpeakTTSCurrentWord);
            _quizUIController.RegisterRetryAction(RequestReset);
            _quizUIController.RegisterNextLevelAction(RequestNextLevel);
        }

        public void DisposeSceneComponent()
        {
            _quizUIController = null;
        }

        public void SetQuizData(string json)
        {
            _quizContainer.SetQuizData(json);
        }

        public void QuizStart()
        {
            var selectedPartOfSpeech = _quizServiceRequestHandler.GetSelectedLevel().PartOfSpeech;
            _quizUIController.ShowSelectedPartOfSpeech(selectedPartOfSpeech);

            ChangeState(LearningStep.Filter);

            var nextLevel = _quizServiceRequestHandler.GetNextLevel();
            _quizUIController.SetNextLevel(nextLevel);
        }

        public void Reset()
        {
            _quizContainer.Reset();
            _chanceController.Reset();
            _quizUIController.Reset();
        }

        private void RequestReset()
        {
            _quizServiceRequestHandler.ResetQuiz();
        }

        private void RequestNextLevel()
        {
            _quizServiceRequestHandler.RequestNextLevel();
        }

        #region QUIZ CONTAINER

        public void AddFailedQuiz(Quiz quiz)
        {
            _quizContainer.AddFailedWords(quiz);
        }

        #endregion

        #region LEARNING STATE

        private void Answer(int answer)
        {
            _currentState.Answer(answer);
        }

        private void Skip()
        {
            _currentState.Skip();
        }

        public void ChangeState(LearningStep learningStep)
        {
            ILearningState newState = null;
            switch (learningStep)
            {
                case LearningStep.Filter:
                    newState = new FilterState(this);
                    break;

                case LearningStep.Memorize:
                    newState = new MemorizeState(this);
                    break;

                case LearningStep.Test:
                    newState = new TestState(this);
                    break;
                case LearningStep.End:
                    _quizServiceRequestHandler.OnChangeState(learningStep);
                    return;
                default:
                    LHELogger.LogWarning($"{learningStep}: 정의되지 않은 LearningStep 입니다.");
                    break;
            }

            _quizServiceRequestHandler.OnChangeState(learningStep, OnCharacterMoveComplete);

            void OnCharacterMoveComplete()
            {
                SoundManager.Instance.PlaySFX(SFXType.StateChange);
                VibrationProvider.VibratePeek();
                _quizUIController.ShowLearningStep(learningStep, OnComplete);
            }

            void OnComplete()
            {
                _currentState = newState;
                _currentState?.StartState();
            }
        }

        #endregion

        #region TTS

        public void SpeakTTSCurrentWord()
        {
            _ttsController.Speak(_currentState.CurrentWord);
        }

        #endregion

        #region UI

        public void SetRemainQuizCountView(int count)
        {
            _quizUIController.ShowRemainQuizCount(count);
        }

        public void ShowMemorizeView(Quiz quiz)
        {
            _quizUIController.ShowMemorizeView(quiz);
        }

        public void HideMemorizeView()
        {
            _quizUIController.HideMemorizeView();
        }

        public void ShowQuizView(Quiz quiz)
        {
            _quizUIController.ShowQuizAndExamples(quiz);
        }

        public void HideQuizView()
        {
            _quizUIController.HideQuizView();
        }

        public void ShowMemorizeTime(float time)
        {
            _quizUIController.ShowMemorizeTime(time);
        }

        public void HideMemorizeTime()
        {
            _quizUIController.HideMemorizeTime();
        }

        public void OnAnswer(int? select, int answer, Action onComplete)
        {
            _quizServiceRequestHandler.OnAnswer(select, answer, onComplete);

            if (select == answer)
            {
                SoundManager.Instance.PlaySFX(SFXType.CorrectAnswer);
                VibrationProvider.VibratePop();
            }
            else
            {
                SoundManager.Instance.PlaySFX(SFXType.WrongAnswer);
                VibrationProvider.VibrateNope();
            }

            _quizUIController.ShowTestResult(select, answer);
        }

        public void ShowStepProgress(LearningStep learningStep, float stepProgress)
        {
            _quizUIController.ShowStepProgress(learningStep, stepProgress);
        }

        public void ShowRemainChance(int chance)
        {
            _quizUIController.ShowRemainChance(chance);
        }

        #endregion

        #region CHANCE CONTROL

        public void DiscountChance()
        {
            _chanceController.DiscountChance();
        }

        #endregion

        #region GAME OVER

        public void QuizOver()
        {
            var result = new QuizResult
            {
                IsClear = _currentState.RemainQuizCount == 0,
                ComboRecord = _quizServiceRequestHandler.GetComboRecord(),
                RemainQuiz = _currentState.RemainQuizCount,
                RemainChance = _chanceController.RemainChance,
                FailedWords = _quizContainer.FailedQuizzes
            };

            HideQuizView();
            _quizUIController.ShowGameResult(result);
        }

        // 부활 (광고보고 부활) 등의 기능을 통해 이미 종료된 스테이지가 이어서 진행될 때 호출
        private void ResumeQuiz()
        {
            _quizServiceRequestHandler.OnResume();
            _quizUIController.HideGameResult();
            _quizContainer.ClearFailedWords();
            _chanceController.AddChance(QuizServiceDefinition.MaxChanceCount);
            _currentState.Resume();
        }

        private void ExitQuiz()
        {
            _quizUIController.HideGameResult();
            _quizServiceRequestHandler.RequestEnterLobby();
        }

        #endregion

        #region Test Code

        public void ForceClear()
        {
            _currentState.ForceClear();
        }

        #endregion
    }
}
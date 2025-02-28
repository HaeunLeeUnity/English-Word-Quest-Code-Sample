using System;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.UI;
using LHEPackage.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CommonQuizFramework.StageService
{
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _partOfSpeechList;
        [SerializeField] private GameObject _levelList;

        [SerializeField] private Button[] _partOfSpeechSelectButtons;
        [SerializeField] private TextMeshProUGUI[] _partOfSpeechSelectButtonTexts;

        [SerializeField] private Button[] _levelSelectButtons;
        private LevelButtonIndicator[] _levelSelectButtonIndicators;

        [SerializeField] private Button _returnToPartOfSpeechButton;
        [SerializeField] private TextMeshProUGUI _selectTypeIndicator;
        [SerializeField] private Tooltip _partOfSpeechTooltip;

        [SerializeField] private Image _title;

        public void Initialization(StageProgress[] partOfSpeechProgresses)
        {
            for (var i = 0; i < _partOfSpeechSelectButtonTexts.Length; i++)
            {
                var buttonText = GetPartOfSpeechProgressText(partOfSpeechProgresses[i]);
                _partOfSpeechSelectButtonTexts[i].text = buttonText;
            }

            _levelSelectButtonIndicators = new LevelButtonIndicator[_levelSelectButtons.Length];
            for (var i = 0; i < _levelSelectButtons.Length; i++)
                _levelSelectButtonIndicators[i] = _levelSelectButtons[i].GetComponentInChildren<LevelButtonIndicator>();

            _returnToPartOfSpeechButton.onClick.AddListener(ShowPartOfSpeechList);

            _title.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, -(Screen.height - Screen.safeArea.yMax));
        }

        public void RegisterPartOfSpeechButtons(Action<PartOfSpeech> onSelectPartOfSpeech)
        {
            for (var i = 0; i < _partOfSpeechSelectButtons.Length; i++)
            {
                var ii = i;
                _partOfSpeechSelectButtons[i].onClick.AddListener(OnClickPartOfSpeechButton);
                continue;

                void OnClickPartOfSpeechButton()
                {
                    LHELogger.Log((PartOfSpeech)ii);
                    onSelectPartOfSpeech?.Invoke((PartOfSpeech)ii);
                }
            }
        }

        public void RegisterLevelButtons(Action<int> onSelectLevel)
        {
            for (var i = 0; i < _levelSelectButtons.Length; i++)
            {
                var ii = i;
                _levelSelectButtons[i].onClick.AddListener(OnClickLevelButton);
                continue;

                void OnClickLevelButton()
                {
                    onSelectLevel?.Invoke(ii);
                }
            }
        }

        public void ShowPartOfSpeechList()
        {
            _partOfSpeechList.SetActive(true);
            _levelList.SetActive(false);
            _selectTypeIndicator.text = "<font-weight=600>품사를 선택하세요";
            _partOfSpeechTooltip.gameObject.SetActive(false);
            _partOfSpeechTooltip.HideTooltip();
            _returnToPartOfSpeechButton.gameObject.SetActive(false);
        }

        public void ShowLevelList(StageMetaV2 stageMetaV1, int[] clearRatings)
        {
            _partOfSpeechList.SetActive(false);
            _levelList.SetActive(true);
            _returnToPartOfSpeechButton.gameObject.SetActive(true);

            var indicatorText =
                GetPartOfSpeechProgressText(new StageProgress(stageMetaV1.PartOfSpeech, clearRatings));
            _selectTypeIndicator.text = indicatorText;

            _partOfSpeechTooltip.gameObject.SetActive(true);
            _partOfSpeechTooltip.SetText(StageServiceDefinition.PartOfSpeechDescription[(int)stageMetaV1.PartOfSpeech]);

            foreach (var levelSelectButton in _levelSelectButtons) levelSelectButton.gameObject.SetActive(false);

            for (var i = 0; i < stageMetaV1.Levels.Count; i++)
            {
                _levelSelectButtons[i].gameObject.SetActive(true);
                _levelSelectButtonIndicators[i]
                    .Initalize(stageMetaV1.Levels[i].MajorID, stageMetaV1.Levels[i].MinorID, clearRatings[i]);
            }

            return;
        }

        private string GetPartOfSpeechProgressText(StageProgress stageProgress)
        {
            return
                $"<font-weight=600>{CommonDefinition.PartOfSpeechKorean[(int)stageProgress.PartOfSpeech]} <size=\"64px\">({stageProgress.Progress * 100:F0}%)</width=\"22px\"></font-weight>";
        }
    }
}
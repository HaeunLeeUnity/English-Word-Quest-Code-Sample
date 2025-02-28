using System;
using System.Linq;
using CommonQuizFramework.CommonClass;
using JetBrains.Annotations;
using UnityEngine;

namespace CommonQuizFramework.StageService
{
    public class StageServiceProvider : ISceneComponentAdapter
    {
        private IStageServiceRequestHandler _stageServiceRequestHandler;
        private StageContainer _stageContainer;
        private LobbyUIController _lobbyUIController;

        public StageServiceProvider(IStageServiceRequestHandler stageServiceRequestHandler)
        {
            _stageServiceRequestHandler = stageServiceRequestHandler;
            _stageContainer = new StageContainer();
        }

        public void SetStageData(string json)
        {
            _stageContainer.SetStageData(json);
        }

        public void RegisterLobbyUIController(LobbyUIController lobbyUIController)
        {
            _lobbyUIController = lobbyUIController;
            _lobbyUIController.Initialization(GetStageProgress());
            _lobbyUIController.RegisterPartOfSpeechButtons(OnSelectPartOfSpeechButton);
            _lobbyUIController.RegisterLevelButtons(OnSelectLevel);

            void OnSelectPartOfSpeechButton(PartOfSpeech partOfSpeech)
            {
                _stageContainer.SetPartOfSpeech(partOfSpeech);
                var selectedStage = _stageContainer.GetSelectedStage();
                var clearRatings = _stageServiceRequestHandler.GetLevelClearRatings(partOfSpeech);
                _lobbyUIController.ShowLevelList(selectedStage, clearRatings);
            }

            void OnSelectLevel(int level)
            {
                _stageContainer.SetLevel(level);
                _stageServiceRequestHandler.RequestEnterStage();
            }
        }

        /// <summary>
        /// GC Collect 로 수집될 수 있도록 Lobby Scene 의 Component 참조를 해제함
        /// </summary>
        public void DisposeSceneComponent()
        {
            _lobbyUIController = null;
        }

        public void SetNextLevel()
        {
            _stageContainer.SetNextLevel();
        }

        public StageMetaV2[] GetStages()
        {
            return _stageContainer.GetStages().Values.ToArray();
        }

        private StageProgress[] GetStageProgress()
        {
            var stages = _stageContainer.GetStages();
            var partOfSpeechProgressArray = new StageProgress[stages.Count];

            for (var i = 0; i < stages.Count; i++)
            {
                var clearRating = _stageServiceRequestHandler.GetLevelClearRatings((PartOfSpeech)i);
                partOfSpeechProgressArray[i] = new StageProgress((PartOfSpeech)i, clearRating);
            }

            return partOfSpeechProgressArray;
        }

        public string GetQuizListJsonFileName()
        {
            return _stageContainer?.GetQuizListJsonFileName();
        }

        public AssetIDs GetLevelAssetIDs()
        {
            return _stageContainer?.GetSelectedLevelAssetIDs();
        }

        public SelectedLevel GetSelectedLevel()
        {
            return _stageContainer.GetSelectedLevel();
        }

        [CanBeNull]
        public SelectedLevel GetNextLevel()
        {
            return _stageContainer.GetNextLevel();
        }
    }
}
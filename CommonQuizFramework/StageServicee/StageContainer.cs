using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CommonQuizFramework.CommonClass;
using JetBrains.Annotations;
using LHEPackage.Helper;
using Newtonsoft.Json;

namespace CommonQuizFramework.StageService
{
    public class StageContainer
    {
        private Dictionary<PartOfSpeech, StageMetaV2> _stages;
        private StageMetaV2 _selectedStage;
        private SelectedLevel _selectedLevel = new();

        public void SetStageData(string json)
        {
            _stages = JsonConvert.DeserializeObject<Dictionary<PartOfSpeech, StageMetaV2>>(json);
        }

        public void SetPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            _selectedLevel.PartOfSpeech = partOfSpeech;
            if (_stages.TryGetValue(partOfSpeech, out var stage))
                _selectedStage = stage;
            else
                LHELogger.LogWarning($"선택된 스테이지가 존재하지 않습니다. {partOfSpeech}");
        }

        public void SetLevel(int level)
        {
            _selectedLevel.Level = level;
        }

        public SelectedLevel GetSelectedLevel()
        {
            return _selectedLevel;
        }


        [CanBeNull]
        public SelectedLevel GetNextLevel()
        {
            // 테스트를 위해 Quiz 씬에서 바로 시작한 경우 예외처리
            if (_selectedStage == null)
            {
                return null;
            }

            if (_selectedStage.Levels.Count <= _selectedLevel.Level + 1)
            {
                LHELogger.LogError($"{_selectedStage.PartOfSpeech} - {_selectedLevel.Level + 1} 은 없는 레벨 입니다.");
                return null;
            }

            if (_selectedStage.Levels[_selectedLevel.Level + 1] == null)
            {
                LHELogger.LogError($"{_selectedStage.PartOfSpeech} - {_selectedLevel.Level + 1} 은 NULL 입니다.");
                return null;
            }

            var nextLevel = new SelectedLevel
            {
                PartOfSpeech = _selectedLevel.PartOfSpeech,
                Level = _selectedLevel.Level + 1,
                MajorID = _selectedStage.Levels[_selectedLevel.Level + 1].MajorID,
                MinorID = _selectedStage.Levels[_selectedLevel.Level + 1].MinorID
            };

            return nextLevel;
        }

        public void SetNextLevel()
        {
            _selectedLevel = GetNextLevel();
        }

        public StageMetaV2 GetSelectedStage()
        {
            return _selectedStage;
        }

        public Dictionary<PartOfSpeech, StageMetaV2> GetStages()
        {
            if (_stages == null || _stages.Count == 0) LHELogger.LogError("Stage 가 아직 초기화 되지 않았습니다.");
            return _stages;
        }

        public string GetQuizListJsonFileName()
        {
            if (_selectedStage == null)
            {
                LHELogger.LogWarning($"선택된 스테이지가 존재하지 않습니다.\n 테스트 레벨을 반환합니다.");
                return "TestCase.json";
            }

            if (_selectedStage.Levels.Count >= _selectedLevel.Level)
                return _selectedStage.Levels[_selectedLevel.Level].Filename;


            LHELogger.LogWarning($"레벨이 없습니다. {_selectedLevel}");
            return null;
        }

        public AssetIDs GetSelectedLevelAssetIDs()
        {
            if (_selectedStage == null)
            {
                LHELogger.LogWarning($"선택된 스테이지가 존재하지 않습니다.\n 테스트 레벨을 반환합니다.");
                return new AssetIDs();
            }

            if (_selectedStage.Levels.Count >= _selectedLevel.Level)
                return _selectedStage.Levels[_selectedLevel.Level].AssetIDs;


            LHELogger.LogWarning($"레벨이 없습니다. {_selectedLevel}");
            return null;
        }
    }
}
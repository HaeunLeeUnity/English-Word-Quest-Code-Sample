using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.StageService
{
    public class StageClearData
    {
        public Dictionary<PartOfSpeech, int[]> ClearRatings;

        public void Initialization(StageMetaV2[] stages)
        {
            ClearRatings = new Dictionary<PartOfSpeech, int[]>();

            foreach (var stage in stages)
            {
                ClearRatings.Add(stage.PartOfSpeech, new int[stage.Levels.Count]);
            }
        }

        public int[] GetClearRatings(PartOfSpeech partOfSpeech)
        {
            if (ClearRatings == null)
            {
                LHELogger.LogWarning("Clear Data 가 초기화되지 않았습니다.");
                return null;
            }

            if (!ClearRatings.TryGetValue(partOfSpeech, out var clearRatings))
            {
                LHELogger.LogWarning($"Clear Data 의 {partOfSpeech} 에 해당하는 배열이 없습니다.");
                return null;
            }

            return clearRatings;
        }

        public int GetClearRating(PartOfSpeech partOfSpeech, int levelIndex)
        {
            if (ClearRatings == null)
            {
                LHELogger.LogWarning("Clear Data 가 초기화되지 않았습니다.");
                return 0;
            }

            if (!ClearRatings.TryGetValue(partOfSpeech, out var clearRatings))
            {
                LHELogger.LogWarning($"Clear Data 의 {partOfSpeech} 에 해당하는 배열이 없습니다.");
                return 0;
            }

            if (clearRatings.Length < levelIndex)
            {
                LHELogger.LogWarning($"Clear Data:{partOfSpeech} 배열 길이가 요청받은 레벨 인덱스보다 짧습니다. {levelIndex}");
                return 0;
            }

            return clearRatings[levelIndex];
        }

        public void SetClearRating(SelectedLevel selectedLevel, int clearRating)
        {
            if (ClearRatings == null)
            {
                LHELogger.LogWarning("Clear Data 가 초기화되지 않았습니다.");
                return;
            }

            if (!ClearRatings.TryGetValue(selectedLevel.PartOfSpeech, out var clearRatings))
            {
                LHELogger.LogWarning($"Clear Data 의 {selectedLevel.PartOfSpeech} 에 해당하는 배열이 없습니다.");
                return;
            }

            if (clearRatings.Length < selectedLevel.Level)
            {
                LHELogger.LogWarning(
                    $"Clear Data:{selectedLevel.PartOfSpeech} 의 배열 길이가 요청받은 레벨 인덱스보다 짧습니다. {selectedLevel.Level}");
            }

            ClearRatings[selectedLevel.PartOfSpeech][selectedLevel.Level] = clearRating;
        }
    }
}
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController
    {
        private void LevelChange()
        {
            var stageQuizListFileName = _stageServiceProvider?.GetQuizListJsonFileName();
            var stageAssetIds = _stageServiceProvider?.GetLevelAssetIDs();
            var loadQuizList = new LoadQuizList(_fileLoader, stageQuizListFileName, _quizServiceController);

            LHELogger.Assert(stageAssetIds != null, "Stage Asset ID is Null");

            var releaseGameAssets = new ReleaseGameAssets(_combatServiceController);
            var setBGMClip = new SetBGMClip(stageAssetIds.BGMID);
            var spawnQuizMap = new SpawnMap(stageAssetIds.MapID, _combatServiceController);
            var spawnCharacter = new SpawnCharacter(stageAssetIds.CharacterID, _combatServiceController);
            var monsterCharacter = new SpawnMonsters(stageAssetIds.MonsterIDs, _combatServiceController);
            var cleanUpMemory = new CleanupMemory();

            _sequenceQueue?.Enqueue(releaseGameAssets);
            _sequenceQueue?.Enqueue(loadQuizList);
            _sequenceQueue?.Enqueue(setBGMClip);
            _sequenceQueue?.Enqueue(spawnQuizMap);
            _sequenceQueue?.Enqueue(spawnCharacter);
            _sequenceQueue?.Enqueue(monsterCharacter);
            _sequenceQueue?.Enqueue(cleanUpMemory);
            _sequenceQueue?.StartSequence(_loadIndicator, OnComplete);

            void OnComplete()
            {
                ResetQuiz();
            }
        }
    }
}
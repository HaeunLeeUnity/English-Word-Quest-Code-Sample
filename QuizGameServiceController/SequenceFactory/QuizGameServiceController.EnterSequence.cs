using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController
    {
        private ISequenceQueue _sequenceQueue = new SequenceQueue();

        private void EnterStage()
        {
            var disposeSceneComponent = new DisposeSceneComponents(new ISceneComponentAdapter[]
            {
                _stageServiceProvider,
                _settingController
            });

            var stageQuizListFileName = _stageServiceProvider?.GetQuizListJsonFileName();
            var stageAssetIds = _stageServiceProvider?.GetLevelAssetIDs();

            var loadScene = new LoadScene(SceneType.Stage);
            var loadQuizList = new LoadQuizList(_fileLoader, stageQuizListFileName, _quizServiceController);

            LHELogger.Assert(stageAssetIds != null, "Stage Asset ID is Null");

            var setBGMClip = new SetBGMClip(stageAssetIds.BGMID);
            var spawnMap = new SpawnMap(stageAssetIds.MapID, _combatServiceController);
            var spawnCharacter = new SpawnCharacter(stageAssetIds.CharacterID, _combatServiceController);
            var spawnMonsters = new SpawnMonsters(stageAssetIds.MonsterIDs, _combatServiceController);
            var cleanUpMemory = new CleanupMemory();

            _sequenceQueue.Enqueue(disposeSceneComponent);
            _sequenceQueue?.Enqueue(loadScene);
            _sequenceQueue?.Enqueue(loadQuizList);
            _sequenceQueue?.Enqueue(setBGMClip);
            _sequenceQueue?.Enqueue(spawnMap);
            _sequenceQueue?.Enqueue(spawnCharacter);
            _sequenceQueue?.Enqueue(spawnMonsters);
            _sequenceQueue?.Enqueue(cleanUpMemory);
            _sequenceQueue?.StartSequence(_loadIndicator, OnComplete);

            void OnComplete()
            {
                RegisterQuizUIController();
                RegisterCombatComponent();
                RegisterSettingView();

                var pauseController = FindAnyObjectByType<PauseController>();
                pauseController.RegisterRetryAction(ResetQuiz);
                pauseController.RegisterExitAction(RequestEnterLobby);

                ResetQuiz();
            }
        }

        private void EnterLobby()
        {
            var disposeSceneComponent = new DisposeSceneComponents(new ISceneComponentAdapter[]
            {
                _combatServiceController,
                _quizServiceController,
                _settingController
            });
            var loadScene = new LoadScene(SceneType.Lobby);
            var setBGMClip = new SetBGMClip(QuizGameDefinition.LobbyBGMID);
            var spawnLobbyMap = new SpawnLobbyMap(Random.Range(0, 2));
            var spawnLobbyCharacter = new SpawnLobbyCharacter(Random.Range(0, 2));
            var cleanUpMemory = new CleanupMemory();

            _sequenceQueue.Enqueue(disposeSceneComponent);
            _sequenceQueue.Enqueue(loadScene);
            _sequenceQueue.Enqueue(setBGMClip);
            _sequenceQueue.Enqueue(spawnLobbyMap);
            _sequenceQueue.Enqueue(spawnLobbyCharacter);
            _sequenceQueue?.Enqueue(cleanUpMemory);
            _sequenceQueue.StartSequence(_loadIndicator, OnComplete);

            void OnComplete()
            {
                RegisterStageUIController();
                RegisterSettingView();
                SoundManager.Instance.PlayBGM();
            }
        }
    }
}
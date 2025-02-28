using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.StageService;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SpawnLobbyCharacter : SequenceElement
    {
        private int _characterID;

        public SpawnLobbyCharacter(int characterID)
        {
            _characterID = characterID;
        }

        public override void Execute()
        {
            AssetProvider.Instance.SpawnInstance(EAssetType.Character, _characterID,
                StageServiceDefinition.RunnerPosition, OnCompleteSpawn);

            void OnCompleteSpawn(GameObject gameObject)
            {
                var runnerComponent = gameObject.AddComponent<LobbyRunnerCharacter>();
                runnerComponent.Initialization();
                runnerComponent.RunLoop(-StageServiceDefinition.MapSpeed);
                OnComplete();
            }
        }
    }
}
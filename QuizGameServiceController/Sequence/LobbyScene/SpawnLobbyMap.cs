using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.StageService;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SpawnLobbyMap : SequenceElement
    {
        private int _mapID;

        public SpawnLobbyMap(int mapID)
        {
            _mapID = mapID;
        }

        public override void Execute()
        {
            AssetProvider.Instance.SpawnInstance(EAssetType.Map, _mapID, StageServiceDefinition.MapPosition,
                OnCompleteSpawn);

            void OnCompleteSpawn(GameObject gameObject)
            {
                var mapComponent = gameObject.GetComponent<InfiniteScrollMap>();
                mapComponent.Initialization();
                mapComponent.MoveLoop(StageServiceDefinition.MapSpeed);
                OnComplete();
            }
        }
    }
}
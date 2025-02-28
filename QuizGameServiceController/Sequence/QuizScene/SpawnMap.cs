using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SpawnMap : SequenceElement
    {
        private int _mapID;
        private CombatServiceController _combatServiceController;

        public SpawnMap(int mapID, CombatServiceController combatServiceController)
        {
            _mapID = mapID;
            _combatServiceController = combatServiceController;
        }

        public override void Execute()
        {
            AssetProvider.Instance.SpawnInstance(EAssetType.Map, _mapID, CombatServiceDefinition.MapPosition,
                OnCompleteSpawn);

            void OnCompleteSpawn(GameObject gameObject)
            {
                var mapComponent = gameObject.GetComponent<InfiniteScrollMap>();
                _combatServiceController.SetMap(mapComponent);
                OnComplete();
            }
        }
    }
}
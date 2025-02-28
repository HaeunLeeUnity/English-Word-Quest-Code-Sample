using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SpawnMonsters : SequenceElement
    {
        private int[] _monstersID;
        private CombatServiceController _combatServiceController;

        public SpawnMonsters(int[] monstersID, CombatServiceController combatServiceController)
        {
            _monstersID = monstersID;
            _combatServiceController = combatServiceController;
        }

        public override void Execute()
        {
            var spawnedMonsters = new IGameUnit[_monstersID.Length];
            var spawnedMonsterCount = 0;

            AssetProvider.Instance.SpawnInstance(EAssetType.Monster, _monstersID[0],
                CombatServiceDefinition.MonsterRestPosition, OnCompleteSpawn);

            void OnCompleteSpawn(GameObject gameObject)
            {
                spawnedMonsters[spawnedMonsterCount] = gameObject.GetComponent<IGameUnit>();
                spawnedMonsterCount++;

                if (_monstersID.Length <= spawnedMonsterCount)
                {
                    OnCompleteAllSpawn();
                    return;
                }

                AssetProvider.Instance.SpawnInstance(EAssetType.Monster, _monstersID[spawnedMonsterCount],
                    CombatServiceDefinition.MonsterRestPosition, OnCompleteSpawn);
            }

            void OnCompleteAllSpawn()
            {
                _combatServiceController.SetMonsters(spawnedMonsters);
                OnComplete();
            }
        }
    }
}
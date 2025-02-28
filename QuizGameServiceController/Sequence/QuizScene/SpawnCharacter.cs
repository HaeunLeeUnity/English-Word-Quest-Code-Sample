using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SpawnCharacter : SequenceElement
    {
        private int _characterID;
        private CombatServiceController _combatServiceController;

        public SpawnCharacter(int characterID, CombatServiceController combatServiceController)
        {
            _characterID = characterID;
            _combatServiceController = combatServiceController;
        }

        public override void Execute()
        {
            AssetProvider.Instance.SpawnInstance(EAssetType.Character, _characterID,
                CombatServiceDefinition.CharacterRestPosition,
                OnCompleteSpawn);

            void OnCompleteSpawn(GameObject gameObject)
            {
                var characterComponent = gameObject.GetComponent<PlayerCharacterController>();
                _combatServiceController.SetPlayerCharacter(characterComponent);
                OnComplete();
            }
        }
    }
}
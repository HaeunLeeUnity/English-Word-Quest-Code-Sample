using System;
using CommonQuizFramework.CommonClass;
using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    public class PlayerCharacterController : GameUnitController
    {
        public override void MoveWithFloor()
        {
            var stageSpeed = CombatServiceDefinition.MapMoveDistance / CombatServiceDefinition.MapMoveDuration;
            animator.SetFloat(MoveSpeedParameterID, stageSpeed);
        }

        public override void OnStopFloor()
        {
            animator.SetFloat(MoveSpeedParameterID, 0);
        }

        protected override Vector2 GetRestPosition()
        {
            return CombatServiceDefinition.CharacterRestPosition;
        }

        protected override Vector2 GetBattlePosition()
        {
            return CombatServiceDefinition.CharacterBattlePosition;
        }

        protected override float GetMoveDuration()
        {
            return CombatServiceDefinition.CharacterMoveDuration;
        }

        protected override Vector2 GetTargetPosition()
        {
            return CombatServiceDefinition.MonsterBattlePosition;
        }
    }
}
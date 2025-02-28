using CommonQuizFramework.CommonClass;
using System.Collections;
using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    public class MonsterController : GameUnitController
    {
        public override void MoveWithFloor()
        {
            animator.SetTrigger(ResetParameterID);
            transform.position = GetRestPosition();

            var targetPosition = new Vector2(transform.position.x - CombatServiceDefinition.MapMoveDistance,
                transform.position.y);

            SetSpriteDirection(targetPosition);

            MoveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_MoveTarget(targetPosition,
                CombatServiceDefinition.MapMoveDuration));
        }

        private IEnumerator Co_MoveTarget(Vector2 targetPosition, float moveDuration)
        {
            Vector2 startPosition = transform.position;
            var moveTime = 0f;
            var moveSpeed = 1 / moveDuration;

            while (1 > moveTime)
            {
                moveTime += Time.deltaTime * moveSpeed;
                transform.position = Vector2.Lerp(startPosition, targetPosition, moveTime);
                yield return null;
            }
        }

        public override void OnStopFloor()
        {
        }

        protected override Vector2 GetRestPosition()
        {
            return CombatServiceDefinition.MonsterRestPosition;
        }

        protected override Vector2 GetBattlePosition()
        {
            return CombatServiceDefinition.MonsterBattlePosition;
        }

        protected override float GetMoveDuration()
        {
            return CombatServiceDefinition.MonsterMoveDuration;
        }

        protected override Vector2 GetTargetPosition()
        {
            return CombatServiceDefinition.CharacterBattlePosition;
        }
    }
}
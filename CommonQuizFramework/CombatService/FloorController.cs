using System;
using System.Collections;
using CommonQuizFramework.CommonClass;
using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    [Obsolete("InfiniteScrollMap 을 사용할 것.")]
    public class FloorController : MonoBehaviour
    {
        private Action _reserveBehaviorCallback = null;
        private Coroutine _progressBehaviour = null;

        public void ForceEndBehaviour()
        {
            if (_progressBehaviour == null) return;

            CoroutineManager.Instance.CoroutineStop(_progressBehaviour);
            _reserveBehaviorCallback?.Invoke();
        }

        public void Move(Action onComplete)
        {
            _reserveBehaviorCallback = onComplete;
            var targetPosition = new Vector2(transform.position.x - CombatServiceDefinition.MapMoveDistance,
                transform.position.y);
            CoroutineManager.Instance.CoroutineStart(Co_MoveTarget(targetPosition,
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

            transform.position = startPosition;
            _reserveBehaviorCallback?.Invoke();
        }
    }
}
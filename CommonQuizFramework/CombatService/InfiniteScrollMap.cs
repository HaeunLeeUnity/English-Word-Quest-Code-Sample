using System;
using System.Collections;
using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;


namespace CommonQuizFramework.CombatService
{
    // 2D 사이드뷰 무한 스크롤 맵 기능 구현
    // 레이어별 이동속도 배율을 적용하여 공간감을 구현한다.

    public class InfiniteScrollMap : MonoBehaviour, IGameMap
    {
        [Header("맵 레이어")] [SerializeField] private SpriteRenderer[] mapLayers;

        [Space] [Header("각 레이어의 이동 속도 배율")] [SerializeField]
        private float[] layerSpeed;

        // Deque (Linked List) 를 통해 위치 체크 횟수를 최소화 한다.
        private LinkedList<SpriteRenderer>[] _layerLinkedLists;
        private Transform[] _layerParent;
        private Border[] _borders;
        private float[] _widths;

        private bool _isInit = false;

        [Space] [Header("함수 호출 없이 이동 반복")] [SerializeField]
        private bool scrollOnAwake = false;

        [SerializeField] private float scrollSpeed = 0f;
        private Coroutine _moveCoroutine;

        // 맵이 반복될 위치 한계선
        // 왼쪽으로 이동 시 MinX 를 넘었는지 체크한다.
        // 오른쪽으로 이동 시 MaxX 를 넘었는지 체크한다.
        private class Border
        {
            public Border(Vector2 origin, float width)
            {
                MinX = origin.x - (width + width / 2);
                MaxX = origin.x + (width + width / 2);
            }

            public float MinX;
            public float MaxX;
        }

        private void Start()
        {
            Initialization();
        }

        public void Initialization()
        {
            if (_isInit) return;

            _layerLinkedLists = new LinkedList<SpriteRenderer>[mapLayers.Length];
            _borders = new Border[mapLayers.Length];
            _layerParent = new Transform[mapLayers.Length];
            _widths = new float[mapLayers.Length];

            // 맵을 양쪽으로 무한히 반복할 수 있도록 2개의 복사본을 생성하여 Deque 차례로 넣는다.
            for (var i = 0; i < mapLayers.Length; i++)
            {
                _layerParent[i] = new GameObject(mapLayers[i].name).transform;
                _layerParent[i].SetParent(transform);
                _layerLinkedLists[i] = new LinkedList<SpriteRenderer>();

                var originPosition = mapLayers[i].transform.position;
                var width = GetWidth(mapLayers[i]);

                _widths[i] = width;
                mapLayers[i].transform.SetParent(_layerParent[i]);

                _borders[i] = new Border(originPosition, width);

                var duplicatedLayerLeft = Instantiate(mapLayers[i],
                    GetLeftPosition(originPosition, width),
                    Quaternion.identity, _layerParent[i]);

                _layerLinkedLists[i].AddLast(duplicatedLayerLeft);

                _layerLinkedLists[i].AddLast(mapLayers[i]);

                var duplicatedLayerRight = Instantiate(mapLayers[i],
                    GetRightPosition(originPosition, width),
                    Quaternion.identity, _layerParent[i]);

                _layerLinkedLists[i].AddLast(duplicatedLayerRight);
            }

            _isInit = true;
            if (scrollOnAwake) MoveLoop(scrollSpeed);
        }

        public void MoveLoop(float speed)
        {
            _moveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_MoveLoop(speed));
        }

        public void Move(float distance, float duration, Action onComplete)
        {
            if (distance < 0)
            {
                _moveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_MoveRight(distance, duration, onComplete));
            }
            else
            {
                _moveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_MoveLeft(distance, duration, onComplete));
            }
        }

        public void Stop()
        {
            if (_moveCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(_moveCoroutine);
            }
        }

        public void Release()
        {
            Stop();
            Destroy(gameObject);
        }

        private IEnumerator Co_MoveLeft(float distance, float duration, Action onComplete)
        {
            var moveTime = 0f;
            var moveSpeed = distance / duration;
            var targetPositions = new float[_layerParent.Length];

            for (var i = 0; i < targetPositions.Length; i++)
            {
                targetPositions[i] = _layerParent[i].position.x + distance * layerSpeed[i];
            }

            while (moveTime < duration)
            {
                for (var ii = 0; ii < mapLayers.Length; ii++)
                {
                    // 1. 각 레이어의 부모 오브젝트를 옮긴다.
                    _layerParent[ii].Translate(moveSpeed * layerSpeed[ii] * Time.deltaTime, 0, 0);

                    // 2. Deque 의 마지막 오브젝트의 위치가 한계선을 넘었는지 체크한다.
                    var findTarget = _layerLinkedLists[ii].Last.Value;
                    if (!(_borders[ii].MaxX < findTarget.transform.position.x)) continue;

                    // 3. 시작 오브젝트의와 이어지는 왼쪽 위치로 끝 오브젝트의 위치를 이동한다.
                    var firstTarget = _layerLinkedLists[ii].First.Value;
                    findTarget.transform.position = GetLeftPosition(firstTarget.transform.position, _widths[ii]);

                    // 4. 끝 오브젝트를 Deque 의 앞에 삽입한다.
                    _layerLinkedLists[ii].RemoveLast();
                    _layerLinkedLists[ii].AddFirst(findTarget);
                }

                moveTime += Time.deltaTime;
                yield return null;
            }

            // 5. 최초 목표한 위치로 각 Layer 의 위치를 조정한다.
            for (var iii = 0; iii < _layerParent.Length; iii++)
            {
                _layerParent[iii].transform.position =
                    new Vector2(targetPositions[iii], _layerParent[iii].transform.position.y);
            }

            onComplete?.Invoke();
        }

        private IEnumerator Co_MoveRight(float distance, float duration, Action onComplete)
        {
            var moveTime = 0f;
            var moveSpeed = distance / duration;
            var targetPositions = new float[_layerParent.Length];

            for (var i = 0; i < targetPositions.Length; i++)
            {
                targetPositions[i] = _layerParent[i].position.x + distance * layerSpeed[i];
            }

            while (moveTime < duration)
            {
                for (var ii = 0; ii < mapLayers.Length; ii++)
                {
                    _layerParent[ii].Translate(moveSpeed * layerSpeed[ii] * Time.deltaTime, 0, 0);

                    var findTarget = _layerLinkedLists[ii].First.Value;
                    if (!(findTarget.transform.position.x < _borders[ii].MinX)) continue;

                    var lastTarget = _layerLinkedLists[ii].Last.Value;
                    findTarget.transform.position = GetRightPosition(lastTarget.transform.position, _widths[ii]);

                    _layerLinkedLists[ii].RemoveFirst();
                    _layerLinkedLists[ii].AddLast(findTarget);
                }

                moveTime += Time.deltaTime;
                yield return null;
            }

            for (var iii = 0; iii < _layerParent.Length; iii++)
            {
                _layerParent[iii].transform.position =
                    new Vector2(targetPositions[iii], _layerParent[iii].transform.position.y);
            }

            onComplete?.Invoke();
        }

        private IEnumerator Co_MoveLoop(float speed)
        {
            while (true)
            {
                for (var ii = 0; ii < mapLayers.Length; ii++)
                {
                    _layerParent[ii].Translate(speed * layerSpeed[ii] * Time.deltaTime, 0, 0);

                    var findTarget = _layerLinkedLists[ii].First.Value;
                    if (!(findTarget.transform.position.x < _borders[ii].MinX)) continue;

                    var lastTarget = _layerLinkedLists[ii].Last.Value;
                    findTarget.transform.position = GetRightPosition(lastTarget.transform.position, _widths[ii]);

                    _layerLinkedLists[ii].RemoveFirst();
                    _layerLinkedLists[ii].AddLast(findTarget);
                }

                yield return null;
            }
        }

        private void OnDestroy()
        {
            try
            {
                Stop();
            }
            catch (Exception e)
            {
                LHELogger.Log($"Infinite scroll map is removed by stop the app {e.Message}");
            }
        }

        #region Calculate

        private static float GetWidth(SpriteRenderer target)
        {
            return target.size.x * target.transform.localScale.x;
        }

        private static Vector2 GetLeftPosition(Vector2 origin, float width)
        {
            return new Vector2(origin.x - width, origin.y);
        }

        private static Vector2 GetRightPosition(Vector2 origin, float width)
        {
            return new Vector2(origin.x + width, origin.y);
        }

        #endregion
    }
}
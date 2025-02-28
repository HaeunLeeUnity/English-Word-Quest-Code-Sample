using System;
using System.Collections;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CommonQuizFramework.StageService
{
    // 로비 씬에서 달리는 캐릭터 구현
    public class LobbyRunnerCharacter : MonoBehaviour
    {
        private static readonly int MoveSpeedParameterID = Animator.StringToHash("MoveSpeed");

        private bool _isInit = false;
        private Animator _animator;

        private Coroutine _runCoroutine;
        private Coroutine _moveCoroutine;

        public void Initialization()
        {
            if (_isInit) return;

            _animator = GetComponentInChildren<Animator>();
            LHELogger.Assert(_animator != null);

            _isInit = true;
        }

        public void RunLoop(float speed)
        {
            LHELogger.Assert(_isInit, "Runner didn't initialize");
            _runCoroutine = CoroutineManager.Instance.CoroutineStart(Co_RunLoop(speed));
        }

        private IEnumerator Co_RunLoop(float baseSpeed)
        {
            while (true)
            {
                var currentSpeed = GetRandomSpeed();
                _animator.SetFloat(MoveSpeedParameterID, currentSpeed);

                if (_moveCoroutine != null) CoroutineManager.Instance.CoroutineStop(_moveCoroutine);
                _moveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_Move(currentSpeed - baseSpeed));

                yield return YieldInstructionCache.WaitForSeconds(0.8f);
            }

            float GetRandomSpeed()
            {
                // 화면 밖으로 벗어나지 않도록 속도 설정
                if (StageServiceDefinition.RunnerAccelerationThreshold < transform.position.x)
                {
                    return Random.Range(baseSpeed * 0.7f, baseSpeed);
                }

                if (transform.position.x < StageServiceDefinition.RunnerDecelerationThreshold)
                {
                    return Random.Range(baseSpeed, baseSpeed * 1.3f);
                }

                return Random.Range(baseSpeed * 0.7f, baseSpeed * 1.3f);
            }
        }

        private IEnumerator Co_Move(float speed)
        {
            while (true)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnDestroy()
        {
            try
            {
                if (_runCoroutine != null) CoroutineManager.Instance.CoroutineStop(_runCoroutine);
                if (_moveCoroutine != null) CoroutineManager.Instance.CoroutineStop(_moveCoroutine);
            }
            catch (Exception e)
            {
                LHELogger.Log($"Lobby Runner is removed by stop the app {e.Message}");
            }
        }
    }
}
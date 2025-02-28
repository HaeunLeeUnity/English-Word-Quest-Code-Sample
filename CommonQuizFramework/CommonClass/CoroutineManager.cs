using System;
using System.Collections;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.CommonClass
{
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager Instance;

        public void Initialization()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Coroutine CoroutineStart(IEnumerator iEnumerator)
        {
            return StartCoroutine(iEnumerator);
        }

        public void CoroutineStop(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        #region ReserveCall

        /// <summary>
        /// 딜레이 후 인자로 전달된 함수를 실행한다
        /// - Invoke 를 대체하여 사용한다.
        /// </summary>
        public void ReserveCall(float delay, Action onAfterDelay)
        {
            StartCoroutine(Co_ReserveCall(delay, onAfterDelay));
        }

        private IEnumerator Co_ReserveCall(float delay, Action onAfterDelay)
        {
            yield return YieldInstructionCache.WaitForSeconds(delay);
            onAfterDelay?.Invoke();
        }

        #endregion
    }
}
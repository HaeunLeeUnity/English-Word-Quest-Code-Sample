using System.Collections;
using CommonQuizFramework.CommonClass;
using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    public class EffectController : MonoBehaviour
    {
        private Coroutine _destroyTimer;
        private EffectPool _effectPool;
        private int _id;

        public void OnCreate(int id, EffectPool effectPool)
        {
            _id = id;
            _effectPool = effectPool;
        }

        public void Initialization()
        {
            _destroyTimer = CoroutineManager.Instance.CoroutineStart(DestroyTimer());
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(2);
            _effectPool.OnReturnToPool(_id, gameObject);
        }
        
        private void OnDestroy()
        {
            if (_destroyTimer != null)
            {
                CoroutineManager.Instance.CoroutineStop(_destroyTimer);
            }
        }
    }
}
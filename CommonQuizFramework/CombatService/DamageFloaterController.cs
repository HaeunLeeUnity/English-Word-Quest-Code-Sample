using System.Collections;
using CommonQuizFramework.CommonClass;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CombatService
{
    public class DamageFloaterController : MonoBehaviour
    {
        private Coroutine _destroyTimer;
        private DamageFloaterPool _damageFloaterPool;

        [SerializeField] private Text text;
        [SerializeField] private float destroyTime = 0.25f;
        
        public void OnCreate(DamageFloaterPool objectPool)
        {
            _damageFloaterPool = objectPool;
        }
        
        public void Initialization(float damage, int combo)
        {
            text.text = $"Combo{combo}\n{damage:F1}";
            _destroyTimer = CoroutineManager.Instance.CoroutineStart(DestroyTimer());
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(destroyTime);
            _damageFloaterPool.ReturnToPool(gameObject);
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
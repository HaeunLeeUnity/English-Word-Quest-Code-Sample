using System;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    public class EffectPool : ObjectPool
    {
        public void SpawnToPool(int id, Vector2 position)
        {
            base.SpawnToPool(id, position, OnCompleteSpawn);

            void OnCompleteSpawn(GameObject spawnedEffect)
            {
                var effectController = spawnedEffect.GetComponent<EffectController>() ??
                                       spawnedEffect.AddComponent<EffectController>();
                effectController.Initialization();
            }
        }

        public void OnReturnToPool(int id, GameObject effectGameObject)
        {
            OnReturnedToPool(id, effectGameObject);
        }

        protected override void CreateGameObject(int id, Vector3 position, Action<GameObject> onComplete)
        {
            AssetProvider.Instance.SpawnInstance(EAssetType.Effect, id, position, OnComplete);

            void OnComplete(GameObject instance)
            {
                var effectController = instance.GetComponent<EffectController>() ??
                                       instance.AddComponent<EffectController>();
                effectController.OnCreate(id, this);
                onComplete?.Invoke(instance);
            }
        }
    }
}
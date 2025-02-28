using System;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CommonQuizFramework.CombatService
{
    public class DamageFloaterPool : ObjectPool
    {
        [SerializeField] private GameObject damageFloaterPrefab;
        [SerializeField] private Transform worldCanvas;

        public void SpawnToPool(float damage, Vector2 position, int combo)
        {
            base.SpawnToPool(0, position, OnCompleteSpawnObject);

            void OnCompleteSpawnObject(GameObject spawnedDamageFloaterText)
            {
                spawnedDamageFloaterText.SetActive(true);
                spawnedDamageFloaterText.transform.position = GetFloaterPosition(position);

                var damageFloaterController = spawnedDamageFloaterText.GetComponent<DamageFloaterController>() ??
                                              spawnedDamageFloaterText.AddComponent<DamageFloaterController>();
                damageFloaterController.Initialization(damage, combo);

                Vector2 GetFloaterPosition(Vector2 targetPosition)
                {
                    return new Vector2(
                        targetPosition.x + Random.Range(-CombatServiceDefinition.FloaterRandomRangeX,
                            CombatServiceDefinition.FloaterRandomRangeX),
                        targetPosition.y + Random.Range(CombatServiceDefinition.FloaterRandomRangeBottom,
                            CombatServiceDefinition.FloaterRandomRangeTop));
                }
            }
        }

        public void ReturnToPool(GameObject returnedGameObject)
        {
            base.OnReturnedToPool(0, returnedGameObject);
        }

        protected override void CreateGameObject(int id, Vector3 position, Action<GameObject> onComplete)
        {
            if (damageFloaterPrefab == null)
            {
                LHELogger.Log("Damage Floater Prefab 이 없습니다.");
                return;
            }

            var createdObject = Instantiate(damageFloaterPrefab, worldCanvas, true);
            var damageFloaterController = createdObject.GetComponent<DamageFloaterController>() ??
                                          createdObject.AddComponent<DamageFloaterController>();

            damageFloaterController.OnCreate(this);
            onComplete?.Invoke(createdObject);
        }
    }
}
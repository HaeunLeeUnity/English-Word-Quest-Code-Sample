using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonQuizFramework.CommonClass
{
    public abstract class ObjectPool : MonoBehaviour
    {
        private Dictionary<int, Queue<GameObject>> _objectPool = new();

        public virtual void SpawnToPool(int id, Vector3 position, Action<GameObject> onComplete)
        {
            if (!_objectPool.ContainsKey(id))
            {
                _objectPool?.Add(id, new Queue<GameObject>());
            }

            if (!_objectPool[id].TryDequeue(out var spawnedObject))
            {
                CreateGameObject(id, position, OnCreateComplete);

                void OnCreateComplete(GameObject createdGameObject)
                {
                    createdGameObject?.SetActive(true);
                    onComplete?.Invoke(createdGameObject);
                }
            }
            else
            {
                spawnedObject?.SetActive(true);
                spawnedObject.transform.position = position;
                onComplete?.Invoke(spawnedObject);
            }
        }

        public virtual void OnReturnedToPool(int id, GameObject returnedGameObject)
        {
            returnedGameObject.SetActive(false);
            _objectPool[id].Enqueue(returnedGameObject);
        }
        
        protected abstract void CreateGameObject(int id, Vector3 position, Action<GameObject> onComplete);

        private void OnDestroy()
        {
            _objectPool.Clear();
            _objectPool = null;
        }
    }
}
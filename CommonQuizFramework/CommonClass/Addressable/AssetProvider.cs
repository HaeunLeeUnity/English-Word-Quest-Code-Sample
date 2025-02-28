using System;
using System.Collections;
using LHEPackage.Helper;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CommonQuizFramework.CommonClass.Addressable
{
    // Unity Addressable을 사용하여 런타임 중 Asset 관리 기능 구현. 
    public class AssetProvider : MonoBehaviour
    {
        public static AssetProvider Instance;

        [SerializeField]
        private AssetReferenceGameObject[] characterReferences;

        [SerializeField]
        private AssetReferenceGameObject[] monsterReferences;

        [SerializeField]
        private AssetReferenceGameObject[] effectReferences;

        [SerializeField]
        private AssetReferenceGameObject[] mapReferences;

        [SerializeField]
        private AssetReference[] bgmReferences;

        private bool _isInit = false;

        public void Initialization()
        {
            if (_isInit) return;

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _isInit = true;
        }


        #region Download

        public void CheckAssetUpdate(Action<float> onProgress, Action<bool, long> onComplete)
        {
            CoroutineManager.Instance.CoroutineStart(Co_CheckAssetUpdate(onProgress, onComplete));
        }

        private IEnumerator Co_CheckAssetUpdate(Action<float> onProgress, Action<bool, long> onComplete)
        {
            var getDownloadSize = Addressables.GetDownloadSizeAsync("default");

            while (!getDownloadSize.IsDone)
            {
                onProgress?.Invoke(getDownloadSize.PercentComplete * 1f);
                yield return null;
            }

            if (getDownloadSize.Status != AsyncOperationStatus.Succeeded)
            {
                onComplete?.Invoke(false, 0);
                Addressables.Release(getDownloadSize);
                yield break;
            }

            onComplete.Invoke(true, getDownloadSize.Result);
            Addressables.Release(getDownloadSize);
        }

        public void DownloadAssets(Action<float> onProgress, Action<bool> onComplete)
        {
            CoroutineManager.Instance.CoroutineStart(Co_DownloadAssets(onProgress, onComplete));
        }

        private IEnumerator Co_DownloadAssets(Action<float> onProgress, Action<bool> onComplete)
        {
            var downloadDependenciesAsync = Addressables.DownloadDependenciesAsync("default");

            while (!downloadDependenciesAsync.IsDone)
            {
                onProgress?.Invoke(downloadDependenciesAsync.PercentComplete);
                yield return null;
            }

            Addressables.Release(downloadDependenciesAsync);
            onComplete?.Invoke(true);
        }

        #endregion

        #region Release

        public void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }

        public void ReleaseBGM(AudioClip clip)
        {
            Addressables.Release(clip);
        }

        #endregion

        #region Get / Spawn Asset

        public void SpawnInstance(EAssetType type, int id, Vector3 position, Action<GameObject> onComplete)
        {
            var targetReferences = GetGameObjectReference(type, id);

            if (targetReferences == null)
            {
                LHELogger.LogError($"References is Null {type} - {id}");
                onComplete?.Invoke(null);
                return;
            }

            targetReferences.InstantiateAsync(position, Quaternion.identity).Completed += (handle) =>
            {
                handle.Result.AddComponent<AddressableInstance>();
                onComplete?.Invoke(handle.Result);
            };
        }

        public void GetBGMAudioClip(int id, Action<AudioClip> onComplete)
        {
            if (bgmReferences[id] == null)
            {
                LHELogger.LogError($"References is Null {EAssetType.BGM} - {id}");
                onComplete?.Invoke(null);
                return;
            }

            bgmReferences[id].LoadAssetAsync<AudioClip>().Completed += (handle) =>
            {
                onComplete?.Invoke(handle.Result);
            };
        }

        #endregion

        #region Internal Logic

        private AssetReferenceGameObject GetGameObjectReference(EAssetType type, int id)
        {
            switch (type)
            {
                case EAssetType.Character:
                    return characterReferences[id];

                case EAssetType.Monster:
                    return monsterReferences[id];

                case EAssetType.Effect:
                    return effectReferences[id];

                case EAssetType.Map:
                    return mapReferences[id];

                default:
                    LHELogger.LogError($"Prefab Type is Wrong : {type}");
                    break;
            }

            return null;
        }

        #endregion
    }

    public enum EAssetType
    {
        Character,
        Monster,
        Effect,
        Map,
        BGM
    }
}
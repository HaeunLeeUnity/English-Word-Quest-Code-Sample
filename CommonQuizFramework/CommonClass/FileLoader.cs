using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CommonQuizFramework.CommonClass
{
    public class FileLoader
    {
        public void LoadFileInStreamingAssets(string fileName, Action<string> onComplete,
            Action onLoadFailed = null, Action<float> onProgress = null)
        {
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_IOS
            var filePath = $"file://{Path.Combine(Application.streamingAssetsPath, fileName)}";
#else
            var filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

            CoroutineManager.Instance.CoroutineStart(Co_LoadFile(filePath, onComplete, onLoadFailed, onProgress));
        }

        public void LoadFileInPersistentDataPath(string fileName, Action<string> onComplete,
            Action onLoadFailed = null, Action<float> onProgress = null)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            CoroutineManager.Instance.CoroutineStart(Co_LoadFile(filePath, onComplete, onLoadFailed, onProgress));
        }

        private IEnumerator Co_LoadFile(string filePath, Action<string> onComplete, Action onFailed = null,
            Action<float> onProgress = null)
        {
            var request = UnityWebRequest.Get(filePath);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                yield return null;
                onProgress?.Invoke(operation.progress);
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
    }
}
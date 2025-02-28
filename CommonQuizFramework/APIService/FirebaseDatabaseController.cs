using System;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

namespace CommonQuizFramework.APIService
{
    public class FirebaseDatabaseController
    {
        private FirebaseFirestore _defaultDataBase;

        public void Initialize()
        {
            _defaultDataBase = FirebaseFirestore.DefaultInstance;
        }

        public void RequestAppConfig(Action<AppConfig> onSuccess, Action<APIError> onFailed = null)
        {
            var docRef = _defaultDataBase.Collection("AppConfig").Document("Global");
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {
                    Debug.Log(task.Exception);
                    onFailed?.Invoke(APIError.FAILED);
                    return;
                }

                var doc = task.Result;
                var appConfig = doc.ConvertTo<AppConfig>();
                onSuccess?.Invoke(appConfig);
            });
        }
    }
}
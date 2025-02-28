using System;

namespace CommonQuizFramework.APIService
{
    public class APIServiceController
    {
        private FirebaseDatabaseController _firebaseDatabaseController;

        public APIServiceController()
        {
            _firebaseDatabaseController = new FirebaseDatabaseController();
            _firebaseDatabaseController.Initialize();
        }

        public void RequestAppConfig(Action<bool, AppConfig> onComplete)
        {
            _firebaseDatabaseController.RequestAppConfig(OnSuccess, OnFailed);

            void OnSuccess(AppConfig appConfig)
            {
                onComplete?.Invoke(true, appConfig);
            }

            void OnFailed(APIError apiError)
            {
                onComplete?.Invoke(false, null);
            }
        }
    }
}
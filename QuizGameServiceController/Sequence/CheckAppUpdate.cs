using CommonQuizFramework.APIService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.CommonClass.UI;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class CheckAppUpdate : SequenceElement
    {
        private AppUpdatePopupView _appUpdatePopupView;
        private APIServiceController _apiServiceController;

        public CheckAppUpdate(APIServiceController apiServiceController, AppUpdatePopupView appUpdatePopupView)
        {
            _apiServiceController = apiServiceController;
            _appUpdatePopupView = appUpdatePopupView;
        }

        public override void Execute()
        {
            _apiServiceController.RequestAppConfig(OnGetAppConfigComplete);

            void OnGetAppConfigComplete(bool isSuccess, AppConfig appConfig)
            {
                if (isSuccess)
                {
                    if (!CompareVersion(appConfig.MinimumVersion))
                    {
#if UNITY_IOS
                        _appUpdatePopupView.ShowPopup(appConfig.iOSStoreURL);
#else
                        _appUpdatePopupView.ShowPopup(appConfig.AOSStoreURL);
#endif
                    }
                    else
                    {
                        OnComplete();
                    }
                }
                else
                {
                    LHELogger.LogError($"APP VERSION CHECK ERROR");
                }
            }
        }

        // 현재 버전과 비교하는 함수
        // True  - 현재 앱 버전이 비교 대상보다 높아 실행이 가능함.
        // False - 현재 앱 버전이 비교 대상보다 낮거나 파싱할 수 없어 실행이 불가능함.
        private bool CompareVersion(string targetVersion)
        {
            var splitCurrentVersion = Application.version.Split('.');
            var splitTargetVersion = targetVersion.Split('.');

            for (var i = 0; i < splitCurrentVersion.Length; i++)
            {
                if (!int.TryParse(splitCurrentVersion[i], out var current)) return false;
                if (!int.TryParse(splitTargetVersion[i], out var target)) return false;

                if (current > target) return true;
                if (current < target) return false;
            }

            return true;
        }
    }
}
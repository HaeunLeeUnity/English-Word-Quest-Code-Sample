using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController
    {
        // 앱 실행 시 실행되는 시퀀스
        private void InitializationApp()
        {
            _loadIndicator.ForceShowIndicator();
            var downloadPopup = FindAnyObjectByType<AssetDownloadPopupView>(FindObjectsInactive.Include);
            var appUpdatePopup = FindAnyObjectByType<AppUpdatePopupView>(FindObjectsInactive.Include);

            var checkAppUpdate = new CheckAppUpdate(_apiServiceController, appUpdatePopup);
            var checkAssetUpdate = new CheckAssetUpdate(downloadPopup);
            var updateAsset = new UpdateAssets();
            var loadSettingsData = new LoadSettingsData(_settingController, _fileLoader);
            var loadStageMeta = new LoadStageMeta(_fileLoader, _stageServiceProvider);
            var loadClearData = new LoadClearData(_fileLoader, _userDataServiceProvider);

            _sequenceQueue.Enqueue(checkAppUpdate);
            _sequenceQueue.Enqueue(checkAssetUpdate);
            _sequenceQueue.Enqueue(updateAsset);
            _sequenceQueue.Enqueue(loadSettingsData);
            _sequenceQueue.Enqueue(loadStageMeta);
            _sequenceQueue.Enqueue(loadClearData);

            // 테스트 시 씬 전환이 필요없도록 실행중인 씬에 맞는 Enter Sequence를 추가로 진행
            switch (SceneManager.GetActiveScene().name)
            {
                case "Lobby":
                    EnterLobby();
                    break;
                case "Stage":
                    EnterStage();
                    break;
                default:
                    EnterLobby();
                    break;
            }
        }
    }
}
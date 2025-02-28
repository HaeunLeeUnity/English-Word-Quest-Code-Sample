using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.CommonClass.UI;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    // Asset Download가 필요한지 체크 후 팝업 표시
    public class CheckAssetUpdate : SequenceElement
    {
        private AssetDownloadPopupView _assetDownloadPopupView;

        public CheckAssetUpdate(AssetDownloadPopupView assetDownloadPopupView)
        {
            _assetDownloadPopupView = assetDownloadPopupView;
        }

        public override void Execute()
        {
            AssetProvider.Instance.CheckAssetUpdate(OnProgress, OnCheckComplete);

            void OnCheckComplete(bool isSuccess, long size)
            {
                // TODO: 다운로드 크기 확인에 실패한 경우에 대한 처리를 추가
                // - ex) 크기 확인 재시도 후 네트워크 상태를 요청하는 팝업을 표시
                LHELogger.Assert(isSuccess, "Check Asset Update Failed");

                if (0 < size)
                {
                    _assetDownloadPopupView.ShowPopup(size, OnComplete);
                }
                else
                {
                    OnComplete();
                }
            }
        }
    }
}
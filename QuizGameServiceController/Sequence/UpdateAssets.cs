using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class UpdateAssets : SequenceElement
    {
        public override void Execute()
        {
            AssetProvider.Instance.DownloadAssets(OnProgress, OnCompleteDownload);
            return;

            void OnCompleteDownload(bool isSuccess)
            {
                // TODO: 다운로드 실패 시 처리를 추가
                // - ex) 다운로드 재시도 후 네트워크 상태를 요청하는 팝업을 표시
                LHELogger.Assert(isSuccess, "Failed to Download Addressable");
                OnComplete();
            }
        }
    }
}
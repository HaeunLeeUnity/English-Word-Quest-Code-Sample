using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class SetBGMClip : SequenceElement
    {
        private int _bgmId;

        public SetBGMClip(int bgmId)
        {
            _bgmId = bgmId;
        }

        public override void Execute()
        {
            if (SoundManager.Instance.CurrentBGMID == _bgmId)
            {
                OnComplete();
            }
            else
            {
                AssetProvider.Instance.GetBGMAudioClip(_bgmId, OnLoadComplete);

                void OnLoadComplete(AudioClip bgmClip)
                {
                    LHELogger.Assert(bgmClip != null, "BGM Clip is null");

                    AssetProvider.Instance.ReleaseBGM(SoundManager.Instance.BGMClip);
                    SoundManager.Instance.BGMClip = bgmClip;
                    SoundManager.Instance.CurrentBGMID = _bgmId;

                    OnComplete();
                }
            }
        }
    }
}
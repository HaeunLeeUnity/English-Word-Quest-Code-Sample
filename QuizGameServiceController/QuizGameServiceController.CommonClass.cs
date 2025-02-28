using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.UI;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController
    {
        private void RegisterSettingView()
        {
            _settingController.RegisterSettingView(FindAnyObjectByType<SettingPopupView>(FindObjectsInactive.Include));
        }

        private void ApplySettings(Settings settings)
        {
            SoundManager.Instance.BGMVolume = settings.BGMVolume;
            SoundManager.Instance.SFXVolume = settings.SFXVolume;
            VibrationProvider.UseVibration = settings.UseVibration;
            _quizServiceController.TTSVolume = settings.TTSVolume;
        }
    }
}
using System;
using CommonQuizFramework.CommonClass.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace CommonQuizFramework.CommonClass
{
    public class SettingController : ISceneComponentAdapter
    {
        private SettingPopupView _settingPopupView;
        private Settings _currentSettings;
        private Action<Settings> _onSettingApply;

        public string SettingsJsonData
        {
            set => CurrentSettings = JsonConvert.DeserializeObject<Settings>(value);
        }

        public Settings CurrentSettings
        {
            set
            {
                _currentSettings = value;
                _onSettingApply?.Invoke(_currentSettings);
            }
        }

        public SettingController(Action<Settings> onSettingApply)
        {
            _onSettingApply = onSettingApply;
        }

        public void RegisterSettingView(SettingPopupView settingPopupView)
        {
            _settingPopupView = settingPopupView;
            _settingPopupView.Initialization(_currentSettings, ChangeSetting);
        }

        public void DisposeSceneComponent()
        {
            _settingPopupView = null;
        }

        public void CreateSettings()
        {
            CurrentSettings = new Settings();
            SaveSettings();
        }

        private void ChangeSetting(Settings settings)
        {
            CurrentSettings = settings;
            SaveSettings();
        }

        private void SaveSettings()
        {
            var settingJson = JsonConvert.SerializeObject(_currentSettings);
            FileSaver.SaveFileInPersistentDataPath(CommonDefinition.SettingsFileName, settingJson);
        }
    }

    public class Settings
    {
        public float SFXVolume = 1f;
        public float BGMVolume = 1f;
        public float TTSVolume = 1f;
        public bool UseVibration = true;
    }
}
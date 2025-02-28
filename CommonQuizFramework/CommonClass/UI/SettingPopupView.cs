using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.UI
{
    public class SettingPopupView : MonoBehaviour
    {
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider ttsSlider;
        [SerializeField] private Toggle vibrationToggle;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button applyButton;

        [SerializeField] private TextMeshProUGUI sfxText;
        [SerializeField] private TextMeshProUGUI bgmText;
        [SerializeField] private TextMeshProUGUI ttsText;

        [SerializeField] private GameObject vibrationOnIndicator;
        [SerializeField] private GameObject vibrationOffIndicator;

        [SerializeField] private TextMeshProUGUI versionCodeIndicator;

        private Action<Settings> _onSettingChanged;
        private Settings _currentSettings;

        private bool _isInit = false;

        public void Initialization(Settings settings, Action<Settings> onSettingChanged)
        {
            _currentSettings = settings;
            _onSettingChanged = onSettingChanged;

            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            ttsSlider.onValueChanged.AddListener(SetTTSVolume);
            vibrationToggle.onValueChanged.AddListener(ToggleVibration);
            resetButton.onClick.AddListener(Reset);
            applyButton.onClick.AddListener(ApplySetting);

            versionCodeIndicator.text = $"English Words Quest - Version v.{Application.version}";

            _isInit = true;
        }

        private void OnEnable()
        {
            if (!_isInit) return;
            Reset();
        }

        private void SetSFXVolume(float volume)
        {
            applyButton.interactable = true;
            sfxText.text = $"{volume * 100:0}";
        }

        private void SetBGMVolume(float volume)
        {
            applyButton.interactable = true;
            bgmText.text = $"{volume * 100:0}";
        }

        private void SetTTSVolume(float volume)
        {
            applyButton.interactable = true;
            ttsText.text = $"{volume * 100:0}";
        }


        private void ToggleVibration(bool isOn)
        {
            applyButton.interactable = true;

            if (isOn)
            {
                vibrationOffIndicator.gameObject.SetActive(false);
                vibrationOnIndicator.gameObject.SetActive(true);
            }
            else
            {
                vibrationOffIndicator.gameObject.SetActive(true);
                vibrationOnIndicator.gameObject.SetActive(false);
            }
        }

        private void Reset()
        {
            applyButton.interactable = false;

            sfxSlider.SetValueWithoutNotify(_currentSettings.SFXVolume);
            bgmSlider.SetValueWithoutNotify(_currentSettings.BGMVolume);
            ttsSlider.SetValueWithoutNotify(_currentSettings.TTSVolume);
            vibrationToggle.SetIsOnWithoutNotify(_currentSettings.UseVibration);

            sfxText.text = $"{_currentSettings.SFXVolume * 100:0}";
            bgmText.text = $"{_currentSettings.BGMVolume * 100:0}";
            ttsText.text = $"{_currentSettings.TTSVolume * 100:0}";

            if (_currentSettings.UseVibration)
            {
                vibrationOffIndicator.gameObject.SetActive(false);
                vibrationOnIndicator.gameObject.SetActive(true);
            }
            else
            {
                vibrationOffIndicator.gameObject.SetActive(true);
                vibrationOnIndicator.gameObject.SetActive(false);
            }
        }

        private void ApplySetting()
        {
            applyButton.interactable = false;

            _currentSettings.SFXVolume = sfxSlider.value;
            _currentSettings.BGMVolume = bgmSlider.value;
            _currentSettings.TTSVolume = ttsSlider.value;
            _currentSettings.UseVibration = vibrationToggle.isOn;

            _onSettingChanged?.Invoke(_currentSettings);
        }
    }
}
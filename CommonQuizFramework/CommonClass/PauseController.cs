using System;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject pausePopup;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        // 배속을 기능이 있는 게임 (자동 사냥 게임 등) 에서 적용이 가능하도록
        // 일시중지를 해제할 때 캐시된 Time Scale 을 적용한다.
        private float _cachedTimeScale = 1;

        private void Awake()
        {
            pauseButton.onClick.AddListener(Pause);
            resumeButton.onClick.AddListener(Resume);
        }

        private void Pause()
        {
            pausePopup.SetActive(true);
            _cachedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        private void Resume()
        {
            pausePopup.SetActive(false);
            Time.timeScale = _cachedTimeScale;
        }

        public void RegisterRetryAction(Action onRetry)
        {
            retryButton.onClick.AddListener(OnClickRetry);
            return;

            void OnClickRetry()
            {
                onRetry();
                Resume();
            }
        }

        public void RegisterExitAction(Action onExit)
        {
            exitButton.onClick.AddListener(OnClickExit);
            return;

            void OnClickExit()
            {
                onExit();
                Resume();
            }
        }
    }
}
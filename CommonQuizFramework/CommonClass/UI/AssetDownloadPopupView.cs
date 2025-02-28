using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.UI
{
    public class AssetDownloadPopupView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button deniedButton;

        public void ShowPopup(long size, Action onAccept)
        {
            gameObject.SetActive(true);
            deniedButton.onClick.AddListener(OnClickDenied);
            acceptButton.onClick.AddListener(OnClickAccept);

            text.text =
                $"게임 플레이를 위해 리소스 다운로드가 필요합니다.\n(WiFI 연결 권장)\n\n추가 다운로드 용량\n<font-weight=500>{ByteSizeFormatter.FormatBytes(size)}";

            void OnClickDenied()
            {
                Application.Quit();
            }

            void OnClickAccept()
            {
                gameObject.SetActive(false);
                onAccept?.Invoke();
            }
        }
    }
}
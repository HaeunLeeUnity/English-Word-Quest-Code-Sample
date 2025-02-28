using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.UI
{
    public class AppUpdatePopupView : MonoBehaviour
    {
        [SerializeField] private Button acceptButton;

        public void ShowPopup(string storeURL)
        {
            gameObject.SetActive(true);
            acceptButton.onClick.AddListener(OnClickAccept);

            void OnClickAccept()
            {
                Application.OpenURL(storeURL);
            }
        }
    }
}
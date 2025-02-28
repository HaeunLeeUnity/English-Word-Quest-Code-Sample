using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.Sequence
{
    public class LoadIndicator : MonoBehaviour
    {
        private static LoadIndicator _instance;

        [SerializeField] private TextMeshProUGUI loadProgressText;
        [SerializeField] private Slider loadProgressSlider;
        [SerializeField] private Animator animator;

        private static readonly int Show = Animator.StringToHash("Show");

        // 단일 객체로 존재하나 전역적으로 접근이 필요한 객체가 아님
        // 따라서 private 로 선언함
        public void Initialization()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void HideIndicator()
        {
            loadProgressText.text = $"<font-weight=\"600\">100%</font-weight>";
            loadProgressSlider.value = 1;
            animator.SetBool(Show, false);
        }

        public void OnCompleteHide()
        {
            gameObject.SetActive(false);
        }

        // 로딩 화면의 Fade-In 애니메이션을 재생하지 않고 즉시 표시함
        // *앱 실행 시에는 로딩을 즉시 표시
        public void ForceShowIndicator()
        {
            animator.SetTrigger("ForceShow");
        }

        public void ShowIndicator(float loadProgress)
        {
            gameObject.SetActive(true);
            animator.SetBool(Show, true);
            loadProgressText.text = $"<font-weight=\"600\">{loadProgress * 100:F0}%</font-weight>";
            loadProgressSlider.value = loadProgress;
        }
    }
}
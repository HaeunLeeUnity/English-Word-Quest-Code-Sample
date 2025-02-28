using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltipText;
        [SerializeField] private GameObject tooltipPanel;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ShowTooltip);
        }

        private void ShowTooltip()
        {
            tooltipPanel.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipPanel.SetActive(false);
        }

        public void SetText(string text)
        {
            tooltipText.text = text;
        }
    }
}
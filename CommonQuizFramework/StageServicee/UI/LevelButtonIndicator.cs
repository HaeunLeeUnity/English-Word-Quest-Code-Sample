using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CommonQuizFramework.StageService
{
    public class LevelButtonIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelIndicator;
        [SerializeField] private GameObject[] starsOff = new GameObject[3];
        [SerializeField] private GameObject[] starsOn = new GameObject[3];

        public void Initalize(int majorID, int minorID, int clearRating)
        {
            levelIndicator.text = $"{majorID}-{minorID}";

            switch (clearRating)
            {
                case 0:
                    starsOff[0].SetActive(true);
                    starsOff[1].SetActive(true);
                    starsOff[2].SetActive(true);
                    starsOn[0].SetActive(false);
                    starsOn[1].SetActive(false);
                    starsOn[2].SetActive(false);
                    break;
                case 1:
                    starsOff[0].SetActive(false);
                    starsOff[1].SetActive(true);
                    starsOff[2].SetActive(true);
                    starsOn[0].SetActive(true);
                    starsOn[1].SetActive(false);
                    starsOn[2].SetActive(false);
                    break;
                case 2:
                    starsOff[0].SetActive(false);
                    starsOff[1].SetActive(false);
                    starsOff[2].SetActive(true);
                    starsOn[0].SetActive(true);
                    starsOn[1].SetActive(true);
                    starsOn[2].SetActive(false);
                    break;
                case 3:
                    starsOff[0].SetActive(false);
                    starsOff[1].SetActive(false);
                    starsOff[2].SetActive(false);
                    starsOn[0].SetActive(true);
                    starsOn[1].SetActive(true);
                    starsOn[2].SetActive(true);
                    break;
            }

            foreach (var staroff in starsOff)
            {
                staroff.SetActive(true);
            }
        }
    }
}
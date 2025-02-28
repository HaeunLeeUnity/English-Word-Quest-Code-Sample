using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CommonQuizFramework.CommonClass.UI
{
    public static class CommonUIFunction
    {
        public static IEnumerator Co_SetSliderValueGradual(Slider slider, float value, float duration)
        {
            var timeRatio = 1 / duration;
            var startValue = slider.value;
            var currentTime = 0f;

            while (currentTime < 1)
            {
                yield return null;
                currentTime += timeRatio * Time.deltaTime;
                slider.value = Mathf.Lerp(startValue, value, currentTime);
            }
        }
    }
}
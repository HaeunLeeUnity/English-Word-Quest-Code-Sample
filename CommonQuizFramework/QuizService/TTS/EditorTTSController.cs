using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizService.TTS
{
    public class EditorTTSController : INativeTTSController
    {
        public float Volume { get; set; }

        public void Speak(string text)
        {
            LHELogger.Log($"Native TTS Speak: {text}");
        }
    }
}
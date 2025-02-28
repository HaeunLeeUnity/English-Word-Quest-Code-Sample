using UnityEngine;

namespace CommonQuizFramework.QuizService.TTS
{
    public class AOSTTSController : INativeTTSController
    {
        private static AndroidJavaObject _TTSPlugin;

        private const float Pitch = 1.2f;
        private const float SpeechRate = 0.8f;

        public float Volume
        {
            set => _TTSPlugin.CallStatic("setVolume", value);
        }

        public AOSTTSController()
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pluginClass = new AndroidJavaClass("com.superpixel.plugin.TTSPlugin");
            _TTSPlugin = pluginClass;
            _TTSPlugin.CallStatic("initializeTTS", activity);
            _TTSPlugin.CallStatic("setPitch", Pitch);
            _TTSPlugin.CallStatic("setSpeechRate", SpeechRate);
        }

        public void Speak(string text)
        {
            _TTSPlugin?.CallStatic("speak", text);
        }
    }
}
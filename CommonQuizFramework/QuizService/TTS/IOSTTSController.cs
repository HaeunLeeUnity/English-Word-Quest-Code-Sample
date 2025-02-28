using System.Runtime.InteropServices;

namespace CommonQuizFramework.QuizService.TTS
{
    public class IOSTTSController : INativeTTSController
    {
        private const float Pitch = 1.2f;
        private const float SpeechRate = 0.45f;

        public float Volume
        {
            set => SetVolume(value);
        }

        [DllImport("__Internal")]
        private static extern void InitializeSynthesizerAsync();

        [DllImport("__Internal")]
        private static extern void NativeSpeak(string text);

        [DllImport("__Internal")]
        private static extern void SetSpeechRate(float rate);

        [DllImport("__Internal")]
        private static extern void SetPitchMultiplier(float pitch);

        [DllImport("__Internal")]
        private static extern void SetVolume(float vol);

        public IOSTTSController()
        {
            SetPitchMultiplier(Pitch);
            SetSpeechRate(SpeechRate);
            InitializeSynthesizerAsync();
        }

        public void Speak(string text)
        {
            NativeSpeak(text);
        }
    }
}
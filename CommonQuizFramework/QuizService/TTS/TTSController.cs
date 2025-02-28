namespace CommonQuizFramework.QuizService.TTS
{
    public class TTSController
    {
        private INativeTTSController _nativeTTSController;

        public float Volume
        {
            set => _nativeTTSController.Volume = value;
        }

        public TTSController()
        {
#if UNITY_EDITOR
            _nativeTTSController = new EditorTTSController();
#elif UNITY_ANDROID
            _nativeTTSController = new AOSTTSController();
#elif UNITY_IOS
            _nativeTTSController = new IOSTTSController();
#endif
        }


        public void Speak(string text)
        {
            _nativeTTSController.Speak(text);
        }
    }
}
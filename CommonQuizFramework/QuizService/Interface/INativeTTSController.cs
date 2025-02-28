namespace CommonQuizFramework.QuizService
{
    public interface INativeTTSController
    {
        public float Volume { set; }
        public void Speak(string text);
    }
}
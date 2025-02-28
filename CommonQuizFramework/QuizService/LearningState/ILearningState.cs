namespace CommonQuizFramework.QuizService.LearningState
{
    public interface ILearningState
    {
        public string CurrentWord { get; }
        public int RemainQuizCount { get; }
        public void StartState();
        public void Answer(int answer);
        public void Skip();
        public void Resume();

        #region Test

        public void ForceClear();

        #endregion
    }
}
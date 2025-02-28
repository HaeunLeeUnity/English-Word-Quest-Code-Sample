namespace CommonQuizFramework.QuizService
{
    public class QuizResult
    {
        public bool IsClear;
        public int ComboRecord;
        public int RemainQuiz;
        public int RemainChance;

        // 오답 노트 표기를 위한 틀린 단어 배열
        // 1 = 단어, 2 = 뜻
        public (string, string)[] FailedWords = new (string, string)[10];
    }
}
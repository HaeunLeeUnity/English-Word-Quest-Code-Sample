using System;

namespace CommonQuizFramework.QuizService
{
    [Obsolete("단방향 참조 원칙을 지켜 구현하려고 하였으나 개발 단계에서 코드 수정이 잦아 보류")]
    public interface ILearningHandler
    {
        public bool CheckAnswer(int answer);
        public void RemoveRemainQuiz();
        public void ShowQuizAndAnswer();
        public void ShowQuizAndExamples();
    }
}
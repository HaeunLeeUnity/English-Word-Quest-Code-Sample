using System;
using CommonQuizFramework.CommonClass;
using JetBrains.Annotations;

namespace CommonQuizFramework.QuizService
{
    public interface IQuizServiceRequestHandler
    {
        public SelectedLevel GetSelectedLevel();

        [CanBeNull]
        public SelectedLevel GetNextLevel();

        public int GetComboRecord();
        public void OnChangeState(LearningStep learningStep, Action onComplete = null);
        public void OnAnswer(int? select, int answer, Action onComplete = null);
        public void OnResume();
        public void ResetQuiz();
        public void RequestEnterLobby();
        public void RequestNextLevel();
    }
}
using System;
using System.Collections.Generic;

namespace CommonQuizFramework.QuizService.LearningState
{
    public interface ILearningStateRequestHandler
    {
        #region UI 표시

        public void ShowMemorizeView(Quiz quiz);
        public void ShowQuizView(Quiz quiz);
        public void SetRemainQuizCountView(int count);
        public void ShowStepProgress(LearningStep learningStep, float stepProgress);
        public void ShowMemorizeTime(float time);
        public void HideMemorizeTime();

        #endregion

        #region Quiz List

        public List<Quiz> RemainQuizList { get; set; }
        public void AddFailedQuiz(Quiz quiz);

        #endregion

        public void SpeakTTSCurrentWord();
        public void OnAnswer(int? select, int answer, Action onComplete);
        public void ChangeState(LearningStep learningStep);
        public void HideQuizView();
        public void HideMemorizeView();
    }
}
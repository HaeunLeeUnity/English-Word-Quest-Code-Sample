using System.Collections;
using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;

namespace CommonQuizFramework.QuizService.LearningState
{
    public abstract class LearningState : ILearningState
    {
        public string CurrentWord => CurrentQuiz.Word;
        public abstract int RemainQuizCount { get; }

        protected ILearningStateRequestHandler Handler;

        protected List<Quiz> InitialQuizList;
        protected int QuizIndex;
        protected Quiz CurrentQuiz;

        protected int CorrectAnswer
        {
            get
            {
                for (var i = 0; i < QuizServiceDefinition.MaxChoiceCount; i++)
                {
                    if (!CurrentQuiz.Choices[i].Correct) continue;
                    return i;
                }

                return 0;
            }
        }

        protected float StepProgress => QuizIndex / (float)InitialQuizList.Count * 100;

        public abstract void StartState();
        public abstract void Answer(int answer);
        public abstract void Skip();

        public void Resume()
        {
            CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
        }

        protected abstract void EndState();

        protected void StartQuiz()
        {
            if (InitialQuizList.Count <= QuizIndex)
            {
                EndState();
                return;
            }

            CurrentQuiz = InitialQuizList[QuizIndex];
            Handler.ShowQuizView(CurrentQuiz);
        }

        protected virtual IEnumerator Co_AfterAnswer()
        {
            Handler.HideQuizView();
            yield return YieldInstructionCache.WaitForSeconds(1.1f);
            StartQuiz();
        }

        #region Test

        public abstract void ForceClear();

        #endregion
    }
}
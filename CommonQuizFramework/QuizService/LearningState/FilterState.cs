using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizService.LearningState
{
    // [거르기]
    // 사용자가 이미 알고 있는 단어를 거르는 단계.
    // 단어의 뜻과 같이 보기가 출제된다.

    public class FilterState : LearningState
    {
        private List<Quiz> _filteredQuizList;
        public override int RemainQuizCount => InitialQuizList.Count - (QuizIndex - _filteredQuizList.Count);

        public FilterState(ILearningStateRequestHandler handler)
        {
            Handler = handler;
            var rawQuizList = Handler.RemainQuizList;
            InitialQuizList = Shuffler.ShuffleQuizzes(rawQuizList);
            _filteredQuizList = new List<Quiz>();
        }

        public override void StartState()
        {
            Handler.SetRemainQuizCountView(InitialQuizList.Count);
            StartQuiz();
        }

        public override void Answer(int answer)
        {
            if (CorrectAnswer != answer)
            {
                _filteredQuizList.Add(CurrentQuiz);
            }

            QuizIndex++;
            Handler.SetRemainQuizCountView(RemainQuizCount);
            Handler.ShowStepProgress(LearningStep.Filter, StepProgress);
            Handler.SpeakTTSCurrentWord();
            Handler.OnAnswer(answer, CorrectAnswer, OnComplete);
            return;

            void OnComplete()
            {
                CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
            }
        }

        public override void Skip()
        {
            _filteredQuizList.Add(CurrentQuiz);
            QuizIndex++;
            Handler.ShowStepProgress(LearningStep.Filter, StepProgress);
            Handler.SpeakTTSCurrentWord();
            Handler.OnAnswer(null, CorrectAnswer, OnComplete);

            void OnComplete()
            {
                CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
            }
        }

        protected override void EndState()
        {
            LHELogger.Log("[거르기] 단계 종료");
            if (_filteredQuizList.Count == 0)
            {
                Handler.ChangeState(LearningStep.End);
            }
            else
            {
                Handler.RemainQuizList = _filteredQuizList;
                Handler.ChangeState(LearningStep.Memorize);
            }
        }

        #region Test

        public override void ForceClear()
        {
            _filteredQuizList.Clear();
            QuizIndex = InitialQuizList.Count;
            EndState();
        }

        #endregion
    }
}
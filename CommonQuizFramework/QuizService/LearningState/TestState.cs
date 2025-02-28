using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizService.LearningState
{
    // [시험]
    // 사용자가 외운 단어를 최종적으로 시험보는 단계
    // 틀린 문제가 있다면 시험 종료 후 [외우기] 단계로 돌아간다.

    public class TestState : LearningState
    {
        // 틀린 문제 리스트.
        // 해당 리스트에 요소는 [외우기] -> [시험] 단계를 진행함.
        private List<Quiz> _filteredQuizList;

        public override int RemainQuizCount => InitialQuizList.Count - (QuizIndex - _filteredQuizList.Count);

        public TestState(ILearningStateRequestHandler handler)
        {
            Handler = handler;
            var rawQuizList = Handler.RemainQuizList;
            InitialQuizList = Shuffler.ShuffleQuizzes(rawQuizList);
            _filteredQuizList = new List<Quiz>();
        }

        public override void StartState()
        {
            StartQuiz();
        }

        public override void Answer(int answer)
        {
            if (answer != CorrectAnswer)
            {
                _filteredQuizList.Add(CurrentQuiz);
                Handler.AddFailedQuiz(CurrentQuiz);
            }

            QuizIndex++;
            Handler.SetRemainQuizCountView(RemainQuizCount);
            Handler.ShowStepProgress(LearningStep.Test, StepProgress);
            Handler.SpeakTTSCurrentWord();
            Handler.OnAnswer(answer, CorrectAnswer, OnComplete);

            void OnComplete()
            {
                CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
            }
        }

        public override void Skip()
        {
            _filteredQuizList.Add(CurrentQuiz);
            Handler.AddFailedQuiz(CurrentQuiz);
            QuizIndex++;
            Handler.ShowStepProgress(LearningStep.Test, StepProgress);
            Handler.SpeakTTSCurrentWord();
            Handler.OnAnswer(null, CorrectAnswer, OnComplete);

            void OnComplete()
            {
                CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
            }
        }

        protected override void EndState()
        {
            LHELogger.Log("[시험] 단계 종료");

            if (0 < _filteredQuizList.Count)
            {
                LHELogger.Log($"잔여문제 {_filteredQuizList.Count}개 - [외우기] 단계 재시작");
                Handler.RemainQuizList = _filteredQuizList;
                Handler.ChangeState(LearningStep.Memorize);
            }
            else
            {
                Handler.ChangeState(LearningStep.End);
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
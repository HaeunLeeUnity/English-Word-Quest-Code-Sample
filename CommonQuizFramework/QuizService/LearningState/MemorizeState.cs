using System.Collections;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizService.LearningState
{
    // [외우기]
    // 단어를 암기하는 단계
    // 뜻과 정답을 먼저 공개하고 이후 숨겨진다.

    public class MemorizeState : LearningState
    {
        private EMemorizeStep _currentMemorizeStep = EMemorizeStep.Watch;
        public override int RemainQuizCount => InitialQuizList.Count;

        public MemorizeState(ILearningStateRequestHandler handler)
        {
            Handler = handler;
            var rawQuizList = Handler.RemainQuizList;
            InitialQuizList = Shuffler.ShuffleQuizzes(rawQuizList);
        }

        public override void StartState()
        {
            StartMemorize();
        }

        public override void Answer(int answer)
        {
            Handler.SpeakTTSCurrentWord();
            Handler.OnAnswer(answer, CorrectAnswer, OnComplete);

            if (CurrentQuiz.Choices[answer].Correct)
            {
                QuizIndex++;
                Handler.ShowStepProgress(LearningStep.Memorize, StepProgress);
            }
            else
            {
                Handler.AddFailedQuiz(CurrentQuiz);
            }

            void OnComplete()
            {
                LHELogger.Log("외우기 정답 연출 완료 후 호출");
                CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
            }
        }

        public override void Skip()
        {
            if (_currentMemorizeStep == EMemorizeStep.Watch)
            {
                _currentMemorizeStep = EMemorizeStep.Wait;
                CoroutineManager.Instance.CoroutineStart(Co_MemorizeComplete());
            }
            else
            {
                Handler.AddFailedQuiz(CurrentQuiz);
                Handler.SpeakTTSCurrentWord();
                Handler.OnAnswer(null, CorrectAnswer, OnComplete);

                void OnComplete()
                {
                    CoroutineManager.Instance.CoroutineStart(Co_AfterAnswer());
                }
            }
        }

        protected override void EndState()
        {
            LHELogger.Log("[외우기] 단계 종료");
            Handler.ChangeState(LearningStep.Test);
        }

        protected override IEnumerator Co_AfterAnswer()
        {
            Handler.HideQuizView();
            yield return YieldInstructionCache.WaitForSeconds(1.1f);
            StartMemorize();
        }

        private void StartMemorize()
        {
            _currentMemorizeStep = EMemorizeStep.Watch;

            if (InitialQuizList.Count <= QuizIndex)
            {
                EndState();
                return;
            }

            CurrentQuiz = InitialQuizList[QuizIndex];
            Handler.ShowMemorizeView(InitialQuizList[QuizIndex]);
        }

        private IEnumerator Co_MemorizeComplete()
        {
            Handler.HideMemorizeView();

            var currentTime = QuizServiceDefinition.MemorizeTestWaitTime;

            while (currentTime > 0)
            {
                Handler.ShowMemorizeTime(currentTime);
                yield return null;
                currentTime = Mathf.Clamp(currentTime - Time.unscaledDeltaTime, 0,
                    QuizServiceDefinition.MemorizeTestWaitTime);
            }

            _currentMemorizeStep = EMemorizeStep.Test;
            Handler.HideMemorizeTime();
            Handler.ShowQuizView(InitialQuizList[QuizIndex]);
        }

        #region Test

        public override void ForceClear()
        {
        }

        #endregion
    }

    internal enum EMemorizeStep
    {
        Watch,
        Wait,
        Test
    }
}
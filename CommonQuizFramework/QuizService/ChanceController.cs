using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizService
{
    public class ChanceController
    {
        private QuizServiceController _quizServiceController;
        private int _remainChance;
        public int RemainChance => _remainChance;

        public ChanceController(QuizServiceController quizServiceController)
        {
            _quizServiceController = quizServiceController;
            _remainChance = QuizServiceDefinition.MaxChanceCount;
        }

        public void Reset()
        {
            _remainChance = QuizServiceDefinition.MaxChanceCount;
        }

        public void DiscountChance()
        {
            _remainChance--;
            _quizServiceController.ShowRemainChance(_remainChance);

            if (_remainChance == 0)
            {
                LHELogger.Log("퀴즈 종료");
            }
        }

        public void AddChance(int chance)
        {
            _remainChance += chance;
            _quizServiceController.ShowRemainChance(_remainChance);
        }
    }
}
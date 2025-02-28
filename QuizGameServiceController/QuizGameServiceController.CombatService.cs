using CommonQuizFramework.CombatService;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController : ICombatServiceRequestHandler
    {
        public bool IsGameOver => _quizServiceController.IsGameOver;

        private void RegisterCombatComponent()
        {
            var damageFloaterPool = FindAnyObjectByType<DamageFloaterPool>();
            var effectPool = FindAnyObjectByType<EffectPool>();

            _combatServiceController.RegisterObjectPool(damageFloaterPool, effectPool);
        }

        public void DiscountChance()
        {
            _quizServiceController.DiscountChance();
        }
    }
}
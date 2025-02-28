namespace CommonQuizFramework.CombatService
{
    public interface ICombatServiceRequestHandler
    {
        public bool IsGameOver { get; }
        public void DiscountChance();
        public void OnPlayerDead();
    }
}
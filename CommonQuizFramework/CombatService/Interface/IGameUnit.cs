using System;

namespace CommonQuizFramework.CombatService
{
    public interface IGameUnit
    {
        public void Initialization(EffectPool effectPool, DamageFloaterPool damageFloaterPool);
        public void Reset();
        public void MoveToBattlePosition(Action onComplete);
        public void MoveToRestPosition(Action onComplete);
        public void MoveWithFloor();
        public void OnStopFloor();
        public void Attack(Action onAttackImpact, Action onAttackComplete);
        public void Hit();
        public void Dead(Action onDeadComplete);
        public void Reborn();
        public int GetComboRecord();
        public void ResetCombo();
        public void Release();
    }
}
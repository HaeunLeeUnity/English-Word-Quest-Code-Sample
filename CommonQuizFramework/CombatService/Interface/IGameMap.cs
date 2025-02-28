using System;

namespace CommonQuizFramework.CombatService
{
    public interface IGameMap
    {
        public void Initialization();
        public void Move(float distance, float duration, Action onComplete);
        public void Stop();
        public void Release();
    }
}
using System;
using CommonQuizFramework.CommonClass;

namespace CommonQuizFramework.CombatService
{
    public class CombatServiceController : ISceneComponentAdapter
    {
        private ICombatServiceRequestHandler _combatServiceRequestHandler;

        private IGameMap _map;
        private IGameUnit _playerCharacter;

        private IGameUnit[] _monsters;
        private IGameUnit _currentMonster;

        private BattleStep _currentBattleStep = BattleStep.Minion;
        private DamageFloaterPool _damageFloaterPool;
        private EffectPool _effectPool;

        public CombatServiceController(ICombatServiceRequestHandler combatServiceRequestHandler)
        {
            _combatServiceRequestHandler = combatServiceRequestHandler;
        }

        public void SetMap(IGameMap map)
        {
            _map = map;
            map.Initialization();
        }

        public void SetPlayerCharacter(IGameUnit character)
        {
            _playerCharacter = character;

            if (_effectPool == null) return;
            if (_damageFloaterPool == null) return;

            _playerCharacter.Initialization(_effectPool, _damageFloaterPool);
        }

        public void SetMonsters(IGameUnit[] monsters)
        {
            _monsters = monsters;

            if (_effectPool == null) return;
            if (_damageFloaterPool == null) return;

            foreach (var monster in _monsters)
            {
                monster.Initialization(_effectPool, _damageFloaterPool);
            }
        }

        public void RegisterObjectPool(DamageFloaterPool damageFloaterPool, EffectPool effectPool)
        {
            _damageFloaterPool = damageFloaterPool;
            _effectPool = effectPool;

            if (_playerCharacter != null)
            {
                _playerCharacter.Initialization(_effectPool, _damageFloaterPool);
            }

            if (_monsters == null)
            {
                return;
            }

            foreach (var monster in _monsters)
            {
                monster.Initialization(_effectPool, _damageFloaterPool);
            }
        }

        public void ReleaseGameAssets()
        {
            _map?.Release();
            _playerCharacter?.Release();
            if (_monsters != null)
                foreach (var monster in _monsters)
                {
                    monster?.Release();
                }

            _map = null;
            _playerCharacter = null;
            _currentMonster = null;
            _monsters = null;
        }
        
        public void DisposeSceneComponent()
        {
            ReleaseGameAssets();
            _effectPool = null;
            _damageFloaterPool = null;
        }

        public void Reset()
        {
            _map.Stop();
            _playerCharacter.Reset();

            foreach (var monster in _monsters)
            {
                monster.Reset();
            }
        }

        public void ReadyToCharacter(Action onComplete)
        {
            _playerCharacter.MoveToBattlePosition(onComplete);
        }

        public void ChangeStep(BattleStep battleStep, Action onComplete = null)
        {
            switch (battleStep)
            {
                case BattleStep.Minion:
                    MoveToNextMonster();
                    break;

                case BattleStep.MiniBoss:
                    _currentMonster.MoveToRestPosition(MoveToNextMonster);
                    break;

                case BattleStep.Boss:
                    _currentMonster.Dead(MoveToNextMonster);
                    break;

                case BattleStep.Clear:
                    _currentMonster.Dead(onComplete);
                    break;
            }

            _currentBattleStep = battleStep;

            void MoveToNextMonster()
            {
                _map.Move(-CombatServiceDefinition.MapMoveDistance, CombatServiceDefinition.MapMoveDuration,
                    OnMoveFloorComplete);
                _playerCharacter.MoveWithFloor();

                _currentMonster = _monsters[(int)battleStep];
                _currentMonster.MoveWithFloor();
            }

            void OnMoveFloorComplete()
            {
                _playerCharacter.OnStopFloor();
                _currentMonster.OnStopFloor();
                onComplete?.Invoke();
            }
        }

        #region Attack & Hit

        public void PlayerAttack(Action onComplete)
        {
            _playerCharacter.Attack(OnAttackImpact, OnAttackEnd);

            void OnAttackImpact()
            {
                _currentMonster.Hit();
            }

            void OnAttackEnd()
            {
                onComplete?.Invoke();
            }
        }

        public void MonsterAttack(Action onComplete)
        {
            // 선별 단계의 몬스터는 공격하지 않는다.
            // 단, 문제를 맞추는데 실패하였으므로 콤보는 초기화 한다. 
            if (_currentBattleStep == BattleStep.Minion)
            {
                _playerCharacter.ResetCombo();
                onComplete?.Invoke();
                return;
            }

            _currentMonster.Attack(OnAttackImpact, OnAttackEnd);

            void OnAttackImpact()
            {
                _combatServiceRequestHandler.DiscountChance();

                if (_combatServiceRequestHandler.IsGameOver)
                {
                    _playerCharacter.Dead(OnDeadEnd);
                }
                else
                {
                    _playerCharacter.Hit();
                }
            }

            void OnAttackEnd()
            {
                if (!_combatServiceRequestHandler.IsGameOver)
                {
                    onComplete?.Invoke();
                }
            }

            void OnDeadEnd()
            {
                _combatServiceRequestHandler.OnPlayerDead();
            }
        }

        #endregion

        public void Reborn()
        {
            _playerCharacter.Reborn();
        }

        public int GetComboRecord()
        {
            return _playerCharacter.GetComboRecord();
        }
    }
}
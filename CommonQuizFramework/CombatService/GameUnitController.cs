using System;
using System.Collections;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace CommonQuizFramework.CombatService
{
    public abstract class GameUnitController : MonoBehaviour, IGameUnit
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Animator animator;

        [Header("Sprite 원본이 좌측을 바라보고 있는 경우 체크")] [SerializeField]
        protected bool isFlippedSprite = false;

        [SerializeField] private int baseAttackDamage = 100;

        // 콤보 횟수에 따라 추가되는 데미지
        // - 데미지 공식: (기본 데미지) + (콤보 횟수 + 콤보 데미지)
        // - 예시 : 기본 데미지 100 + (콤보 3, 콤보 데미지 10) = 최종 데미지 130   
        [SerializeField] private int comboDamage = 0;

        [SerializeField] private int attackSFXID = 0;
        [SerializeField] protected int attackEffectID = 0;

        [Header("연속 공격 횟수에 따라 강력한 공격 사용")] [SerializeField]
        private bool hasSpecialAttack = false;

        [FormerlySerializedAs("specialAttackThreshold")] [SerializeField]
        private int specialAttackComboThreshold = 10;

        [SerializeField] private int specialAttackEffectID = 1;
        [SerializeField] private int specialAttackSFXID = 1;

        private bool _isInit = false;

        private Action _onAttackImpact;
        private Action _onBehaviorComplete;

        private int _comboCount = 0;
        private int _comboRecord = 0;

        private EffectPool _effectPool;
        private DamageFloaterPool _damageFloaterPool;

        protected Coroutine MoveCoroutine;

        #region Animation Parameter Key

        protected static readonly int SpecialAttackModeID = Animator.StringToHash("IsSpecialAttack");
        protected static readonly int ResetParameterID = Animator.StringToHash("Reset");
        protected static readonly int AttackParameterID = Animator.StringToHash("Attack");
        protected static readonly int HitParameterID = Animator.StringToHash("Hit");
        protected static readonly int DieParameterID = Animator.StringToHash("Die");
        protected static readonly int MoveSpeedParameterID = Animator.StringToHash("MoveSpeed");

        #endregion

        public void Initialization(EffectPool effectPool, DamageFloaterPool damageFloaterPool)
        {
            if (_isInit) return;

            if (animator == null) animator = GetComponentInChildren<Animator>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _effectPool = effectPool;
            _damageFloaterPool = damageFloaterPool;
            _isInit = true;
        }

        public virtual void Reset()
        {
            if (MoveCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(MoveCoroutine);
            }

            ResetCombo();
            _comboRecord = 0;
            transform.position = GetRestPosition();
            animator.SetTrigger(ResetParameterID);
        }

        public void MoveToBattlePosition(Action onComplete)
        {
            transform.position = GetRestPosition();

            var readyPosition = GetBattlePosition();
            var enterSpeed = GetMoveDuration();

            MoveTarget(readyPosition, enterSpeed);

            _onBehaviorComplete = onComplete;
        }

        public void MoveToRestPosition(Action onComplete)
        {
            var exitPosition = GetRestPosition();
            var exitSpeed = GetMoveDuration();

            MoveTarget(exitPosition, exitSpeed);

            _onBehaviorComplete = onComplete;
        }

        // 맵의 이동(Belt Scroll)에 따른 유닛의 동작
        // - 플레이어: 걷는 애니메이션 재생 /위치 이동 X
        // - 몬스터: 맵과 동일한 속도로 위치 이동 / 애니메이션 재생 X
        public abstract void MoveWithFloor();
        public abstract void OnStopFloor();

        public virtual void Attack(Action onAttackImpact, Action onAttackComplete)
        {
            ForceEndBehavior();

            _comboCount++;

            if (_comboRecord < _comboCount)
            {
                _comboRecord = _comboCount;
            }

            animator.SetBool(SpecialAttackModeID, IsSpecialAttack());
            animator.SetTrigger(AttackParameterID);

            _onAttackImpact = onAttackImpact;
            _onBehaviorComplete = onAttackComplete;
        }

        public virtual void Hit()
        {
            ForceEndBehavior();
            ResetCombo();
            animator.SetTrigger(HitParameterID);
        }

        public virtual void Dead(Action onDeadComplete)
        {
            ForceEndBehavior();
            _onBehaviorComplete = onDeadComplete;
            animator.SetTrigger(DieParameterID);
        }

        public virtual void Reborn()
        {
            animator.SetTrigger(ResetParameterID);
        }

        public int GetComboRecord()
        {
            return _comboRecord;
        }

        public void ResetCombo()
        {
            _comboCount = 0;
        }

        #region Unity Animation Event Handler

        public virtual void OnAttackImpact()
        {
            if (IsSpecialAttack())
            {
                _effectPool.SpawnToPool(specialAttackEffectID, GetTargetPosition());
                SoundManager.Instance.PlaySFX(SFXType.AttackSound, specialAttackSFXID);
            }
            else
            {
                _effectPool.SpawnToPool(attackEffectID, GetTargetPosition());
                SoundManager.Instance.PlaySFX(SFXType.AttackSound, attackSFXID);
            }

            _damageFloaterPool.SpawnToPool(AttackDamage(), GetTargetPosition(), _comboCount);
            _onAttackImpact?.Invoke();

            int AttackDamage()
            {
                return baseAttackDamage + _comboCount * comboDamage;
            }
        }

        public virtual void OnAttackComplete()
        {
            _onBehaviorComplete?.Invoke();
            _onBehaviorComplete = null;
        }

        public virtual void OnDeadComplete()
        {
            _onBehaviorComplete?.Invoke();
            _onBehaviorComplete = null;
        }

        public void Release()
        {
            if (MoveCoroutine != null)
            {
                CoroutineManager.Instance.CoroutineStop(MoveCoroutine);
            }

            Destroy(gameObject);
        }

        #endregion

        protected abstract Vector2 GetRestPosition();
        protected abstract Vector2 GetBattlePosition();
        protected abstract float GetMoveDuration();
        protected abstract Vector2 GetTargetPosition();

        private void ForceEndBehavior()
        {
            if (MoveCoroutine == null) return;

            CoroutineManager.Instance.CoroutineStop(MoveCoroutine);
            _onBehaviorComplete?.Invoke();
            _onBehaviorComplete = null;
        }

        private void MoveTarget(Vector2 targetPosition, float moveSpeed)
        {
            ForceEndBehavior();
            MoveCoroutine = CoroutineManager.Instance.CoroutineStart(Co_MoveTarget(targetPosition, moveSpeed));
        }

        private IEnumerator Co_MoveTarget(Vector2 targetPosition, float moveDuration)
        {
            Vector2 startPosition = transform.position;
            var moveTime = 0f;
            var moveSpeed = 1 / moveDuration;

            SetSpriteDirection(targetPosition);
            SetAnimationSpeed(moveSpeed);

            while (1 > moveTime)
            {
                moveTime += Time.deltaTime * moveSpeed;
                transform.position = Vector2.Lerp(startPosition, targetPosition, moveTime);
                yield return null;
            }

            animator.SetFloat(MoveSpeedParameterID, 0);
            _onBehaviorComplete?.Invoke();
            _onBehaviorComplete = null;

            void SetAnimationSpeed(float speed)
            {
                animator.SetFloat(MoveSpeedParameterID, speed);
            }
        }

        protected void SetSpriteDirection(Vector2 targetPosition)
        {
            if (isFlippedSprite)
            {
                spriteRenderer.flipX = targetPosition.x > transform.position.x;
            }
            else
            {
                spriteRenderer.flipX = targetPosition.x < transform.position.x;
            }
        }

        private bool IsSpecialAttack()
        {
            return specialAttackComboThreshold <= _comboCount && hasSpecialAttack;
        }

        private void OnDestroy()
        {
            if (MoveCoroutine != null)
            {
                try
                {
                    CoroutineManager.Instance.CoroutineStop(MoveCoroutine);
                }
                catch (Exception e)
                {
                    LHELogger.Log($"NTT is removed by stop the app {e.Message}");
                }
            }
        }
    }
}
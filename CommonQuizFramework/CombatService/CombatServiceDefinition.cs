using UnityEngine;

namespace CommonQuizFramework.CombatService
{
    public static class CombatServiceDefinition
    {
        public static readonly Vector2 CharacterRestPosition = new(-5.25f, 1.93f);
        public static readonly Vector2 CharacterBattlePosition = new(-1.25f, 1.93f);

        public const float CharacterMoveDuration = 1.4f;

        public static readonly Vector2 MonsterRestPosition = new(6.245f, 1.93f);
        public static readonly Vector2 MonsterBattlePosition = new(1.33f, 1.93f);

        public const float MonsterMoveDuration = 0.5f;

        public const float MapMoveDuration = 3;
        public const float MapMoveDistance = 4.915f;
        public static readonly Vector2 MapPosition = new(0, 3.06f);

        public const float FloaterRandomRangeX = 0.25f;
        public const float FloaterRandomRangeTop = 0.35f;
        public const float FloaterRandomRangeBottom = 0.15f;
    }

    public enum BattleStep
    {
        Minion,
        MiniBoss,
        Boss,
        Clear
    }
}
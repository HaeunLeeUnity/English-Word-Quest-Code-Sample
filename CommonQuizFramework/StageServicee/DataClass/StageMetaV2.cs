using System;
using System.Collections.Generic;
using CommonQuizFramework.CommonClass;
using UnityEngine.Scripting;

namespace CommonQuizFramework.StageService
{
    [Preserve]
    public class StageMetaV2
    {
        public PartOfSpeech PartOfSpeech;
        public List<LevelMetaV2> Levels;

        public StageMetaV2()
        {
        }

        public StageMetaV2(StageMetaV1 stageMetaV1)
        {
            PartOfSpeech = stageMetaV1.PartOfSpeech;
            Levels = new List<LevelMetaV2>();

            foreach (var level in stageMetaV1.Levels)
            {
                Levels.Add(new LevelMetaV2(level));
            }
        }
    }

    [Preserve]
    public class LevelMetaV2
    {
        public Difficulty Difficulty = Difficulty.Easy;
        public string Filename = "Quiz1";
        public int MajorID;
        public int MinorID;
        public AssetIDs AssetIDs;

        public LevelMetaV2()
        {
        }

        public LevelMetaV2(LevelMetaV1 levelMetaV1)
        {
            Difficulty = levelMetaV1.Difficulty;
            Filename = levelMetaV1.Filename;
            MajorID = levelMetaV1.MajorID;
            MinorID = levelMetaV1.MinorID;
            AssetIDs = new AssetIDs();
        }
    }

    [Preserve]
    public class AssetIDs
    {
        public int MapID = 0;
        public int CharacterID = 0;
        public int[] MonsterIDs = { 0, 0, 0 };
        public int BGMID = 0;
    }

    [Preserve]
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        VeryHard
    }

    #region Legacy

// [Obsolete("StageMetaV2 를 사용해야한다.")]
    [Preserve]
    public class StageMetaV1
    {
        public PartOfSpeech PartOfSpeech;
        public List<LevelMetaV1> Levels;
    }

    // [Obsolete("LevelMetaV2 를 사용해야한다.")]
    [Preserve]
    public class LevelMetaV1
    {
        public Difficulty Difficulty = Difficulty.Easy;
        public bool IsCleared = false;
        public int BestRecord = 0;
        public string Filename = "Quiz1";
        public int MajorID;
        public int MinorID;
    }

    #endregion
}
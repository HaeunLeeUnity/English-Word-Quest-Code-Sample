using System;
using System.Collections.Generic;
using System.IO;
using CommonQuizFramework.CommonClass;
using Newtonsoft.Json;
using UnityEngine;

namespace CommonQuizFramework.StageService
{
    public class StageJsonWriter : MonoBehaviour
    {
        private Dictionary<PartOfSpeech, StageMetaV2> _stages;

        private void Awake()
        {
            // _stages = new Dictionary<PartOfSpeech, Stage>();
            //
            // var newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Noun,
            //     Levels = new List<Level>()
            // };
            //
            // var newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Verbs,
            //     Levels = new List<Level>()
            // };
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Adjectives,
            //     Levels = new List<Level>()
            // };
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Adverbs,
            //     Levels = new List<Level>()
            // };
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Prepositions,
            //     Levels = new List<Level>()
            // };
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Pronouns,
            //     Levels = new List<Level>()
            // };
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // newStage = new Stage
            // {
            //     PartOfSpeech = PartOfSpeech.Conjunctions,
            //     Levels = new List<Level>()
            // };
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Easy,
            //     Filename = "TestCase.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // newLevel = new Level
            // {
            //     Difficulty = Difficulty.Normal,
            //     Filename = "TestCase2.json"
            // };
            //
            // newStage.Levels.Add(newLevel);
            //
            // _stages.Add(newStage.PartOfSpeech, newStage);
            //
            // var stageDataJson = JsonConvert.SerializeObject(_stages);
            //
            // var filePath = Application.dataPath + "/StreamingAssets/Stages.json";
            // File.WriteAllText(filePath, stageDataJson);
        }
    }
}
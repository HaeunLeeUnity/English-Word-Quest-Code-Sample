using System.Collections.Generic;
using System.IO;
using CommonQuizFramework.CommonClass;
using LHEPackage.Helper;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace CommonQuizFramework.StageService
{
#if UNITY_EDITOR
    public class StageMetaJsonUpdater : EditorWindow
    {
        private const string StagesV2FileName = "StageV2Metas.json";

        [MenuItem("QuizGame/StageMeta Json Update")]
        public static void UpdateStageJson()
        {
            LHELogger.Log("Update Stage Json Start");

            var oldStageJson = LoadFile();
            LHELogger.Log("Old File Load Success");

            var oldStages = JsonConvert.DeserializeObject<Dictionary<PartOfSpeech, StageMetaV1>>(oldStageJson);
            LHELogger.Log("Old Stage Json Deserialize Success");

            var newStages = ConvertStageV2(oldStages);
            LHELogger.Log("Convert Success");

            var newStagesJson = JsonConvert.SerializeObject(newStages, Formatting.Indented);
            LHELogger.Log("StageMetaV2 Serialize Success");

            SaveFile(newStagesJson);
            LHELogger.Log("New File Save Success");

            LHELogger.Log("<color=green> StageMeta Update Success");
        }

        private static string LoadFile()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, StageServiceDefinition.StagesJsonFileName);
            LHELogger.Log($"Try Old StageMeta File Load To {filePath}");
            return File.ReadAllText(filePath);
        }

        private static void SaveFile(string json)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, StagesV2FileName);
            LHELogger.Log($"Try StageMetaV2 File Save To {filePath}");
            File.WriteAllText(filePath, json);
        }

        private static Dictionary<PartOfSpeech, StageMetaV2> ConvertStageV2(
            Dictionary<PartOfSpeech, StageMetaV1> oldStages)
        {
            LHELogger.Log($"Try Convert Old StageMeta To StagesMetaV2");

            var stageV2Metas = new Dictionary<PartOfSpeech, StageMetaV2>();

            foreach (var oldStage in oldStages)
            {
                stageV2Metas.Add(oldStage.Key, new StageMetaV2(oldStage.Value));
            }

            return stageV2Metas;
        }
    }
#endif
}
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.StageService;
using LHEPackage.Helper;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class LoadStageMeta : SequenceElement
    {
        private FileLoader _fileLoader;
        private StageServiceProvider _stageServiceProvider;

        public LoadStageMeta(FileLoader fileLoader, StageServiceProvider stageServiceProvider)
        {
            _fileLoader = fileLoader;
            _stageServiceProvider = stageServiceProvider;
        }

        public override void Execute()
        {
            _fileLoader.LoadFileInStreamingAssets(StageServiceDefinition.StagesJsonFileName, OnCompleteLoadStages,
                OnLoadFailed, OnProgress);

            void OnCompleteLoadStages(string json)
            {
                _stageServiceProvider.SetStageData(json);
                OnComplete();
            }

            void OnLoadFailed()
            {
                LHELogger.Assert(true, "Failed to Load Stages File Load");
            }
        }
    }
}
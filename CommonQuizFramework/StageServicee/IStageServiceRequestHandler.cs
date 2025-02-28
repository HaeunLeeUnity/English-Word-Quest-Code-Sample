using CommonQuizFramework.CommonClass;

namespace CommonQuizFramework.StageService
{
    public interface IStageServiceRequestHandler
    {
        public void RequestEnterStage();
        public int[] GetLevelClearRatings(PartOfSpeech partOfSpeech);
    }
}
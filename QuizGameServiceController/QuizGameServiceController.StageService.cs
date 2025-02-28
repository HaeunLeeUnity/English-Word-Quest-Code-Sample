using CommonQuizFramework.CommonClass;
using CommonQuizFramework.StageService;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController : IStageServiceRequestHandler
    {
        private void RegisterStageUIController()
        {
            _stageServiceProvider.RegisterLobbyUIController(FindAnyObjectByType<LobbyUIController>());
        }

        public int[] GetLevelClearRatings(PartOfSpeech partOfSpeech)
        {
            return _userDataServiceProvider.GetStageClearRatings(partOfSpeech);
        }
    }
}
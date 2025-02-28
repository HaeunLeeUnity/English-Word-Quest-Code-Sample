using CommonQuizFramework.CommonClass;
using CommonQuizFramework.StageService;

namespace CommonQuizFramework.UserDataService
{
    public class UserDataServiceProvider
    {
        private IUserDataServiceRequestHandler _userDataServiceRequestHandler;
        private StageClearDataController _stageClearDataController;

        public UserDataServiceProvider(IUserDataServiceRequestHandler userDataServiceRequestHandler)
        {
            _userDataServiceRequestHandler = userDataServiceRequestHandler;
            _stageClearDataController = new StageClearDataController(this);
        }

        public void SetStageClearData(string stageClearJson)
        {
            _stageClearDataController.SetStageClearData(stageClearJson);
        }

        public void CreateClearData()
        {
            _stageClearDataController.CreateClearData();
        }

        public int[] GetStageClearRatings(PartOfSpeech partOfSpeech)
        {
            return _stageClearDataController.GetStageClearRatings(partOfSpeech);
        }

        public StageMetaV2[] GetStages()
        {
            return _userDataServiceRequestHandler.GetStages();
        }

        public void SetClearRating(SelectedLevel selectedLevel, int clearRating)
        {
            _stageClearDataController.SetClearRating(selectedLevel, clearRating);
        }
    }
}
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.StageService;
using Newtonsoft.Json;

namespace CommonQuizFramework.UserDataService
{
    public class StageClearDataController
    {
        private StageClearData _stageClearData;
        private UserDataServiceProvider _userDataServiceProvider;

        public StageClearDataController(UserDataServiceProvider userDataServiceProvider)
        {
            _userDataServiceProvider = userDataServiceProvider;
        }

        public void SetStageClearData(string json)
        {
            _stageClearData = JsonConvert.DeserializeObject<StageClearData>(json);
        }

        public void CreateClearData()
        {
            var stages = _userDataServiceProvider.GetStages();
            _stageClearData = new StageClearData();
            _stageClearData.Initialization(stages);
            SaveData();
        }

        private void SaveData()
        {
            var stageClearDataJson = JsonConvert.SerializeObject(_stageClearData);
            FileSaver.SaveFileInPersistentDataPath(UserDataServiceDifinition.StageClearDataFileName,
                stageClearDataJson);
        }

        public int[] GetStageClearRatings(PartOfSpeech partOfSpeech)
        {
            return _stageClearData.GetClearRatings(partOfSpeech);
        }

        public void SetClearRating(SelectedLevel selectedLevel, int clearRating)
        {
            _stageClearData.SetClearRating(selectedLevel, clearRating);
            SaveData();
        }
    }
}
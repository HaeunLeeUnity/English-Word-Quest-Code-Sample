using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.UserDataService;

namespace CommonQuizFramework.QuizGameServiceController
{
    // 클리어 데이터 로드 후 적용 (없을 시 신규 생성)
    public class LoadClearData : SequenceElement
    {
        private FileLoader _fileLoader;
        private UserDataServiceProvider _userDataServiceProvider;

        public LoadClearData(FileLoader fileLoader, UserDataServiceProvider userDataServiceProvider)
        {
            _fileLoader = fileLoader;
            _userDataServiceProvider = userDataServiceProvider;
        }

        public override void Execute()
        {
            _fileLoader.LoadFileInPersistentDataPath(UserDataServiceDifinition.StageClearDataFileName, OnSuccess,
                OnFailed, OnProgress);

            void OnSuccess(string json)
            {
                _userDataServiceProvider.SetStageClearData(json);
                OnComplete();
            }

            void OnFailed()
            {
                _userDataServiceProvider.CreateClearData();
                OnComplete();
            }
        }
    }
}
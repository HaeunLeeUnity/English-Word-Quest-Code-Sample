using System;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;

namespace CommonQuizFramework.QuizGameServiceController
{
    // 설정 파일 로드 후 적용 (없을 시 신규 생성)
    public class LoadSettingsData : SequenceElement
    {
        private SettingController _settingController;
        private FileLoader _fileLoader;

        public LoadSettingsData(SettingController settingController, FileLoader fileLoader)
        {
            _settingController = settingController;
            _fileLoader = fileLoader;
        }

        public override void Execute()
        {
            _fileLoader.LoadFileInPersistentDataPath(CommonDefinition.SettingsFileName, OnSuccess,
                OnFailed, OnProgress);


            void OnSuccess(string json)
            {
                _settingController.SettingsJsonData = json;
                OnComplete();
            }

            void OnFailed()
            {
                _settingController.CreateSettings();
                OnComplete();
            }
        }
    }
}
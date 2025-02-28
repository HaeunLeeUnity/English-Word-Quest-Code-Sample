using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using CommonQuizFramework.QuizService;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class LoadQuizList : SequenceElement
    {
        private FileLoader _fileLoader;
        private string _filename;
        private QuizServiceController _quizServiceController;

        public LoadQuizList(FileLoader fileLoader, string filename, QuizServiceController quizServiceController)
        {
            _fileLoader = fileLoader;
            _filename = filename;
            _quizServiceController = quizServiceController;
        }

        public override void Execute()
        {
            _fileLoader.LoadFileInStreamingAssets(_filename, OnCompleteFileLoad);

            void OnCompleteFileLoad(string json)
            {
                _quizServiceController.SetQuizData(json);
                OnComplete();
            }
        }
    }
}
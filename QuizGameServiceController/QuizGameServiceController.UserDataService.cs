using System;
using CommonQuizFramework.QuizService;
using CommonQuizFramework.StageService;
using CommonQuizFramework.UserDataService;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController : IUserDataServiceRequestHandler
    {
        public StageMetaV2[] GetStages()
        {
            return _stageServiceProvider.GetStages();
        }
    }
}
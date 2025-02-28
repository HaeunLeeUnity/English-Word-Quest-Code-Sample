using CommonQuizFramework.StageService;

namespace CommonQuizFramework.UserDataService
{
    public interface IUserDataServiceRequestHandler
    {
        public StageMetaV2[] GetStages();
    }
}
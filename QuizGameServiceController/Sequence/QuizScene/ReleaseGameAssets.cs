using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class ReleaseGameAssets : SequenceElement
    {
        private CombatServiceController _combatServiceController;

        public ReleaseGameAssets(CombatServiceController combatServiceController)
        {
            _combatServiceController = combatServiceController;
        }

        public override void Execute()
        {
            _combatServiceController.ReleaseGameAssets();
            OnComplete();
        }
    }
}
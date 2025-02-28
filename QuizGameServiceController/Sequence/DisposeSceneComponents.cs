using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class DisposeSceneComponents : SequenceElement
    {
        private ISceneComponentAdapter[] _sceneComponentAdapters;

        public DisposeSceneComponents(ISceneComponentAdapter[] sceneComponentAdapters)
        {
            _sceneComponentAdapters = sceneComponentAdapters;
        }

        public override void Execute()
        {
            foreach (var sceneComponentAdapter in _sceneComponentAdapters)
            {
                sceneComponentAdapter.DisposeSceneComponent();
            }

            OnComplete();
        }
    }
}
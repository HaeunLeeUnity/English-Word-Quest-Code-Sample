using LHEPackage.Helper;

namespace CommonQuizFramework.CommonClass.Sequence
{
    public abstract class SequenceElement
    {
        private ISequenceQueue _sequenceQueue;

        public void RegisterSequenceQueue(ISequenceQueue sequenceQueue)
        {
            _sequenceQueue = sequenceQueue;
        }

        public abstract void Execute();

        protected void OnProgress(float progress)
        {
            _sequenceQueue.OnSequenceProgress(progress);
        }

        protected void OnComplete()
        {
            LHELogger.Log($"Sequence {GetType().Name} is done");
            _sequenceQueue.OnSequenceComplete();
        }
    }
}
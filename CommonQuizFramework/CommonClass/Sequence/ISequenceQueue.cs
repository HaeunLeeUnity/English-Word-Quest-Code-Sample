using System;

namespace CommonQuizFramework.CommonClass.Sequence
{
    public interface ISequenceQueue
    {
        public void StartSequence(LoadIndicator loadIndicator, Action onComplete = null);
        public void OnSequenceComplete();
        public void Enqueue(SequenceElement sequenceElement);
        public void OnSequenceProgress(float progress);
    }
}
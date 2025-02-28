using System;
using System.Threading.Tasks;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class CleanupMemory : SequenceElement
    {
        public override void Execute()
        {
            CleanupAsync(OnComplete);
        }

        private async Task CleanupAsync(Action onComplete)
        {
            await Resources.UnloadUnusedAssets();
            OnProgress(0.5f);
            GC.Collect();
            onComplete?.Invoke();
        }
    }
}
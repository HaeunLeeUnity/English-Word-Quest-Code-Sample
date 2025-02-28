using System;
using System.Collections.Generic;
using LHEPackage.Helper;

namespace CommonQuizFramework.CommonClass.Sequence
{
    public class SequenceQueue : ISequenceQueue
    {
        // 로딩 화면이 표시되고 실제 시퀀스가 시작될 때 까지의 시간
        // - Fade-In 이 완료되기 전 시퀀스가 시작되는 것을 막기 위해 설정
        // - 예를들어 Fade-In 진행 중 Asset 정리 작업이 진행되면 어색한 화면이 노출될 수 있다. 
        private const float StartDelay = 0.083f;

        private Queue<SequenceElement> _sequenceQueue = new();
        private LoadIndicator _loadIndicator;

        private int _initialSequenceCount;
        private int _processedSequenceCount;
        private float _progressByStep;
        private Action _onComplete;

        public void Enqueue(SequenceElement sequenceElement)
        {
            sequenceElement.RegisterSequenceQueue(this);
            _sequenceQueue.Enqueue(sequenceElement);
        }

        public void StartSequence(LoadIndicator loadIndicator, Action onComplete = null)
        {
            _loadIndicator = loadIndicator;
            _onComplete = onComplete;

            _processedSequenceCount = 0;
            _initialSequenceCount = _sequenceQueue.Count;
            _progressByStep = 1f / _initialSequenceCount;
            _loadIndicator.ShowIndicator(0f);

            CoroutineManager.Instance.ReserveCall(StartDelay, NextSequence);
        }

        private void NextSequence()
        {
            if (_sequenceQueue.Count <= 0)
            {
                LHELogger.Log("<color=green>Sequence Complete</color>");
                _loadIndicator.HideIndicator();
                _onComplete?.Invoke();
                return;
            }

            var nextSequence = _sequenceQueue.Dequeue();
            LHELogger.Log($"Sequence {nextSequence.GetType().Name} start");
            nextSequence?.Execute();
        }

        public void OnSequenceProgress(float progress)
        {
            _loadIndicator.ShowIndicator(_processedSequenceCount * _progressByStep + progress * _progressByStep);
            LHELogger.Log(
                $"Sequence progress: {(_processedSequenceCount * _progressByStep + progress * _progressByStep) * 100} %");
        }

        public void OnSequenceComplete()
        {
            _processedSequenceCount++;
            _loadIndicator.ShowIndicator(_processedSequenceCount * _progressByStep);
            LHELogger.Log($"Sequence progress: {_processedSequenceCount * _progressByStep * 100} %");
            NextSequence();
        }
    }
}
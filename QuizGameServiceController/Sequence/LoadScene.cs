using System;
using System.Collections;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.CommonClass.Sequence;
using LHEPackage.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommonQuizFramework.QuizGameServiceController
{
    public class LoadScene : SequenceElement
    {
        private string _targetScene;

        // 씬 전환 이후 호출
        // 남은 시퀀스에서 Scene 에 존재하는 GameObject, Component 를 캐싱해야하는 경우 사용    
        private Action _onChangedScene;

        public LoadScene(SceneType sceneType, Action onChangedScene = null)
        {
            switch (sceneType)
            {
                case SceneType.Lobby:
                    _targetScene = "Lobby";
                    break;

                case SceneType.Stage:
                    _targetScene = "Stage";
                    break;
                default:
                    LHELogger.LogWarning($"정의되지 않은 Scene Type: {sceneType}");
                    break;
            }

            _onChangedScene = onChangedScene;
        }

        public override void Execute()
        {
            if (SceneManager.GetActiveScene().name == _targetScene)
            {
                OnComplete();
                return;
            }

            CoroutineManager.Instance.CoroutineStart(Co_MoveScene(_targetScene));
        }

        private IEnumerator Co_MoveScene(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                OnProgress(asyncLoad.progress * 1.1111f);
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;
            yield return null;
            _onChangedScene?.Invoke();
            OnComplete();
        }
    }

    public enum SceneType
    {
        Lobby,
        Stage
    }
}
using System;
using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.QuizService;
using JetBrains.Annotations;
using UnityEngine;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController : IQuizServiceRequestHandler
    {
        public void RequestEnterStage()
        {
            SoundManager.Instance.StopBGM();
            EnterStage();
        }

        private void RegisterQuizUIController()
        {
            _quizServiceController.RegisterUIController(FindAnyObjectByType<QuizUIController>());
        }

        public void ResetQuiz()
        {
            SoundManager.Instance.PlayBGM();

            _quizServiceController.Reset();
            _combatServiceController.Reset();
            _combatServiceController.ReadyToCharacter(OnCharacterReadyComplete);

            void OnCharacterReadyComplete()
            {
                _quizServiceController.QuizStart();
            }
        }

        public void OnChangeState(LearningStep learningStep, Action onComplete = null)
        {
            switch (learningStep)
            {
                case LearningStep.Filter:
                    _combatServiceController.ChangeStep(BattleStep.Minion, onComplete);
                    break;
                case LearningStep.Memorize:
                    _combatServiceController.ChangeStep(BattleStep.MiniBoss, onComplete);
                    break;
                case LearningStep.Test:
                    _combatServiceController.ChangeStep(BattleStep.Boss, onComplete);
                    break;
                case LearningStep.End:
                    _combatServiceController.ChangeStep(BattleStep.Clear, OnClearLevel);
                    break;
            }

            void OnClearLevel()
            {
                var selectedLevel = _stageServiceProvider.GetSelectedLevel();
                var remainChance = _quizServiceController.RemainChance;
                var clearRating = 0;

                foreach (var standard in QuizServiceDefinition.ResultStarStandards)
                {
                    if (standard <= remainChance)
                    {
                        clearRating++;
                    }
                }

                _userDataServiceProvider.SetClearRating(selectedLevel, clearRating);
                _quizServiceController.QuizOver();
            }
        }

        public void RequestEnterLobby()
        {
            SoundManager.Instance.StopBGM();
            EnterLobby();
        }

        public void OnAnswer(int? select, int answer, Action onComplete = null)
        {
            if (select == answer)
            {
                _combatServiceController?.PlayerAttack(onComplete);
            }
            else
            {
                _combatServiceController?.MonsterAttack(onComplete);
            }
        }

        public void OnPlayerDead()
        {
            _quizServiceController.QuizOver();
        }

        public void OnResume()
        {
            _combatServiceController.Reborn();
        }

        public int GetComboRecord()
        {
            return _combatServiceController.GetComboRecord();
        }

        public SelectedLevel GetSelectedLevel()
        {
            return _stageServiceProvider.GetSelectedLevel();
        }

        [CanBeNull]
        public SelectedLevel GetNextLevel()
        {
            return _stageServiceProvider.GetNextLevel();
        }

        public void RequestNextLevel()
        {
            _stageServiceProvider.SetNextLevel();
            LevelChange();
        }
    }
}
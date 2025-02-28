using System.IO;
using System.Collections.Generic;
using System.Linq;
using CommonQuizFramework.CommonClass;
using Newtonsoft.Json;
using UnityEngine;


namespace CommonQuizFramework.QuizService
{
    public class QuizContainer
    {
        private List<Quiz> _initialQuizList;
        private List<Quiz> _remainQuizList;
        private Quiz _currentQuiz;
        private HashSet<(string, string)> failedWords = new();

        public List<Quiz> RemainQuizList
        {
            get => _remainQuizList;
            set => _remainQuizList = value;
        }

        public (string, string)[] FailedQuizzes
        {
            get
            {
                var failedWordsArray = new (string, string)[10];
                var failedWordsArrayRaw = failedWords.ToArray();

                var targetLength = 10;
                if (failedWordsArrayRaw.Length < targetLength) targetLength = failedWordsArrayRaw.Length;

                for (var i = 0; i < targetLength; i++)
                {
                    failedWordsArray[i] = failedWordsArrayRaw[i];
                }

                return failedWordsArray;
            }
        }

        public void SetQuizData(string json)
        {
            _initialQuizList = JsonConvert.DeserializeObject<List<Quiz>>(json);
            _remainQuizList = _initialQuizList.ToList();
        }

        public void AddFailedWords(Quiz quiz)
        {
            failedWords.Add((quiz.Word, quiz.Mean));
        }

        public void ClearFailedWords()
        {
            failedWords.Clear();
        }

        public void Reset()
        {
            _remainQuizList = _initialQuizList.ToList();
            failedWords.Clear();
        }
    }
}
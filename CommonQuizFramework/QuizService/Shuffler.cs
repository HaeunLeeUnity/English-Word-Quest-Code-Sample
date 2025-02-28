using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonQuizFramework.QuizService
{
    // 문제 / 보기 배열을 섞기 기능을 제공하는 정적 클래스
    public static class Shuffler
    {
        public static Choice[] ShuffleExamples(Choice[] examples)
        {
            var rand = new Random();
            return examples.OrderBy(x => rand.Next()).ToArray();
        }
        
        public static List<Quiz> ShuffleQuizzes(List<Quiz> quizList)
        {
            foreach (var quiz in quizList)
            {
                quiz.Choices = ShuffleExamples(quiz.Choices);
            }
            
            var rand = new Random();
            return quizList.OrderBy(x => rand.Next()).ToList();
        }
    }
}
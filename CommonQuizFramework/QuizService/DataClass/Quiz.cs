namespace CommonQuizFramework.QuizService
{
    public class Quiz
    { 
        public string Word;
        public string Mean;

        public Choice[] Choices;
    }

    public class Choice
    {
        public bool Correct = false;
        public string Word = "NULL";
    } 
}
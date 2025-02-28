namespace CommonQuizFramework.QuizService
{
    public static class QuizServiceDefinition
    {
        public static int MaxChoiceCount = 4;
        public static int MaxChanceCount = 10;
        public static float MemorizeTestWaitTime = 3;
        public static string[] StepName = { "선별", "암기", "시험" };

        public static string[] StepDescription =
        {
            "알고있는 단어를 거르는 단계입니다.\n맞춘 단어는 다음 단계에서 출제되지 않습니다.",
            "단어와 뜻을 암기하는 단계입니다.\n준비완료 버튼을 누르면 문제가 출제됩니다.",
            "단어를 암기했는지 시험하는 단계입니다.\n틀린 문제가 있으면 외우기 단계를 다시 진행합니다."
        };

        public static int[] ResultStarStandards = { 0, 3, 7 };
    }

    public enum LearningStep
    {
        Filter,
        Memorize,
        Test,
        End
    }
}
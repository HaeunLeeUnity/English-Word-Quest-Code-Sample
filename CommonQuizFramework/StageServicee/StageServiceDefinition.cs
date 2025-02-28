using System.Linq;
using CommonQuizFramework.CommonClass;
using UnityEngine;

namespace CommonQuizFramework.StageService
{
    public static class StageServiceDefinition
    {
        public const string StagesJsonFileName = "StageMetaV2.json";

        public static readonly string[] PartOfSpeechDescription =
        {
            "사람, 사물, 장소, 개념 등을 가리키는 말입니다.\\n예시) Apple, Day, Time, Life, Cat\\n<font-weight=500>7-1 부터 시작하는 것을 추천합니다.</font-weight>",
            "동작이나 상태를 나타내는 말입니다.\\n예시) go, make, take, see, come, say, know\\n<font-weight=500>3-1 부터 시작하는 것을 추천합니다.</font-weight>",
            "명사를 수식하여 상태나 성질 등을 나타내는 말입니다.\n예시) Big, Happy, Fast, Easy, Hot 등\n<font-weight=500>2-1 부터 시작하는 것을 추천합니다.</font-weight>",
            "동사, 형용사, 다른 부사, 문장 전체 등을 수식하는 말입니다.\n예시) Very, Really, Always, Never, Slowly\n<font-weight=500>1-1 부터 시작하는 것을 추천합니다.</font-weight>"
        };

        public static Vector2 MapPosition = new(0, 2.45f);
        public static Vector2 RunnerPosition = new(0, 1.5f);

        public const float RunnerAccelerationThreshold = 1.55f;
        public const float RunnerDecelerationThreshold = -1.55f;

        public const float MapSpeed = -1f;
    }

    public class StageProgress
    {
        public StageProgress(PartOfSpeech partOfSpeech, int[] clearRatings)
        {
            var clearedLevel = 0f;

            foreach (var rating in clearRatings)
            {
                if (0 < rating)
                {
                    clearedLevel++;
                }
            }


            PartOfSpeech = partOfSpeech;
            Progress = clearedLevel / clearRatings.Length;
        }

        public PartOfSpeech PartOfSpeech;
        public float Progress;
    }
}
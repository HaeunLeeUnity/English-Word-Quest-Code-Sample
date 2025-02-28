namespace CommonQuizFramework.CommonClass
{
    public class CommonDefinition
    {
        public const string SettingsFileName = "Settings.json";
        public static readonly string[] PartOfSpeechKorean = { "명사", "동사", "형용사", "부사", "NULL" };
    }

    public enum PartOfSpeech
    {
        Noun = 0,
        Verbs = 1,
        Adjectives = 2,
        Adverbs = 3,
        None
    }
}
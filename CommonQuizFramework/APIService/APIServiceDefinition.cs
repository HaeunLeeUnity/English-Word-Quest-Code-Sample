using Firebase.Firestore;

namespace CommonQuizFramework.APIService
{
    public class APIServiceDefinition
    {
    }

    [FirestoreData]
    public class AppConfig
    {
        [FirestoreProperty] public string AOSStoreURL { get; set; }
        [FirestoreProperty] public string iOSStoreURL { get; set; }
        [FirestoreProperty] public string MinimumVersion { get; set; }
        [FirestoreProperty] public string LatestVersion { get; set; }
        [FirestoreProperty] public bool MaintenanceMode { get; set; }
    }

    public enum APIError
    {
        FAILED,
        NOELEMENT,
        NODOCUMENT,
        CANCELED,
        DUPLICATION,
        UNDEFINEDUID
    }
}
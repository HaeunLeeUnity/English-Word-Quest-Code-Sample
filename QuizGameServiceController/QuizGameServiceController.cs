using CommonQuizFramework.CombatService;
using CommonQuizFramework.CommonClass;
using CommonQuizFramework.QuizService;
using CommonQuizFramework.StageService;
using CommonQuizFramework.UserDataService;
using CommonQuizFramework.APIService;
using CommonQuizFramework.CommonClass.Addressable;
using CommonQuizFramework.CommonClass.Sequence;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CommonQuizFramework.QuizGameServiceController
{
    public partial class QuizGameServiceController : MonoBehaviour
    {
        private static QuizGameServiceController _instance;

        private QuizServiceController _quizServiceController;
        private StageServiceProvider _stageServiceProvider;
        private UserDataServiceProvider _userDataServiceProvider;
        private CombatServiceController _combatServiceController;
        private APIServiceController _apiServiceController;

        private FileLoader _fileLoader;
        private LoadIndicator _loadIndicator;
        private SettingController _settingController;

        [SerializeField] private GameObject loadIndicatorPrefab;
        [SerializeField] private GameObject coroutineManagerPrefab;
        [SerializeField] private GameObject soundManagerPrefab;
        [SerializeField] private GameObject assetProviderPrefab;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            CreateSubClass();
            CreateEssentialPrefabs();

            Application.targetFrameRate = 180;
            VibrationProvider.Initialization();

            InitializationApp();
        }

        private void CreateSubClass()
        {
            _stageServiceProvider = new StageServiceProvider(this);
            _quizServiceController = new QuizServiceController(this);
            _userDataServiceProvider = new UserDataServiceProvider(this);
            _combatServiceController = new CombatServiceController(this);
            _apiServiceController = new APIServiceController();
            _settingController = new SettingController(ApplySettings);
            _fileLoader = new FileLoader();
        }

        private void CreateEssentialPrefabs()
        {
            var coroutineManagerInstance = Instantiate(coroutineManagerPrefab);
            coroutineManagerInstance.GetComponent<CoroutineManager>().Initialization();

            var soundManagerInstance = Instantiate(soundManagerPrefab);
            soundManagerInstance.GetComponent<SoundManager>().Initialization();

            var assetProviderInstance = Instantiate(assetProviderPrefab);
            assetProviderInstance.GetComponent<AssetProvider>().Initialization();

            // 로딩화면을 런타임 중 생성하면 앱 실행 후 1프레임동안 로딩화면이 표시되지 않는 현상이 발생한다.
            // 따라서 Scene 에 사전 배치된 Load Indicator 를 찾아 참조하고 없는 경우 생성하는 것으로 한다. 
            FindAnyObjectByType<LoadIndicator>(FindObjectsInactive.Include);

            if (_loadIndicator == null)
            {
                var loadIndicatorInstance = Instantiate(loadIndicatorPrefab);
                _loadIndicator = loadIndicatorInstance.GetComponent<LoadIndicator>();
            }

            _loadIndicator.Initialization();
        }

        #region Test Code

#if UNITY_EDITOR
        private void Update()
        {
            var keyboard = Keyboard.current;

            if (keyboard.digit1Key.isPressed)
            {
                Time.timeScale += 1;
            }

            if (keyboard.digit2Key.isPressed)
            {
                Time.timeScale = 1;
            }

            if (keyboard.digit5Key.isPressed)
            {
                ForceClear();
            }
        }

        [ContextMenu("Force Clear")]
        public void ForceClear()
        {
            _quizServiceController.ForceClear();
        }
#endif

        #endregion
    }
}
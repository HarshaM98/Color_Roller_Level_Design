using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Timers;
using System.IO;
using System;

//TODO: LOAD WHILE ON TUTORIAL
public class MPLController : MonoBehaviour
{
    /******************************** Global Variables & Constants ********************************/
    [Header("SDK Settings")]
    public bool debugMode;
    public bool isLandscape;
    public string sceneName;
    public Sprite backgroundImage;
    public bool isTutorialEnabled;
    public Sprite gameLogoTypeSprite;
    public List<Sprite> tutorialCards;
    public bool useAssetBundles;
    public List<string> assetBundleNames;

    [Space(50)]
    public GameObject mplSdkObject;
    public GameObject timerCanvas;
    public GameObject multiplayerGamesHandler;
    private bool isAsyncMode;
    public static string gameConfigJson { get; private set; }
    public AndroidJavaObject activityContext { get; private set; }
    public string assetBundlesDirectory { get; private set; }
    public MPLGameConfig gameConfig { get; private set; }
    public MPLGameConfig gameConfig1 { get; private set; }
    private string encryptedUserToken = "";
    
    public bool apiCallsRequired;
    string tutorialEntryPoint = "";
    public enum MPLTutorialType
    {
        Lobby,
        GameEndScreen
    }
    private MPLTutorialType tutorialType;
    private bool isGameAlreadyPlayed;
    private bool hasIntent;
    public Sprite diamondImage,tokenImage;
    public AndroidJavaObject intent;
    private string logFilePath = "", logFileDirectory="";
    private string CallbackUrl;
    public readonly string VERSION = "17";
    public readonly string BUILDVERSION = "3";
    [HideInInspector] public MPLHackController hackDetector;
    private AsyncOperation async;
    public MPLGameSessionEventProperties gameSessionEventProperties { get; private set; }
    public BattleRoomEventProperties battleRoomEventProperties { get; private set; }
    public BattleSessionEventProperties battleSessionEventProperties { get; private set; }
    public float timeSpent;
    public CanvasScaler canvasScaler;
    public GameObject smartFoxManager;
	 private bool hasBalanceFromInteractive;
    private double availableAmount;
    private double tokenBalance;
    
    
    //Event names 
    public readonly string GAME_STARTED = "Game Started";
    public readonly string GAME_PAUSED = "Game Paused";
    public readonly string GAME_RESUMED = "Game Resumed";
    public readonly string GAME_ENDED = "Game Ended";
    public readonly string USER_FRAUD_DETECTED = "User Fraud Detected";
    public static  string EVENT_BATTLE_CONFIRMED = "Battle Confirmed";
    public static string EVENT_BATTLE_STARTED = "Battle Started";
    public static string EVENT_BATTLE_ENDED = "Battle Ended";
    public static string EVENT_BATTLE_REMATCH_REQUESTED = "Battle Rematch Requested";
    public static string EVENT_BATTLE_REMATCH_RESPONDED = "Battle Rematch Responded";
    public static string EVENT_BATTLE_EXITED = "Battle Exited";
    public static string EVENT_BATTLE_POPUP_SHOWN = "Pop Up Shown";
    public static string EVENT_BATTLE_CONNECTION_LOST = "Connection Lost";
    public static string EVENT_BATTLE_BUTTON_CLICKED = "Button Clicked";
    public static string EVENT_BATTLE_MATCHING_STARTED = "Battle Matching Started";
    public static string EVENT_BATTLE_MATCHING_ENDED = "Battle Matching Ended";
    public static string EVENT_BATTLE_CREATION_STARTED = "Battle Creation Started";
    public static string EVENT_APP_SCREEN_VIEWED = "App Screen Viewed";
    public static string EVENT_BATTLE_MIC_TOGGLE = "Battle Mic Toggle";
    public static string EVENT_BATTLE_OPPONENT_MUTE_TOGGLE = "Battle Opponent Mute Toggle";
    public static string EVENT_PROFILE_FOLLOW = "Profile Followed";
    public static string EVENT_INGAME_COLLECTIBLES = "Virtual Good Changed";
    string arguments = "";
    private static bool created = false;
    private MPLCanvasController mplCanvasController;
    [SerializeField] GameObject deviceOrientationPopUp;
    bool shouldPopUpAppear;
    [SerializeField] GameObject magnificationDetectedPopup;
    [SerializeField] GameObject DeveloperOptionsDetectedPopup;
    [SerializeField] GameObject USBDebuggingDetectedPopup;
    bool isMagnificationDetected;
    public bool isDeveloperOptionsDetected;
    public bool isUSBDebuggingDetected;
    public bool isGameHaltedDueToFraudSettingsPopup;
    public delegate void MPLStartGameEvent();
    public event MPLStartGameEvent StartAsyncGame;
    public Dictionary<string, AssetBundle> loadedAssetBundles;
    public GameObject mPLEventSystem;
    private bool gameAssetBundlesLoaded;
    private static MPLController instance;
    public bool isItAnIndoBuild;
    private Timer gameLoadingTimer;
    public int gameLoadingTime { get; private set; }
    public int gameLoadingMinimizes { get; private set; }
    private bool pauseGameLoadingTimer, paused;
    public bool gameLoadingTimeCaluclated { get; private set; }

    public GameObject landscapeInteractive, portraitInteractive, interactiveContainer;
    public string GetCallBackURL()
    {
        return CallbackUrl;
    }

    public string GetEncryptedToken()
    {
        return encryptedUserToken;
    }
	public bool IsBalanceAvailable()
    {
        return hasBalanceFromInteractive;
    }
    public void SetIsBalanceAvailable(bool hasBalance)
    {
        hasBalanceFromInteractive= hasBalance;
    }
    public double GetAvailableBalance()
    {
        return availableAmount;
    }
    public void SetAvailableBalance(double availableBalance)
    {
        availableAmount= availableBalance;
    }
    public double GetAvailableTokenBalance()
    {
        return tokenBalance;
    }
    public void SetAvailableTokenBalance(double availableBalance)
    {
        tokenBalance = availableBalance;
    }
    public SessionResult GetCurrentGameResult(MPLGameEndReason.GameEndReasons reason)
    {
        if (Session.Instance.OnCreateSessionResult == null)
        {
            
            Debug.LogError("No Game is registered to the event... so returning empty result");
            return new SessionResult(MPLController.Instance.gameConfig.SessionId, 0, 0, 0, reason);
        }

        //if(action.)
        return Session.Instance.OnCreateSessionResult.Invoke(reason);
    }

    public void SendStartGameEvent()
    {
        StartAsyncGame?.Invoke();
    }
    private void OnDisable()
    {
        if (gameConfig.IsLogEnabled)
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }
    }
    public void SetTutorialEntryPoint(string entryPoint)
    {
        this.tutorialEntryPoint = entryPoint;
    }
    public string GetTutorialEntryPoint()
    {
        return tutorialEntryPoint;
    }
    
   
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        
        if((gameConfig!=null && !gameConfig.IsLogEnabled) || string.IsNullOrEmpty(logFilePath))
        {
            return;
        }
        if(type==LogType.Assert|| type == LogType.Warning)
        {
            return;
        }
        if(type == LogType.Log || type == LogType.Error)
        {
            writeToFile(DateTime.Now + ": " + logString);
        }
        else
        {
            writeToFile("");
            writeToFile("Oh No! An exception");
            writeToFile(DateTime.Now + ": Exception Log :" + logString);
            writeToFile(DateTime.Now + ": Exception Stack Trace :" + stackTrace);
            writeToFile("");

            
          
        }
       
    }
    public bool IsItIndo()
    {
        return isItAnIndoBuild;
    }

    void writeToFile(string message)
    {
        try
        {
              StreamWriter streamWriter = new StreamWriter(logFilePath, true);
             streamWriter.WriteLine(message);
             streamWriter.Close();


        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("cannot write into the file="+logFilePath);
        }
    }

    public LocalisationDetails localisationDetails;

    /************************************** Private Functions **************************************/

    void SetOrientation(bool setLandscape)
    {
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        if (setLandscape)
        {
           
            Screen.autorotateToLandscapeLeft = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.autorotateToPortrait = true;
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

     

    void Awake()
    {
        instance = this;
        SetOrientation(false);
        
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(deviceOrientationPopUp.transform.root.gameObject);
           // DontDestroyOnLoad(reporter);
            created = true;
        }
        DontDestroyOnLoad(mPLEventSystem);
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        LoadLocalizedText();
        Debug.Log("Unity: Build Version=" + BUILDVERSION);
    }
    void LoadLocalizedText()
    {
        var languageJson = Resources.Load<TextAsset>("Languages/Indonasian").text;
        Debug.Log("languageJson : " + languageJson);
        localisationDetails = JsonUtility.FromJson<LocalisationDetails>(languageJson);
        localisationDetails.SetDictionary();
    }
    public static MPLController Instance
    {
        get { if (instance == null) instance = FindObjectOfType<MPLController>(); return instance; }
    }

    public void SetTutorialType(MPLTutorialType tutorialType)
    {
        this.tutorialType = tutorialType;
    }
    public MPLTutorialType GetTutorialType()
    {
        return this.tutorialType;
    }
    public IEnumerator StartAsyncLoadedScene()
    {
        if (async == null)
        {
            Debug.Log("MPL: Can't load async scene, because async object is null.");
            yield break;
        }

        while(async.progress < 0.9f)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Async scene load progress = " + async.progress);
        }

        pauseGameLoadingTimer = false;

        //Already in the scene
        if (async.progress >= 1f)
        {
            Session.Instance.StartAddingGameplayDuration = true;
            Debug.Log("Restarting Session");

           
                Session.Instance.RestartSession();
            
        }
        //Start scene
        else
        {
            Debug.Log("MPL: Starting scene");
            Session.Instance.StartAddingGameplayDuration = true;
            
            async.allowSceneActivation = true;
            
        }
    }
 
  
    public bool IsUnityDeviceDebug()
    {
        return debugMode;
    }


    public void PrintExtraLog(string content)
    {
       if(gameConfig.IsAgencyBuild)
        {
            return;
        }
        Debug.Log(content);
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MPLBaseScene") return;
        Debug.Log("<color=blue>GAME SCENE LOADEDDDD</color>");
        StopGameLoadingTime();
        
    }

    public void SetBattleSessionId(string id) 
    {

        MPLController.Instance.gameConfig.SessionId = id;
        if (battleSessionEventProperties == null) {
            Debug.Log("battleSessionEventProperties null");
            return;
        }

        battleSessionEventProperties.GameSessionID = id;
        Debug.Log("Battle session id = " + id + " = " + battleSessionEventProperties.GameSessionID);
    }

    void Start()
    {

        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        mplCanvasController = GameObject.Find("Canvas").GetComponent<MPLCanvasController>();
        loadedAssetBundles = new Dictionary<string, AssetBundle>();
        hackDetector = GetComponent<MPLHackController>();
        gameAssetBundlesLoaded = false;
        gameLoadingTimer = new Timer(100);
        gameLoadingTimer.Elapsed += GameLoadingTimer_Elapsed;
        SceneManager.sceneLoaded += OnSceneLoaded;


        if (Application.platform == RuntimePlatform.Android)
        {

            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");



            }
            intent = activityContext.Call<AndroidJavaObject>("getIntent");

        }
       
        if (intent != null && !IsUnityDeviceDebug())
        {
            arguments = intent.Call<string>("getStringExtra", "json_extra");

            hasIntent = true;
        }
        else
        {

            hasIntent = false;
        }
        Debug.Log("arguments=" + arguments);
        if (string.IsNullOrEmpty(gameConfigJson))
        {
            if ((Application.isEditor || IsUnityDeviceDebug()))
            {
                TextAsset txt = (TextAsset)Resources.Load("Configurations/session_config", typeof(TextAsset));
                if (txt != null)
                {


                    MPLController.gameConfigJson = txt.text;
                    gameConfig = JsonUtility.FromJson<MPLGameConfig>(txt.text);


                }
                else
                {
                    Debug.Log("MPL: Incorrect path for session_config on ");
                }
                


            }
            else
            {
                MPLController.gameConfigJson = arguments;
                gameConfig = JsonUtility.FromJson<MPLGameConfig>(arguments);





            }


        }
        StartGame(gameConfig);

    }

	private void Update()
	{
        timeSpent += (Time.deltaTime/Time.timeScale);
      


        if (gameAssetBundlesLoaded)
        { 
            ScreenChangeDetection();

        }
        //  Screen.orientation=  Globals.GAME_ORIENTATIONS[gameConfig.GameId];

    }

    void ScreenChangeDetection()
    {
        if ((Screen.width > Screen.height && !isLandscape) ||
            (Screen.width<Screen.height && isLandscape))
        {
            shouldPopUpAppear = true;
        }
        else
        {
            shouldPopUpAppear = false;
        }
        deviceOrientationPopUp.SetActive(shouldPopUpAppear);
     }
    void DetectScreenMagnification()
    {
     
        isMagnificationDetected = false;
#if UNITY_ANDROID
        try
            {
                using (AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                    using (AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                    {
                        int val = clsSecure.CallStatic<int>("getInt", objResolver, "accessibility_display_magnification_enabled");
                    if(val!=1)
                        val = clsSecure.CallStatic<int>("getInt", objResolver, "accessibility_display_magnification_navbar_enabled");
                    //i = Settings.Secure.getInt(this.getContentResolver(), "accessibility_display_magnification_navbar_enabled");
                    isMagnificationDetected = (val != 0);
                    }
                }
            }
            catch (System.Exception) { }
#endif
       
        if(magnificationDetectedPopup.activeSelf != isMagnificationDetected)
            magnificationDetectedPopup.SetActive(isMagnificationDetected);

}

    void DetectDeveloperOptions()
    {
        isDeveloperOptionsDetected = false;
#if UNITY_ANDROID
        try
        {
            using (AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                using (AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                {
                    int val = clsSecure.CallStatic<int>("getInt", objResolver, "development_settings_enabled");
                    isDeveloperOptionsDetected = (val != 0);
                }
            }
        }
        catch (System.Exception) { }
#endif

        if (DeveloperOptionsDetectedPopup.activeSelf != isDeveloperOptionsDetected && !isMagnificationDetected)
            DeveloperOptionsDetectedPopup.SetActive(isDeveloperOptionsDetected);
    }

    void DetectUSBDebugging()
    {
        isUSBDebuggingDetected = false;
#if UNITY_ANDROID
        try
        {
            using (AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                using (AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure"))
                {
                    int val = clsSecure.CallStatic<int>("getInt", objResolver, "adb_enabled");
                    isUSBDebuggingDetected = (val != 0);
                }
            }
        }
        catch (System.Exception) { }
#endif
        if (USBDebuggingDetectedPopup.activeSelf != isUSBDebuggingDetected && !isMagnificationDetected && !isDeveloperOptionsDetected)
            USBDebuggingDetectedPopup.SetActive(isUSBDebuggingDetected);

    }


    void SaveGameId(int gameId)
    {
        string savedGameIds = "";
        if (PlayerPrefs.HasKey(Globals.SAVED_GAME_IDS)) savedGameIds = PlayerPrefs.GetString(Globals.SAVED_GAME_IDS);

        if (!string.IsNullOrEmpty(savedGameIds)) savedGameIds += ",";
        savedGameIds += gameId;

        PlayerPrefs.SetString(Globals.SAVED_GAME_IDS, savedGameIds);
        PlayerPrefs.Save();
    }

    public bool IsGameAlreadyPlayed(int gameId)
    {
        string savedGameIdsStr = "";
        if (PlayerPrefs.HasKey(Globals.SAVED_GAME_IDS)) savedGameIdsStr = PlayerPrefs.GetString(Globals.SAVED_GAME_IDS);
        if (!string.IsNullOrEmpty(savedGameIdsStr))
        {
            string[] savedGameIds = savedGameIdsStr.Split(',');
            for (int i = 0; i < savedGameIds.Length; i++)
            {
                if (int.Parse(savedGameIds[i]) == gameId) return true;
            }
        }

        SaveGameId(gameId);

        return false;
    }

    IEnumerator StartSinglePlayerGame()
    {

        yield return new WaitForSeconds(0.1f);
        Session.Instance.SetQuitReason(Session.MPLQuitReason.home_quit);

        LoadGameSceneFromAssetBundle(gameConfig.GameId);

        Session.Instance.StartAddingGameplayDuration = true;
    }

    void InitSession()
    {
        if(gameConfig.Is1v1)
        {
            smartFoxManager.SetActive(true);
        }
        if (Is1v1Game())
        {
            
            new Session1v1();
        }
        else
        {
            new Session();
        }
    }

   

    /************************************** Public Functions **************************************/
 
    
    string GetTimestamp(DateTime value)
    {
        return value.ToString("ddMMyyyyHHmmss");
    }

    public bool IsThirdPartyGame()
    {
        return apiCallsRequired;
    }
    public bool IsAsyncGame()
    {
        return isAsyncMode;
    }
    public void StartGame(MPLGameConfig mPLGameConfig)
    {
        //PlayerPrefs.DeleteAll();
        
        gameConfig = mPLGameConfig;
        
        
        ControlLogs(gameConfig.IsLogEnabled);
        //PrintLargeLog("MPl Controller game configz json", gameConfigJson, 1);
        SetPlayerProperties();

        Debug.Log("arguments="+arguments);
        CallbackUrl = string.IsNullOrEmpty(gameConfig.HostUrl) ? "https://dapi.mpl.live" : gameConfig.HostUrl;
        if (gameConfig.CountryCode!=null && gameConfig.CountryCode.ToUpper() == "ID")
        {
            isItAnIndoBuild = true;
        }


        
        
        
        SetOrientation(isLandscape);
        //PrintLargeLog("MPL: Game config", gameConfig.ToString(), 1);
        PrintExtraLog("MPL: Parsed Game Config=" + gameConfig.ToString());


        //Find asset bundles
        assetBundlesDirectory = Application.streamingAssetsPath + "/" + gameConfig.GameId + "/"; //Get from streaming assets

        if (gameConfig.FraudMagnificationCheckEnabled)
        {
            DetectScreenMagnification();
        }
        if (gameConfig.FraudDeveloperOptionEnabled)
        {
            if (gameConfig.FraudDeveloperOptionDisabledGameIds != null)
            {


                foreach (var v_gameid in gameConfig.FraudDeveloperOptionDisabledGameIds)
                {
                    if (v_gameid == gameConfig.GameId)
                    {
                        return;
                    }
                }
            }
            DetectDeveloperOptions();
            DetectUSBDebugging();
        }
        mplSdkObject.SetActive(true);
        



    }
    
    public void SetPlayerProperties()
    {
        if (MPLController.Instance.IsUnityDeviceDebug())
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, Globals.USER_IDS.Count - 1);
            int userId = Globals.USER_IDS[randomNumber];
            gameConfig.Profile.id = userId;
            gameConfig.AuthToken = Globals.USER_TO_AUTH[userId];
            gameConfig.Profile.displayName = "User" + userId;

        }
        string UserToken = MPLController.Instance.gameConfig.AuthToken;
        byte[] utf = System.Text.Encoding.UTF8.GetBytes(UserToken);
        encryptedUserToken = BitConverter.ToString(utf);

    }
	public void StartInteractiveFTUE()
    {
        
        SetTutorialEntryPoint("First Game Play Session");
        if(gameConfig.IsInteractiveTrainingLobby)
        {
            SetTutorialEntryPoint("Game View");
        }
        SetTutorialType(MPLTutorialType.Lobby);
        LoadInteractiveFTUE();
    }
    public void LoadInteractiveFTUE()
    {   
        if (isLandscape)
        {
            landscapeInteractive.SetActive(true);
        }
        else
        {
            portraitInteractive.SetActive(true);
        }
    }
    void InitTutorial()
    {
        isGameAlreadyPlayed = IsGameAlreadyPlayed(gameConfig.GameId);
        Debug.Log("isalreadyplayedmpl=" + isGameAlreadyPlayed);
		if(Is1v1Game())
        {
            DontDestroyOnLoad(smartFoxManager);
            DontDestroyOnLoad(mplSdkObject);
        }
        if (gameConfig.IsInteractiveTrainingEnabled && gameConfig.IsIntractiveFtueEnabled)
        {
            DontDestroyOnLoad(interactiveContainer);
        }

        if ((!isGameAlreadyPlayed || gameConfig.IsInteractiveTrainingLobby)&& isTutorialEnabled)
        {
           if (gameConfig.IsInteractiveTrainingEnabled && gameConfig.IsIntractiveFtueEnabled)
            {
                StartInteractiveFTUE();
            }
            else
            {
                mplCanvasController.ShowTutorial(gameConfig); //Show tutorial
            }
        }
        else
        {
            mplCanvasController.DontShowTutorial(gameConfig);
                
        }
       

        gameSessionEventProperties = new MPLGameSessionEventProperties(gameConfig.SessionId, gameConfig.TournamentId, gameConfig.TournamentName,gameConfig.IsFTUE, gameConfig.GameId, gameConfig.GameName, VERSION, gameConfig.GameConfigName, gameConfig.MobileNumber, gameConfig.ReactVersion, (int)gameConfig.EntryFee, gameConfig.EntryCurrency, gameConfig.Profile.tier,gameConfig.TournamentType);
        //Submit game started event
       
       // Time.timeScale = 0f;
       // StartCoroutine(SendGameLoaded());
    }

    

   
    private void RestartGame()
    {
        Debug.Log("MPL: Restarting game :: mplcontroller");


        if (smartFoxManager.activeSelf)
        {
            SmartFoxManager.Instance.ReadyToPlay();
        }

        
    }
    

    public void LoadGameSceneFromAssetBundle(int gameId)
    {

        Debug.Log("LoadGameSceneFromAssetBundle");

        StartGameLoadingTime();

        /*if (gameId == 2)
        {
             SceneManager.LoadScene(Globals.GAMES_SCENES[gameId]);
             return;
        }*/


        if (gameAssetBundlesLoaded)
        {
            if (IsThirdPartyGame())
            {
                LoadSceneSync(sceneName);
            }
			else if (Session.Instance.IsInteractiveTutorial())
            {
                StartCoroutine(LoadSceneAsync(sceneName, (loaded) =>
                {
                    Debug.Log("MPL: Loding for battle tutorial");
                    pauseGameLoadingTimer = true;

                }));
                
            }
            else
            {
                RestartGame();
            }
            return;
        }

        gameAssetBundlesLoaded = true;
        if (!useAssetBundles)
        {
            LoadSceneForGame(this.sceneName);
            return;
        }
        List<string> assetBundles =assetBundleNames;

        for (int i = 0; i < assetBundles.Count - 1; i++)
        {
            string assetBundleDir = assetBundlesDirectory + assetBundles[i];
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleDir);
            loadedAssetBundles.Add(assetBundleDir, assetBundle);
            Debug.Log("MPL: Loaded Asset Bundle: " + assetBundle);
        }

        var scenesAssetBundle = AssetBundle.LoadFromFile(assetBundlesDirectory + assetBundles[assetBundles.Count-1]);
        loadedAssetBundles.Add(assetBundlesDirectory + assetBundles[assetBundles.Count - 1], scenesAssetBundle);

        if (scenesAssetBundle == null)
        {
            Debug.Log("MPL: Scenes asset bundle is null");
            return;
        }
        Debug.Log("MPL: Loaded Scenes Asset Bundle: " + scenesAssetBundle);

        string sceneToLoad = sceneName;
        string[] scenes = scenesAssetBundle.GetAllScenePaths();
        for (int i = 0; i < scenes.Length; i++)
        {
            string[] splittedScenePath = scenes[i].Split('/');
            string[] splittedSceneName = splittedScenePath[splittedScenePath.Length - 1].Split('.');
            string sceneName = splittedSceneName[0];

            if (sceneName.Equals(sceneToLoad))
            {
                LoadSceneForGame(sceneName);

                break;
            }
        }
    }
    void LoadSceneForGame(string sceneName)
    {
        if (Is1v1Game() && !IsThirdPartyGame()&& !Session.Instance.IsInteractiveTutorial())
        {
            Debug.Log("MPL: Loading Scene for 1v1: " + sceneName);
            Debug.Log("MPL: Loading Scene for 1v1 Time=" + System.DateTime.Now.ToString());
            StartCoroutine(LoadSceneAsync(sceneName, (loaded) =>
            {
                Debug.Log("MPL: Async scene loaded, calling ready to play");
                pauseGameLoadingTimer = true;
                if (smartFoxManager.activeSelf)
                {
                    SmartFoxManager.Instance.ReadyToPlay();
                }
            }));
        }
		 else if(Session.Instance.IsInteractiveTutorial()) {
                    StartCoroutine(LoadSceneAsync(sceneName, (loaded) =>
                    {
                        Debug.Log("MPL: Loding for battle tutorial");
                        pauseGameLoadingTimer = true;
                       
                    }));
                }
        else
        {
            Debug.Log("MPL: Loading Scene for single player: " + sceneName);
            Debug.Log("MPL: Loading Scene Time=" + System.DateTime.Now.ToString());

            LoadSceneSync(sceneName);
           
        }
    }
    public void GoToFraudPolicyButtonClick()
    {
        
        Session.Instance.SetQuitReason(Session.MPLQuitReason.view_fraud_policy);
        Session.Instance.Quit();
       
    }
    public void ExitButtonClick()
    {

        Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
        Session.Instance.Quit();

    }

    public void StartGame()
    {

        
        if (gameConfig.FraudDeveloperOptionEnabled && (isUSBDebuggingDetected || isDeveloperOptionsDetected))
        {
            isGameHaltedDueToFraudSettingsPopup = true;
            return;
        }
        isGameHaltedDueToFraudSettingsPopup = false;
        
        //GameStarted = true;
        bool continueGame = true;
        if (gameConfig.IsRealityCheckEnabled)
        {
            timerCanvas.SetActive(true);
            continueGame = TimerCanvasController.Instance.continueGame;
        }

        
        if (continueGame)
        {

            if (Is1v1Game())
            {
                Start1v1Game();
            }
            else
            {
                StartCoroutine(StartSinglePlayerGame());
            }
        }
    }
      

   

    IEnumerator LoadSceneAsync(string sceneName, UnityAction<bool> callback)
    {
       
        async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("MPL: Loading " + sceneName + " async, progress = " + async.progress);
        }

        callback(true);
    }

    void LoadSceneSync(string sceneName)
    {
       
       SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        
       
    }

    bool Is1v1Game()
    {
        /* bool is1v1;
         if(IsThirdPartyGame())
         {
             is1v1= false;
         }
         else
         {
             is1v1=gameConfig.Is1v1;
         }
         return is1v1;*/
        return gameConfig.Is1v1;
    }

    public string GetSessionConfig()
    {
        return gameConfigJson;
    }
    public void SetSessionConfig(string config)
    {
        gameConfigJson = config;
    }

    public long GetRemainingTournamentTime()
    {
        return (gameConfig.TournamentEndTime - gameConfig.StartTime);
    }

    

	

    void ControlLogs(bool isLogsEnabled)
    {
        if (isLogsEnabled)
        {
            Application.logMessageReceivedThreaded += HandleLog;
            logFileDirectory = Path.Combine(Application.persistentDataPath, "UnityLogs");
            Directory.CreateDirectory(logFileDirectory);

            logFilePath = Path.Combine(logFileDirectory, "LogFileFor_" + gameConfig.GameId + "_" + GetTimestamp(DateTime.Now) + ".txt");


            Debug.Log("File Path=" + logFilePath);
        }
        Debug.unityLogger.logEnabled = isLogsEnabled;
        

    }
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Debug.Log("On APPLICATION FOCUS");
            if(gameConfig != null && gameConfig.FraudMagnificationCheckEnabled)
            {
            DetectScreenMagnification();
            }
            if (gameConfig!=null && gameConfig.FraudDeveloperOptionEnabled)
            {
                if (gameConfig.FraudDeveloperOptionDisabledGameIds != null)
                {


                    foreach (var v_gameid in gameConfig.FraudDeveloperOptionDisabledGameIds)
                    {
                        if (v_gameid == gameConfig.GameId)
                        {
                            return;
                        }
                    }
                }
                DetectDeveloperOptions();
                DetectUSBDebugging();
            }
        }
       
    }
    void OnApplicationPause(bool status)
    {
        if (status)
        {
            paused = true;
        }
        else 
        {
            paused = false;
            if (gameConfig != null && gameConfig.FraudMagnificationCheckEnabled)
            {
                DetectScreenMagnification();
            }
            if (gameConfig != null && gameConfig.FraudDeveloperOptionEnabled)
            {
                if (gameConfig.FraudDeveloperOptionDisabledGameIds != null)
                {


                    foreach (var v_gameid in gameConfig.FraudDeveloperOptionDisabledGameIds)
                    {
                        if (v_gameid == gameConfig.GameId)
                        {
                            return;
                        }
                    }
                }
                DetectDeveloperOptions();
                DetectUSBDebugging();
                if (isGameHaltedDueToFraudSettingsPopup)
                    StartGame();
            }
            //DetectScreenMagnificationButton();
        }
    }
    
    void Start1v1Game()
    {

        MPLSdkBridgeController.Instance.StartMultiplayerGame();
        
        
    }
    public void InitGame()
    {
        InitSession();

        InputHandler.mInstance.Init();
        isAsyncMode = gameConfig.IsAsyncGame;
        gameConfig.ICLostTitle = "Game ended due to Bad Internet Connection";
        gameConfig.ICLostMessage = "Please make sure that you have good internet connectivity before playing";
        Session.Instance.modeOfConnection = (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) ? "Data" : "WIFI";
        string tournamentType;
        if (gameConfig.TotalPlayers < 3)
        {
            tournamentType = "1v1";
        }
        else
        {
            tournamentType = "1vN";
        }
        Debug.Log("MPL: Starting 1v1 game");



        string tournamentID = gameConfig.LobbyId.ToString();

        battleRoomEventProperties = new BattleRoomEventProperties(tournamentID, gameConfig.IsFTUE, tournamentType, gameConfig.EntryFee.ToString(), gameConfig.EntryCurrency, gameConfig.TotalPlayers.ToString(), "1", gameConfig.GameId.ToString(), gameConfig.GameName, gameConfig.GameConfigName, gameConfig.CashEntryFee, gameConfig.TokenEntryFee, gameConfig.PrizeCurrency, gameConfig.WinnersEndRank, gameConfig.CashPrizeOffered, gameConfig.TokenPrizeOffered, gameConfig.TournamentStyle, gameConfig.BonusLimit.ToString());
        battleSessionEventProperties = new BattleSessionEventProperties(battleRoomEventProperties, gameConfig.SessionId, gameConfig.TournamentName, VERSION, gameConfig.MobileNumber, gameConfig.ReactVersion, gameConfig.Profile.tier);
        Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
        
        multiplayerGamesHandler.SetActive(true);
        InitTutorial();
    }





    void GameLoadingTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        if (pauseGameLoadingTimer) return;
        if (paused) return;

        gameLoadingTime += 100;
    }

    void StartGameLoadingTime()
    {
        if (gameLoadingTimer.Enabled)
        {
            Debug.Log("gameLoadingTimer already started");
            return;
        }

        Debug.Log("Start game loading");
        gameLoadingTime = 0;
        gameLoadingMinimizes = 0;
        gameLoadingTimeCaluclated = false;
        gameLoadingTimer.Start();
        
    }

    void StopGameLoadingTime()
    {
        Debug.Log("Stop game loading");
        gameLoadingTimer.Stop();
        gameLoadingTimeCaluclated = true;

        Debug.Log("finalGameLoadingTime" + gameLoadingTime);
    }

  
}

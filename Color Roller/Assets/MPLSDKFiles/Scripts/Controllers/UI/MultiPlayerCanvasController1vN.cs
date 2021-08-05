 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Threading;

using Sfs2X.Entities;

using System.IO;

public class MultiPlayerCanvasController1vN : MonoBehaviour
{
    public System.Timers.Timer gameStartTimer;
    

    public bool isGameStarted=false;
    public Text errorTitle, errorSubtitle;
    public InGame1vNController multiplayerInGameController;
    public bool gameSceneLoaded, onStartBattleCalled, connectionRetrying,started, onScoreSubmittedCalled, findingMatch;
    public MatchMakingController1vN matchMakingController1VN;
    public Image sponserSlim;
    public BattleEndController1vN battleEndController1VN;
    public GameObject backGroundImage,matchFailed, retryPanel,connectionLost;
    
    
    private static MultiPlayerCanvasController1vN instance;
    
    public bool isGameRunning=false;

    public Button errorPopupButton;
    public Text errorPopupButtonText;

    private MultiplayerGamesHandler multiplayerGamesHandler;

    public bool isItConnectionLostPopup;

    public enum MatchType
    {
        NONE = 0,
        NEW_MATCH = 1,
        DIFFERENT_MATCH = 2,
        REMATCH = 3
    }
    public static Dictionary<MatchType, string> MatchTypeNames = new Dictionary<MatchType, string>() { { MatchType.NEW_MATCH, "New Match" }, { MatchType.DIFFERENT_MATCH, "Different Match" }, { MatchType.REMATCH, "Re-Match" } };

    private bool created;
   

    bool scoreSubmitted = false;

    public static MultiPlayerCanvasController1vN Instance
    {
        get { return instance; }
    }
    public void SessionRestart()
    {
       
            SceneManager.LoadScene(MPLController.Instance.sceneName);
        
    }
    void OnDisable()
    {
        SmartFoxManager.Instance.OnStartBattle -= OnStartBattle;
        SmartFoxManager.Instance.OnScoreSubmitted -= OnScoreSubmitted;
        SmartFoxManager.Instance.MatchFound -= MatchFound;
        SmartFoxManager.Instance.OnFightAgainStateChange -= OnFightAgainStateChange;

        SmartFoxManager.Instance.OnServerError -= OnServerError;
        SmartFoxManager.Instance.ConnectionFailed -= ConnectionLost;
        SmartFoxManager.Instance.OnConnectRetrying -= OnConnectRetrying;
        SmartFoxManager.Instance.OnConnectionResumed -= OnConnectionResumed;
        SmartFoxManager.Instance.OnBattleFinish -= OnBattleFinish;
        SmartFoxManager.Instance.ConnectionLost -= ConnectionLost;
        SmartFoxManager.Instance.OnGameLoaded -= OnGameLoaded;
        SmartFoxManager.Instance.OnUserVarsUpdated -= OnUserVarsUpdate;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SmartFoxManager.Instance.OpponentMissedPing -= OpponentMissedPing;
        Session.Instance.Restart -= SessionRestart;
        gameStartTimer.Elapsed -= HandleGameStartTimer;
        Session.Instance.WentInBackGround -= EnableWentInBackgroundPopup;
    }
    void OnFightAgainStateChange(Dictionary<string, string> state)
    {

    }
    void OnUserVarsUpdate(User user, List<string> changed)
    {

        if (changed.Contains("updatedData") && changed.Contains("mplUserId"))
        {

            if ((int)user.GetVariable("mplUserId").Value != MPLController.Instance.gameConfig.Profile.id)
            {
                Debug.Log("Data recieved from userid=" + (int)user.GetVariable("mplUserId").Value + "data recieved=" + (string)user.GetVariable("updatedData").Value);
                Session.Instance.UpdatedData((string)user.GetVariable("updatedData").Value);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Session.Instance.Restart += SessionRestart;
        mPLSdkBridgeController = MPLSdkBridgeController.Instance;
        if (!MPLController.Instance.gameConfig.sponsorBattle)
        {
            if (sponserSlim != null)
                sponserSlim.gameObject.SetActive(false);
        }
        //isLocalMute = isRemoteMute = false;
        multiplayerGamesHandler = MultiplayerGamesHandler.Instance;
        Session.Instance.WentInBackGround += EnableWentInBackgroundPopup;

        //mInitialized = false;
    }
    private void Awake()
    {
        
        Application.targetFrameRate = 60;
        instance = this;

       
        if (!created)
        {
            created = true;
            DontDestroyOnLoad(this.gameObject);
        }

        errorTitle.text = MPLController.Instance.gameConfig.ICLostTitle;
        errorSubtitle.text = MPLController.Instance.gameConfig.ICLostMessage;
    }
    private MPLSdkBridgeController _mPLSdkBridgeController;
    public MPLSdkBridgeController mPLSdkBridgeController
    {
        get
        {
            if (_mPLSdkBridgeController == null)
            {
                _mPLSdkBridgeController = MPLSdkBridgeController.Instance;
            }
            return _mPLSdkBridgeController;
        }
        set
        {
            _mPLSdkBridgeController = value;
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MPLBaseScene") return;

        if(MPLController.Instance.gameConfig.GameId!= 1000162)
        {
            if(scene.name != MPLController.Instance.sceneName)
            {
                return;
            }
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
        if (MPLController.Instance.IsThirdPartyGame())
        {
            mPLSdkBridgeController.SendEventToGame("startGameScene");
        }
        else
        {
            scoreSubmitted = false;
            gameSceneLoaded = true;
            Session.Instance.CallReadyToPlay();
            if (MPLController.Instance.IsAsyncGame())
            {
                MPLController.Instance.SendStartGameEvent();
            }
            mPLSdkBridgeController.SendEventToGame("startGameScene");
            StartCoroutine(Util.WaitAndExecute(0f, () =>
            {
                if (!MPLController.Instance.IsAsyncGame())
                {
                    StartCoroutine(StartingGame1v1Sync());
                }

            }));
        }
    }

  

   

    public void OnBattleFinish(List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList,bool isFightAgainDisabled)
    {

        Debug.Log("IS it find again=" + MultiplayerGamesHandler.Instance.isThisFindAgain);

        if (!MultiplayerGamesHandler.Instance.isThisFindAgain) //tis
        {
            if (matchMakingController1VN.gameObject.activeSelf)
            {
                matchMakingController1VN.gameObject.SetActive(false);
                
                // GameBattaleIdScreenHandler.mInstance.ShowBattleId();
            }
            if (matchFailed.activeSelf)
            {
                matchFailed.SetActive(false);
            }

            Debug.Log("onScoreSubmittedCalled=" + onScoreSubmittedCalled);
            if (onScoreSubmittedCalled) return;
            Debug.Log("Setting onScoreSubmittedCalled true: OnBattleFinish");
            onScoreSubmittedCalled = true;
            Time.timeScale = 1;

            Debug.Log("OnBatOnBattleFinish AA ####");
            StartCoroutine(WaitAndBattleFinish(mPLBattleFinishPlayersInfoList, isFightAgainDisabled));
        }
    }

    IEnumerator WaitAndBattleFinish(List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList,bool isFightAgainDisabled)
    {
        Debug.Log("OnBatOnBattleFinish BB ####");
        yield return new WaitWhile(() => (!battleEndController1VN.gameObject.activeSelf));
        Debug.Log("OnBatOnBattleFinish CC ####");
        StartCoroutine(battleEndController1VN.EnqueueBattleFinished(mPLBattleFinishPlayersInfoList, isFightAgainDisabled));
    }
    void Start()
    {

        

        int timer = MPLController.Instance.gameConfig.CreateGameTimeout * 1000;
        gameStartTimer = new System.Timers.Timer(timer);
        gameStartTimer.Elapsed += HandleGameStartTimer;
        if (!started) started = true;

    }

    public void StartMultiplayerController()
    {
        isConnectionLostShown = false;
       
        

        if (!MPLController.Instance.IsThirdPartyGame())
        {
            SmartFoxManager.Instance.OnStartBattle += OnStartBattle;
            SmartFoxManager.Instance.OnScoreSubmitted += OnScoreSubmitted;
            SmartFoxManager.Instance.OnUserVarsUpdated += OnUserVarsUpdate;
            SmartFoxManager.Instance.MatchFound += MatchFound;
            SmartFoxManager.Instance.OnServerError += OnServerError;
            SmartFoxManager.Instance.OnFightAgainStateChange += OnFightAgainStateChange;
            SmartFoxManager.Instance.ConnectionFailed += ConnectionLost;
            SmartFoxManager.Instance.OnConnectRetrying += OnConnectRetrying;
            SmartFoxManager.Instance.OnConnectionResumed += OnConnectionResumed;
            SmartFoxManager.Instance.ConnectionLost += ConnectionLost;
            SmartFoxManager.Instance.OnBattleFinish += OnBattleFinish;
            SmartFoxManager.Instance.OnGameLoaded += OnGameLoaded;
            SmartFoxManager.Instance.OpponentMissedPing += OpponentMissedPing;
        }


        multiplayerGamesHandler.ResetMatchMakingRetriesValue(true);
        multiplayerGamesHandler.SetBackgroundImage(backGroundImage);
        StartMultiplayer(MatchType.NEW_MATCH);
    }



    public void StartMultiplayer(MatchType matchType)
    {
        isConnectionLostShown = false;
        Debug.Log("Retries left=" + multiplayerGamesHandler.maximumMatchMakingRetries);
        findingMatch = true;
       

        

        if (battleEndController1VN.gameObject.activeSelf == true)
        {
            
            DisableBattleEnd();
        }

        if (matchMakingController1VN.gameObject.activeSelf == true)
        {
           
            matchMakingController1VN.gameObject.SetActive(false);
        }

        matchMakingController1VN.gameObject.SetActive(true);
        
    }

    void DisableBattleEnd()
    {
        battleEndController1VN.gameObject.SetActive(false);
    }

    void EnableBattleEnd()
    {
        battleEndController1VN.gameObject.SetActive(true);
    }
    public void StopMatchMakingOnFailedToJoin()
    {

        Debug.Log("Time over");

        multiplayerGamesHandler.maximumMatchMakingRetries = 0;
        SmartFoxManager.Instance.mmRetryAttempt = 0;
        matchMakingController1VN.OnRoomJoinFailed(SmartFoxManager.SmartFoxManagerReasons.FailedToFindMatch);


    }


    void HandleGameStartTimer(object sender, EventArgs e)
    {
        Debug.Log("Game start timer handler called, restarting matchmaking");
        StopGameStartTimer();

        //Grey area
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            StartCoroutine(Util.WaitAndExecute(1, () =>
            {

                    Debug.Log("Game start timer handling default");
                    if (!onStartBattleCalled)
                    {
                    multiplayerGamesHandler.SubmitBattleStartedEvent(false, SmartFoxManager.SmartFoxManagerReasons.MatchFoundTimedOut.ToString());
                    StopMatchMakingOnFailedToJoin();
                    }
                
            }));
        });
    }

    public void OnStartBattle()
    {



        multiplayerGamesHandler.SetGameState(MultiplayerGamesHandler.MPLGameState.in_game);
        onStartBattleCalled = true;
        AudioListener.volume = 1f;


        //Event
        MultiplayerGamesHandler.Instance.SubmitBattleStartedEvent(true, "");



        Debug.Log("Battle Started");








        Debug.Log("Setting onScoreSubmittedCalled false: OnStartBattle");
        onScoreSubmittedCalled = false;
        DisableUI();

        // isGameStarted = false;


    }
    public void DisableUI()
    {
        StartCoroutine(DisablingUI());
    }
    IEnumerator DisablingUI()
    {
        if (matchMakingController1VN.gameObject.activeSelf == true)
        {
            matchMakingController1VN.gameObject.SetActive(false);
        }
        if (battleEndController1VN.gameObject.activeSelf == true)
        {
            DisableBattleEnd();
        }
        if (backGroundImage.activeSelf == true)
        {
            backGroundImage.SetActive(false);
        }

        if (connectionLost.activeSelf)
        {
            connectionLost.SetActive(false);
        }
        if (sponserSlim != null && sponserSlim.gameObject.activeSelf)
            sponserSlim.gameObject.SetActive(false);
        Debug.Log("Removing background");
        yield return new WaitForSeconds(1f);
        backGroundImage.SetActive(false);
    }
    public void ShowErrorPopUp(MPLGameEndReason.GameEndReasons errorReason, string title, string description)
    {

        errorTitle.text = MPLController.Instance.gameConfig.ICLostTitle;
        errorSubtitle.text = MPLController.Instance.gameConfig.ICLostMessage;
        if (errorReason == MPLGameEndReason.GameEndReasons.CONNECTION_LOST|| errorReason == MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND)
        {
            
            MultiplayerGamesHandler.Instance.SetConnectionFlags("Connection Lost Third Party");
            
            MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Connection Lost", errorTitle.text, errorSubtitle.text);
        }
        else
        {
            MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Server Error", errorTitle.text, errorSubtitle.text);
        }


        if (errorReason == MPLGameEndReason.GameEndReasons.MATCH_NOT_FOUND)
        {

            Debug.Log("Match Not Found");
            if (matchMakingController1VN.gameObject.activeSelf)
            {

                matchMakingController1VN.StopMatchMaking(true);

            }

        }
        else
        {

            isItConnectionLostPopup = true;
            connectionLost.SetActive(true);
            if (battleEndController1VN.gameObject.activeSelf)
            {
                if (battleEndController1VN.autoStartNextButton.gameObject.activeSelf && battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>() != null)
                {
                    battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                }
            }
        }




    }
    void OnStartBattle(UserProfile opponent)
    {

         Debug.Log("Is game running=" + isGameRunning);
        Debug.Log("findingMatch MPL:=" + findingMatch);
        Debug.Log("is it find again=" + multiplayerGamesHandler.isThisFindAgain);
        Debug.Log("playercount=" + SmartFoxManager.Instance.GetJoinedRoom().UserCount);
        multiplayerGamesHandler.readyUsers = SmartFoxManager.Instance.GetAllReadyUsers();
        if (SmartFoxManager.Instance.GetJoinedRoom().UserCount != 1)
        {
            isGameRunning = true;
            Debug.Log("Start Battle Called");
            if (findingMatch)
            {
                Debug.Log("Finding match, can't start battle in between");
                return;
            }

            
            multiplayerGamesHandler.SubmitBattleStartedEvent(true, "");
            StopGameStartTimer();
            multiplayerGamesHandler.SetGameState(MultiplayerGamesHandler.MPLGameState.in_game);


            Debug.Log("Battle Started");
            StartCoroutine(StartGamePlay());


            StartCoroutine(MPLController.Instance.StartAsyncLoadedScene());

            if (MPLController.Instance.IsAsyncGame())
            {
                StartCoroutine(StartingGame());
                StartCoroutine(RemoveBackground());
            }
            Debug.Log("Setting onScoreSubmittedCalled false: OnStartBattle");
            onScoreSubmittedCalled = false;
            onStartBattleCalled = true;
            gameSceneLoaded = false;

           
        }
        else
        {
            Debug.Log("not enough players to start the game");
            multiplayerGamesHandler.maximumMatchMakingRetries = 0;
            matchMakingController1VN.OnRoomJoinFailed(SmartFoxManager.SmartFoxManagerReasons.FailedToFindMatch);
        }
    }

    IEnumerator StartGamePlay()
    {
        Debug.Log("Starting co MPL");
        yield return new WaitForSeconds(3.0f);

    }
    void OnGameLoaded()
    {
    }

    IEnumerator StartingGame1v1Sync()
    {
        matchMakingController1VN.gameObject.SetActive(false);
        
        GameBattaleIdScreenHandler.mInstance.ShowBattleId();
        yield return new WaitForSeconds(0f);

       

        if (battleEndController1VN.gameObject.activeSelf == true)
        {
            Debug.Log("SmartFoxManager.Instance.battleFinished = " + SmartFoxManager.Instance.battleFinished);
            if (!SmartFoxManager.Instance.battleFinished)
            {
                DisableBattleEnd();
            }
            else 
            {
                Session.Instance.EndGame(MPLGameEndReason.GameEndReasons.OPPONENT_DISCONNECTED, "Last Player Left");
            }
        }

        multiplayerInGameController.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        if (!SmartFoxManager.Instance.battleFinished && !scoreSubmitted)
        {
            backGroundImage.SetActive(false);
        }

    }

    IEnumerator StartingGame()
    {
       

        yield return new WaitForSeconds(1f);

        if (matchMakingController1VN.gameObject.activeSelf == true)
        {
            matchMakingController1VN.gameObject.SetActive(false);
            
            GameBattaleIdScreenHandler.mInstance.ShowBattleId();
        }
        if (battleEndController1VN.gameObject.activeSelf == true)
        {
            DisableBattleEnd();
        }
        if (backGroundImage.activeSelf == true)
        {
            backGroundImage.SetActive(false);
        }

        if(!MPLController.Instance.IsThirdPartyGame())
        {
            multiplayerInGameController.gameObject.SetActive(true);
        }
        
    }

    IEnumerator RemoveBackground()
    {
        Debug.Log("Removing background");
        yield return new WaitForSeconds(1f);
        backGroundImage.SetActive(false);
    }

    public void FindNewPlayer()
    {
       
        isGameRunning = false;
        if (battleEndController1VN.gameObject.activeSelf == true)
        {
            DisableBattleEnd();
        }
        if (matchFailed.activeSelf == true)
        {
            matchFailed.SetActive(false);
        }

        StartMultiplayer(MatchType.DIFFERENT_MATCH);
    }
    public void StopGameStartTimer()
    {
        if (gameStartTimer.Enabled) gameStartTimer.Stop();
    }
    

    void OnConnectRetrying()
    {
        Debug.Log("OnConnectRetrying");
        connectionRetrying = true;
        retryPanel.transform.GetChild(0).GetComponent<Text>().text = "Network Issue! Trying To Reconnect";
        retryPanel.SetActive(true);
    }

    void OnConnectionResumed()
    {
        Debug.Log("OnConnectionResumed");

        connectionRetrying = false;
        retryPanel.SetActive(false);
    }

    float opponentMissedT;
    void OpponentMissedPing()
    {
        opponentMissedT = MPLController.Instance.timeSpent + (MPLController.Instance.gameConfig.PingInterval * Time.timeScale);
        retryPanel.transform.GetChild(0).GetComponent<Text>().text = "Opponent is trying To Reconnect";
    }

    void ConnectionLost(MPLGameEndReason.GameEndReasons reason)
    {
        Debug.Log("ConnectionLost = " + reason);

        if (reason != MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND)
        {
            ShowConnectionLost(reason);
        }
    }
    
    void OnServerError(ServerErrorData serverErrorData)
    {
        errorTitle.text = serverErrorData.title;
        errorSubtitle.text = serverErrorData.description;
       
        multiplayerGamesHandler.SubmitBattlePopupShownEvent("Server Error", serverErrorData.title, serverErrorData.description);
        EnableConnectionLostPopup(MPLGameEndReason.GameEndReasons.SERVER_ERROR);
    }
    bool isConnectionLostShown = false;
    public void ShowConnectionLost(MPLGameEndReason.GameEndReasons reason)
    {
        Debug.Log("isConnectionLostShown AA = " + isConnectionLostShown);
        if (!isConnectionLostShown)
        {
            Debug.Log("isConnectionLostShown BB = " + isConnectionLostShown);
            SmartFoxManager.Instance.SetConnectionFlags(reason.ToString());
          
            errorTitle.text = MPLController.Instance.gameConfig.ICLostTitle;
            errorSubtitle.text = MPLController.Instance.gameConfig.ICLostMessage;
            EnableConnectionLostPopup(reason);
                      
            isConnectionLostShown = true;
        }

    }

    public void GoToFairPlayPolicy()
    {
        Session.Instance.SetQuitReason(Session.MPLQuitReason.view_fraud_policy);
        Session.Instance.Quit();
    }

    void EnableConnectionLostPopup(MPLGameEndReason.GameEndReasons reason)
    {
        Debug.Log("EnableConnectionLostPopup AA");

    
        errorPopupButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("GO TO BATTLE ROOM");
        isItConnectionLostPopup = true;
        if (MPLController.Instance.gameConfig.ShowRefundPopup && multiplayerGamesHandler.isMatchFound && Session.Instance.isConnectedToInternet=="YES" && Session.Instance.isConnectedToSmartFox=="NO" && Session.Instance.isConnectionModeSwitched=="NO")
        {
            Debug.Log("EnableConnectionLostPopup BB");
            errorTitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("Something went wrong... Game Ended");
            errorSubtitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("We are refunding your Entry Fees. We apologize for the trouble");
        }                  

        //else if (reason == Session.MPLGameEndReason.WENT_IN_BACKGROUND)
        //{
        //    isItConnectionLostPopup = false;
        //    Debug.Log("EnableConnectionLostPopup CC");
        //    errorPopupButtonText.text = "Okay";
         
        //    errorTitle.text = "Game ended due to leaving the app";
        //    errorSubtitle.text = "Please don’t leave the app in the middle of the game. In such cases the game will end for you and your opponent might win.";
        //}

        Debug.Log("EnableConnectionLostPopup DD");
        connectionLost.SetActive(true);
         
        multiplayerGamesHandler.SubmitBattlePopupShownEvent("Connection Lost", errorTitle.text, errorSubtitle.text);
        if (battleEndController1VN.gameObject.activeSelf)
        {
            if(battleEndController1VN.autoStartNextButton.gameObject.activeSelf && battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>()!=null)
            {
                battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
            }
        }
    }

   public void EnableWentInBackgroundPopup()
    {
        if (!MPLController.Instance.gameConfig.ShowMinimisePopupEnabled)
            return;
        if (isItConnectionLostPopup)
            return;
        errorPopupButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("Okay");
        errorTitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("Game ended due to leaving the app");
        errorSubtitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("Please don’t leave the app in the middle of the game. In such cases the game will end for you and your opponent might win.");
        //}
        isItConnectionLostPopup = false;
        multiplayerGamesHandler.SubmitBattlePopupShownEvent("Went in background", errorTitle.text, errorSubtitle.text);
        connectionLost.SetActive(true);
        if (battleEndController1VN.gameObject.activeSelf)
        {
            if (battleEndController1VN.autoStartNextButton.gameObject.activeSelf && battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>() != null)
            {
                battleEndController1VN.autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
            }
        }
    }
    public void DisableWentInBackgroundPopup()
    {
        connectionLost.SetActive(false);
    }

    public void OnErrorPopupButtonClicked()
    {
        if(isItConnectionLostPopup)
        {
            Debug.Log("Unity Quit");
            multiplayerGamesHandler.QuitUnity();
        }
        else
        {
            Debug.Log("Background Popup");
            DisableWentInBackgroundPopup();
        }
    }

    void MatchFound(List<UserProfile> userList)
    {
        

        Debug.Log("Found the match");
       
        findingMatch = false;
    }

    void OnScoreSubmitted(SessionResult sessionResult)
    {
        if (multiplayerGamesHandler.GetGameState() != MultiplayerGamesHandler.MPLGameState.in_game)
        {
            return;
        }
        if (onScoreSubmittedCalled) return;
        Debug.Log("Setting onScoreSubmittedCalled true: OnScoreSubmitted");
        onScoreSubmittedCalled = true;
        
        Debug.Log("Setting onScoreSubmittedCalled false: Update");
        StartCoroutine(WaitAndOnScoreSubmitted());
        onScoreSubmittedCalled = false;
        isGameRunning = false;
        Time.timeScale = 1;
        //Event

        multiplayerGamesHandler.SubmitBattleEndedEvent(sessionResult.Score, sessionResult.ToString(), 0, "Undecided", "Score Submitted", MPLController.Instance.gameConfig.Profile.TotalBalance, MPLController.Instance.gameConfig.Profile.TokenBalance, false, (int)sessionResult.GameplayDurationSDK);
    }


    IEnumerator WaitAndOnScoreSubmitted()
    {
        scoreSubmitted = true;
        yield return new WaitWhile(() => !started);

        Debug.Log("WaitAndOnScoreSubmitted");
        EnableBattleEnd();
        backGroundImage.SetActive(true);
        //gameOverThings.SetActive(true);
         multiplayerInGameController.gameObject.SetActive(false);

        battleEndController1VN.ShowGameEnd();

    }
    public void OnScoreSubmitted(MPLGameEndReason.GameEndReasons submissionReason, int score)
    {
        if(multiplayerGamesHandler.GetGameState()!=MultiplayerGamesHandler.MPLGameState.in_game)
        {
            return;
        }
        if (onScoreSubmittedCalled) return;
        Debug.Log("Setting onScoreSubmittedCalled true: OnScoreSubmitted");
        onScoreSubmittedCalled = true;

        Debug.Log("Setting onScoreSubmittedCalled false: Update");
        StartCoroutine(WaitAndOnScoreSubmitted());
        onScoreSubmittedCalled = false;

        Time.timeScale = 1;
        //Event

        MultiplayerGamesHandler.Instance.SubmitBattleEndedEvent(score, "", 0, "Undecided", "Score Submitted", MPLController.Instance.gameConfig.Profile.TotalBalance, MPLController.Instance.gameConfig.Profile.TokenBalance, false, 0);
    }

    
    private void Update()
    {

       

        if (battleEndController1VN.gameObject.activeSelf || matchMakingController1VN.gameObject.activeSelf)
        {
            retryPanel.SetActive(false);
            return;
        }
        if (connectionRetrying) return;

        if (MPLController.Instance.timeSpent < opponentMissedT)
        {
            retryPanel.SetActive(true);
        }
        else
        {
            retryPanel.SetActive(false);
        }
    }

    void OnApplicationPause(bool status)
    {
        if (status == true)
        {  
            Session.Instance.userMinimised = "YES";

            if(MPLController.Instance.IsThirdPartyGame())
            {
                return;
            }
            if (onStartBattleCalled && !gameSceneLoaded)
            {
                Debug.Log("Mul canvas controller = " + onStartBattleCalled + " = " + gameSceneLoaded);
                Debug.Log("OnApplicationPaue=MultiplayerController1vN");
                //NEW
                SessionResult result = MPLController.Instance.GetCurrentGameResult(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND);
                result.Score = Session.Instance.GetScore();

                Session.Instance.ForceEndGame(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND, Session.Instance.GetReasonString(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND), result);
                
              
                Disconnect(true, MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND);
                

            }
        }

    }

   



    Thread _thread;
    MPLGameEndReason.GameEndReasons reason;
    public void Disconnect(bool showConnectionLost, MPLGameEndReason.GameEndReasons reason)
    {
        this.reason = reason;
        _thread = new Thread(DisconnectOnSeparateThread);
        _thread.Start();

        //Task.Factory.StartNew(() => SmartFoxManager.Instance.Disconnect());
        Debug.Log("showConnectionLost = " + showConnectionLost);
        if (showConnectionLost) ShowConnectionLost(reason);
    }

    void DisconnectOnSeparateThread()
    {
        SmartFoxManager.Instance.Disconnect(reason);
        _thread.Join();
    }

    
   
    public int GetUserCount()
    {
        return multiplayerGamesHandler.userListOnMatch.Count;

    }
}


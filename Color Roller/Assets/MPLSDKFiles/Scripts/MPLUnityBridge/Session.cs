using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using UnityEngine;

public class Session
{
    /******************************** Global Variables & Constants ********************************/
    private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly String INPUT_FILE_PATH = "Configurations/session_config";
    private static Session _instance;

    private int maxPauseDuration;
    public List<double> pingDurations;
    public bool isRoomOwner;
    public string modeOfConnection;
    public string playersInTheRoom;
    private String _rawInput;
    public string extraDisconnectionReason;
    private int remainingTimeAtPause;
    private bool isMaster;
    public delegate void MPLStartGameEvent();
    public event MPLStartGameEvent ReadyToPlay;
    public delegate void MPLUpdatedDataEvent(string mplUpdatedData);
    public string isConnectedToInternet = "YES";
    public string isConnectedToSmartFox = "NO";
    public String isConnectionModeSwitched = "NO";
    public event MPLUpdatedDataEvent OnDataUpdate;

    public delegate void TutorialEvent();
    public event TutorialEvent OnTutorialStart, OnTutorialEnd;
    private int totalPauseDuration;
    private long remainingTournamentEndTime;
    private int pauseCount;
    private long startTime;
    private bool scoreTampered, fieldTampered;
    public string userMinimised="NO";
    private bool quit;
    
    private static bool gameEndScoreSubmitted;
    public MPLController mplController;
    private Timer tournamentEndTimer;
    private SessionInfo info;
    protected SessionResult result;
    protected long lastScoreLogAddedTime;
    protected List<int> scoreLog;
    
    private List<ScoreTamperData> scoreTamperDataList;
    Dictionary<string, string> fieldsDictionary;
    public Dictionary<string, double> fields;
    private Dictionary<string, List<FieldTamperData>> fieldsTamperDict;
    private bool isItQuitByAndroid;
    //Android (Java) function names
    protected int score, randomAdder, gameplayDuration;
    protected double moneyToAdd;
    public int remainingPauseTime { get; private set; }
    public bool StartAddingGameplayDuration;
    

    public enum MPLQuitReason
    {
        battle_won = 0,
        battle_lost = 1,
        battle_issue = 2,
        battle_quit = 3,
        home_quit = 4,
        battle_add_money_insufficient = 5,
        battle_add_token=6,
        assets_modified=7,
        challenge_quit=8,
        challenge_won=9,
        challenge_lost=10,
        challenge_tied=11,
        challenge_pending=12,
        challenge_add_token=13,
        challenge_add_money=14,
        view_fraud_policy=15,
        view_transaction_history=16,
        knockout_quit=17,
        rummy_tournament_quit = 18
        


    }
    bool isInteractiveTutorial=false;
    public bool IsInteractiveTutorial()
    {
        return isInteractiveTutorial;
    }
    public void SetInteractiveTutorial(bool value)
    {
        isInteractiveTutorial = value;
    }
    public void StartingTraining()
    {
        OnTutorialStart?.Invoke();
    }
    public void EndInteractiveTutorial()
    {
        Debug.Log("Ending tutorial");
        OnTutorialEnd?.Invoke();
    }
    public virtual void UpdatedData(string data)
    {
        Debug.Log("Updated Data");
        OnDataUpdate?.Invoke(data);

    }



    public virtual void UpdateData(string data)
    {

  


    }
	 public void EndTraining()
    {
        EndGame(MPLGameEndReason.GameEndReasons.END_TRAINING, "End Training");
    }
    public string GetReasonString(MPLGameEndReason.GameEndReasons reason)
    {
        string reasonString="";
        foreach(string word in reason.ToString().Split('_'))
        {
            reasonString = reasonString + word.ToLower()+" ";
            
        }
        string resultString = char.ToUpper(reasonString[0]) + reasonString.Substring(1);
        return resultString.Trim();

    }

    public enum MPLDisableType
    {
        PAUSE_BUTTON = 0,
        ON_ESCAPE = 1
    }
    private MPLQuitReason quitReason;


    public delegate SessionResult Action(MPLGameEndReason.GameEndReasons gameEndReason);
    public Action OnCreateSessionResult;

    public delegate void MPLGameEndNotification(MPLNotificationEventArgs args);
    public event MPLGameEndNotification GameEnd;

    public delegate void MPLRestartNotification();
    public event MPLRestartNotification Restart;

    public delegate void MPLWentInBackgroundEvent();
    public event MPLWentInBackgroundEvent WentInBackGround;

    /// <summary>
    /// Singleton Instance
    /// </summary>
    /// <value>Singleton Instance</value>
    public static Session Instance
    {
        get
        {
            return _instance;
        }
    }

    /**************************************** Constructors & Destructor ****************************************/
    static Session() { }
    public Session()
    {
        Debug.Log("MPL: Session created :: " + this);
        _instance = this;

        

        

        tournamentEndTimer = new Timer(1000);
        tournamentEndTimer.Elapsed += HandleTournamentEndTimer;

        InitData();
    }

    /************************************** Public Functions **************************************/
    public virtual void InitData()
    {
        this.result = null;
        Debug.LogError("StartAddingGameplayDuration made false from InitData of Session");
        this.StartAddingGameplayDuration = false;
        //Init data
        mplController = MPLController.Instance;
        
        scoreTampered = false;
        scoreLog = new List<int>();
        scoreTamperDataList = new List<ScoreTamperData>();
        fieldsDictionary = new Dictionary<string, string>();
        fields = new Dictionary<string, double>();
        fieldsTamperDict = new Dictionary<string, List<FieldTamperData>>();
        gameplayDuration = 0;
        fieldTampered = false;
        //Init pause data
        maxPauseDuration = mplController.gameConfig.MaxPauseDuration;
        gameplayDuration = 0;
        moneyToAdd = 0;
        isItQuitByAndroid = MPLController.Instance.gameConfig.isItQuitByAndroid;
        remainingPauseTime = maxPauseDuration;
        //Init game end timer

        //Init tournament end data
        if (mplController.gameConfig.TournamentEndTime > 0)
        {
            remainingTournamentEndTime = mplController.GetRemainingTournamentTime();
            Debug.Log("MPL: Remaining Tournament Time = " + remainingTournamentEndTime);
            tournamentEndTimer.Start();
        }

        //Init score data
        randomAdder = UnityEngine.Random.Range(1, 1000);
        score = randomAdder;
        lastScoreLogAddedTime = remainingTournamentEndTime;

       
        Debug.Log("***MPL: Session data Init*** = " + this.result);
        Debug.LogError("*********** GameplayDurationSDK value reset from InitData ");
    }
    public void CallReadyToPlay()
    {

        if (ReadyToPlay != null)
        {
            ReadyToPlay();
        }
        else
        {
            Debug.Log("ReadyToPlay is null");
        }
    }
    /// <summary>
    /// Returns the active session info
    /// </summary>
    /// <returns>Active SessionInfo.</returns>
    /// <typeparam name="SI">Type of class that extends session info</typeparam>
    public virtual SI GetSessionInfo<SI>() where SI : SessionInfo
    {
       /* if (info != null)
        {
            try
            {
                return (SI)info;
            }
            catch (Exception e)
            {
                Debug.Log("Invalid SessionInfo Cast: " + e);
            }
        }*/
        _rawInput = mplController.GetSessionConfig();
        startTime = (long)(DateTime.UtcNow - EPOCH).TotalMilliseconds;
        //MPLController.Instance.PrintLargeLog("MPL Session: raw input: ", _rawInput, 1);
        
        //SI sessionInfo = JsonUtility.FromJson<SI>(_rawInput);
        SI sessionInfo = JsonConvert.DeserializeObject<SI>(_rawInput);

        info = sessionInfo;
        this.result = null;
        //Debug.Log("MPL: Returning Session Info: " + sessionInfo);

        return sessionInfo;
    }
    public void SetMaster(bool isMaster)
    {
        this.isMaster = isMaster;
    }
    public bool IsMaster()
    {
        return isMaster;
    }
    /// <summary>
    /// Submits the game result (Score + Other data).
    /// </summary>
    /// <param name="result">Game Result.</param>

    public virtual void SubmitResult(SessionResult result)
    {
        Debug.Log("MPL: GameEnd reason = " + result.GameEndReason);
        if (this.result != null)
        {
            Debug.Log("MPL: Result already submitted");
            return;
        }

        Debug.Log("MPL: Initiated Submit Result: " + result);
        this.result = CleanSubmitScore(result, true);
    
        
        if (tournamentEndTimer.Enabled) tournamentEndTimer.Stop();
        if (result.GameEndReason == MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND.ToString() || result.GameEndReason ==GetReasonString(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND))
        {
            WentInBackGround?.Invoke();

        }

       
    }
    public virtual void InvokeWentInBG()
    {
     
        WentInBackGround?.Invoke();
    }

    public SessionResult GetCleanSubmitScore(SessionResult result)
    {
        return CleanSubmitScore(result, true);
    }

   
    protected SessionResult CleanSubmitScore(SessionResult result, bool addScoreLog)
    {
        UpdateScore(result.Score, result.Score);
       
        result.StartTime = startTime;
        result.EndTime = (long)(DateTime.UtcNow - EPOCH).TotalMilliseconds; //Overwritten by Java submit result func
        result.GameId = mplController.gameConfig.GameId;
        result.GameplayDurationSDK = gameplayDuration;
        result.GamePauseDuration = totalPauseDuration;
        result.GamePauseCount = pauseCount;
        result.GameVersion = mplController.VERSION;
        result.ModeOfConnection = modeOfConnection;
        result.ExtraDisconnectionReason = extraDisconnectionReason;
        result.IsConnectedToInternet = isConnectedToInternet;
        result.IsConnectedToSmartfox = isConnectedToSmartFox;
        result.IsConnectionModeSwitched = isConnectionModeSwitched;
        result.EntryFee = MPLController.Instance.gameConfig.EntryFee;
        result.UserMinimised = userMinimised;
        result.MaxPlayers = MPLController.Instance.gameConfig.TotalPlayers;

        List<Field> fields = new List<Field>();
        foreach (KeyValuePair<string,string> entry in fieldsDictionary)
        {
            Field field = new Field(entry.Key, entry.Value);
            fields.Add(field);
        }
        result.fieldsSequence = fields;
        string lastFivePingDurations="";
        if (pingDurations!=null&& Is1v1())
        {
            for (int i = 4; i >= 0; i--)
            {
                if (i < pingDurations.Count)
                {
                    lastFivePingDurations = pingDurations[i] + "," + lastFivePingDurations;
                }
            }
               
        }
        
         result.lastFivePingDurations = lastFivePingDurations;
        
        //result.gameLog = mplController.getGameLogs();
        //mplController.resetGameLogs();
        //mplController.setStartStoringLogs(false);
        //Add last score log
        if (addScoreLog)
        {
            lastScoreLogAddedTime = remainingTournamentEndTime + mplController.gameConfig.SequenceInterval;
            AddScoreLog(score - randomAdder);
            result.SetSequenceList(scoreLog);
        }
       /* if (mplController.gameConfig.GameId == 1000049)
        {
            
            result.SetSequenceList(chessTimerLog);
        }*/

        return result;
    }

    /// <summary>
    /// Quit/End session without submitting result.
    /// </summary>
    public void Quit()
    {
        Debug.Log("Game Quit Reason: " + quitReason);
        


        if (quit)
        {
            Debug.Log("MPL: Already quitted");
            return;
        }

        Debug.Log("MPL: Quitting without submitting Result.");

        quit = true;


        if (Application.platform == RuntimePlatform.Android)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {

                string reasonToQuit = " ";
                if (quitReason == MPLQuitReason.battle_add_money_insufficient)
                {
                    reasonToQuit = MPLQuitReason.battle_add_money_insufficient.ToString() + "_" + moneyToAdd;
                }
                else
                {
                    reasonToQuit = quitReason.ToString();
                }
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
                AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", MPLController.Instance.gameConfig.AppId);
                launchIntent.Call<AndroidJavaObject>("setClassName", MPLController.Instance.gameConfig.AppId, "com.mpl.androidapp.react.MPLReactContainerActivity");
                launchIntent.Call<AndroidJavaObject>("putExtra", "gameScore", "quitGame" + reasonToQuit);
                //currentActivity.Call<bool>("moveTaskToBack", true);
                currentActivity.Call("startActivity", launchIntent);
                if (GetSDKLevel() >= 21)
                {
                    currentActivity.Call("finishAndRemoveTask");
                }
                else
                {
                    currentActivity.Call("finish");
                }
                unityPlayer.Dispose();
                currentActivity.Dispose();
                packageManager.Dispose();
                launchIntent.Dispose();

                Application.Quit();

            });
        }
        else
        {



            Application.Quit();


        }
    }
    public int GetSDKLevel()
    {
        var clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
        var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
        var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
        return sdkLevel;
    }
   

  

    public virtual void StartGameplayTimer()
    { 
    //This is used for Battles in Session1v1
    }

    /// <summary>
    /// Pause the session.
    /// </summary>
    
    /// <summary>
    /// Resume the session.
    /// <returns>Total Pause time (in seconds)</returns>
    /// </summary>
    

    /// <summary>
    /// Set the previous score and the current score.
    /// </summary>
    public virtual void UpdateScore(int previousScore, int score)
    {
        int storedScore = (this.score - this.randomAdder);

        if (storedScore > previousScore)
        {
            Debug.Log("MPL: stored score in sdk > previous score, dafuq?");
            return;
        }

        //Score tampered with
        if (storedScore != previousScore)
        {
            scoreTampered = true;
            ScoreTamperData scoreTamperData = new ScoreTamperData(storedScore, previousScore);
            scoreTamperDataList.Add(scoreTamperData);
            Debug.Log("MPL: Score tampered with (score stored in session not equal to previous score)\nStored score = " + storedScore + ", prev score sent to sdk = " + previousScore);
            FraudDetected("score", scoreTamperData.ToString());
        }

        this.score = randomAdder + score;
    }

    /// <summary>
    /// Detects if a visible field in the game is tampered. Set the previous field and the current field value.
    /// </summary>
    /// 
    
    public virtual void UpdateField(string fieldName, double previousField, double newField)
    {
        //Check if this field is known
        if (!fields.ContainsKey(fieldName))
        {

            fields.Add(fieldName, randomAdder + newField);

            fieldsTamperDict.Add(fieldName, new List<FieldTamperData>());
            return;
        }

        double storedField = (fields[fieldName] - this.randomAdder);
       

        //Timer tampered
        if (storedField != previousField)
        {
            FieldTamperData fieldTamperData = new FieldTamperData(storedField, previousField);
            fieldsTamperDict[fieldName].Add(fieldTamperData);
            Debug.Log("MPL: " + fieldName + " tampered (field stored in session not equal to previous field)\nStored field = " + storedField + ", prev field sent to sdk = " + previousField);
            if (MPLController.Instance.gameConfig.GameId != 1000051)
            {
                //FraudDetected(fieldName, fieldTamperData.ToString());
            }
        }

        fields[fieldName] = randomAdder + newField;

        if(fieldsDictionary.ContainsKey(fieldName))
        {
            fieldsDictionary[fieldName]=fieldsDictionary[fieldName] + (fields[fieldName] - randomAdder) + ",";
        }
        else
        {
            fieldsDictionary.Add(fieldName, (fields[fieldName] - randomAdder) + ",");
        }
        
    }

    public virtual void FraudDetected(string type, string proof)
    {

    }

    /// <summary>
    /// Clean up stuff.
    /// </summary>
    public void CleanUp()
    {
        

        if (tournamentEndTimer.Enabled) tournamentEndTimer.Stop();
        tournamentEndTimer.Close();
    }

    public bool CanPause()
    {
        return (pauseCount < 1);
    }

    /************************************** Private Functions **************************************/
    /// <summary>
    /// Handles the pause timer. Notifies when pause time limit exceeds.
    /// </summary>
   

    /// <summary>
    /// Checks the remaining pause time every and takes appropriate action.
    /// </summary>
    



    /// <summary>
    /// Tells if some frodulant activity took place.
    /// </summary>
    protected SessionResult FraudCheck(SessionResult result)
    {

            //Flags
            if (scoreTampered)
            {
                String scoreMismatches = "";
                for (int i = 0; i < scoreTamperDataList.Count; i++)
                {
                    scoreMismatches += ("{" + scoreTamperDataList[i].ScoreStoredInSDK + ":" + scoreTamperDataList[i].PreviousGameScore + "}");
                    if (i < scoreTamperDataList.Count - 1) scoreMismatches += ",";
                }
                Debug.Log("MPL: Score Mismatches: " + scoreMismatches);

                result.ScoreTampered = scoreTampered;
           
                UserFraudDetectedEventProperties userFraudDetectedEventProperties = new UserFraudDetectedEventProperties(UserFraudDetectedEventProperties.FraudCheckType.SCORE_TAMPERED, scoreMismatches, ("" + mplController.gameConfig.GameId), mplController.gameConfig.GameName, mplController.gameConfig.MobileNumber, mplController.gameConfig.IsRooted, mplController.gameConfig.IsModAppFound);
                MPLSdkBridgeController.Instance.SubmitEvent(mplController.USER_FRAUD_DETECTED, userFraudDetectedEventProperties.ToString());
            
            }

            //Configs mismatch
           /* if (result.GetConfigsMismatch())
            {
                Debug.Log("MPL: Data Tamering! Output Config != Input Config");
            if (MPLController.Instance.gameConfig.GameId != 1000051)
            {
                UserFraudDetectedEventProperties userFraudDetectedEventProperties = new UserFraudDetectedEventProperties(UserFraudDetectedEventProperties.FraudCheckType.GAME_CONFIG_MISMATCH, ("" + mplController.gameConfig.GameId), mplController.gameConfig.GameName, mplController.gameConfig.MobileNumber, mplController.gameConfig.IsRooted, mplController.gameConfig.IsModAppFound);
                mplController.SubmitEvent(mplController.USER_FRAUD_DETECTED, userFraudDetectedEventProperties.ToString());
            }
            }*/

            //FFlags
            string fflags = "1";
            foreach (KeyValuePair<string, List<FieldTamperData>> entry in fieldsTamperDict)
            {
                bool isTampered = (entry.Value.Count > 0);
                fflags += Convert.ToInt32(isTampered);

                if (isTampered)
                {
                    if (!fieldTampered) fieldTampered = true;

                    string mismatches = "";
                    List<FieldTamperData> mismatchesList = entry.Value;
                    for (int i = 0; i < mismatchesList.Count; i++)
                    {
                        mismatches += mismatchesList[i];
                        if (i < mismatchesList.Count - 1) mismatches += ",";
                    }
                if (MPLController.Instance.gameConfig.GameId != 1000051)
                {
                   // UserFraudDetectedEventProperties userFraudDetectedEventProperties = new UserFraudDetectedEventProperties(UserFraudDetectedEventProperties.FraudCheckType.FIELD_TAMPERED, entry.Key, mismatches, ("" + mplController.gameConfig.GameId), mplController.gameConfig.GameName, mplController.gameConfig.MobileNumber, mplController.gameConfig.IsRooted, mplController.gameConfig.IsModAppFound);
                    //mplController.SubmitEvent(mplController.USER_FRAUD_DETECTED, userFraudDetectedEventProperties.ToString());
                }
                }
            }

            fflags += mplController.hackDetector.GetFlags(); //Anti-hack's detections
            result.FFlags = fflags;
            return result;


    }

    void HandleTournamentEndTimer(object sender, EventArgs e)
    {
        remainingTournamentEndTime -= 1000;

        AddScoreLog(score - randomAdder);

        if(StartAddingGameplayDuration)
        gameplayDuration++;

        //End game
        if (remainingTournamentEndTime <= 0)
        {
            Debug.Log("MPL: Tournament ended, ending Game");
            tournamentEndTimer.Stop();
            EndGame(MPLGameEndReason.GameEndReasons.TOURNAMENT_ENDED, "Tournament Ended");
        }

        //Debug.LogError("GameplayDuration increased from Tournament: " + gameplayDuration + " : "+ remainingTournamentEndTime);
    }

    protected virtual void AddScoreLog(int gameScore)
    {
        
        if (Math.Abs(lastScoreLogAddedTime - remainingTournamentEndTime) >= mplController.gameConfig.SequenceInterval)
        {
            
            scoreLog.Add(gameScore);
            lastScoreLogAddedTime = remainingTournamentEndTime;
        }
    }
   
    public void SetQuitReason(MPLQuitReason quitReason)
    {
        this.quitReason = quitReason;
    }

    public void SetMoneyToAdd(double toAdd)
    {
        this.moneyToAdd = toAdd;
    }

    public MPLQuitReason GetQuitReason()
    {
        return this.quitReason;
    }


    public int GetScore()
    {
        return score - randomAdder;
    }
    public double GetFieldValue(String FieldName)
    {
        return Math.Round((fields[FieldName] - randomAdder),1);
    }


    public bool Is1v1()
    {
        return MPLController.Instance.gameConfig.Is1v1;
    }

    /// <summary>
    /// To end the game with a reason
    /// </summary>
    public virtual void EndGame(MPLGameEndReason.GameEndReasons reason, string message)
    {
        Debug.Log("MPL: Ending game with reason = " + reason);
        GameEnd?.Invoke(new MPLNotificationEventArgs(reason, message));
    }

    /// <summary>
    /// To restart the game
    /// </summary>
    public void RestartSession()
    {
        Debug.Log("MPL: Restarting game");
        InitData();
        StartAddingGameplayDuration = true;
        Restart?.Invoke();
    }

    public virtual List<MPLDisableType> GetObjectsToDisable()
    {
        return null;
    }

    public virtual void ForceEndGame(MPLGameEndReason.GameEndReasons reason, string message,SessionResult result)
    {

    }

    public long GetGameplayDuration()
    {
        return gameplayDuration;
    }

   
}

public class MPLNotificationEventArgs : EventArgs
{
    public readonly string message;
    public readonly MPLGameEndReason.GameEndReasons notification;

    public MPLNotificationEventArgs(MPLGameEndReason.GameEndReasons notification, string message)
    {
        this.notification = notification;
        this.message = message;
    }
}
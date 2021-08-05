using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Events;
using System.Timers;
using System.Threading;
using System.IO;

using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Logging;
using System.Net;

public class SmartFoxManager : MonoBehaviour
{
    #region PrivateVariables
    private enum BattleState
    {
        None,
        MatchMaking,
        Fighting,
        Results,
        ConnectionLost
    }

    public SmartFox smartFox;
    public int mmRetryAttempt = 0;
    private int connectionRetries = -10;
    private UnityAction<AccountBalance> accountBalanceCallback;
    private bool manuallyDisconnected, connectionLostCalled;
    public bool battleFinished {get; private set; }
    private System.Timers.Timer pingTimer;
    private float lastPongT;
    private GreconnectionInitiatedEventProperties greconnectionInitiatedEventProperties;
    private static SmartFoxManager instance;
    private MPLGameEndReason.GameEndReasons disconnectionReason;
    private float timeSpent;
    private BattleState battleState;

    #endregion

    #region Constants
    private const int PORT = 9933;

    private const string FIND_MATCH = "findMatch";
    private const string SEND_BROADCAST = "SendBroadcast";
    private const string ON_READY = "onReady";
    private const string ON_GAME_LOADED = "onGameLoaded";
    private const string START_BATTLE = "START_BATTLE";
    private const string SUBMIT_SCORE = "submitScore";
    private const string BATTLE_FINISHED = "BATTLE_FINISHED";
    private const string FIND_MATCH_FAILED = "FAILED_TO_FIND_MATCH";
    private const string OPPONENT_DID_NOT_JOIN = "OPPONENT_DID_NOT_JOIN";
    private const string OPPONENT_DID_NOT_JOIN_KNOCKOUT = "OPPONENT_DID_NOT_JOIN_KNOCKOUT";
    private const string USER_DID_NOT_JOIN_KNOCKOUT = "USER_DID_NOT_JOIN_KNOCKOUT";
    private const string OPPONENTS_DID_NOT_JOIN = "OPPONENTS_DID_NOT_JOIN";
    private const string BATTLE_AGAIN = "fightAgain";
    private const string GET_ACCOUNT_BALANCE = "getAccountBalance";
    private const string GAME_END = "END_GAME";
    private const string FRAUD_DETECTED = "fraudDetected";
    private const string FIGHT_AGAIN_REQUESTED = "FIGHT_AGAIN_REQUESTED";
    private const string OPPONENT_FINISHED = "OPPONENT_FINISHED";
    private const string FIGHT_AGAIN_STATE_CHANGED = "FIGHT_AGAIN_STATE_CHANGED";
    private const string GAME_VERSION = "gameVersion";
    private const string COUNTRY_CODE = "countryCode";
    private const string MATCH_FOUND = "MATCH_FOUND";

    private const string MATCH_USER_IN = "MATCH_USER_IN";
    List<UserProfile> usersKnock;
    private const string KNOCKOUT_MATCH_USER_IN = "KNOCKOUT_MATCH_USER_IN";
    private const string KNOCKOUT_MATCH_USER_OUT = "KNOCKOUT_MATCH_USER_OUT";
    private const string MATCH_USER_OUT = "MATCH_USER_OUT";
    private const string MATCH_GROUP_CHANGE = "MATCH_GROUP_CHANGE";
    private const string SERVER_ERROR = "SERVER_ERROR";
    private const string GAME_LOADED = "GAME_LOADED";
    private const string PING = "PING";
    private const string PONG = "PONG";
    private const string CHALLENGE_ID = "challengeId";
    private const string CLIENT_BROADCAST = "CLIENT_BROADCAST";
    private const string OPPONENT_MISSED_PING = "OPPONENT_MISSED_PING";

    private const string LOBBY_ID = "lobbyId";
    private const string RETRY = "retry";
    private const string TOTAL_PLAYERS = "totalPlayers";
    private const string AUTH_TOKEN = "authToken";
    private const string NAME = "displayName";
    private const string AVATAR = "avatar";
    private const string IS_PRO = "isPro";
    private const string TIER = "tier";
    private const string MPL_USER_ID = "mplUserId";
    private const string SCORE = "score";
    private const string FINAL_SCORE = "finalScore";
    private const string GAME_DATA = "gameData";
    private const string WINNERS = "winners";
    private const string IS_IN_DEBUG_MODE = "isInDebugMode";
    private const string FRAUD_TYPE = "fraudType";
    private const string FRAUD_PROOF = "fraudProof";
    private const string FIGHT_AGAIN = "fightAgain";
    private const string CANCEL_CHALLENGE = "cancelChallenge";
    private const string DATA = "data";
    private const string PROTOCOL = "protocol";
    List<UserProfile> users;
    List<UserProfile> currentUsers;

    public bool gReconnect { get; private set; } = false;
    private int gReconnectionRetriesAllowed = 0;
    private int gReconnectionMaxTimeAllowed = 0;
    private int gReconnectionRetries = 0;
    public int gReconnectionAttempt;
    public float gReconnectionInitiatedAt { get; private set; }
    private IEnumerator submitResultCoroutine = null;
    private bool scoreSubmitted = false;
    public bool gReconnectForPause { get; private set; }
    private string gReconnectPingState = null;
    public string battleId { get; private set; }
    public int gReconnectionsInitiated { get; private set; }
    public int gReconnectionsConcluded { get; private set; }
    public string gReconnectionState { get; private set; } = "Unused";

    private const string COUNT = "count";

    // private static bool created = false;
    static int sentClientBroadcasts, receivedClientBroadcasts;

    private const string APP_VERSION = "appVersion";
    private const string MOBILE_NUMBER = "mobileNumber";
    #endregion

    #region PublicVariables
    public enum SmartFoxManagerReasons
    {
        FailedToFindMatch = 0,
        OpponentDidNotJoin = 1,
        RoomJoinError = 2,
        MonitorTimeout = 3,
        WentInBackgroundMM = 4,
        ConnectionError = 5,
        Hacked = 6,
        MatchFoundTimedOut = 7,
        PingPongTimeout = 8,
        WentInBackgroundTransition = 9,
        AppQuit = 10,
        ManuallyDisconnected = 11,
        ConnectionTimeout = 12,
        EncryptionInitializationFailed = 13,
        OpponentsDidNotJoin = 14,
        OpponentDidNotJoinKnockout = 15,
        UserDidNotJoinKnockout = 16,
        LoginError=17
    }
    public UserProfile me, opponent;
    public bool isRoomOwner;

    public delegate void SmartFoxManagerEvent();
    public event SmartFoxManagerEvent OnConnectRetrying, OnConnectionResumed, OnGameLoaded, UserDisconnected, OpponentMissedPing;

    //public delegate void SmartFoxManagerUsersEvent(List<UserProfile> user);
    //public event SmartFoxManagerUsersEvent OnRoomJoined, MatchFound;

    public delegate void SmartFoxManagerUserEvent(UserProfile user);
    public event SmartFoxManagerUserEvent OnRoomJoined, OnStartBattle;

    public delegate void SmartFoxOpponentFinishedEvent(int userId);
    public event SmartFoxOpponentFinishedEvent OpponentFinished;

    public delegate void SmartFoxManagerMatchUserEvent(UserProfile user, List<UserProfile> users, int timeLeft);
    public event SmartFoxManagerMatchUserEvent MatchUserIn, MatchUserOut;

    public delegate void SmartFoxManagerKnockoutMatchUserEvent(List<UserProfile> users, int timeLeft, string roundName, int minPlayers, List<UserProfile> joinedUsers, UserProfile joinedUser);
    public event SmartFoxManagerKnockoutMatchUserEvent KnockoutMatchUserIn;

    public delegate void SmartFoxManagerGroupChangeEvent(List<UserProfile> users, int timeLeft);
    public event SmartFoxManagerGroupChangeEvent MatchGroupChange;


    public delegate void SmartFoxManagerUsersEvent(List<UserProfile> user);
    public event SmartFoxManagerUsersEvent MatchFound;

    public delegate void SmartFoxOpponentDidnotJoinKnockout(List<UserProfile> users, int winnerId, List<UserProfile> joinedUsers);
    public event SmartFoxOpponentDidnotJoinKnockout OpponentDidnotJoinKnockout;


    public delegate void SmartFoxManagerUserVarsUpdatedEvent(User user, List<string> changedVars);
    public event SmartFoxManagerUserVarsUpdatedEvent OnUserVarsUpdated;

    public delegate void SmartFoxManagerFinishEvent(List<MPLSdkBattleFinishPlayersInfo> mPLSdkBattleFinishPlayersInfos, bool isFightAgainDisabled);
    public event SmartFoxManagerFinishEvent OnBattleFinish;

    public delegate void SmartFoxManagerFailureEvent(SmartFoxManagerReasons reason);
    public event SmartFoxManagerFailureEvent OnRoomJoinFailed;

    public delegate void SmartFoxManagerConnectionLostEvent(MPLGameEndReason.GameEndReasons reason);
    public event SmartFoxManagerConnectionLostEvent ConnectionLost, ConnectionFailed;



    public delegate void SmartFoxManagerFightAgainChangeEvent(Dictionary<string, string> state);
    public event SmartFoxManagerFightAgainChangeEvent OnFightAgainStateChange;

    public delegate void SmartFoxManagerServerErrorEvent(ServerErrorData serverErrorData);
    public event SmartFoxManagerServerErrorEvent OnServerError;

    public delegate void SmartFoxManagerScoreSubmittedEvent(SessionResult result);
    public event SmartFoxManagerScoreSubmittedEvent OnScoreSubmitted;

    //public delegate void SmartFoxManagerObjectMessageEvent(string cmd, SFSObject obj);
    //public event SmartFoxManagerObjectMessageEvent ObjectMessageReceived;

    public delegate void SmartFoxManagerClientBroadcastEvent(SFSObject obj);
    public event SmartFoxManagerClientBroadcastEvent OnClientBroadcast;

    public MPLGameConfig gameConfig;
    public string gameConfigStr;
    #endregion

    private static bool created = false;

    #region MonoBehaviourCallbacks
    void Awake()
    {
        if (!created)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        Application.runInBackground = true;

       
        gameConfig = MPLController.Instance.gameConfig;
        
        if (MPLController.Instance.gameConfig.PingInterval > 0)
        {
            pingTimer = new System.Timers.Timer(MPLController.Instance.gameConfig.PingInterval * 1000);
            pingTimer.Elapsed += HandlePingTimer;
        }
    }
    string connectionMode;
    void Update()
    {
        timeSpent += Time.deltaTime;

        // As Unity is not thread safe, we process the queued up callbacks on every frame
        if (smartFox != null)
        {
            smartFox.ProcessEvents();
        }

        float hasntGreconnectedFor = Time.realtimeSinceStartup - gReconnectionInitiatedAt;
        //Debug.Log("MPL: SFS2X Update = " + CanGReconnect() + ", " + Time.realtimeSinceStartup + "-" + gReconnectionInitiatedAt + " = " + hasntGreconnectedFor + ">" + gReconnectionMaxTimeAllowed + ", " + connectionLostCalled);
        if (CanGReconnect() && gReconnectionInitiatedAt>0 && hasntGreconnectedFor>=gReconnectionMaxTimeAllowed && !connectionLostCalled)
        {
            Debug.Log("MPL: SFS2X, greconnection max time finished: " + hasntGreconnectedFor + ">" + gReconnectionMaxTimeAllowed);
            gReconnectPingState = "NO";
            Disconnect(MPLGameEndReason.GameEndReasons.GRECONNECTION_MAX_TIME_FINISHED);
        }
    }


    void OnApplicationQuit()
    {
        StopPinger();
        if (IsConnected())
        {
            SmartFoxManager.Instance.SetConnectionFlags("Application Quit");
            Disconnect(MPLGameEndReason.GameEndReasons.APPLICATION_QUIT);
        }
    }
    #endregion

    #region PrivateFunctions
    void SetState(BattleState battleState)
    {
        if(MPLController.Instance.IsThirdPartyGame())
        {
            return;
        }
        Debug.Log("Battle state changed from " + this.battleState + " to " + battleState);
        this.battleState = battleState;
        try
        {
            if (this.battleState != BattleState.ConnectionLost) MPLController.Instance.battleRoomEventProperties.SetBattleState(this.battleState.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Battle state couldn't be set in battleRoomEventProperties: " + e);
        }
    }

    private void CleanUp()
    {
        if (smartFox == null)
        {
            Debug.Log("MPL: SFS2X smartFox already null");
            return;
        }

        smartFox.RemoveAllEventListeners();



        smartFox = null;
    }

    void CreateRoom()
    {
        RoomSettings settings = new RoomSettings("Piggy");
        settings.MaxUsers = 40;
        smartFox.Send(new CreateRoomRequest(settings));
    }
    private void StopSubmitResultLoop()
    {
        if (this.submitResultCoroutine != null)
        {
            StopCoroutine(this.submitResultCoroutine);
            this.submitResultCoroutine = null;
            Debug.Log("Stopped submitResultCoroutine");
        }
    }
    void BattleFinished(BattleFinishData battleFinishData)
    {
        SetState(BattleState.Results);

        if (battleFinished)
        {
            Debug.Log("MPL: SFS2X battle already finished, can't finish again");
            return;
        }

        if (!scoreSubmitted)
        {
            Debug.Log("MP: SFS2X score not submitted but battle ended, let's submit score first");
            Session.Instance.EndGame(MPLGameEndReason.GameEndReasons.BATTLE_ALREADY_FINISHED, "Battle already finished");
            return;
        }
        

        battleFinished = true;

        Debug.Log("MPL: SFS2X battle finish data = " + battleFinishData + ", me = " + gameConfig.Profile.id + ", winner = " + battleFinishData.winner.mplUserId);

        StopSubmitResultLoop();
        List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList = new List<MPLSdkBattleFinishPlayersInfo>();
        FinalScore[] scores = battleFinishData.scores;
        for (int i = 0; i < scores.Length; i++)
        {
            FinalScore score = scores[i];
            double cashWinnings = 0, tokenWinnings = 0;
            bool isCashReward = true;
            string extReward = "";
            Reward[] rewards = score.rewards;
            if (rewards.Length == 2)
            {
                if (rewards[0].currency.ToUpper() == "CASH")
                {
                    cashWinnings = rewards[0].amount;
                    isCashReward = rewards[0].isCashReward;
                    extReward = rewards[0].extReward;

                    tokenWinnings = rewards[1].amount;

                }
                else
                {
                    cashWinnings = rewards[1].amount;
                    isCashReward = rewards[1].isCashReward;
                    extReward = rewards[1].extReward;

                    tokenWinnings = rewards[0].amount;
                }
            }
            else if (rewards.Length == 1)
            {
                if (rewards[0].currency.ToUpper() == "CASH")
                {
                    cashWinnings = rewards[0].amount;
                    isCashReward = rewards[0].isCashReward;
                    extReward = rewards[0].extReward;



                }
                else
                {
                    tokenWinnings = rewards[0].amount;
                }
            }
            else
            {
                cashWinnings = tokenWinnings = 0;
            }

            MPLSdkBattleFinishPlayersInfo mPLSdkBattleFinishPlayersInfo = new MPLSdkBattleFinishPlayersInfo(score.mplUserId, score.finalScore, score.rank, cashWinnings, tokenWinnings, score.canPlayAgain, score.nextLobbySuggestedConfig, score.extraInfo, isCashReward, extReward);
            mPLBattleFinishPlayersInfoList.Add(mPLSdkBattleFinishPlayersInfo);
        }
        bool amIWinner = (gameConfig.Profile.id == battleFinishData.winner.mplUserId);

        OnBattleFinish?.Invoke(mPLBattleFinishPlayersInfoList, battleFinishData.isFightAgainDisabled);
    }

    private void GreconnectPingCheck()
    {
        gReconnectPingState =MultiplayerGamesHandler.Instance.checkIfConnectedToInternet() ? "YES" : "NO";
        Debug.Log("GreconnectPingCheck = " + gReconnectPingState);
    }
	
    IEnumerator RetryConnection()
    {
        if (CanGReconnect())
        {
            Debug.Log("MPL: SFS2X greconnection retry timeout in " + gReconnectionRetries);
            if (!IsConnected() && gReconnectionRetries > 0)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("MPL: SFS2X greconnection retry");
                gReconnectionRetries--;
                Connect();
                yield break;
            }

            if (gReconnectionRetries <= 0)
            {
                Debug.Log("MPL: SFS2X greconnection timdeout");
                SetConnectionFlags("MPL: SFS2X greconnection timdeout");
                CallConnectionLost(MPLGameEndReason.GameEndReasons.GRECONNECTION_RETRIES_FINISHED);
            }
        }
        else
        {
            Debug.Log("MPL: SFS2X connection retry timeout in " + connectionRetries);
            if (!IsConnected() && connectionRetries > 0)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("MPL: SFS2X connection retry");
                connectionRetries--;
                Connect();
            }

            if (connectionRetries <= 0)
            {
                Debug.Log("MPL: SFS2X Connection timeout");
                SetConnectionFlags("SFS2X Connection timeout");
                ConnectionFailed?.Invoke(MPLGameEndReason.GameEndReasons.CONNECTION_RETRY_TIMEOUT);
            }
        }
    }

    
    public void StopMonitoringAndCallLost()
    {
        
        SetConnectionFlags("SFS2X monitoring retry timed out");
        CallConnectionLost(MPLGameEndReason.GameEndReasons.MONITIRING_TIMEOUT);
    }

    void CallConnectionLost(MPLGameEndReason.GameEndReasons reason)
    {
        SetState(BattleState.ConnectionLost);

        Debug.Log("connectionLostCalled = " + connectionLostCalled);
        
        if (connectionLostCalled) return;

        if (gReconnect && gReconnectionAttempt > 0) SubmitGreconnectionConcluded(false);
        
        if (ConnectionLost != null)
        {
            connectionLostCalled = true;
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                ConnectionLost?.Invoke(reason);
                connectionLostCalled = true;
            });
        }
    }

    public string IsConnectedToInternet()
    {
        string c = Session.Instance.isConnectedToInternet;

        if (gReconnect && gReconnectionAttempt > 0 && gReconnectPingState != null)
        {
            c = gReconnectPingState;
        }

        Debug.Log("IsConnectedToInternet = " + c);

        return c;
    }

    float lastPingTime;
    void HandlePingTimer(object sender, EventArgs e)
    {
        if (!IsConnected())
        {
            Debug.Log("MPL: SFS2X, can't ping when not connected = " + CanGReconnect() + " = " + gReconnectionInitiatedAt);
            StopPinger();

            if (CanGReconnect() && gReconnectionInitiatedAt == 0)
            {
                SetConnectionFlags("MPL: SFS2X HandlePingTimer not connected.");
                gReconnectionAttempt = gReconnectionAttempt + 1;
                SubmitReconnectionInitiated("Ping Missed");
                Disconnect(MPLGameEndReason.GameEndReasons.PONG_MISS_GRECONNECT);
                GreconnectPingCheck();
                Connect();
            }

            return;
        }

        if (Mathf.Abs(MPLController.Instance.timeSpent - lastPongT) > MPLController.Instance.gameConfig.MaxPongDelay)
        {
            Debug.Log("MPL: SFS2X, last pong was at " + lastPongT + ". Current T = " + MPLController.Instance.timeSpent + ". Max pong delay = " + MPLController.Instance.gameConfig.MaxPongDelay + ". Disconnecting");
            StopPinger();
            SetConnectionFlags("Smartfox Ping Pong TimeOut");
            Disconnect(MPLGameEndReason.GameEndReasons.PING_PONG_TIMEOUT);
            return;
        }

        if ((Mathf.Abs(MPLController.Instance.timeSpent - lastPongT) > MPLController.Instance.gameConfig.PingInterval)
            && CanGReconnect()
            && gReconnectionInitiatedAt == 0)
        {
            Debug.Log("MPL: SFS2X, last pong was at " + lastPongT + ". Current T = " + MPLController.Instance.timeSpent + ". Max pong delay = " + MPLController.Instance.gameConfig.PingInterval + ". Disconnecting for GReconnecting");
            gReconnectionAttempt = gReconnectionAttempt + 1;
            SubmitReconnectionInitiated("Pong Missed");
            Disconnect(MPLGameEndReason.GameEndReasons.PONG_MISS_GRECONNECT);
            GreconnectPingCheck();
            Connect();
            return;
        }

        Ping();

        transmitActiveMessage();
    }

    void Ping()
    {
        Debug.Log("MPL: SFS2X ping!" + MPLController.Instance.timeSpent);
        ISFSObject parameters = SFSObject.NewInstance();
        smartFox.Send(new ExtensionRequest(PING, parameters));
        lastPingTime = MPLController.Instance.timeSpent;
    }

    void StartPinger()
    {
        
        connectionRetries = connectionRetries = Session.Instance.mplController.gameConfig.ConnectionRetryTimeout;

        if (MPLController.Instance.gameConfig.PingInterval == 0) return;
        if (pingTimer.Enabled) return;

        Debug.Log("MPL: SFS2X starting ping timer");
        pingTimer.Start();
    }

    void StopPinger()
    {
        if (!pingTimer.Enabled) return;

        Debug.Log("MPL: SFS2X stopping ping timer");
        pingTimer.Stop();
    }

    #endregion

    #region PublicFunctions
    public static SmartFoxManager Instance
    {
        get { return instance; }
    }

    public void Connect()
    {
        connectionLostCalled = false;
        if (battleState == BattleState.ConnectionLost && !MPLController.Instance.IsThirdPartyGame()&& MultiplayerGamesHandler.Instance.GetGameState() != MultiplayerGamesHandler.MPLGameState.match_making) return;

        //Debug.Log("Connect 00 = " + CanGReconnect() + " = " + gReconnectForPause + " = " + Time.realtimeSinceStartup + " = " + gReconnectionInitiatedAt + " = " + (int)(Time.realtimeSinceStartup - gReconnectionInitiatedAt));
        if (CanGReconnect() && gReconnectForPause)
        {
            gReconnectForPause = false;
            int pausedFor = (int)(Time.realtimeSinceStartup - gReconnectionInitiatedAt);
            gReconnectionRetries -= pausedFor;
            //Debug.Log("Connect 11 = " + gReconnectionRetries);
            if (gReconnectionRetries <= 0) {
                Debug.Log("MPL: SFS2X, in bg for too long, gotta disconnect.");
                gReconnectPingState = "NO";
                CallConnectionLost(MPLGameEndReason.GameEndReasons.PAUSE_GRECONNECT);
            }
            else
            {
                Debug.Log("MPL: SFS2X, in bg for not too long, giving a chance to greconnect.");
                GreconnectPingCheck();
                //gReconnectionRetries = gReconnectionRetriesAllowed;
            }
        }

        gameConfig = MPLController.Instance.gameConfig;

        if (connectionRetries == -10) connectionRetries = Session.Instance.mplController.gameConfig.ConnectionRetryTimeout;
        
        manuallyDisconnected = false;
        isRoomOwner = false;
        //battleFinished = false;

        //Already connected
        if (IsConnected())
        {
            Debug.Log("MPL: SFS2X already connected, finding a room");
            FindRoom();
            Session.Instance.isConnectedToSmartFox = "YES";
            return;
        }

        StartConnection();
    }

    void StartConnection()
    {
        CleanUp();

        Debug.Log("MPL: Connecting SmartFox at=" + DateTime.Now);
        smartFox = new SmartFox();
        smartFox.ThreadSafeMode = true;
        smartFox.AddLogListener(LogLevel.INFO, OnInfoMessage);
        smartFox.AddLogListener(LogLevel.WARN, OnWarnMessage);
        smartFox.AddLogListener(LogLevel.ERROR, OnErrorMessage);

        smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
        smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        smartFox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        smartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
        smartFox.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
        smartFox.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);
        smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        smartFox.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
        smartFox.AddEventListener(SFSEvent.CONNECTION_RETRY, OnConnectionRetry);
        smartFox.AddEventListener(SFSEvent.CONNECTION_RESUME, OnConnectionResume);
        smartFox.AddEventListener(SFSEvent.CRYPTO_INIT, OnEncryptionInitialized);
        smartFox.AddEventListener(SFSEvent.OBJECT_MESSAGE, OnObjectMessage);
        smartFox.AddEventListener(SFSEvent.UDP_INIT, OnUdpInit);

        ConfigData cfg = new ConfigData();
        cfg.Host = Session.Instance.mplController.gameConfig.Host;
        cfg.Port = PORT;
        cfg.Zone = Session.Instance.mplController.gameConfig.Zone;
        cfg.Debug = false;
        Debug.Log("MPL SFX: Starting Connection");
        smartFox.Connect(cfg);

    }

    public void Disconnect(MPLGameEndReason.GameEndReasons reason)
    {
        if (smartFox == null)
        {
            return;
        }

        disconnectionReason = reason;
        Debug.Log("MPL: SFS2X disconnect called = " + disconnectionReason);

        if (reason != MPLGameEndReason.GameEndReasons.PAUSE_GRECONNECT && reason != MPLGameEndReason.GameEndReasons.PONG_MISS_GRECONNECT) manuallyDisconnected = true;

        smartFox.Disconnect();
        SetConnectionFlags(reason.ToString());
        if (MPLController.Instance.IsUnityDeviceDebug() && reason == MPLGameEndReason.GameEndReasons.LOGIN_ERROR)
        {

            OnRoomJoinFailed(SmartFoxManagerReasons.LoginError);
        }
        else
        {
            if (reason == MPLGameEndReason.GameEndReasons.PING_PONG_TIMEOUT)
                CallConnectionLost(MPLGameEndReason.GameEndReasons.CONNECTION_LOST);
            CleanUp();

            if (reason == MPLGameEndReason.GameEndReasons.PAUSE_GRECONNECT || reason == MPLGameEndReason.GameEndReasons.PONG_MISS_GRECONNECT)
            {
                if (reason == MPLGameEndReason.GameEndReasons.PAUSE_GRECONNECT)
                {
                    gReconnectForPause = true;
                    SmartFoxManager.Instance.gReconnectPingState = "NO";
                }
                return;
            }


            CallConnectionLost(reason);
        }
    }

    public bool IsConnected()
    {
        //Debug.Log("MPl: SmartFox Dhanesh Is Connected=" + smartFox.IsConnected);
        return (smartFox != null && smartFox.IsConnected);
    }

    public bool IsInRoom()
    {
        return (IsConnected() && smartFox.JoinedRooms.Count > 0);
    }

    public Sfs2X.Entities.Room GetJoinedRoom()
    {
        if (!IsInRoom())
        {
            Debug.Log("MPL: SFS2X no room joined");
            return null;
        }

        Sfs2X.Entities.Room room = smartFox.LastJoinedRoom;
        //Debug.Log("MPL: SFS2X getting last joined room = " + room);
        return room;
    }

    public void Login(string userName, string password)
    {
        Debug.Log("MPL: SFS2X Logging in");

        UserProfile profile = MPLController.Instance.gameConfig.Profile;

        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutUtfString(AUTH_TOKEN, Session.Instance.mplController.gameConfig.AuthToken);
       // Debug.Log("User profile = " + profile);
        parameters.PutUtfString(NAME, profile.displayName);
        
        parameters.PutUtfString(MOBILE_NUMBER, profile.mobileNumber);
        
        parameters.PutUtfString(AVATAR, profile.avatar);
        
        parameters.PutUtfString(TIER, profile.tier);
        
        parameters.PutBool(IS_PRO, profile.pro);
        
        parameters.PutBool(IS_IN_DEBUG_MODE, Session.Instance.mplController.gameConfig.IsInDebugMode);
        
        parameters.PutInt(APP_VERSION, System.Convert.ToInt32(MPLController.Instance.gameConfig.AppVersionCode));
        
        parameters.PutInt(GAME_VERSION, MPLController.Instance.gameConfig.InstalledApkVersionCode);
        parameters.PutUtfString(COUNTRY_CODE, MPLController.Instance.gameConfig.CountryCode);

       
        
        userName = Guid.NewGuid().ToString();
        smartFox.Send(new LoginRequest(userName, password, Session.Instance.mplController.gameConfig.Zone, parameters));
        Debug.Log("login request sent");
    }

    public void FindRoom()
    {
        Debug.Log("MPL: SFS2X can find room? = " + CanGReconnect() + " = " + MultiplayerGamesHandler.Instance.isThisFindAgain);
        if (CanGReconnect() && !MultiplayerGamesHandler.Instance.isThisFindAgain && MultiplayerGamesHandler.Instance.GetGameState() != MultiplayerGamesHandler.MPLGameState.match_making) return;

        SetState(BattleState.MatchMaking);
        StopSubmitResultLoop();
        connectionLostCalled = false;

        Debug.Log("MPL: SFS2X finding room at : " + DateTime.Now);
        //FightAgain(false);

        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutInt(LOBBY_ID, Session.Instance.mplController.gameConfig.LobbyId);
        parameters.PutInt(RETRY, mmRetryAttempt);
        smartFox.Send(new ExtensionRequest(FIND_MATCH, parameters));
        Session.Instance.pingDurations = new List<double>();
    }

    public void JoinRoom(string roomName = "")
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("MPL: SFS2X empty room name, will join the first room if any rooms exist in the zone");
            Debug.Log("MPL: SFS2X rooms in zone = " + smartFox.RoomList.Count);
            if (smartFox.RoomList.Count > 0)
            {
                smartFox.Send(new JoinRoomRequest(smartFox.RoomList[0].Name));
            }
            //else create room
        }
        else
        {
            Debug.Log("MPL: SFS2X joining " + roomName);
            smartFox.Send(new JoinRoomRequest(roomName));
        }
    }

    public void SendUserVariables(List<UserVariable> userVariables)
    {
        if (!IsConnected())
        {
            Debug.Log("MPL: SFS2X Can't send user variables when not connected = " + manuallyDisconnected);

            if (manuallyDisconnected)
            {
                Debug.Log("MPL: SFS2X Manually disconnected");
                SetConnectionFlags("Can't send user variables when not connected");
                CallConnectionLost(MPLGameEndReason.GameEndReasons.CONNECTION_LOST);
            }

            return;
        }

        //Debug.Log("PLAUYERS in room = " + GetJoinedRoom().PlayerList.Count);
        //Debug.Log("MPL: SFS2X Sending user variables = " + userVariables);
        smartFox.Send(new SetUserVariablesRequest(userVariables));
    }

    public void ReadyToPlay()
    {
        if (!IsConnected())
        {
            Debug.Log("MPL SFS2X: Can't send ready to play when not connected");
            return;
        }

        Debug.Log("MPL: SFS2X Telling server we're ready to play");
        //Debug.Log("MPL: SFS2X Joined rooms = " + smartFox.JoinedRooms.Count);
        ISFSObject parameters = SFSObject.NewInstance();
        smartFox.Send(new ExtensionRequest(ON_READY, parameters, GetJoinedRoom()));
    }

    public void GameLoaded()
    {
        if (!IsConnected())
        {
            Debug.Log("MPL SFS2X: Can't send game loaded when not connected");
            return;
        }

        Debug.Log("MPL: SFS2X Telling server game is loaded");
        ISFSObject parameters = SFSObject.NewInstance();
        smartFox.Send(new ExtensionRequest(ON_GAME_LOADED, parameters, GetJoinedRoom()));
    }

    public bool CanGReconnect()
    {
        //Debug.Log("Can gReconnect = " + gReconnect + " = " + battleState);
        return (gReconnect && battleState == BattleState.Fighting && !MPLController.Instance.IsThirdPartyGame());
    }

    public void SubmitResult(SessionResult result)
    {
        if (CanGReconnect())
        {
            this.submitResultCoroutine = SubmitResultUntilBattleFinished(result);
            StartCoroutine(submitResultCoroutine);
            return;
        }
        
        SubmitResultFinally(result);
    }

    private IEnumerator SubmitResultUntilBattleFinished(SessionResult result)
    {
        while (battleState != BattleState.Results)
        {
            Debug.Log("SubmitResultUntilBattleFinished = " + battleState + " = " + connectionLostCalled + " = " + battleFinished);
            if (connectionLostCalled) break;
            if (battleFinished) break;

            SubmitResultFinally(result);
            yield return new WaitForSeconds(2); //TODO: Can be configurable?
        }
        Debug.Log("SubmitResultUntilBattleFinished broken");
    }

    public void SubmitResultFinally(SessionResult result)
    {
        if (!IsConnected())
        {
            Debug.Log("MPL: Can't submit score when not connected");
            if (gReconnectionRetries <= 0)
            {
                CallConnectionLost(MPLGameEndReason.GameEndReasons.CONNECTION_ALREADY_LOST);
            }
            return;
        }

        if (battleFinished)
        {
            Debug.Log("MPL: Battle finished, can't submit result");
            return;
        }

        Debug.Log("MPL: SFS2X submitting score = " + result.Score + ", game data = " + result);

        ISFSObject parameters = SFSObject.NewInstance();

        parameters.PutInt(FINAL_SCORE, result.Score);


        parameters.PutUtfString(GAME_DATA, result.ToString());
        smartFox.Send(new ExtensionRequest(SUBMIT_SCORE, parameters, GetJoinedRoom()));
        scoreSubmitted = true;

        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            OnScoreSubmitted?.Invoke(result);
        });
    }

    public List<UserProfile> GetAllCurrentUsers(MatchFoundUserProfile[] matchUserProfiles)
    {
        currentUsers = new List<UserProfile>();
        if (matchUserProfiles == null)
        {
            Debug.Log("MPL: SFS2X cant return opponent because users are null");
            return currentUsers;
        }

        for (int i = 0; i < matchUserProfiles.Length; i++)
        {
            MatchFoundUserProfile profile = matchUserProfiles[i];
            currentUsers.Add(new UserProfile(profile.mplUserId, profile.mobileNumber, profile.displayName, profile.avatar, profile.tier, profile.isPro, profile.appVersion));
        }

        return currentUsers;
    }
    public List<UserProfile> GetAllUsers(MatchFoundData matchFoundData)
    {
        users = new List<UserProfile>();

        if (matchFoundData.users == null)
        {
            Debug.Log("MPL: SFS2X cant return opponent because users are null");
            return null;
        }

        for (int i = 0; i < matchFoundData.users.Length; i++)
        {
            MatchFoundUserProfile profile = matchFoundData.users[i];
            users.Add(new UserProfile(profile.mplUserId, profile.mobileNumber, profile.displayName, profile.avatar, profile.tier, profile.isPro, profile.appVersion));
        }

        return users;
    }

    public List<UserProfile> GetAllReadyUsers()
    {
        return users;
    }

    public UserProfile GetJoinedPlayer(MatchFoundUserProfile user)
    {
        if (user == null)
        {
            Debug.Log("MPL: SFS2X cant return player because users are null");
            return null;
        }
        MatchFoundUserProfile profile = user;
        UserProfile player = new UserProfile(profile.mplUserId, profile.mobileNumber, profile.displayName, profile.avatar, profile.tier, profile.isPro, profile.appVersion);
        return player;

    }
    public UserProfile GetOpponent(MatchFoundData matchFoundData)
    {
        Debug.Log("Getting oppponent...");
        Debug.Log("A = " + matchFoundData);
        Debug.Log("B = " + matchFoundData.users);
        Debug.Log("C = " + matchFoundData.users.Length);

        if (matchFoundData.users == null)
        {
            Debug.Log("MPL: SFS2X cant return opponent because users are null");
            return null;
        }

        if (matchFoundData.users.Length < 2)
        {
            Debug.Log("MPL: SFS2X cant return opponent because <2 users are in the room");
            return null;
        }

        for (int i = 0; i < matchFoundData.users.Length; i++)
        {
            MatchFoundUserProfile profile = matchFoundData.users[i];
            Debug.Log("MPL: APP VERSION= " + MPLController.Instance.gameConfig.AppVersionCode);
            Debug.Log("MPL: APP VERSION INT= " + System.Convert.ToInt32(MPLController.Instance.gameConfig.AppVersionCode));
            if (matchFoundData.users[i].mplUserId != MPLController.Instance.gameConfig.Profile.id)
            {
                opponent = new UserProfile(profile.mplUserId, profile.mobileNumber, profile.displayName, profile.avatar, profile.tier, profile.isPro, profile.appVersion);
            }
            else
            {
                me = new UserProfile(profile.mplUserId, profile.mobileNumber, profile.displayName, profile.avatar, profile.tier, profile.isPro, System.Convert.ToInt32(MPLController.Instance.gameConfig.AppVersionCode));
            }
        }



        if (me == null) me = opponent;

        //opponent.displayName = "" + UnityEngine.Random.Range(1, 1000);
        //me.displayName = "" + UnityEngine.Random.Range(1, 1000);
        if (opponent != null)
        {
            Debug.Log("Opponent is not null=" + opponent);
        }
        if (me != null)
        {
            Debug.Log("Me is not null=" + me);
        }

        Debug.Log("MPL: SFS2X opponent = " + opponent.displayName);
        Debug.Log("MPL: SFS2X me = " + me.displayName);

        return opponent;
    }

    void FightAgain(bool should)
    {
        if (!IsConnected())
        {
            Debug.Log("MPL: SFS2X can't send fight again when not connected");
            return;
        }

        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutBool(FIGHT_AGAIN, should);
        smartFox.Send(new ExtensionRequest(BATTLE_AGAIN, parameters));
    }
    void ChallengeCancel(string challengeId)
    {
        Debug.Log("MPL challengeId=" + challengeId);
        if (!IsConnected())
        {
            Debug.Log("MPL: SFS2X can't cancel challenge when not connected");
            return;
        }

        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutText(CHALLENGE_ID, challengeId);
        smartFox.Send(new ExtensionRequest(CANCEL_CHALLENGE, parameters));
    }


    public void BattleAgain(bool should)
    {
        Debug.Log("MPL: SFS2X requesting battle again");
        FightAgain(should);
    }
    

    public void GetAccountBalance(UnityAction<AccountBalance> callback)
    {
        Debug.Log("MPL: SFS2X fetching account balance");
        accountBalanceCallback = callback;
        ISFSObject parameters = SFSObject.NewInstance();
        if (smartFox != null)
        {
            smartFox.Send(new ExtensionRequest(GET_ACCOUNT_BALANCE, parameters));
        }
    }

    public void FraudDetected(string type, string proof)
    {
        Debug.Log("MPL: SFS2X fraud detected = " + type + " = " + proof);
        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutUtfString(FRAUD_TYPE, type);
        parameters.PutUtfString(FRAUD_PROOF, proof);
        smartFox.Send(new ExtensionRequest(FRAUD_DETECTED, parameters, GetJoinedRoom()));
    }

    public SmartFox GetSmartFoxInstance()
    {
        return smartFox;
    }

    public bool IsRoomOwner()
    {
        Debug.Log("MPL: SFS2X, my id = " + gameConfig.Profile.id + ", opponent id = " + opponent.id + ", is room owner = " + (me.id < opponent.id));

#if !UNITY_EDITOR
        return true;
#endif

        return false;
        //return (gameConfig.Profile.id > opponent.id);
    }
    public UserProfile GetCurrentUserProfile()
    {
        return gameConfig.Profile;
    }



    public void SendClientBroadcast(SFSObject data, bool useUdp)
    {
        if (!IsConnected())
        {
            Debug.Log("MPL: SFS2X can't send client broadcast when not connected");
            return;
        }

        if (useUdp && !smartFox.UdpInited) useUdp = false;

        string protocol = (useUdp) ? "UDP" : "TCP";

        Debug.Log("MPL: SFS2X sending client broadcast over udp? = " + useUdp + ", Protocol = " + protocol + "\nEventID " + data.GetUtfString("ID"));
        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutInt(COUNT, sentClientBroadcasts++);
        parameters.PutSFSObject(DATA, data);

        parameters.PutUtfString(PROTOCOL, protocol);

        smartFox.Send(new ExtensionRequest(SEND_BROADCAST, parameters, GetJoinedRoom(), useUdp));
    }
    #endregion

    #region SmartFoxEvents
    void OnInfoMessage(BaseEvent evt)
    {
        string message = (string)evt.Params["message"];
        Debug.Log(message);
    }

    void OnWarnMessage(BaseEvent evt)
    {
        string message = (string)evt.Params["message"];
        Debug.Log(message);
    }

    void OnErrorMessage(BaseEvent evt)
    {
        string message = (string)evt.Params["message"];
        Debug.Log(message);
    }

    void OnConnection(BaseEvent evt)
    {
        if (battleState == BattleState.ConnectionLost && !MPLController.Instance.IsThirdPartyGame() && MultiplayerGamesHandler.Instance.GetGameState()!=MultiplayerGamesHandler.MPLGameState.match_making) Disconnect(MPLGameEndReason.GameEndReasons.CONNECTION_ALREADY_LOST);

        bool success = (bool)evt.Params["success"];
        Debug.Log("MPL: SFS2X OnConnection = " + success);

        if (success)
        {
            Debug.Log("MPL: SFS2X Connection established successfully at=" + DateTime.Now);
            Debug.Log("MPL: SFS2X API version: " + smartFox.Version);
            Debug.Log("MPL: SFS2X Connection mode is: " + smartFox.ConnectionMode);
            Session.Instance.isConnectedToSmartFox = "YES";

            // Initialize encrypted connection
#if UNITY_WINRT && !UNITY_EDITOR
            Debug.Log("A-A");
            smartFox.InitCrypto();
#else
            Debug.Log("B-B");
            StartCoroutine(smartFox.InitCrypto());
#endif
        }
        else
        {
            Debug.Log("MPL: SFS2X Connection failed: is the server running at all?");
            StartCoroutine(RetryConnection());
        }
    }

    void OnEncryptionInitialized(BaseEvent evt)
    {
        if (battleState == BattleState.ConnectionLost&& !MPLController.Instance.IsThirdPartyGame()&& MultiplayerGamesHandler.Instance.GetGameState() != MultiplayerGamesHandler.MPLGameState.match_making) Disconnect(MPLGameEndReason.GameEndReasons.CONNECTION_ALREADY_LOST);

        Debug.Log("MPL: SFS2X OnEncryptionInitialized = " + evt.Params["success"]+" at="+DateTime.Now);
        if ((bool)evt.Params["success"])
        {
            Debug.Log("MPL: SFS2X OnEncryptionInitialized successful, loggin in");
            Login(Session.Instance.mplController.gameConfig.Profile.displayName, "");
            StartPinger();
            lastPongT = MPLController.Instance.timeSpent;
        }
        else
        {
            Debug.Log("MPL: SFS2X OnEncryptionInitialized failed, disconnecting");
            SetConnectionFlags("SFS2X OnEncryptionInitialized failed");
            CallConnectionLost(MPLGameEndReason.GameEndReasons.ENCRYPTION_INITIALIZED_FAILED);
            
        }
    }

    void OnConnectionLost(BaseEvent evt)
    {
        Debug.LogError("MPL: SFS2X Connection was lost; reason is: " + (string)evt.Params["reason"]);

        Debug.LogError("MPL: SFS2X can gReconnect? = " + CanGReconnect() + " = " + gReconnectionInitiatedAt);
        if (CanGReconnect())
        {
            StartCoroutine(Util.WaitAndExecute(0.5f, () =>
            {
                if (gReconnectionInitiatedAt > 0) return;
                Debug.LogError("MPL: SFS2X please gReconnect");
                gReconnectionAttempt = gReconnectionAttempt + 1;
                GreconnectPingCheck();
                SubmitReconnectionInitiated("Connection Lost");
                Connect();
             }));
        }
        else
        {
            OnConnectionLostFinally(evt);
        }
    }

    void OnConnectionLostFinally(BaseEvent evt)
    {
        Debug.LogError("MPL: SFS2X Connection was lost finally; reason is: " + (string)evt.Params["reason"]);

        SetConnectionFlags((string)evt.Params["reason"]);
        disconnectionReason = MPLGameEndReason.GameEndReasons.CONNECTION_LOST;
        CallConnectionLost(MPLGameEndReason.GameEndReasons.CONNECTION_LOST);

        StopPinger();
        CleanUp();
    }

    void OnLogin(BaseEvent evt)
    {
        if (battleState == BattleState.ConnectionLost && !MPLController.Instance.IsThirdPartyGame() && MultiplayerGamesHandler.Instance.GetGameState() != MultiplayerGamesHandler.MPLGameState.match_making) Disconnect(MPLGameEndReason.GameEndReasons.CONNECTION_ALREADY_LOST);

        User user = (User)evt.Params["user"];
        Debug.Log("MPL: SFS2X login succesful = " + user+" at= "+DateTime.Now);
        Ping();
        FindRoom();
        smartFox.InitUDP(gameConfig.Host, 9944);

        if (CanGReconnect()) {
            if (gReconnectionAttempt > 0) SubmitGreconnectionConcluded(true);
            gReconnectionRetries = gReconnectionRetriesAllowed;
            gReconnectForPause = false;
            gReconnectPingState = null;
        }
    }

    void OnLoginError(BaseEvent evt)
    {
        Debug.Log("MPL: SFS2X login error = " + (string)evt.Params["errorMessage"]);
        SetConnectionFlags((string)evt.Params["errorMessage"]);
        Disconnect(MPLGameEndReason.GameEndReasons.LOGIN_ERROR);
    }

    void OnRoomJoin(BaseEvent evt)
    {
        Sfs2X.Entities.Room room = (Sfs2X.Entities.Room)evt.Params["room"];
        Debug.Log("MPL: SFS2X joined room = " + room.Name);
    }

    void OnRoomJoinError(BaseEvent evt)
    {
        Debug.Log("MPL: SFSX2 join room error = " + (string)evt.Params["errorMessage"]);
        OnRoomJoinFailed(SmartFoxManagerReasons.RoomJoinError);
    }

    void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Sfs2X.Entities.Room room = (Sfs2X.Entities.Room)evt.Params["room"];

        Debug.Log("MPL: User entered room = " + user.Name + " = " + room.Name);
    }
    public void SetConnectionFlags(string extraReason)
    {
        
        Session.Instance.isConnectedToSmartFox = IsConnected() ? "YES" : "NO";
        MultiplayerGamesHandler.Instance.SetConnectionFlags(extraReason);
    }
    void OnUserExitRoom(BaseEvent evt)
    {
        if (battleFinished) return;

        User user = (User)evt.Params["user"];

        if (user == smartFox.MySelf) return;

        //1. If game doesn't start b4 this happens, it wont know, so it wont submit result. Handle this sheet. 
        //2. User will exit from earlier room as well. Handle that.
        //StartCoroutine(WaitAndExecutedUserDisconnected());

        Debug.Log("MPL: SFS2X - User disconnected");
        if (UserDisconnected != null) UserDisconnected();
        else
        {
            Debug.Log("BCBCBC User disconnected when UserDisconnected = null");
        }
    }

    void OnRoomAdded(BaseEvent evt)
    {
        Sfs2X.Entities.Room room = (Sfs2X.Entities.Room)evt.Params["room"];
        Debug.Log("MPL: SFS2X, A new Room was added: " + room.Name);
    }

    void OnRoomCreationError(BaseEvent evt)
    {
        Debug.Log("MPL: SFS2X, An error occurred while attempting to create the Room: " + (string)evt.Params["errorMessage"]);
    }

    void OnExtensionResponse(BaseEvent evt)
    {
        if(evt==null)
        {
            return;
        }
        if(evt.Params==null||evt.Params.Count==0)
        {
            return;
        }
        string cmd="";
        if (evt.Params.Contains("cmd"))
        {
             cmd= (string)evt.Params["cmd"];
        }
        else
        {
            return;
        }
        
        Debug.Log("MPL: SFS2X " + cmd + " extension response received at="+DateTime.Now);

        ISFSObject responseParams;
        if (evt.Params.Contains("params"))
        {
            responseParams = (SFSObject)evt.Params["params"];
        }
        else
        {
            return;
        }
         
        
         
        
        
        if (cmd == FIND_MATCH)
        {
        }
        else if (cmd == ON_READY)
        {

        }
        else if (cmd == GAME_LOADED)
        {
            OnGameLoaded?.Invoke();
            //StartCoroutine(Util.WaitAndExecute(10f, () =>
            //{
            //    smartFox.KillConnection(); //to test reconnection
            //}));
        }
        else if (cmd == START_BATTLE)
        {
            SetState(BattleState.Fighting);
            if (OnStartBattle != null) OnStartBattle(opponent);
        }
        else if (cmd == BATTLE_FINISHED)
        {
            BattleFinished(JsonConvert.DeserializeObject<BattleFinishData>(responseParams.ToJson()));
        }
        else if (cmd == FIND_MATCH_FAILED)
        {
            OnRoomJoinFailed?.Invoke(SmartFoxManagerReasons.FailedToFindMatch);
        }
        else if (cmd == OPPONENT_DID_NOT_JOIN)
        {
            OnRoomJoinFailed?.Invoke(SmartFoxManagerReasons.OpponentDidNotJoin);
        }
        else if (cmd == OPPONENTS_DID_NOT_JOIN)
        {
            OnRoomJoinFailed?.Invoke(SmartFoxManagerReasons.OpponentsDidNotJoin);
        }

        else if (cmd == OPPONENT_DID_NOT_JOIN_KNOCKOUT)
        {
            OpponentDidnotJoinKnockout?.Invoke(GetAllCurrentUsers(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).currentUsers), JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).winnerUserId, GetAllCurrentUsers(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).joinedUsers));
        }
        else if (cmd == USER_DID_NOT_JOIN_KNOCKOUT)
        {
            OnRoomJoinFailed?.Invoke(SmartFoxManagerReasons.UserDidNotJoinKnockout);
        }
        else if (cmd == GET_ACCOUNT_BALANCE)
        {
            accountBalanceCallback?.Invoke(JsonConvert.DeserializeObject<AccountBalance>(responseParams.ToJson()));
        }
        else if (cmd == GAME_END)
        {
            ServerGameEndData gameEndData = JsonConvert.DeserializeObject<ServerGameEndData>(responseParams.ToJson());

            string gameEndReason = "";
            if (string.IsNullOrEmpty(gameEndData.reason)) gameEndReason = "Server commanded EOG!";
            else if (gameEndData.reason == "OPPONENT_SCORED_LESS")
            {
                if (MPLController.Instance.gameConfig.TotalPlayers < 3)
                {
                    gameEndReason = "You have beaten " + opponent.displayName + "'s Score!";
                }
                else
                {
                    gameEndReason = "You have beaten all the players";
                }
            }
            else if (gameEndData.reason == "USER_FRAUD_DETECTED") gameEndReason = opponent.displayName + " commited a fraud";

            Debug.Log("MPL: SFS2X server commanded to end game with reason = " + gameEndData.reason + " = " + gameEndReason);
            //TODO: EndGame
            Session.Instance.EndGame(MPLGameEndReason.GameEndReasons.SERVER_GAME_END, gameEndReason);
        }
        else if (cmd == OPPONENT_FINISHED)
        {
            OpponentFinished?.Invoke(JsonConvert.DeserializeObject<OpponentFinishedData>(responseParams.ToJson()).finishedUser);
        }
        else if (cmd == FIGHT_AGAIN_STATE_CHANGED)
        {
            FightAgainState fightAgainState = JsonConvert.DeserializeObject<FightAgainState>(responseParams.ToJson());
            fightAgainState.InitData();
            OnFightAgainStateChange?.Invoke(fightAgainState.fightAgainStateDict);
        }
        else if (cmd == MATCH_FOUND)
        {

            MPLController.Instance.SetBattleSessionId(smartFox.LastJoinedRoom.Name);
            MPLController.Instance.gameConfig.SessionId = smartFox.LastJoinedRoom.Name;
            MPLController.Instance.battleRoomEventProperties.SetBattleId(GetBattleId());
            if (MPLSdkBridgeController.Instance != null)
            {
                MPLSdkBridgeController.Instance.roomName = smartFox.LastJoinedRoom.Name;
            }
            MatchFound?.Invoke(GetAllUsers(JsonConvert.DeserializeObject<MatchFoundData>(responseParams.ToJson())));

            battleFinished = false;
            scoreSubmitted = false;
            gReconnectionAttempt = 0;

            MatchFoundData matchFoundData = JsonConvert.DeserializeObject<MatchFoundData>(responseParams.ToJson());

            OnRoomJoined?.Invoke(GetOpponent(matchFoundData));
            GetOpponent(matchFoundData);
            MultiplayerGamesHandler.Instance.roomOwner = matchFoundData.owner;

            if (smartFox.MySelf.Id == MultiplayerGamesHandler.Instance.roomOwner)
            { isRoomOwner = true;
                Session.Instance.isRoomOwner = true;
            }

            else { isRoomOwner = false;
                Session.Instance.isRoomOwner = false;
            }            
            
            if (MPLController.Instance.IsThirdPartyGame())
            {
                connectionRetries = 0;
                MultiplayerGamesHandler.Instance.forceDisconnect=true;
                smartFox.Disconnect();
            }

            gReconnect = matchFoundData.gReconnect;
            gReconnectionRetriesAllowed = matchFoundData.gReconnectionRetries;
            gReconnectionMaxTimeAllowed = matchFoundData.gReconnectTimeout / 1000;
            gReconnectionRetries = gReconnectionRetriesAllowed;
            gReconnectionState = "Unused";
            gReconnectionsInitiated = 0;
            gReconnectionsConcluded = 0;
            
        }
        else if (cmd == MATCH_USER_IN)
        {
            Debug.Log("SFX:Match User In");
            MatchUserIn?.Invoke(GetJoinedPlayer(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).user), GetAllCurrentUsers(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).currentUsers), JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).timeLeft);
        }
        else if (cmd == MATCH_USER_OUT)
        {
            Debug.Log("SFX:Match User Out");
            MatchUserOut?.Invoke(GetJoinedPlayer(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).user), GetAllCurrentUsers(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).currentUsers), JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).timeLeft);
        }

        else if (cmd == MATCH_GROUP_CHANGE)
        {
            Debug.Log("SFX: Group Change");
            MatchGroupChange?.Invoke(GetAllCurrentUsers(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).currentUsers), JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).timeLeft);
        }
        else if (cmd == KNOCKOUT_MATCH_USER_IN)
        {
            Debug.Log("SFX:KNOCKOUT Match User In");
            MatchUserInData matchUserInData = JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson());
            KnockoutMatchUserIn?.Invoke(GetAllCurrentUsers(matchUserInData.currentUsers), matchUserInData.timeLeft, matchUserInData.roundName, matchUserInData.minPlayers, GetAllCurrentUsers(matchUserInData.joinedUsers), GetJoinedPlayer(JsonConvert.DeserializeObject<MatchUserInData>(responseParams.ToJson()).joinedUser));
        }

        else if (cmd == KNOCKOUT_MATCH_USER_OUT)
        {
            Debug.Log("SFX:KNOCKOUT Match User Out");

        }
        else if (cmd == SERVER_ERROR)
        {
            SetConnectionFlags(JsonConvert.DeserializeObject<ServerErrorData>(responseParams.ToJson()).title);
            OnServerError?.Invoke(JsonConvert.DeserializeObject<ServerErrorData>(responseParams.ToJson()));
        }
        else if (cmd == PONG)
        {
            lastPongT = MPLController.Instance.timeSpent;

            if (Session.Instance!=null && Session.Instance.pingDurations != null)
            {
                Session.Instance.pingDurations.Add(Math.Abs(lastPongT - lastPingTime));
            }
            Debug.Log("MPL: Pong time=" + lastPongT);
        }
        else if (cmd == CLIENT_BROADCAST)
        {
            //TODO: Should receive ACK in bg, or not???

            int count = responseParams.GetInt(COUNT);
            SFSObject broadcastData = (SFSObject)responseParams.GetSFSObject(DATA);

            if (broadcastData.ContainsKey(KEY_RM_META))
            {
                Debug.Log("MPL: SFS2X extension response received Event = " + broadcastData.GetUtfString("ID") + "\n***processClientBroadcast *** = " + broadcastData.ContainsKey(KEY_RM_META));
                processClientBroadcast(broadcastData);
            }
            else
            {
                OnClientBroadcast?.Invoke(broadcastData);
            }

            if (count < receivedClientBroadcasts)
            {
                //Debug.Log("MPL: SFS2X total received client broadcasts = " + receivedClientBroadcasts + ", this one's count = " + count + ". Returning");
                //return;
            }

            receivedClientBroadcasts++;
        }
        else if (cmd == OPPONENT_MISSED_PING)
        {
            OpponentMissedPing?.Invoke();
        }
    }

    public string GetBattleId()
    {
        Room battleRoom = GetJoinedRoom();
        return battleRoom != null ? battleRoom.Name : "";
    }

    void OnUserVariableUpdate(BaseEvent evt)
    {

        SFSUser user = (SFSUser)evt.Params["user"];

        if (user == smartFox.MySelf) return;

        List<string> changedVars = (List<string>)evt.Params["changedVars"];

        //for (int i = 0; i < changedVars.Count; i++)
        //{
        //    Debug.Log("Changed Variable = " + changedVars[i]);
        //}

        if (OnUserVarsUpdated != null)
        {
            OnUserVarsUpdated.Invoke(user, changedVars);
        }


        if (changedVars.Contains(SCORE))
        {
            Debug.Log("MPL: SFS2X user vars updated = " + user.Name + " = " + changedVars[0]);
            if (OnUserVarsUpdated != null) OnUserVarsUpdated.Invoke(user, changedVars);
        }

    }

    void OnConnectionRetry(BaseEvent evt)
    {
        Debug.Log("MPL SFS2X retrying connection");
        OnConnectRetrying?.Invoke();
    }

    void SwitchConnection()
    {
        smartFox.Disconnect();
        CleanUp();
        StartConnection();
    }

    void OnConnectionResume(BaseEvent evt)
    {
        Debug.Log("MPL SFS2X connection resumed");
        OnConnectionResumed?.Invoke();
    }

    void OnObjectMessage(BaseEvent evt)
    {
        SFSObject dataObj = (SFSObject)evt.Params["message"];
        string commandID = dataObj.GetUtfString("ID");

        Debug.Log("MPL: SFS2X Object Message received = " + commandID);

        //if (ObjectMessageReceived != null) ObjectMessageReceived(commandID, dataObj);
    }

    void OnUdpInit(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            Debug.Log("UDP init sucessful!");
        }
        else
        {
            Debug.Log("UDP init failed!");
        }

       // FindRoom();
    }

    public void SubmitReconnectionInitiated(string reason)
    {
        gReconnectionInitiatedAt = Time.realtimeSinceStartup;
        gReconnectionsInitiated = gReconnectionsInitiated + 1;
        gReconnectionState = "Unsuccessful";
        string state = MultiplayerGamesHandler.Instance == null ? "" : MultiplayerGamesHandler.Instance.GetPingState();
        double avg = MultiplayerGamesHandler.Instance == null ? 0 : MultiplayerGamesHandler.Instance.GetAvgPing();
        greconnectionInitiatedEventProperties = new GreconnectionInitiatedEventProperties
        (
            MPLController.Instance.battleRoomEventProperties,
            gReconnectionAttempt,
            gReconnectionRetriesAllowed,
            Session.Instance.isConnectedToInternet,
            Session.Instance.isConnectedToSmartFox,
            Session.Instance.isConnectionModeSwitched,
            Session.Instance.userMinimised,
            reason,
            avg,
            state
        );
        MPLSdkBridgeController.Instance.SubmitEvent(GreconnectionInitiatedEventProperties.EVENT_NAME, greconnectionInitiatedEventProperties.ToString());
    }


    public void SubmitGreconnectionConcluded(bool success)
    {
        if (gReconnectionInitiatedAt == 0) return;
        float greconnectionTimeLeft = Time.realtimeSinceStartup - gReconnectionInitiatedAt;
        gReconnectionInitiatedAt = 0;
        if (success)
        {
            gReconnectionsConcluded = gReconnectionsConcluded + 1;
            gReconnectionState = "Successful";
        }
        string state = MultiplayerGamesHandler.Instance == null ? "" : MultiplayerGamesHandler.Instance.GetPingState();
        double avg = MultiplayerGamesHandler.Instance == null ? 0 : MultiplayerGamesHandler.Instance.GetAvgPing();
        GreconnectionConcludedEventProperties eventProperties = new GreconnectionConcludedEventProperties
        (
            MPLController.Instance.battleRoomEventProperties,
            gReconnectionAttempt,
            success,
            (int)greconnectionTimeLeft,
            Session.Instance.isConnectedToInternet,
            Session.Instance.isConnectedToSmartFox,
            Session.Instance.isConnectionModeSwitched,
            avg,
            state,
            gReconnectionRetries,
            (greconnectionInitiatedEventProperties == null) ? "" : greconnectionInitiatedEventProperties.reason
        );
        MPLSdkBridgeController.Instance.SubmitEvent(GreconnectionConcludedEventProperties.EVENT_NAME, eventProperties.ToString());
    }

    #endregion

    #region Kaustubh
    private int rmCounter = 0;
    private int ackCounter = 0;
    private ReliableMessage activeMessage;
    private long activeMessageTransmissionTime;
    private Queue<ReliableMessage> rmQueue = new Queue<ReliableMessage>();

    private const String KEY_RM_META = "__RM_META__";
    private const String KEY_RM_PARAM_TYPE = "type";
    private const String KEY_RM_PARAM_ID = "id";
    private const String KEY_RM_TYPE_MSG = "MSG";
    private const String KEY_RM_TYPE_ACK = "ACK";

    public void sendReliableMessage(SFSObject data)
    {
        int id = Interlocked.Increment(ref rmCounter);
        ReliableMessage reliableMessage = new ReliableMessage(id, data);
        rmQueue.Enqueue(reliableMessage);
        processNextReliableMessage();
    }

    void processNextReliableMessage()
    {
        if (activeMessage != null || rmQueue.Count == 0) return;
        activeMessage = rmQueue.Dequeue();
        transmitActiveMessage();
    }

    void transmitActiveMessage()
    {
        if (activeMessage == null || (DateTime.Now.Ticks - activeMessageTransmissionTime) < 2000) return;
        activeMessageTransmissionTime = DateTime.UtcNow.Ticks;
        SendClientBroadcast(activeMessage.data, false);
    }

    void processClientBroadcast(SFSObject data)
    {
        SFSObject rmMeta = (SFSObject)data.GetSFSObject(KEY_RM_META);
        if (rmMeta == null) return; // Print Error, Should not happen

        String type = rmMeta.GetText(KEY_RM_PARAM_TYPE);
        int id = rmMeta.GetInt(KEY_RM_PARAM_ID);

        switch (type)
        {
            case KEY_RM_TYPE_MSG:
                SFSObject ackMeta = new SFSObject();
                ackMeta.PutText(KEY_RM_PARAM_TYPE, KEY_RM_TYPE_ACK);
                ackMeta.PutInt(KEY_RM_PARAM_ID, id);
                SFSObject outData = new SFSObject();
                outData.PutSFSObject(KEY_RM_META, ackMeta);
                SendClientBroadcast(outData, false);
                if (id > ackCounter)
                {
                    if (OnClientBroadcast != null) OnClientBroadcast(data);
                }
                break;
            case KEY_RM_TYPE_ACK:
                if (activeMessage != null && activeMessage.id == id)
                {
                    activeMessage = null;
                }
                processNextReliableMessage();
                break;
                //default:
                // Print error, should not happen
        }
    }

    private class ReliableMessage
    {
        public int id;
        public SFSObject data;

        public ReliableMessage(int id, SFSObject data)
        {
            this.id = id;
            this.data = data == null ? new SFSObject() : data;
            SFSObject rmMeta = new SFSObject();
            rmMeta.PutText(KEY_RM_PARAM_TYPE, KEY_RM_TYPE_MSG);
            rmMeta.PutInt(KEY_RM_PARAM_ID, id);
            this.data.PutSFSObject(KEY_RM_META, rmMeta);
        }
    }
    #endregion
}

//Need from server to connect ::
//1. Zone
//2. UserName, pass
//3. RoomName, pass

//Set user properties b4 connect : name, avatar, isPro, tier
//Set highly resilient connection
//Game paused = end
//On fraud detected

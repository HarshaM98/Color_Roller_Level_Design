using System;
using System.Collections.Generic;

using UnityEngine;



[Serializable]
public class MPLGameConfig
{
    /******************************** Global Variables & Constants ********************************/
    /// <summary>
    /// The id of the game to start.
    /// </summary>
    ///
    public DynamicReward[] DynamicRewards;
    public int GameId;
    public int TouchTimeInterval;
    public bool IsInteractiveTrainingEnabled;
    public bool IsAsyncGame;
	public bool IsInteractiveTrainingLobby;
    public bool IsIntractiveFtueEnabled;
    public bool SingleEntryBattle;
    public string GameName;
    public string AppId;
    public bool IsAnalytics;
	public bool IsAgencyBuild;
    public int InstalledApkVersionCode;
    public bool FraudTestEnabled;
    public bool UserRatingEnabled;
    public int eloMultiple;
    public bool IsPingCheckEnabled=true;
    public bool IsRealityCheckEnabled;
    public bool Is1vN;
    public int DelayStart;
    public int RealityCheckTimerDuration;
    public int RealityCheckSessions;
    public int MaxMatchMakingRetries;
    public string[] FraudCheckEnabledGameIds;
    public string FraudWarningMessage;
    public string FraudBlockMessage;
    public bool FraudBlockEnabled;
    public bool IsKnockoutLobby;
    public double WinningAmount;
    public string HostUrl;
    public bool WelcomeLobby;
    /// <summary>
    /// The code of the country.
    /// </summary>
    public string CountryCode;
    public int UserRating;
    public int UserMatchRatingMin;
    public int UserMatchRatingMax;
    public bool IsAutoStartEnabled;
    public bool IsUpsellEnabled;
    public float AutoStartTimer;
    public int PlayerCount;
    public string CallbackUrl;
    public int TimeOutCounter;
    public bool sponsorBattle;
    public bool IsFTUE;
    public int SponsorBattleId;
   
    /// <summary>
    /// Tournament end time (in millis).
    /// </summary>
    public long TournamentEndTime;

    

    /// <summary>
    /// Current time (in millis).
    /// </summary>
    public long StartTime;
   
    /// <summary>
    /// The session id.
    /// </summary>
    public string SessionId;
    public int TotalPlayers;
    /// <summary>
    /// Tournament id.
    /// </summary>
    public int TournamentId;
    public string mode;
    /// <summary>
    /// Tournament name.
    /// </summary>
    public string TournamentName;

    /// <summary>
    /// Game config name.
    /// </summary>
    public string GameConfigName;

    /// <summary>
    /// Maximum Pause Duration
    /// </summary>
    public int MaxPauseDuration;

    /// <summary>
    /// Mobile number
    /// </summary>
    public string MobileNumber;

    /// <summary>
    /// React version
    /// </summary>
    public string ReactVersion;

    /// <summary>
    /// Score Log Interval
    /// </summary>
    public long SequenceInterval;

    /// <summary>
    /// Is Log Enabled
    /// </summary>
    public bool IsLogEnabled;

    /// <summary>
    /// Is Device Rooted
    /// </summary>
    public bool IsRooted;

    public bool MultiWinners = true;

    /// <summary>
    /// Is Mod app found on device
    /// </summary>
    public bool IsModAppFound;

    /// <summary>
    /// Is Sponsored
    /// </summary>
    public bool IsSponsored;

    /// <summary>
    /// Sponsor id
    /// </summary>
    public int SponsorId;

    /// <summary>

    /// Is1v1
    /// </summary>
    public bool Is1v1;

    /// <summary>
    /// Auth Token
    /// </summary>
    public string AuthToken;

    /// <summary>
    /// Lobby id
    /// </summary>
    public int LobbyId;

    public bool isItQuitByAndroid;

    /// <summary>
    /// User profile
    /// </summary>
    public UserProfile Profile;

    public Participants[] participants;

    public string[] LoadingMessages1v1;


    /// <summary>
    /// SFS2X Host
    /// </summary>
    public string Host;
   

    /// <summary>
    /// SFS2X Zone
    /// </summary>
    public string Zone;

    /// <summary>
    /// Retry Timeout
    /// </summary>
    public int ConnectionRetryTimeout;

    /// <summary>
    /// Is in debug mode (SFS2X)
    /// </summary>
    public bool IsInDebugMode;

    /// <summary>
    /// Entry fee
    /// </summary>
    public double EntryFee;

    /// <summary>
    /// Entry currency
    /// </summary>
    public string EntryCurrency;


    /// <summary>
    /// Entry currency
    /// </summary>
    public string WinningCurrency;

    /// <summary>
    /// Entry Point
    /// </summary>
    public string EntryPoint; //Ask React

    /// <summary>
    /// Ping Interval
    /// </summary>
    public int PingInterval;

    /// <summary>
    /// Match Found Timeout
    /// </summary>
    public int CreateGameTimeout;

    /// <summary>
    /// Tournament Type
    /// </summary>
    public string TournamentType;

    /// <summary>
    /// Cash Entry Fee
    /// </summary>
    public string CashEntryFee;

    /// <summary>
    /// Token Entry Fee
    /// </summary>
    public string TokenEntryFee;

    /// <summary>
    /// Prize Currency
    /// </summary>
    public string PrizeCurrency;

    /// <summary>
    /// Cash Prize Offered
    /// </summary>
    public string CashPrizeOffered;

    /// <summary>
    /// Token Prize Offered
    /// </summary>
    public string TokenPrizeOffered;

    /// <summary>
    /// Tournament Style
    /// </summary>
    public string TournamentStyle;

    /// <summary>
    /// Winners End Rank
    /// </summary>
    public string WinnersEndRank;

    /// <summary>
    /// Max Pong Delay
    /// </summary>
    public int MaxPongDelay;

    /// <summary>
    /// Battle Again Timer
    /// </summary>
    public int BattleAgainTimeout;

    /// <summary>
    /// IC Lost Title
    /// </summary>
    public string ICLostTitle;

    /// <summary>
    /// IC Lost Message
    /// </summary>
    public string ICLostMessage;

    public bool ShowRefundPopup;


    public bool IsPermissionAccepted;
    public bool IsLocalChannelMuted;
    public bool IsRemoteChannelMuted;
    public bool IsVoiceChatRequired;
    public bool enableAudioChat;
    public string AppVersionCode;
    public string AppVersionName;
    public string AppType;
    public float BattleScoreSyncInSec;
    public bool ApplyBonusLimit;
    public double BonusLimit;
    public string GameFormat;
    public bool isTierEnabled;
    public int MaxPlayers;





    public bool IsAdsEnabled = false;
    public bool IsAnzuAdsEnabled = false;
	public bool collectiblesOn = false;
    public bool CollectiblesOnHansel = false;
    public int NudgePercentage = 0;
    public bool TrialEnable = false;

    public List<AllTask> collectibles;

    public bool unityUid = false;

    public bool FraudDeveloperOptionEnabled;
    public bool FraudMagnificationCheckEnabled;
    public int[] FraudDeveloperOptionDisabledGameIds;
    public bool ShowMinimisePopupEnabled;
    public bool FestiveThemeOn;
    public int FestiveThemeColor; //0-red,1-purple
    public string Port;

    /************************************** Public Functions **************************************/
    public override string ToString()
    {
        return JsonUtility.ToJson(this);

        /*
        return String.Format("GameId: {0}, GameName: {1}, TournamentEndTime: {2}, StartTime: {3}, SessionId: {4}, TournamentId: {5}, TournamentName: {6}, GameConfigName: {7}, MobileNumber: {8}, ReactVersion: {9}, SequenceInterval: {10}, IsLogEnabled: {11}, IsRooted: {12}, IsModAppFound: {13}, IsSponsored: {14}, SponsorId: {15}, Is1v1: {16}, AuthToken: {17}, LobbyId: {18}, Profile: {20}, Hose: {21}, Zone: {22}, RetryTimeout: {23}, IsInDebugMode: {24}, EntryFee: {25}, EntryCurrency: {26}, LoadingMessages1v1: {27}, EntryPoint: {28}, PingInterval: {29}, CreateGameTimeout: {30}, TournamentType: {31}, CashEntryFee: {32}, TokenEntryFee: {33}, PrizeCurrency: {34}, CashPrizeOffered: {35}, TokenPrizeOffered: {36}, TournamentStyle: {37}, WinnersEndRank: {38}, MaxPongDelay: {39}, BattleAgainTimer: {40}, ICLostTitle: {41}, ICLostMessage: {42}",
                             GameId, GameName, TournamentEndTime, StartTime, SessionId, TournamentId, TournamentName, GameConfigName, MobileNumber, ReactVersion, SequenceInterval, IsLogEnabled, IsRooted, IsModAppFound, IsSponsored, SponsorId, Is1v1, AuthToken, LobbyId, Profile, Host, Zone, ConnectionRetryTimeout, IsInDebugMode, EntryFee, EntryCurrency, LoadingMessages1v1, EntryPoint, PingInterval, CreateGameTimeout, TournamentType,
                             CashEntryFee, TokenEntryFee, PrizeCurrency, CashPrizeOffered, TokenPrizeOffered, TournamentStyle, WinnersEndRank, MaxPongDelay, BattleAgainTimer, ICLostTitle, ICLostMessage);
                             */
    }
}

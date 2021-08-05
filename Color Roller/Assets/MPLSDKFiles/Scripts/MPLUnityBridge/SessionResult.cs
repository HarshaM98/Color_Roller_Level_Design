using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SessionResult
{
    /******************************** Global Variables & Constants ********************************/
    /// <summary>
    /// Active session id
    /// </summary>
    public string SessionId;

    public string lastFivePingDurations;

    /// <summary>
    /// Actual game start timestamp (Seconds)
    /// </summary>
    public long StartTime;

    /// <summary>
    /// Actual game end timestamp (Seconds)
    /// </summary>
    public long EndTime;

    public string UserMinimised;
    public double EntryFee;

    public int MaxPlayers;

    public string ModeOfConnection;

    public string ExtraDisconnectionReason;

    public string IsConnectedToInternet;

    public string IsConnectedToSmartfox;
    public string IsConnectionModeSwitched;
    

    /// <summary>
    /// Final game score
    /// </summary>
    public int Score;

    /// <summary>
    /// Game end reason: Why did the game end?
    /// </summary>
    public string GameEndReason;

    /// <summary>
    /// Duration of actual gameplay
    /// </summary>
    public long GameplayDurationSDK;

    /// <summary>
    /// Flags
    /// </summary>
    public string Flags;

    /// <summary>
    /// Flags
    /// </summary>
    public string FFlags;

    /// <summary>
    /// Sequence 
    /// </summary>
    public string Sequence;

    public string ChessTimerSequence;

    /// <summary>
    /// GameId 
    /// </summary>
    public int GameId;

    public int GamePauseDuration;
    public int GamePauseCount;
    public string GameVersion;
    public string AppVersionCode;
    public string AppVersionName;
    public string AppType;

    /// <summary>
    /// Is Score Tampered
    /// </summary>
    [NonSerialized]
    public bool ScoreTampered;

    /// <summary>
    /// Is the output config (session result) not equal to the input config (session info) ?
    /// </summary>
    [NonSerialized]
    public bool ConfigsMismatch;

    /// <summary>
    /// Game end reason type
    /// </summary>
    [NonSerialized]
    public MPLGameEndReason.GameEndReasons GameEndReasonType;

    /// <summary>
    /// Score Sequence Log
    /// </summary>
    [NonSerialized]
    public List<int> SequenceList;


    public List<Field> fieldsSequence;



    /// Screen Touch Press Data
     /// </summary>
     
    public int[] ScreenTouchPressCount = { 0,0,0,0};
    public List<int> TouchPressCountList = new List<int>();


    /// Screen Touch Relese Data
    /// </summary>
    public int[] ScreenTouchReleaseCount = { 0, 0, 0, 0 };

    /// Device Is Mobile Or Not
    /// </summary>
    /// 
    public bool Mobile;

    public string gameLog;



    /**************************************** Constructors ****************************************/
    public SessionResult(string sessionId, long startTime, long endTime, int score, MPLGameEndReason.GameEndReasons gameEndReason)
    {
        SessionId = MPLController.Instance.gameConfig.SessionId;
        StartTime = startTime;
        EndTime = endTime;
        Score = score;
        GameEndReasonType = gameEndReason;
        ScreenTouchPressCount = InputHandler.mInstance._TouchCountPress;
        ScreenTouchReleaseCount = InputHandler.mInstance._TouchCountRelease;
        TouchPressCountList = InputHandler.mInstance.GetTouchCountPressList();
        Mobile = InputHandler.mInstance.isMobile;
        GameEndReason = Session.Instance.GetReasonString(gameEndReason);
        
    }

    /************************************** Private Functions **************************************/
    private void SetFlags()
    {
        //Flags = "1" + Convert.ToInt32(ScoreTampered) + "" + Convert.ToInt32(ConfigsMismatch);
        Flags = "1" + Convert.ToInt32(ScoreTampered) + "" + 0;
    }

    /************************************** Protected Functions **************************************/
    protected void SetConfigsMismatch()
    {
        ConfigsMismatch = true;
    }

    /************************************** Public Functions **************************************/
    public void SetGameEndReason(MPLGameEndReason.GameEndReasons gameEndReasonType)
    {
        this.GameEndReasonType = gameEndReasonType;
        if(!MPLController.Instance.IsItIndo())
        {
            GameEndReason = Session.Instance.GetReasonString(gameEndReasonType);
        }
        else
        {
            GameEndReason = Session.Instance.GetReasonString(gameEndReasonType);
        }

    }

    public void SetSequenceList(List<int> seq)
    {
        SequenceList = seq;

        Sequence = "";
        for (int i = 0; i < SequenceList.Count; i++)
        {
            Sequence += SequenceList[i];
            if (i < SequenceList.Count - 1) Sequence += ",";
        }
    }

    public override string ToString()
    {
        SetFlags();

        AppVersionCode = MPLController.Instance.gameConfig.AppVersionCode;
        AppVersionName = MPLController.Instance.gameConfig.AppVersionCode;
        AppType = MPLController.Instance.gameConfig.AppType;

        return JsonUtility.ToJson(this);
    }

    public bool GetConfigsMismatch()
    {
        return ConfigsMismatch;
    }
}
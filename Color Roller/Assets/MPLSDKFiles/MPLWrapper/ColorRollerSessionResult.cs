using System;
using System.Collections.Generic;

[Serializable]
public class ColorRollerSessionResult : SessionResult
{
    /******************************** Global Variables & Constants ********************************/
    /// <summary>
    /// Player stats
    /// </summary>

    public long sessionStartTime;
    public long gameplayDuration;
    public int screenTouchCount;
    public int noOfLevelsCompleted;
    public List<levelData> levelData = new List<levelData>();

    /**************************************** Constructors ****************************************/
    public ColorRollerSessionResult(
        string sessionId,
        long sessionStartTime,
        long gameStartTime,
        long gameEndTime,
        int score,
        int screenTouchCount,
        int noOfLevelsCompleted,
        List<levelData> levelData,
        MPLGameEndReason.GameEndReasons gameEndReason,
        long gameplayDuration
       )
        : base(sessionId, gameStartTime, gameEndTime, score, gameEndReason)
    {
        this.sessionStartTime = sessionStartTime;
        this.gameplayDuration = gameplayDuration;
        this.levelData = levelData;
        this.noOfLevelsCompleted = noOfLevelsCompleted;
        this.screenTouchCount = screenTouchCount;
    }
}
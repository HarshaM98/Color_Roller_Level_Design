using System;
using Newtonsoft.Json;

[Serializable]
public class BattleMatchingEndedEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Skill Matching Time Out")]
    public string SkillMatchingTimeOut;

    [JsonProperty("Create Game Time Out")]
    public string CreateGameTimeOut;

    [JsonProperty("Ping Interval")]
    public string PingInterval;

    [JsonProperty("Battle Again Timer")]
    public string BattleAgainTimer;

    [JsonProperty("Fail Reason")]
    public string FailReason;

    [JsonProperty("Is Success")]
    public bool IsSuccess;

    [JsonProperty("Total Attempts")]
    public int TotalAttempts;

    [JsonProperty("Total Time")]
    public int TotalTime;

    /**************************************** Constructors ****************************************/
    public BattleMatchingEndedEventProperties(BattleRoomEventProperties battleRoomEventProperties, string skillMatchingTimeOut, string createGameTimeOut, string pingInterval, string battleAgainTimer, string failReason, bool isSuccess,int totalAttempts,int totalTime) : base(battleRoomEventProperties)
    {
        SkillMatchingTimeOut = skillMatchingTimeOut;
        CreateGameTimeOut = createGameTimeOut;
        PingInterval = pingInterval;
        BattleAgainTimer = battleAgainTimer;
        FailReason = failReason;
        IsSuccess = isSuccess;
        TotalAttempts = totalAttempts;
        TotalTime = totalTime;
    }
}

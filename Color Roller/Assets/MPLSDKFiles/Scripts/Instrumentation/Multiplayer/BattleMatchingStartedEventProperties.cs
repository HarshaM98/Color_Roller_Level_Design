using System;
using Newtonsoft.Json;

[Serializable]
public class BattleMatchingStartedEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Skill Matching Time Out")]
    public string SkillMatchingTimeOut;

    [JsonProperty("Create Game Time Out")]
    public string CreateGameTimeOut;

    [JsonProperty("Ping Interval")]
    public string PingInterval;

    [JsonProperty("Battle Again Timer")]
    public string BattleAgainTimer;

    [JsonProperty("Entry Point")]
    public string EntryPoint;

    [JsonProperty("Is Retry")]
    public bool IsRetry;

    [JsonProperty("Attempt")]
    public int Attempt;

    


    /**************************************** Constructors ****************************************/
    public BattleMatchingStartedEventProperties(BattleRoomEventProperties battleRoomEventProperties, string skillMatchingTimeOut, string createGameTimeOut, string pingInterval, string battleAgainTimer, string entryPoint, bool isRetry,int attempt) : base(battleRoomEventProperties)
    {
        SkillMatchingTimeOut = skillMatchingTimeOut;
        CreateGameTimeOut = createGameTimeOut;
        PingInterval = pingInterval;
        BattleAgainTimer = battleAgainTimer;
        EntryPoint = entryPoint;
        IsRetry = isRetry;
        Attempt = attempt;
    }
}




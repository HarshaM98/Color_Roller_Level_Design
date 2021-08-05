using System;
using Newtonsoft.Json;

[Serializable]
public class BattleStartedEventProperties: BattleOpponentEventProperties
{
    [JsonProperty("Is Success")]
    public bool IsSuccess;

    [JsonProperty("Fail Reason")]
    public string FailReason;

    [JsonProperty("Is Mic On")]
    public bool IsMicOn;

    [JsonProperty("Is Opponent Mic On")]
    public bool IsOpponentMicOn;

    [JsonProperty("Is Opponent Muted")]
    public bool IsOpponentMuted;

    [JsonProperty("Game Loading Time")]
    public int GameLoadingTime;

    [JsonProperty("Minimize Count")]
    public int MinimizeCount;

    [JsonProperty("Is AutoStart Enabled")]
    public bool IsAutoStartEnabled;


    /**************************************** Constructors ****************************************/
    public BattleStartedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties, bool isSuccess, string failReason,bool isMicOn,bool isOpponentMicOn,bool isOpponentMuted, int gameLoadingTime, int minimizeCount,bool isAutoStartEnabled) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
        IsSuccess = isSuccess;
        IsAutoStartEnabled = isAutoStartEnabled;
        FailReason = failReason;
        IsMicOn = isMicOn;
        IsOpponentMicOn = isOpponentMicOn;
        IsOpponentMuted= isOpponentMuted;
        GameLoadingTime = gameLoadingTime;
        MinimizeCount = minimizeCount;
    }
}

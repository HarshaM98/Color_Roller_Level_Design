using System;
using Newtonsoft.Json;

[Serializable]
public class BattleEndedEventProperties: BattleOpponentEventProperties
{
    [JsonProperty("User Score")]
    public int UserScore;

    [JsonProperty("Opponent Score")]
    public int OpponentScore;

    [JsonProperty("Session Result")]
    public string SessionResult;

    [JsonProperty("Result Type")]
    public string ResultType;

    [JsonProperty("Battle End Reason")]
    public string BattleEndReason;

    [JsonProperty("Total Cash Balance")]
    public double TotalCashBalance;

    [JsonProperty("Total Token Balance")]
    public double TotalTokenBalance;

    [JsonProperty("Is Enough To Rematch")]
    public bool IsEnoughToRematch;

    [JsonProperty("Duration")]
    public int Duration;

    [JsonProperty("Reconnection Initiated Count")]
    public int reconnectionInitiatedCount;

    [JsonProperty("Reconnection Concluded Count")]
    public int reconnectionConcludedCount;

    [JsonProperty("Reconnection State")]
    public string reconnectionState;

    /**************************************** Constructors ****************************************/
    public BattleEndedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties, int userScore,string sessionResult, int opponentScore, string resultType, string battleEndReason, double totalCashBalance, double totalTokenBalance, bool isEnoughToRematch, int duration) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
        UserScore = userScore;
        SessionResult = sessionResult;
        OpponentScore = opponentScore;
        ResultType = resultType;
        BattleEndReason = battleEndReason;
        TotalCashBalance = totalCashBalance;
        TotalTokenBalance = totalTokenBalance;
        IsEnoughToRematch = isEnoughToRematch;
        Duration = duration;

       
    }
}

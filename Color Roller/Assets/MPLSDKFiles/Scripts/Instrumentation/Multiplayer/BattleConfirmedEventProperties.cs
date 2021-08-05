using System;
using Newtonsoft.Json;

[Serializable]
public class BattleConfirmedEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Total Cash Balance")]
    public double TotalCashBalance;

    [JsonProperty("Total Token Balance")]
    public double TotalTokenBalance;

    [JsonProperty("Has Enough Balance")]
    public bool HasEnoughBalance;

    [JsonProperty("Entry Point")]
    public string EntryPoint;

    [JsonProperty("Match Type")]
    public string MatchType;

    [JsonProperty("Is Success")]
    public bool IsSuccess;

    /**************************************** Constructors ****************************************/
    public BattleConfirmedEventProperties(BattleRoomEventProperties battleRoomEventProperties, double totalCashBalance, double totalTokenBalance, bool hasEnoughBalance, string entryPoint, string matchType, bool isSuccess) : base(battleRoomEventProperties)
    {
        TotalCashBalance = totalCashBalance;
        TotalTokenBalance = totalTokenBalance;
        HasEnoughBalance = hasEnoughBalance;
        EntryPoint = entryPoint;
        MatchType = matchType;
        IsSuccess = isSuccess;
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class UserFraudDetectedEventProperties
{
    [JsonProperty("Fraud Check")]
    public string FraudCheck;

    [JsonProperty("Module Name")]
    public const string ModuleName = "Game Play";

    [JsonProperty("Game ID")]
    public string GameId;

    [JsonProperty("Game Name")]
    public string GameName;

    [JsonProperty("Mobile Number")]
    public string MobileNumber;

    [JsonProperty("Is Rooted")]
    public bool IsRooted;

    [JsonProperty("Is Mod App Found")]
    public bool IsModAppFound;

    [JsonProperty("Score Mismatches")]
    public string ScoreMismatches { get; set; }

    [JsonProperty("Timer Mismatches")]
    public string TimerMismatches { get; set; }

    [JsonProperty("Field Mismatches")]
    public string FieldMismatches { get; set; }

    [JsonProperty("Field Name")]
    public string FieldName { get; set; }

    public enum FraudCheckType { SCORE_TAMPERED, GAME_CONFIG_MISMATCH, TIMER_TAMPERED, FIELD_TAMPERED };
    private Dictionary<FraudCheckType, string> FraudCheckDisplayNames = new Dictionary<FraudCheckType, string>()
    {
        {FraudCheckType.SCORE_TAMPERED, "Score Tampered"},
        {FraudCheckType.GAME_CONFIG_MISMATCH, "Game Config Mismatch"},
        {FraudCheckType.TIMER_TAMPERED, "Timer Tampered"},
        {FraudCheckType.FIELD_TAMPERED, "Field Tampered"}
    };

    /**************************************** Constructors ****************************************/
    public UserFraudDetectedEventProperties(FraudCheckType fraudCheck, string gameId, string gameName, string mobileNumber, bool isRooted, bool isModAppFound)
    {
        FraudCheck = FraudCheckDisplayNames[fraudCheck];
        GameId = gameId;
        GameName = gameName;
        MobileNumber = mobileNumber;
        IsRooted = isRooted;
        IsModAppFound = isModAppFound;
    }

    public UserFraudDetectedEventProperties(FraudCheckType fraudCheck, string mismatches, string gameId, string gameName, string mobileNumber, bool isRooted, bool isModAppFound)
    {
        FraudCheck = FraudCheckDisplayNames[fraudCheck];
        GameId = gameId;
        GameName = gameName;

        if (fraudCheck == FraudCheckType.SCORE_TAMPERED) ScoreMismatches = mismatches;
        else if (fraudCheck == FraudCheckType.TIMER_TAMPERED) TimerMismatches = mismatches;

        MobileNumber = mobileNumber;
        IsRooted = isRooted;
        IsModAppFound = isModAppFound;
    }

    public UserFraudDetectedEventProperties(FraudCheckType fraudCheck, string fieldName, string mismatches, string gameId, string gameName, string mobileNumber, bool isRooted, bool isModAppFound)
    {
        FraudCheck = FraudCheckDisplayNames[fraudCheck];
        GameId = gameId;
        GameName = gameName;

        FieldMismatches = mismatches;
        FieldName = fieldName;

        MobileNumber = mobileNumber;
        IsRooted = isRooted;
        IsModAppFound = isModAppFound;
    }

    /************************************** Public Functions **************************************/

    public bool ShouldSerializeScoreMismatches()
    {
        return !string.IsNullOrEmpty(ScoreMismatches);
    }

    public bool ShouldSerializeTimerMismatches()
    {
        return !string.IsNullOrEmpty(TimerMismatches);
    }

    public bool ShouldSerializeFieldMismatches()
    {
        return !string.IsNullOrEmpty(FieldMismatches);
    }

    public bool ShouldSerializeFieldName()
    {
        return !string.IsNullOrEmpty(FieldName);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}
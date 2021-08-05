using System;
using Newtonsoft.Json;

[Serializable]
public class GameEndedEventProperties: MPLGameSessionEventProperties
{
    [JsonProperty("Game End Reason")]
    public string GameEndReason;

    [JsonProperty("Game Start Time")]
    public long GameStartTime;

    [JsonProperty("Game Score")]
    public int GameScore;

    [JsonProperty("Game Duration")]
    public long GameDuration;

    [JsonProperty("Pause Count")]
    public int PauseCount;

    [JsonProperty("Pause Duration")]
    public int PauseDuration;

    [JsonProperty("Score Tampered")]
    public bool ScoreTampered;

    [JsonProperty("Game Config Mismatch")]
    public bool GameConfigMismatch;

    [JsonProperty("Field Tampered")]
    public bool FieldTampered;

    /**************************************** Constructors ****************************************/
    public GameEndedEventProperties(MPLGameSessionEventProperties gameSessionEventProperties, string gameEndReason, long gameStartTime, int gameScore, long gameDuration, int pauseCount, int pauseDuration, bool scoreTampered, bool gameConfigMismatch, bool fieldTampered) : base(gameSessionEventProperties)
    {
        GameEndReason = gameEndReason;
        GameStartTime = gameStartTime;
        GameScore = gameScore;
        GameDuration = gameDuration;
        PauseCount = pauseCount;
        PauseDuration = pauseDuration;
        ScoreTampered = scoreTampered;
        GameConfigMismatch = gameConfigMismatch;
        FieldTampered = fieldTampered;
    }
}

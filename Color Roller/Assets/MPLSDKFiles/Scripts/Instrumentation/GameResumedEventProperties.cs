using System;
using Newtonsoft.Json;

[Serializable]
public class GameResumedEventProperties: MPLGameSessionEventProperties
{
    /// <summary>
    /// Pause duration
    /// </summary>
    [JsonProperty("Pause Duration")]
    public int PauseDuration;

    /// <summary>
    /// The remaining pause time
    /// </summary>
    [JsonProperty("Remaining Pause Time")]
    public int RemainingPauseTime;

    /**************************************** Constructors ****************************************/
    public GameResumedEventProperties(MPLGameSessionEventProperties gameSessionEventProperties, int remainingPauseTime, int pauseDuration): base(gameSessionEventProperties)
    {
        RemainingPauseTime = remainingPauseTime;
        PauseDuration = pauseDuration;
    }
}

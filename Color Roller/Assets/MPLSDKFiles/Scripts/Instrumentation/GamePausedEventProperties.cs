using System;
using Newtonsoft.Json;

[Serializable]
public class GamePausedEventProperties: MPLGameSessionEventProperties
{
    /// <summary>
    /// The remaining pause time
    /// </summary>
    [JsonProperty("Remaining Pause Time")]
    public int RemainingPauseTime;

    /**************************************** Constructors ****************************************/
    public GamePausedEventProperties(MPLGameSessionEventProperties gameSessionEventProperties, int remainingPauseTime): base(gameSessionEventProperties)
    {
        RemainingPauseTime = remainingPauseTime;
    }
}

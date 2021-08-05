using System;
using Newtonsoft.Json;

[Serializable]
public class GameStartedEventProperties: MPLGameSessionEventProperties
{
    /// <summary>
    /// If the user is playing this game for the first time
    /// </summary>
    [JsonProperty("Is First Time")]
    public bool IsFirstTime;

    [JsonProperty("Game Loading Time")]
    public int GameLoadingTime;

    [JsonProperty("Minimize Count")]
    public int MinimizeCount;

    /**************************************** Constructors ****************************************/
    public GameStartedEventProperties(MPLGameSessionEventProperties gameSessionEventProperties, bool isFirstTime, int gameLoadingTime, int minimizeCount) : base(gameSessionEventProperties)
    {
        IsFirstTime = isFirstTime;
        GameLoadingTime = gameLoadingTime;
        MinimizeCount = minimizeCount;
    }
}

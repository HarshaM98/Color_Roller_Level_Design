using System;
using Newtonsoft.Json;

[Serializable]
public class TrainingStartedEventProperties
{
    /// <summary>
    /// Game Name
    /// </summary>
    [JsonProperty("Game Name")]
    public string GameName;
    [JsonProperty("Game Id")]
    public string GameId;
    [JsonProperty("Entry Point")]
    public string EntryPoint;

    /**************************************** Constructors ****************************************/
    public TrainingStartedEventProperties(string gameName,string gameId,string entryPoint)
    {
        GameName = gameName;
        GameId = gameId;
        EntryPoint = entryPoint;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

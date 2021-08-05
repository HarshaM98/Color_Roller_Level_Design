using System;
using Newtonsoft.Json;

[Serializable]
public class TrainingEndedEventProperties
{
    
    [JsonProperty("Game Name")]
    public string GameName;
    [JsonProperty("Game Id")]
    public string GameId;
    [JsonProperty("End Reason")]
    public string EndReason;

    /**************************************** Constructors ****************************************/
    public TrainingEndedEventProperties(string gameName, string gameId, string endReason)
    {
        GameName = gameName;
        GameId = gameId;
        EndReason = endReason;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

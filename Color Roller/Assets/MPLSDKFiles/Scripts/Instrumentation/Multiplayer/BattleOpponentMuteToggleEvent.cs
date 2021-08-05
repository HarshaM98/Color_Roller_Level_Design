using System;
using Newtonsoft.Json;


public class BattleOpponentMuteToggleEvent
{

    // Use this for initialization
    [JsonProperty("Entry Point")]
    public string EntryPoint;

    [JsonProperty("New State")]
    public string NewState;






    public BattleOpponentMuteToggleEvent(string entryPoint, string newState)
    {
        EntryPoint = entryPoint;
        NewState = newState;


    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }

}

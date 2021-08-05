using System;
using Newtonsoft.Json;


public class BattleMicToggleEvent
{

    // Use this for initialization
    [JsonProperty("Entry Point")]
    public string EntryPoint;

    [JsonProperty("New State")]
    public string NewState;



     


    public BattleMicToggleEvent(string entryPoint,string newState)
    {
        EntryPoint = entryPoint;
        NewState = newState;


    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }

}

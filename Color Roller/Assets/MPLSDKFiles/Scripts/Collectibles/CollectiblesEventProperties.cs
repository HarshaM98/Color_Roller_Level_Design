using Newtonsoft.Json;

[System.Serializable]
public class CollectiblesEventProperties
{
    [JsonProperty("Virtual Good ID")]
    public string VirtualGoodID;

    [JsonProperty("Virtual Good Name")]
    public string VirtualGoodName;

    [JsonProperty("Game Name")]
    public string GameName;

    [JsonProperty("Game ID")]
    public string GameID;

    [JsonProperty("Tier")]
    public string Tier;

    [JsonProperty("Good Count")]
    public string GoodCount;

    [JsonProperty("Status")]
    public string Status;


    [JsonProperty("Entry Point")]
    public string EntryPoint;


    public CollectiblesEventProperties(string _VirtualGoodID, string _VirtualGoodName, string _GameName, string _GameID, string _Tier, string _GoodCount, string _Status, string _EntryPoint)
    {
        VirtualGoodID = _VirtualGoodID;
        VirtualGoodName = _VirtualGoodName;
        GameName = _GameName;
        GameID = _GameID;
        Tier = _Tier;
        Status = _Status;
        GoodCount = _GoodCount;
        EntryPoint = _EntryPoint;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this,Formatting.None);
    }
}

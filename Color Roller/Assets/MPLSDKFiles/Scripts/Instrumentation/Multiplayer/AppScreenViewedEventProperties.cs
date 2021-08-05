using System;
using Newtonsoft.Json;

[Serializable]
public class AppScreenViewedEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Screen Name")]
    public string ScreenName;

    /**************************************** Constructors ****************************************/
    public AppScreenViewedEventProperties(BattleRoomEventProperties battleRoomEventProperties, string screenName) : base(battleRoomEventProperties)
    {
        ScreenName = screenName;
    }
}
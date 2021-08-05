using System;
using Newtonsoft.Json;

[Serializable]
public class BattlePopupShownEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Pop Up Name")]
    public string PopUpName;

    [JsonProperty("Title")]
    public string Title;

    [JsonProperty("Message")]
    public string Message;

    


    /**************************************** Constructors ****************************************/
    public BattlePopupShownEventProperties(BattleRoomEventProperties battleRoomEventProperties, string popUpName, string title, string message) : base(battleRoomEventProperties)
    {
        PopUpName = popUpName;
        Title = title;
        Message = message;
        
    }
}
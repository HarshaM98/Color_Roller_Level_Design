using System;
using Newtonsoft.Json;

[Serializable]
public class BattleButtonClickedEventProperties : BattleRoomEventProperties
{
    [JsonProperty("Button Name")]
    public string ButtonName;

   

    /**************************************** Constructors ****************************************/
    public BattleButtonClickedEventProperties(BattleRoomEventProperties battleRoomEventProperties, string buttonName) : base(battleRoomEventProperties)
    {
        ButtonName = buttonName;
  
    }
}


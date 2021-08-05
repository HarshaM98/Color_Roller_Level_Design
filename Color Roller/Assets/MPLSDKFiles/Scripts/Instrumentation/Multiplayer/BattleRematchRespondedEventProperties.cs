using System;
using Newtonsoft.Json;

[Serializable]
//TODO: Apply everywhere
public class BattleRematchRespondedEventProperties: BattleOpponentEventProperties
{
    [JsonProperty("Is Winner")]
    public bool IsWinner;

    [JsonProperty("Is Accepted")]
    public bool IsAccepted;

    [JsonProperty("Is Responder")]
    public bool IsResponder;

    [JsonProperty("Entry Point")]
    public bool EntryPoint;

    /**************************************** Constructors ****************************************/
    public BattleRematchRespondedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties, bool isWinner, bool isAccepted, bool isResponder) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
        IsWinner = isWinner;
        IsAccepted = isAccepted;
        IsResponder = isResponder;
    }
}

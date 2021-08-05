using System;
using Newtonsoft.Json;

[Serializable]
public class BattleRematchRequestedEventProperties: BattleOpponentEventProperties
{
    [JsonProperty("Is Winner")]
    public bool IsWinner;

    [JsonProperty("Is Initiator")]
    public bool IsInitiator;

    /**************************************** Constructors ****************************************/
    public BattleRematchRequestedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties, bool isWinner, bool isInitiator) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
        IsWinner = isWinner;
        IsInitiator = isInitiator;
    }
}

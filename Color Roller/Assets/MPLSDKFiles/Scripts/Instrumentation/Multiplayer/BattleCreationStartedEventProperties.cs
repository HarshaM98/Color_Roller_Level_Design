using System;
using Newtonsoft.Json;

[Serializable]
public class BattleCreationStartedEventProperties: BattleOpponentEventProperties
{
    /**************************************** Constructors ****************************************/
    public BattleCreationStartedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
    }
}

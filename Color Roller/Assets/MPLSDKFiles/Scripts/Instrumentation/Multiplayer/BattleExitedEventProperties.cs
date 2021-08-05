using System;
using Newtonsoft.Json;

[Serializable]
public class BattleExitedEventProperties: BattleOpponentEventProperties
{
    [JsonProperty("Result Type")]
    public string ResultType;

    [JsonProperty("Exit Point")]
    public string ExitPoint;

    /**************************************** Constructors ****************************************/
    public BattleExitedEventProperties(BattleOpponentEventProperties battleOpponentEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties battleSessionEventProperties, string resultType, string exitPoint) : base(battleSessionEventProperties, battleRoomEventProperties, battleOpponentEventProperties)
    {
        ResultType = resultType;
        ExitPoint = exitPoint;
    }
}

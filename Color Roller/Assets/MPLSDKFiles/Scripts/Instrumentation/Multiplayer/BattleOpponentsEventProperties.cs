using Newtonsoft.Json;

public class BattleOpponentsEventProperties : BattleSessionEventProperties
{
	[JsonProperty("Opponents Data")]
	public string OpponentsData;

	

	/**************************************** Constructors ****************************************/
	public BattleOpponentsEventProperties(BattleSessionEventProperties battleSessionEventProperties, BattleRoomEventProperties battleRoomEventProperties, string opponentsData) : base(battleRoomEventProperties, battleSessionEventProperties)
	{
        OpponentsData = opponentsData;

    }

	public BattleOpponentsEventProperties(BattleSessionEventProperties battleSessionEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleOpponentsEventProperties battleOpponentEventProperties) : base(battleRoomEventProperties, battleSessionEventProperties)
	{
        OpponentsData = battleOpponentEventProperties.OpponentsData;

        
	}
}

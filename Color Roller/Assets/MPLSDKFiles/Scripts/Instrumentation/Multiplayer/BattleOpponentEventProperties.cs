using Newtonsoft.Json;

public class BattleOpponentEventProperties : BattleSessionEventProperties 
{
    [JsonProperty("Opponent User ID")]
     public string OpponentUserID;

     [JsonProperty("Opponent Display Name")]
     public string OpponentDisplayName;

     [JsonProperty("Opponent Identity")]
     public string OpponentIdentity;

     [JsonProperty("Opponent Tier")]
     public string OpponentTier;

    

    /**************************************** Constructors ****************************************/
    public BattleOpponentEventProperties(BattleSessionEventProperties battleSessionEventProperties, BattleRoomEventProperties battleRoomEventProperties, string opponentUserID, string opponentDisplayName, string opponentIdentity, string opponentTier): base(battleRoomEventProperties, battleSessionEventProperties)
    {
        
        OpponentUserID = opponentUserID;
        OpponentDisplayName = opponentDisplayName;
        OpponentIdentity = opponentIdentity;
        OpponentTier = opponentTier;
    }

    public BattleOpponentEventProperties(BattleSessionEventProperties battleSessionEventProperties, BattleRoomEventProperties battleRoomEventProperties, BattleOpponentEventProperties battleOpponentEventProperties) : base(battleRoomEventProperties, battleSessionEventProperties)
    {
        
        OpponentUserID = battleOpponentEventProperties.OpponentUserID;
        OpponentDisplayName = battleOpponentEventProperties.OpponentDisplayName;
        OpponentIdentity = battleOpponentEventProperties.OpponentIdentity;
        OpponentTier = battleOpponentEventProperties.OpponentTier;
    }
}

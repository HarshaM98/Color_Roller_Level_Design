using Newtonsoft.Json;

public class BattleSessionEventProperties : BattleRoomEventProperties
{
    [JsonProperty("Game Session ID")]
    public string GameSessionID;

    [JsonProperty("Tournament Name")]
    public string TournamentName;

    [JsonProperty("Game Version")]
    public string GameVersion;

    [JsonProperty("Mobile Number")]
    public string MobileNumber;

    [JsonProperty("React Version")]
    public string ReactVersion;

    [JsonProperty("Tier")]
    public string Tier;

    /**************************************** Constructors ****************************************/
    public BattleSessionEventProperties(BattleRoomEventProperties battleRoomEventProperties, string gameSessionID, string tournamentName, string gameVersion, string mobileNumber, string reactVersion, string tier) : base(battleRoomEventProperties)
    {
        GameSessionID = gameSessionID;
        TournamentName = tournamentName;
        GameVersion = gameVersion;
        MobileNumber = mobileNumber;
        ReactVersion = reactVersion;
        Tier = tier;
    }

    public BattleSessionEventProperties(BattleRoomEventProperties battleRoomEventProperties, BattleSessionEventProperties gameSessionEventProperties) : base(battleRoomEventProperties)
    {
        
            GameSessionID = gameSessionEventProperties.GameSessionID;
            TournamentName = gameSessionEventProperties.TournamentName;
            GameName = gameSessionEventProperties.GameName;
            GameVersion = gameSessionEventProperties.GameVersion;
            GameConfigName = gameSessionEventProperties.GameConfigName;
            MobileNumber = gameSessionEventProperties.MobileNumber;
            ReactVersion = gameSessionEventProperties.ReactVersion;
            EntryCurrency = gameSessionEventProperties.EntryCurrency;
            Tier = gameSessionEventProperties.Tier;
        
    }
}

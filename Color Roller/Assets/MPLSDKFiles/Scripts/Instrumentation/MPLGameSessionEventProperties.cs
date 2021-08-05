using Newtonsoft.Json;

public class MPLGameSessionEventProperties
{
    [JsonProperty("Game Session ID")]
    public string GameSessionID;

    [JsonProperty("Tournament ID")]
    public int TournamentID;

    [JsonProperty("Is FTUE")]
    public bool IsFTUE;

    [JsonProperty("Tournament Type")]
    public string TournamentType;

    [JsonProperty("Tournament Name")]
    public string TournamentName;

    [JsonProperty("Game ID")]
    public int GameID;

    [JsonProperty("Game Name")]
    public string GameName;

    [JsonProperty("Game Version")]
    public string GameVersion;

    [JsonProperty("Game Config Name")]
    public string GameConfigName;

    [JsonProperty("Mobile Number")]
    public string MobileNumber;

    [JsonProperty("React Version")]
    public string ReactVersion;

    [JsonProperty("Entry Fee")]
    public int EntryFee;

    [JsonProperty("Entry Currency")]
    public string EntryCurrency;

    [JsonProperty("Tier")]
    public string Tier;

    /**************************************** Constructors ****************************************/
    public MPLGameSessionEventProperties(string gameSessionID, int tournamentID, string tournamentName, bool isFTUE,int gameID, string gameName, string gameVersion, string gameConfigName, string mobileNumber, string reactVersion, int entryFee, string entryCurrency, string tier,string tournamentType)
    {
        GameSessionID = gameSessionID;
        TournamentID = tournamentID;
        IsFTUE = isFTUE;
        TournamentName = tournamentName;
        GameID = gameID;
        GameName = gameName;
        GameVersion = gameVersion;
        GameConfigName = gameConfigName;
        MobileNumber = mobileNumber;
        ReactVersion = reactVersion;
        EntryFee = entryFee;
        EntryCurrency = entryCurrency;
        Tier = tier;
        TournamentType = tournamentType;
    }

    public MPLGameSessionEventProperties(MPLGameSessionEventProperties gameSessionEventProperties)
    {
        GameSessionID = gameSessionEventProperties.GameSessionID;
        TournamentID = gameSessionEventProperties.TournamentID;
        TournamentName = gameSessionEventProperties.TournamentName;
        IsFTUE = gameSessionEventProperties.IsFTUE;
        GameID = gameSessionEventProperties.GameID;
        GameName = gameSessionEventProperties.GameName;
        GameVersion = gameSessionEventProperties.GameVersion;
        GameConfigName = gameSessionEventProperties.GameConfigName;
        MobileNumber = gameSessionEventProperties.MobileNumber;
        ReactVersion = gameSessionEventProperties.ReactVersion;
        EntryFee = gameSessionEventProperties.EntryFee;
        EntryCurrency = gameSessionEventProperties.EntryCurrency;
        Tier = gameSessionEventProperties.Tier;
        TournamentType = gameSessionEventProperties.TournamentType;
    }

    /************************************** Public Functions **************************************/
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}
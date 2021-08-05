using Newtonsoft.Json;
using System;

public class BattleRoomEventProperties
{
    [JsonProperty("Battle State")]
    public string BattleState = "";
    [JsonProperty("Tournament ID")]
    public string TournamentID;

    [JsonProperty("Tournament Type")]
    public string TournamentType;

    

   

   

   

    [JsonProperty("Is FTUE")]
    public bool IsFTUE;

    

    [JsonProperty("Entry Fee")]
    public string EntryFee;

    [JsonProperty("Entry Currency")]
    public string EntryCurrency;

    [JsonProperty("Max Players")]
    public string MaxPlayers;

    [JsonProperty("Game Sessions Limit")]
    public string GameSessionsLimit;

    [JsonProperty("Game ID")]
    public string GameID;

    [JsonProperty("Game Name")]
    public string GameName;

    [JsonProperty("Game Config Name")]
    public string GameConfigName;

    [JsonProperty("Cash Entry Fee")]
    public string CashEntryFee;

    [JsonProperty("Token Entry Fee")]
    public string TokenEntryFee;

    [JsonProperty("Prize Currency")]
    public string PrizeCurrency;

    [JsonProperty("Winners End Rank")]
    public string WinnersEndRank;

    [JsonProperty("Cash Prize Offered")]
    public string CashPrizeOffered;

    [JsonProperty("Token Prize Offered")]
    public string TokenPrizeOffered;

    [JsonProperty("Tournament Style")]
    public string TournamentStyle;
    [JsonProperty("Bonus Cash Cap")]
    public string BonusCashCap;

    [JsonProperty("Battle ID")]
    public string BattleId = "";

    [JsonProperty("Timestamp")]
    public string timestamp;

    /**************************************** Constructors ****************************************/
    public BattleRoomEventProperties(string tournamentId, bool isFTUE, string tournamentType,string entryFee, string entryCurrency, string maxPlayers, string gameSessionsLimit, string gameId, string gameName, string gameConfigName, string cashEntryFee, string tokenEntryFee, string prizeCurrency, string winnersEndRank, string cashPrizeOffered, string tokenPrizeOffered, string tournamentStyle, string bonusCashCap)
    {
        TournamentID = tournamentId;
        IsFTUE = isFTUE;
        TournamentType = tournamentType;
       
        EntryFee = entryFee;
        EntryCurrency = entryCurrency;
        MaxPlayers = maxPlayers;
        GameSessionsLimit = gameSessionsLimit;
        GameID = gameId;
        GameName = gameName;
        GameConfigName = gameConfigName;
        CashEntryFee = cashEntryFee;
        TokenEntryFee = tokenEntryFee;
        PrizeCurrency = prizeCurrency;
        WinnersEndRank = winnersEndRank;
        CashPrizeOffered = cashPrizeOffered;
        TokenPrizeOffered = tokenPrizeOffered;
        TournamentStyle = tournamentStyle;
        BonusCashCap = bonusCashCap;
        TournamentType = tournamentType;
        BattleId = "";
        BattleState = "";
    }

    public BattleRoomEventProperties(BattleRoomEventProperties battleRoomEventProperties)
    {
        //try
        //{
            TournamentID = battleRoomEventProperties.TournamentID;
            IsFTUE = battleRoomEventProperties.IsFTUE;
            TournamentType = battleRoomEventProperties.TournamentType;
            
            
            
            EntryFee = battleRoomEventProperties.EntryFee;
            EntryCurrency = battleRoomEventProperties.EntryCurrency;
            MaxPlayers = battleRoomEventProperties.MaxPlayers;
            GameSessionsLimit = battleRoomEventProperties.GameSessionsLimit;
            GameID = battleRoomEventProperties.GameID;
            GameName = battleRoomEventProperties.GameName;
            GameConfigName = battleRoomEventProperties.GameConfigName;
            CashEntryFee = battleRoomEventProperties.CashEntryFee;
            TokenEntryFee = battleRoomEventProperties.TokenEntryFee;
            PrizeCurrency = battleRoomEventProperties.PrizeCurrency;
            WinnersEndRank = battleRoomEventProperties.WinnersEndRank;
            CashPrizeOffered = battleRoomEventProperties.CashPrizeOffered;
            TokenPrizeOffered = battleRoomEventProperties.TokenPrizeOffered;
            TournamentStyle = battleRoomEventProperties.TournamentStyle;
            BonusCashCap = battleRoomEventProperties.BonusCashCap;
            TournamentType = battleRoomEventProperties.TournamentType;
            BattleId = battleRoomEventProperties.BattleId;
            BattleState = battleRoomEventProperties.BattleState;

    }
    public void SetBattleState(string battleState)
    {
        BattleState = battleState;
    }

    public void SetBattleId(string battleId)
    {
        BattleId = battleId;
    }

    /************************************** Public Functions **************************************/
    public override string ToString()
    {
        timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff");
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

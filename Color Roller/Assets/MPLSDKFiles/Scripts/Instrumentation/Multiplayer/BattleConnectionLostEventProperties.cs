using System;
using Newtonsoft.Json;

[Serializable]
public class BattleConnectionLostEventProperties: BattleRoomEventProperties
{
    [JsonProperty("Pop Up Name")]
    public string PopUpName;

    [JsonProperty("Title")]
    public string Title;

    [JsonProperty("Message")]
    public string Message;

    [JsonProperty("MPL Ping State")]
    public string MPLPingState;

    [JsonProperty("User Minimised")]
    public string UserMinimised;

    [JsonProperty("Mode Of Connection")]
    public string ModeOfConnection;

    [JsonProperty("Extra Disconnection Reason")]
    public string ExtraDisconnectionReason;

    [JsonProperty("Is Connected To Internet")]
    public string IsConnectedToInternet;

    [JsonProperty("Is Connected To Smartfox")]
    public string IsConnectedToSmartfox;

    [JsonProperty("Is Connection Mode Switched")]
    public string IsConnectionModeSwitched;

    [JsonProperty("Game Type")]
    public string GameType;

    [JsonProperty("Average Ping")]
    public double AveragePing;

    [JsonProperty("Reconnection Enabled")]
    public bool ReconnectionEnabled;

    [JsonProperty("MPL Game State")]
    public string MPLGameState;

    [JsonProperty("Country Code")]
    public string CountryCode;



    /**************************************** Constructors ****************************************/
    public BattleConnectionLostEventProperties(BattleRoomEventProperties battleRoomEventProperties, string popUpName, string title, string message,string mPLPingState,string userMinimised,string modeOfConnection,string extraDisconnectionReason,string isConnectedToInternet,string isConnectedToSmartfox,string isConnectionModeSwitched,string gameType,double averagePing,bool reconnectionEnabled,string mplGameState,string countryCode) : base(battleRoomEventProperties)
    {
        PopUpName = popUpName;
        Title = title;
        Message = message;
        MPLPingState = mPLPingState;
        
        UserMinimised= userMinimised;

        
        ModeOfConnection= modeOfConnection;

        
        ExtraDisconnectionReason= extraDisconnectionReason;

        
        IsConnectedToInternet= isConnectedToInternet;

        
        IsConnectedToSmartfox= isConnectedToSmartfox;

        
        IsConnectionModeSwitched= isConnectionModeSwitched;
        GameType = gameType;
        AveragePing = averagePing;
        ReconnectionEnabled = reconnectionEnabled;
        MPLGameState = mplGameState;
        CountryCode = countryCode;
    }
}
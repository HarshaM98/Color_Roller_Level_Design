using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GreconnectionConcludedEventProperties: BattleRoomEventProperties
{
    [JsonIgnore]
    public const string EVENT_NAME = "Battle Reconnection Concluded";

    [JsonProperty("Attempt")]
    public int attempt;

    [JsonProperty("Is Success")]
    public bool isSuccess;

    [JsonProperty("Reconnected In")]
    public int reconnectedIn;

    [JsonProperty("Is Connected To Internet")]
    public string isConnectedToInternet;

    [JsonProperty("Is Connected To Smartfox")]
    public string isConnectedToSmartfox;

    [JsonProperty("Is Connection Mode Switched")]
    public string isConnectionModeSwitched;

    [JsonProperty("Average Ping")]
    public double averagePing;

    [JsonProperty("Ping State")]
    public string pingState;

    [JsonProperty("Retries Left")]
    public int retriesLeft;

    [JsonProperty("Reason")]
    public string reason;

    public GreconnectionConcludedEventProperties(BattleRoomEventProperties battleRoomEventProperties, int attempt, bool isSuccess, int reconnectedIn, string isConnectedToInternet, string isConnectedToSmartfox, string isConnectionModeSwitched, double averagePing, string pingState, int retriesLeft,string reason) : base(battleRoomEventProperties)
    {
        this.attempt = attempt;
        this.isSuccess = isSuccess;
        this.reconnectedIn = reconnectedIn;
        this.isConnectedToInternet = isConnectedToInternet;
        this.isConnectedToSmartfox = isConnectedToSmartfox;
        this.isConnectionModeSwitched = isConnectionModeSwitched;
        this.averagePing = averagePing;
        this.pingState = pingState;
        this.retriesLeft = retriesLeft;
        this.reason = reason;
    }
}
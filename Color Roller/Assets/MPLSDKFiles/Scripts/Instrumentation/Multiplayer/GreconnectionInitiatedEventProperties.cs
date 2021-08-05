using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GreconnectionInitiatedEventProperties : BattleRoomEventProperties
{
    [JsonIgnore]
    public const string EVENT_NAME = "Battle Reconnection Initiated";

    [JsonProperty("Attempt")]
    public int attempt;

    [JsonProperty("Retries Allowed")]
    public int retriesAllowed;

    [JsonProperty("Is Connected To Internet")]
    public string isConnectedToInternet;

    [JsonProperty("Is Connected To Smartfox")]
    public string isConnectedToSmartfox;

    [JsonProperty("Is Connection Mode Switched")]
    public string isConnectionModeSwitched;

    [JsonProperty("Is Minimised")]
    public string isMinimised;

    [JsonProperty("Reason")]
    public string reason;

    [JsonProperty("Average Ping")]
    public double averagePing;

    [JsonProperty("Ping State")]
    public string pingState;

    public GreconnectionInitiatedEventProperties(BattleRoomEventProperties battleRoomEventProperties, int attempt, int retriesAllowed, string isConnectedToInternet, string isConnectedToSmartfox, string isConnectionModeSwitched, string isMinimised, string reason, double averagePing, string pingState): base(battleRoomEventProperties)
    {
        this.attempt = attempt;
        this.retriesAllowed = retriesAllowed;
        this.isConnectedToInternet = isConnectedToInternet;
        this.isConnectedToSmartfox = isConnectedToSmartfox;
        this.isConnectionModeSwitched = isConnectionModeSwitched;
        this.isMinimised = isMinimised;
        this.reason = reason;
        this.averagePing = averagePing;
        this.pingState = pingState;
    }
}

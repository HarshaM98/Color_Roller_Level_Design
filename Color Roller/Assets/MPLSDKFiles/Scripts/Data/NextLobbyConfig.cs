
using System;
using UnityEngine;

[Serializable]
public class NextLobbyConfig
{
    public int LobbyId;
    public string EntryCurrency;
    public double EntryFee;
    public double WinningAmount;



    /************************************** Public Functions **************************************/
    public override string ToString()
    {
        return JsonUtility.ToJson(this);

    }
}
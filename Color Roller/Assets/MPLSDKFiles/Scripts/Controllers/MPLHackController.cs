using System;
using UnityEngine;

public class MPLHackController : MonoBehaviour 
{
    private bool speedHacked;
    public enum MPLHackType
    {
        SpeedHack = 0
    }

	private void Start()
	{
        speedHacked = false;
	}

	public void HackDetected(int hackTypeInt)
    {
        MPLHackType hackType = (MPLHackType)hackTypeInt;
        Debug.Log("MPL: Hack detected: " + hackType);

        if (hackType == MPLHackType.SpeedHack) speedHacked = true;

        Session.Instance.FraudDetected(hackType.ToString(), "");
    }

    public string GetFlags()
    {
        string flags = "" + Convert.ToInt32(speedHacked);
        return flags;
    }

    public bool Hacked()
    {
        return speedHacked;
    }
}

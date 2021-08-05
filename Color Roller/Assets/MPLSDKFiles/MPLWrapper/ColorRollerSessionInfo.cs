using System;
using UnityEngine;

[Serializable]
public class ColorRollerSessionInfo : SessionInfo
{
    /******************************** Global Variables & Constants ********************************/

    public float MaxTime;
    public bool QATestingMode;
    public bool LoadParticularLevel;
    public int LevelToLoad; 

    /************************************** Public Functions **************************************/
    public override string ToString()
    {
        return String.Format("");
    }
}
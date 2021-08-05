using System;

[Serializable]
public class ScoreTamperData
{
    public int ScoreStoredInSDK;
    public int PreviousGameScore;

    public ScoreTamperData(int scoreStoredInSdk, int prevGameScore)
    {
        ScoreStoredInSDK = scoreStoredInSdk;
        PreviousGameScore = prevGameScore;
    }

    public override string ToString()
    {
        return ("{" + ScoreStoredInSDK + ":" + PreviousGameScore + "}");
    }
}

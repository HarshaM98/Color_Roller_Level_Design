using System;
using Newtonsoft.Json;
[Serializable]
public class DynamicReward
{

    public double cashWinning;
    public int playerCount;
    public double[] rankWiseWinning;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
        
    }

}

using System;
using Newtonsoft.Json;
[Serializable]
public class Participants {

    public int id;
    public string displayName;
    public string avatar;
    public string tier;
    public bool pro;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
        //return String.Format("id: {0}, finalScore: {1}, mplUserId: {2}, rank: {3}, rewards: {4}",
        //id, finalScore, mplUserId, rank, rewards);
    }

}

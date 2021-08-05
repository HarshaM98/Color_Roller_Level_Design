using Newtonsoft.Json;
using System;

public class ProfileFollowedEvent
{

    [JsonProperty("User ID")]
    public string UserId;

    [JsonProperty("User Name")]
    public string UserName;

    [JsonProperty("User Mobile Number")]
    public string UserMobileNumber;

    [JsonProperty("Has Followed")]
    public bool HasFollowed;

    [JsonProperty("Entry Point")]
    public string EntryPoint;

    [JsonProperty("Follow Value")]
    public int FollowValue;


    public ProfileFollowedEvent(string userId,string userName,string userMobileNumber,bool hasFollowed,string entryPoint,int followValue)
    {
        UserId = userId;
        UserName = userName;
        UserMobileNumber = userMobileNumber;
        HasFollowed = hasFollowed;
        EntryPoint = entryPoint;
        FollowValue = followValue;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

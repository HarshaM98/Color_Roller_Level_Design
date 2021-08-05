using System.Collections.Generic;





[System.Serializable]
public class TaskBase
{
    public string name;
    public int id;
    public string key;
    public string currentValue;
    public string targetValue;
    public string description;

}
[System.Serializable]
public class AllTask
{
    public string name;
    public string id;
    public string category;
    public bool unlocked;
    public bool selected;
    public List<TaskBase> tasks = new List<TaskBase>();
    public string categoryId = "0";

}
[System.Serializable]
public class Payload
{
    public List<AllTask> collectibles;

}
[System.Serializable]
public class ApplicationCollectibleData
{
    public Payload payload;

}


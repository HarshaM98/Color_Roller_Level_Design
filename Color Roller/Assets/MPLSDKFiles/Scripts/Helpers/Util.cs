using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Util
{
    public static bool IsInternetOff()
    {
        return (Application.internetReachability == NetworkReachability.NotReachable);
    }

    public static IEnumerator WaitAndExecute(float waitTime, UnityAction callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback();
    }
}

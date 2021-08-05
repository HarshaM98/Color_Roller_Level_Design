using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;

public enum MPL_SDK_REQUEST_TYPE
{
    NONE = -1,
    POST = 0,
    GET = 1,
    GRAPH_QUERY,
}

public static class MplSdkApiHandler
{
    public const int REQUEST_TIMEOUT = 40;



    public static string BASE_URL = MPLController.Instance.GetCallBackURL();


    public static string skUID = string.Empty;

    public static IEnumerator Request(string urlExtension,
                           string requestBody,
                           MPL_SDK_REQUEST_TYPE webRequestMethodType,
                           WWWForm wWWForm = null,
                           Action<MPLSdkRequestInfo> callBackWithData = null,
                           string userName = null)
    {
        string url = BASE_URL + urlExtension;

        if (false) //Check For internet here
        {
            MPLSdkRequestInfo info = new MPLSdkRequestInfo
            {
                isInterNetConnectionAvailable = false,
                isSuccess = false
            };

            if (callBackWithData != null) callBackWithData(info);
        }
        else
        {
            switch (webRequestMethodType)
            {
                case MPL_SDK_REQUEST_TYPE.POST:
                    {
                        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wWWForm))
                        {
                            unityWebRequest.timeout = (int)REQUEST_TIMEOUT;
                            unityWebRequest.chunkedTransfer = true;
                            string json = requestBody;

                            byte[] payload = Encoding.UTF8.GetBytes(json);
                            UploadHandler headerData = new UploadHandlerRaw(payload);

                            unityWebRequest.uploadHandler = headerData;

                            
                            

                            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

                            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + MPLController.Instance.GetEncryptedToken());

                            unityWebRequest.SetRequestHeader("HeaderEncoding", "UTF-8");

                            //unityWebRequest.SetRequestHeader("Authorization", "Bearer AT/HrqXGSmOO4hzMq7l1kBbxAi8Tp0JqzBT");

                            yield return unityWebRequest.SendWebRequest();

                            MPLSdkRequestInfo info = new MPLSdkRequestInfo();

                            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                            {
                                info.errorDescription = unityWebRequest.error;
                                info.isSuccess = false;
                                Debug.Log("API request isNetworkError or isHttpError ");
                                Debug.Log("API request=" + unityWebRequest.error + ", urlExtension = " + urlExtension);
                                Debug.Log("API request code=" + unityWebRequest.responseCode);
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(unityWebRequest.error) && unityWebRequest.isDone)
                                {
                                    info.callBackData = unityWebRequest.downloadHandler.data;

                                    info.isSuccess = true;
                                }
                                else
                                {
                                    info.errorDescription = unityWebRequest.error;
                                    info.isSuccess = false;
                                    Debug.Log("API request isNetworkError or isHttpError 22222");
                                    Debug.Log("API request2222=" + unityWebRequest.error);
                                }
                            }

                            yield return new WaitForEndOfFrame();

                            if (callBackWithData != null) callBackWithData(info);
                        }
                    }
                    break;

                case MPL_SDK_REQUEST_TYPE.GET:
                    {
                        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
                        {
                            unityWebRequest.timeout = REQUEST_TIMEOUT;
                            unityWebRequest.chunkedTransfer = true;


                            

                            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

                            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + MPLController.Instance.GetEncryptedToken());
                            Debug.Log("token=" + MPLController.Instance.GetEncryptedToken());

                            unityWebRequest.SetRequestHeader("HeaderEncoding", "UTF-8");
                            yield return unityWebRequest.SendWebRequest();

                            MPLSdkRequestInfo info = new MPLSdkRequestInfo();

                            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                            {
                                info.errorDescription = unityWebRequest.error;
                                info.isSuccess = false;
                                Debug.Log("API request isNetworkError or isHttpError ");
                                Debug.Log("API request=" + unityWebRequest.error);
                                Debug.Log("API request code=" + unityWebRequest.responseCode);
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(unityWebRequest.error) && unityWebRequest.isDone)
                                {
                                    if (string.IsNullOrEmpty(unityWebRequest.downloadHandler.text))
                                    {
                                        info.errorDescription = "Content not available";
                                        info.isSuccess = false;
                                    }
                                    else
                                    {
                                        info.isSuccess = true;
                                        info.callBackData = unityWebRequest.downloadHandler.data;
                                    }
                                }
                                else
                                {
                                    info.errorDescription = unityWebRequest.error;
                                    info.isSuccess = false;
                                }
                            }

                            yield return new WaitForEndOfFrame();

                            if (callBackWithData != null) callBackWithData(info);
                        }
                    }
                    break;
            }
        }
    }
}

public class MPLSdkRequestInfo
{
    public bool isSuccess = false;
    public bool isInterNetConnectionAvailable = true;
    public byte[] callBackData = null;
    public string errorDescription = string.Empty;
}



[Serializable]
public class MPLSdkSubmitScoreModel
{
    public int userId;
    public int score;
    public MPLSDKScoreData scoreData;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

[Serializable]
public class MPLSDKScoreData
{
    public string gameEndReason;
    public string playerData;
    public List<AllTask> collectibles;
    public SessionResult3rdParty sessionResult3RdParty; 
}


[Serializable]
public class MPLSdkBattleFinishInfo
{
    public MPLSdkBattleFinishStatusInfo status = new MPLSdkBattleFinishStatusInfo();

    public MPLSdkBattleFinishPayloadInfo payload = new MPLSdkBattleFinishPayloadInfo();
}

[Serializable]
public class MPLSdkBattleFinishPlayersInfo
{
    public int userId;
    public int score;
    public int rank;
    public double cashWinnings;
    public double tokenWinnings;
    public bool canPlayAgain;
    public string nextLobbyConfig;
    public string extraInfo;
    public bool isCashReward=true;
    public string extReward;
    

    public MPLSdkBattleFinishPlayersInfo(int userId,int score,int rank,double cashWinnings,double tokenWinnings,bool canPlayAgain,string nextLobbyConfig, string extraInfo, bool isCashReward, string extReward)
    {
        this.userId = userId;
        this.score = score;
        this.rank = rank;
        this.cashWinnings = cashWinnings;
        this.tokenWinnings = tokenWinnings;
        this.canPlayAgain = canPlayAgain;
        this.nextLobbyConfig = nextLobbyConfig;
        this.extraInfo = extraInfo;
        this.isCashReward = isCashReward;
        this.extReward = extReward;
        
    }

    public MPLSdkBattleFinishPlayersInfo()
    {

    }
}



[Serializable]
public class MPLSdkCreateBattleInfo
{
    public MPLSdkBattleFinishStatusInfo status = new MPLSdkBattleFinishStatusInfo();

    public MPLSdkCreatePayloadInfo payload = new MPLSdkCreatePayloadInfo();
}
[Serializable]
public class MPLSdkAccountBalanceInfo
{
    public MPLSdkBattleFinishStatusInfo status = new MPLSdkBattleFinishStatusInfo();

    public MPLSdkAccountBalancePayloadInfo payload = new MPLSdkAccountBalancePayloadInfo();
}

[Serializable]
public class MPLSdkCreatePayloadInfo
{
    public bool success = false;
    public string eventId;
}



[Serializable]
public class MPLSdkAccountBalancePayloadInfo
{
    public int userId;
    public double tokenBalance;
    public double depositBalance;
    public double bonusBalance;
    public double totalBalance;
    public double withdrawableBalance;
}


[Serializable]
public class MPLSdkBattleFinishPayloadInfo
{
    public List<MPLSdkBattleFinishPlayersInfo> players = new List<MPLSdkBattleFinishPlayersInfo>();

    public bool battleAgainDisabled = false;

    public string battleStatus = string.Empty;
}

[Serializable]
public class MPLSdkBattleFinishStatusInfo
{
    public int code = 0;

    public string message = string.Empty;
}

[Serializable]
public class MPLSdkLobbyConfigInfo
{
    public MPLSdkBattleFinishStatusInfo status = new MPLSdkBattleFinishStatusInfo();
    public MPLSdkLobbyConfigData payload;
}

[Serializable]
public class MPLSdkLobbyConfigData
{
    public string gameData;
}





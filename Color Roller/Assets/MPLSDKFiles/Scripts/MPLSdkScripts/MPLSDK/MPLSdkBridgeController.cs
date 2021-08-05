using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine.Events;


public class MPLSdkBridgeController : MonoBehaviour
{


    
    
    private UnityAction<AccountBalance> accountBalanceCallback;
    public string lobbyConfigJson;
    public delegate void MPLEvents(string eventId);
    public event MPLEvents StartGame;

    public delegate void MPLEndGameEvents();
    public event MPLEndGameEvents UserGameEnd;
      

    public delegate void MPLBattleFinishEvent(string mPLBattleFinishInfo);
    public event MPLBattleFinishEvent BattleFinish;

    public delegate void MPLBattleStartEvent(List<UserProfile> users, string roomName);
    public event MPLBattleStartEvent StartGameScene;



    public bool is1VN;
    public delegate void MPLFailedNotificationEvent(MPLGameEndReason.GameEndReasons gameEndReason);
    public event MPLFailedNotificationEvent ApiCallFailed;
    private static bool created = false;
    public string roomName;

    private string battleID;
    public UserProfile profile;
    public List<UserProfile> players;

    public MPLGameConfig mPLGameConfig;
    private static MPLSdkBridgeController instance;
    public Text _PortraitText, _LandScapeText;
    public static MPLSdkBridgeController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MPLSdkBridgeController>();
            return instance;
        }
    }


    public MultiPlayerCanvasController1vN multiPlayerCanvasSdkControllerPro, multiPlayerCanvasSdkControllerLand, multiPlayerCanvasSdkController;


    public void OnUserScoresUpdate(Dictionary<int, int> usersToScore)
    {
        if (is1VN)
        {
            if (multiPlayerCanvasSdkController.battleEndController1VN != null && multiPlayerCanvasSdkController.battleEndController1VN.gameObject.activeSelf)
            {
                multiPlayerCanvasSdkController.battleEndController1VN.OnUserScoresUpdate(usersToScore);
            }
        }

    }
   
    public MPLGameConfig GetGameConfig()
    {
        return MPLController.Instance.gameConfig;
    }
    public UserProfile GetUserProfile()
    {



        return profile;
    }
	public void StartTutorialAgain()
    {
        MPLController.Instance.SetTutorialType(MPLController.MPLTutorialType.GameEndScreen);
        MPLController.Instance.LoadInteractiveFTUE();
        MPLController.Instance.smartFoxManager.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GetDisplayImage(string dpUrl, UnityAction<Sprite> callback)
    {

        Debug.Log("Fetching User dp");
        

        if (is1VN)
        {
            StartCoroutine(MultiplayerGamesHandler.Instance.GetDpAsyncInGame(dpUrl, callback));
        }

    }




    public void ShowErrorPopup(MPLGameEndReason.GameEndReasons reason)
    {
        string title, description;
        switch (reason)
        {
            case MPLGameEndReason.GameEndReasons.SUBMIT_SCORE_FAILED:
                {
                    title = "Submit Score Failed";
                    description = "Submit Score Failed";
                    break;
                }
            case MPLGameEndReason.GameEndReasons.BATTLE_CREATION_FAILED:
                {
                    title = "Battle Creation Failed";
                    description = "Battle Creation Failed";
                    break;
                }
            case MPLGameEndReason.GameEndReasons.CONNECTION_LOST:
                {
                    title = mPLGameConfig.ICLostTitle;
                    description = mPLGameConfig.ICLostMessage;
                    break;
                }

            case MPLGameEndReason.GameEndReasons.FINISH_BATTLE_FAILED:
                {
                    title = "Finish Battle Failed";
                    description = "Finish Battle Failed";
                    break;
                }
            case MPLGameEndReason.GameEndReasons.MATCH_NOT_FOUND:
                {
                    title = MPLController.Instance?.localisationDetails.GetLocalizedText("Game Ended!");
                    description = MPLController.Instance?.localisationDetails.GetLocalizedText("You got disconnected as MPL app was minimized. Please do not minimize app during a game.");
                    break;
                }
            default:
                {
                    title = "Error Title";
                    description = "Error Body";
                    break;

                }
        }
        if (is1VN)
        {
            multiPlayerCanvasSdkController.ShowErrorPopUp(reason, title, description);
        }

    }
    private void SetLobbyConfig(string lobbyId)
    {
        GetLobbyConfig(lobbyId, GetLobbyConfigResponse);

    }
    private void GetLobbyConfig(string lobbyId, Action<bool, string> callback)
    {
        Debug.Log("MPL:fetching lobby config");

        string url = "/gameplay/lobby/" + lobbyId + "/join";

        Debug.Log("Fetch lobby url=" + url);
        StartCoroutine(MplSdkApiHandler.Request(url, null, MPL_SDK_REQUEST_TYPE.GET, null, (MPLSdkRequestInfo status) =>
        {
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);
                MPLSdkLobbyConfigInfo mPLSdkLobbyConfigInfo = JsonUtility.FromJson<MPLSdkLobbyConfigInfo>(callbackDataInString);
                Debug.LogError("fetching lobby call back=" + callbackDataInString);

                Debug.LogError("Lobby Game Data=" + mPLSdkLobbyConfigInfo.payload.gameData);


                callback(true, mPLSdkLobbyConfigInfo.payload.gameData);

            }
            else
            {

                Debug.LogError("Get lobby config failed = " + status.errorDescription);

                callback(false, "");
            }

        }));

    }

    public void GetLobbyConfigResponse(bool status, string lobbyConfig)
    {
        Debug.Log("Get lobby response=" + lobbyConfig);
        if (status)
        {
            lobbyConfigJson = lobbyConfig;

            if (!string.IsNullOrEmpty(lobbyConfigJson))
            {
                JsonUtility.FromJsonOverwrite(lobbyConfigJson, MPLController.Instance.gameConfig);
                MPLController.Instance.SetSessionConfig(lobbyConfigJson);
                MPLController.Instance.InitGame();
            }
            else
            {
                Debug.Log("Fetched Lobby Config is incorrect");
            }


            
        }
        else
        {

            lobbyConfigJson = "";
        }
    }
    public void SubmitScore(List<MPLSdkSubmitScoreModel> submitScoreData, string eventId)
    {
        Debug.Log("Submitting Score");


        SubmitScoreOnMPL(eventId, submitScoreData, FinishBattleResponse);
    }

    
    public void FinishBattleResponse(bool status, string callbackDataInString)
    {
        
        Debug.Log("Finishing Battle response");
        try
        {
            if (status)
            {



                Debug.Log("Finish Battle Success");

                if (BattleFinish != null)
                {
                    BattleFinish(callbackDataInString);
                }
            }
            else
            {
                if (ApiCallFailed != null)
                {
                    ApiCallFailed(MPLGameEndReason.GameEndReasons.FINISH_BATTLE_FAILED);
                }
            }
        }
        catch (Exception e)
        {

            Debug.Log("Finish Battle Failed");
        }
    }
    public void SubmitEvent(string eventName, string properties)
    {
        Debug.Log("MPL: Submitting event =" + eventName + " with properties =" + properties);
        SubmitEventOnMPL(eventName, properties);
    }
    private void SubmitEventOnMPL(string eventName, string properties)
    {
        string url = "/partners/events/match-events";

        string requestBody = "{" + String.Format("\"eventName\": \"{0}\",\"userId\": \"{1}\",\"eventId\": \"{2}\",\"eventType\":\"{3}\",\"properties\": {4}",
                                             eventName, MPLController.Instance.gameConfig.Profile.id, battleID, "battle", properties) + "}";


        MPLController.Instance.PrintExtraLog("SubmitEventOnMPL requestBody ="+requestBody);



        StartCoroutine(MplSdkApiHandler.Request(url, requestBody, MPL_SDK_REQUEST_TYPE.POST, null, (MPLSdkRequestInfo status) =>
        {

            Debug.Log("Submit Event status:" + status.ToString());
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);
                MPLController.Instance.PrintExtraLog("SubmitEventOnMPL "+callbackDataInString);

                MPLSdkCreateBattleInfo mPLCreateBattleInfo = JsonUtility.FromJson<MPLSdkCreateBattleInfo>(callbackDataInString);









                if (mPLCreateBattleInfo.status.code == 200)
                {
                    Debug.Log("MPL : Event Submitted");
                }
                else
                {

                    Debug.LogError("SubmitEventOnMPL failed innner case = " + status.errorDescription);


                    return;

                }







            }
            else
            {

                Debug.LogError("SubmitEventOnMPL failed in upper case = " + status.errorDescription);

            }
        }));
    }
    public void SubmitBattleFinishData(string callbackDataInString)
    {
        MPLSdkBattleFinishInfo mPLBattleFinishInfo = JsonUtility.FromJson<MPLSdkBattleFinishInfo>(callbackDataInString);
        List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList = mPLBattleFinishInfo.payload.players;

        if (is1VN)
        {
            multiPlayerCanvasSdkController.OnBattleFinish(mPLBattleFinishPlayersInfoList, mPLBattleFinishInfo.payload.battleAgainDisabled);
        }

    }


    public void ShowResultScreen(int playerscore)
    {
        if (is1VN)
        {
            multiPlayerCanvasSdkController.OnScoreSubmitted(MPLGameEndReason.GameEndReasons.ALL_LEVELS_COMPLETED, playerscore);
        }

    }




    private int GetUserId()
    {
        int userId = 0;
        if (MPLController.Instance.IsUnityDeviceDebug())
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, Globals.USER_IDS.Count - 1);
            userId = Globals.USER_IDS[randomNumber];
            MPLController.Instance.gameConfig.AuthToken = Globals.USER_TO_AUTH[userId];

        }
        else
        {
            userId = MPLController.Instance.gameConfig.Profile.id;
            MPLController.Instance.gameConfig.AuthToken = MPLController.Instance.gameConfig.AuthToken;
        }
        return userId;
    }
    public void StartBattle()
    {
        if (is1VN)
        {
            multiPlayerCanvasSdkController.OnStartBattle();
        }



        Debug.Log("Start battle Called");
    }

    public void SendEventToGame(string type)
    {
        switch (type)
        {

            case "startGame":
                {

                    if (StartGame != null)
                    {
                        StartGame(battleID);
                    }
                    break;
                }

            case "startGameScene":
                {
                    Debug.Log("Players Start Game Scene Case=");
                    if(MPLController.Instance.IsThirdPartyGame())
                    {
                        MPLController.Instance.smartFoxManager.SetActive(false);

                    }
                    //

                    battleID = roomName;
                    Debug.Log("BattleId=" + battleID);
                   
                    if (StartGameScene != null)
                    {

                        GameBattaleIdScreenHandler.mInstance.ShowBattleIdThirdParty(battleID);
                        Debug.Log("MPL Room Name=" + roomName);
                        StartGameScene(MultiplayerGamesHandler.Instance.userListOnMatch, roomName);
                        //MultiplayerGamesHandler.Instance.forceDisconnect = false;
                        Debug.Log("Players Start Game Scene=");
                        for (int i = 0; i < MultiplayerGamesHandler.Instance.userListOnMatch.Count; i++)
                        {
                            Debug.Log(MultiplayerGamesHandler.Instance.userListOnMatch[i]);
                        }
                    }
                    else
                    {
                        Debug.Log("Start Game Scene Not there");
                    }
                    break;
                }

            case "userGameEnd":
                {
                    Debug.Log("MPL:ending game for user");
                    if(UserGameEnd!=null)
                    {
                        Debug.Log("MPL:ending game for user available");
                        UserGameEnd();
                    }
                    break;
                }


        }
    }

    void Awake()
    {
        if (Instance == null)  // Use this if you create specific game object to this
        {
            instance = this;
        }

        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            
            created = true;
        }

        Debug.Log("Start MPlSdk");
        

    }

    public void GetAccountBalance(UnityAction<AccountBalance> callback)
    {
        Debug.Log("MPL:fetching account balance");
        accountBalanceCallback = callback;
    
        string url = "/user/balance";

        Debug.Log("Fetch Account Balance url=" + url);
        StartCoroutine(MplSdkApiHandler.Request(url, null, MPL_SDK_REQUEST_TYPE.GET, null, (MPLSdkRequestInfo status) =>
        {
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);
                MPLSdkAccountBalanceInfo mPLSdkAccountBalanceInfo = JsonUtility.FromJson<MPLSdkAccountBalanceInfo>(callbackDataInString);
                Debug.LogError("Account Balance Callback=" + callbackDataInString);
                AccountBalance accountBalance = new AccountBalance(mPLSdkAccountBalanceInfo.payload.tokenBalance, mPLSdkAccountBalanceInfo.payload.depositBalance, mPLSdkAccountBalanceInfo.payload.bonusBalance, mPLSdkAccountBalanceInfo.payload.totalBalance, mPLSdkAccountBalanceInfo.payload.withdrawableBalance);



                accountBalanceCallback(accountBalance);
            }
            else
            {

                Debug.LogError("Get Account Balance failed = " + status.errorDescription);
                if (accountBalanceCallback != null)
                {

                    accountBalanceCallback(null);
                }
            }
        }));

    }
    private void OnEnable()
    {

        if (!MPLController.Instance.IsUnityDeviceDebug())
        {
            SetLobbyConfig(MPLController.Instance.gameConfig.LobbyId.ToString());
        }
        else
        {
            MPLController.Instance.InitGame();
        }
       

    }
    public void StartMultiplayerGame()
    {
        StartMultiplayerController();
    }
  
    private void Start()
    {
        GameBattaleIdScreenHandler.mInstance.TextUpdate(_PortraitText, _LandScapeText);
    }
    void StartMultiplayerController()
    {
        mPLGameConfig = GetGameConfig();
        profile = MPLController.Instance.gameConfig.Profile;
        int id = profile.id;
        profile.id = id;
        Debug.Log("Matchmaking Bridge ID=" + profile.id);



        
        is1VN = true;

        if (!MPLController.Instance.isLandscape)
        {


            if (multiPlayerCanvasSdkControllerPro != null)
            {
                multiPlayerCanvasSdkController = multiPlayerCanvasSdkControllerPro;
            }

        }
        else
        {


            if (multiPlayerCanvasSdkControllerLand != null)
            {
                multiPlayerCanvasSdkController = multiPlayerCanvasSdkControllerLand;
            }
        }
        if (is1VN)
        {
            multiPlayerCanvasSdkController.gameObject.SetActive(true);
            multiPlayerCanvasSdkController.StartMultiplayerController();
        }


    }
    private void OnDisable()
    {

    }


    void Update()
    {

    }

    public void RemoveOpponent(int id)
    {

       

        

    }

    public void CreateBattle()
    {
        Debug.Log("Creating Battle");
        string playerIds = "[";
        for (int i = 0; i < MultiplayerGamesHandler.Instance.userListOnMatch.Count; i++)
        {
            if (i != MultiplayerGamesHandler.Instance.userListOnMatch.Count - 1)
            {
                playerIds = playerIds + MultiplayerGamesHandler.Instance.userListOnMatch[i].id + ",";
            }
            else
            {
                playerIds = playerIds + MultiplayerGamesHandler.Instance.userListOnMatch[i].id + "]";
            }
        }



        CreateBattleOnMPL(MPLController.Instance.gameConfig.LobbyId.ToString(), playerIds, CreateBattleResponse);
    }

    private void CreateBattleResponse(bool status, string eventId)
    {
        if (status)
        {
            Debug.Log("Create Battle Success");
            battleID = eventId;
            SendEventToGame("startGame");
        }
        else
        {

            if (ApiCallFailed != null)
            {
                ApiCallFailed(MPLGameEndReason.GameEndReasons.BATTLE_CREATION_FAILED);
            }
            Debug.Log("Create Battle Failed");
        }
    }


    public void CreateBattleOnMPL(string lobbyId, string userIds, Action<bool, string> callback)
    {
        string url = "/partners/events";

        string requestBody = "{\"info\":{\"lobbyId\":" + lobbyId + "},\"userIds\":" + userIds + ",\"eventId\":\"" + battleID + "\",\"eventType\":\"battle\"}";


        MPLController.Instance.PrintExtraLog("CreateBattleOnMPL requestBody ="+requestBody);
       


        StartCoroutine(MplSdkApiHandler.Request(url, requestBody, MPL_SDK_REQUEST_TYPE.POST, null, (MPLSdkRequestInfo status) =>
        {

            Debug.Log("Create Battle status:" + status.ToString());
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);
                MPLController.Instance.PrintExtraLog("CreateBattleOnMPL =" +callbackDataInString);
                
                MPLSdkCreateBattleInfo mPLCreateBattleInfo = JsonUtility.FromJson<MPLSdkCreateBattleInfo>(callbackDataInString);









                if (mPLCreateBattleInfo.payload.success)
                {
                    if (callback != null)
                    {
                        callback(true, mPLCreateBattleInfo.payload.eventId);
                    }
                }
                else
                {

                    Debug.LogError("CreateBattleOnMPL failed innner case = " + status.errorDescription);

                    if (callback != null)
                    {
                        callback(false, "");
                    }
                    return;

                }







            }
            else
            {

                Debug.LogError("CreateBattleOnMPL failed in upper case = " + status.errorDescription);
                if (callback != null)
                {
                    callback(false, "");
                }
            }
        }));
    }






    //scoreDataKey is if any thing else othr then score u want to pass
    public void SubmitScoreOnMPL(string battleId, List<MPLSdkSubmitScoreModel> submitScoreData, Action<bool, string> callback)
    {
        string url = "/partners/events/scores";

        string scores = "[";

        for (int i = 0; i < submitScoreData.Count; i++)
        {
            if (i != submitScoreData.Count - 1)
            {
                scores = scores + submitScoreData[i].ToString() + ",";
            }
            else
            {
                scores = scores + submitScoreData[i].ToString() + "]";
            }
        }

        string requestBody = "{" + String.Format("\"eventId\": \"{0}\",\"eventType\": \"{1}\",\"scores\": {2}",
                                             battleId, "battle", scores) + "}";
        MPLController.Instance.PrintExtraLog("SubmitScoreOnMPL requestBody="+requestBody);
       


        StartCoroutine(MplSdkApiHandler.Request(url, requestBody, MPL_SDK_REQUEST_TYPE.POST, null, (MPLSdkRequestInfo status) =>
        {
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);

                MPLController.Instance.PrintExtraLog("SubmitScoreOnMPL ="+callbackDataInString);
                


                if (callbackDataInString.Contains("INTERNAL_SERVER_ERROR"))
                {
                    //PopUpHandlerClass.Instance.CreatePopUp ("UPLOAD FAILURE", "INTERNAL_SERVER_ERROR", PopUpType.OK, "Okay", GameAndMPLBridge.Instance.LaunchMPLActivityAndSendData, null, null, null, "", true);


                    Debug.LogError(callbackDataInString + " " + status.errorDescription + " " + status.isInterNetConnectionAvailable + " " + status.isSuccess);

                    if (callback != null)
                    {
                        callback(false, callbackDataInString);
                    }
                    return;
                }
                Dictionary<string, object> response = MiniJSON.Json.Deserialize(callbackDataInString) as Dictionary<string, object>;


                Debug.LogError("SubmitScoreOnMPL success");


                if (callback != null)
                {
                    callback(true, callbackDataInString);
                }
            }
            else
            {
                //Handle Failure Response




                if (callback != null)
                {
                    callback(false, "");
                }
            }
        }));
    }

    


}







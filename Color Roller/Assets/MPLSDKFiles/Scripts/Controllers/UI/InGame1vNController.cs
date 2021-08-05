using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;

public class InGame1vNController : MonoBehaviour
{
    public MultiPlayerCanvasController1vN multiPlayerCanvasController1VN;
    public MatchMakingController1vN matchMakingController1VN;
    public CanvasGroup canvasGroup;
    public GameObject opponentPlayerPrefab;
    public Transform opponentParent;
    public Text localPlayerScore;
    public Image localPlayerDp;
    private MultiplayerGamesHandler multiplayerGamesHandler;
    // Use this for initialization
    public int gameScore = 0;
    private void OnEnable()
    {
        multiplayerGamesHandler = MultiplayerGamesHandler.Instance;
        int childs = opponentParent.childCount;
        for (int i = 0; i < childs; i++)
        {
            Debug.Log("Getting Child Match=" + i);
            Destroy(opponentParent.GetChild(i).gameObject);
        }
        if (multiPlayerCanvasController1VN.sponserSlim != null)
        multiPlayerCanvasController1VN.sponserSlim.gameObject.SetActive(false);
        localPlayerScore.text = "0";
        if (multiplayerGamesHandler.userIdToCandidate!= null && multiplayerGamesHandler.userIdToCandidate.ContainsKey(MPLController.Instance.gameConfig.Profile.id))
        {
            localPlayerDp.sprite = multiplayerGamesHandler.userIdToCandidate[MPLController.Instance.gameConfig.Profile.id].displayPic;
            multiplayerGamesHandler.userIdToCandidate[MPLController.Instance.gameConfig.Profile.id].scoreText = localPlayerScore;
        }
        SmartFoxManager.Instance.OnUserVarsUpdated += OnUserVarsUpdated;
        SmartFoxManager.Instance.ConnectionLost += ConnectionLost;
        SmartFoxManager.Instance.OpponentFinished += OpponentFinished;
        SmartFoxManager.Instance.ConnectionFailed += ConnectionLost;
        
      

        int count = multiplayerGamesHandler.userListOnMatch.Count;
        
        for (int i = 0; i < count; i++)
        {
            int userId = multiplayerGamesHandler.userListOnMatch[i].id;
            if (userId != MPLController.Instance.gameConfig.Profile.id)
            {
                GameObject _opponentScoreItem = Instantiate(opponentPlayerPrefab);
                Image playerDp = _opponentScoreItem.GetComponent<PlayerItemController>().playerDp;
                Text playerScore = _opponentScoreItem.GetComponent<PlayerItemController>().score;
                playerScore.text = "0";
                if(multiplayerGamesHandler.userIdToCandidate!=null && multiplayerGamesHandler.userIdToCandidate.ContainsKey(userId))
                {
                    playerDp.sprite = multiplayerGamesHandler.userIdToCandidate[multiplayerGamesHandler.userListOnMatch[i].id].displayPic;
                    multiplayerGamesHandler.userIdToCandidate[userId].scoreText = playerScore;
                }
                _opponentScoreItem.transform.SetParent(opponentParent);
                _opponentScoreItem.transform.localScale = Vector3.one;
            }
        }
        canvasGroup.alpha = (!MPLController.Instance.IsAsyncGame()) ? 0 : 1;
        canvasGroup.blocksRaycasts = MPLController.Instance.IsAsyncGame();
        multiPlayerCanvasController1VN.gameSceneLoaded = true;
    }

    

    void Start()
    {

    }
    void OnUserVarsUpdated(User user, List<string> changed)
    {
        if (user.ContainsVariable("mplUserId"))
        {
            if (user.ContainsVariable("score"))
            {
                
                if (multiplayerGamesHandler.userIdToCandidate.ContainsKey((int)user.GetVariable("mplUserId").Value))
                {
                    if (multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText != null)
                    {
                        multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText.text = "" + user.GetVariable("score").Value;
                    }
                }


            }
        }

       
       
    }

    private void ConnectionLost(MPLGameEndReason.GameEndReasons reason)
    {
        Debug.Log("XXX123 = " + reason);
        //if (reason == SmartFoxManager.SmartFoxManagerReasons.DisconnectRequested)
        //{

        Session.Instance.EndGame(MPLGameEndReason.GameEndReasons.USER_QUIT, "Disconnected");
        //}

        //Event
        if (reason != MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND)
        {
            multiPlayerCanvasController1VN.ShowConnectionLost(reason);
        }
        MPLController mPLController = MPLController.Instance;

        MultiplayerGamesHandler.Instance.SubmitBattleEndedEvent(Session.Instance.GetScore(), "", 0, "Undecided", Session.Instance.GetReasonString(MPLGameEndReason.GameEndReasons.CONNECTION_LOST), mPLController.gameConfig.Profile.TotalBalance, mPLController.gameConfig.Profile.TokenBalance, false, (int)Session.Instance.GetGameplayDuration());

    }
    void OpponentFinished(int userId)
    {
        if(multiplayerGamesHandler.userIdToCandidate.ContainsKey(userId))
        {
            multiplayerGamesHandler.userIdToCandidate[userId].isScoreSubmitted = true;
        }
    }
    private void OnDisable()
    {
        SmartFoxManager.Instance.OnUserVarsUpdated -= OnUserVarsUpdated;

        SmartFoxManager.Instance.ConnectionLost -= ConnectionLost;
        SmartFoxManager.Instance.OpponentFinished -= OpponentFinished;
        SmartFoxManager.Instance.ConnectionFailed -= ConnectionLost;
    }
    // Update is called once per frame
    void Update()
    {

        gameScore = Session.Instance.GetScore();

        localPlayerScore.text = "" + gameScore;


        if (matchMakingController1VN.matchFailed.activeSelf)
        {
            matchMakingController1VN.matchFailed.SetActive(false);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("MPL: OnApplicationPause = " + pause + " = " + SmartFoxManager.Instance.gReconnectionInitiatedAt + " = " + SmartFoxManager.Instance.gReconnectForPause);

        if (pause)
        {
            Session.Instance.userMinimised = "YES";
            if (!MPLController.Instance.IsAsyncGame()) return;

            Debug.Log("OnApplicationPaue=InGameController1vN");

            if (SmartFoxManager.Instance.CanGReconnect())
            {
                if (SmartFoxManager.Instance.gReconnectionInitiatedAt == 0)
                {
                    Debug.Log("MPL: Pause and disconnect, can later greconnect");
                    SmartFoxManager.Instance.gReconnectionAttempt = SmartFoxManager.Instance.gReconnectionAttempt + 1;
                    SmartFoxManager.Instance.SubmitReconnectionInitiated("Minimized");
                    SmartFoxManager.Instance.Disconnect(MPLGameEndReason.GameEndReasons.PAUSE_GRECONNECT);
                }
                return;
            }

            Debug.Log("MPL: Pause and submit score");
            SessionResult result = MPLController.Instance.GetCurrentGameResult(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND);
            Session.Instance.ForceEndGame(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND, Session.Instance.GetReasonString(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND), result);
            MultiplayerGamesHandler.Instance.SubmitBattleEndedEvent(Session.Instance.GetScore(), "", 0, "Undecided", "Went in Background", MPLController.Instance.gameConfig.Profile.TotalBalance, MPLController.Instance.gameConfig.Profile.TokenBalance, false, (int)Session.Instance.GetGameplayDuration());
        }
        else if (SmartFoxManager.Instance.CanGReconnect() && SmartFoxManager.Instance.gReconnectForPause)
        {
            Debug.Log("MPL: Unpause, greconnect");
            SmartFoxManager.Instance.Connect();
        }
    }    
}

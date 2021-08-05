using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Sfs2X.Entities;
using System.Net;
using System.IO;

public class MultiplayerGamesHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private static MultiplayerGamesHandler instance;
    public MPLController mPLController;
    public int totalPlayers;
    public Sprite defaultimagecircle;
    private Image loaderBG;
    
    private int knockoutMinPlayers;
    private Dictionary<int,GameObject> knockOutPlayers;
    public GameObject knockoutDisclaimerText;
    public GameObject challengeWaitingPopup;


    public List<UserProfile> readyUsers;
    public int roomOwner;

    private bool dataAlreadySent=false;
    

    public bool forceDisconnect=false;
   
    public int playerJoinedCount = 0;
    private GameObject KnockoutErrorObject;
    private Image TimerLoaderImage;

    public bool battleEndedSent,isItUpsell,userEntered;
    private Button knockoutLobbyButton;
    private Text loadingSubtitleText;
    
    private GameObject localPlayer;
    public int maximumMatchMakingRetriesStatic;
    public int maximumMatchMakingRetries;
    public int TotalTime;
    private GameObject errorIcon, startingBattleObject;
    public string nextLobbyConfig;
   
    public bool isMatchFound=false, isThisFindAgain=false, isItFirstEntry;
    public IEnumerator cr;
    public List<UserProfile> userListOnMatch = new List<UserProfile>();
    private GameObject goToLobby;
    private Text timerText, startingBattleText;
    public Dictionary<int, Candidate> userIdToCandidate;
    
    private Dictionary<int, int> userIdToPlayerIndex;
    public List<GameObject> rewardObjects;
    
    private Transform playerListParent, prizeObjectParent;
    public GameObject playerListItemPrefab;
    private GameObject prizeObject,loaderImage;
    private int currentActiveChildIndex;
  
    private GameObject timerParent, battleMatchedObject;

    private List<GameObject> rowParents = new List<GameObject>();
    private GameObject contentItem;
    private GameObject viewPort;
    private bool isPortrait;
    private ScrollRect playersScrollView;
    private Transform scrollViewContent;
    

    private List<GameObject> playersSpawned = new List<GameObject>();

    private const float REDUCED_SCALE_PORTRAIT = 0.83f;
    private const float REDUCED_SCALE_LANDSCAPE = 0.7f;
    private const float REDUCED_HORIZONTAL_SPACING_PORTRAIT = 15f;
    private const float REDUCED_HORIZONTAL_SPACING_LANDSCAPE = 32f;
    private const float REDUCED_VERTICAL_SPACING_PORTRAIT = 0f;
    private const float REDUCED_VERTICAL_SPACING_LANDSCAPE = -15f;
    private const int PLAYER_PROFILES_HORIZONTAL_PADDING = 48;

    private const float NORMAL_HORIZONTAL_SPACING= 32f;

    private const float NORMAL_VERTICAL_SPACING = 24f;

    public bool isAdEnabled;

    private GameObject blurBG;

    private Image fairPlayImage;

    public enum MPLGameState
    {
        before_match_making=0,
        match_making=1,
        in_game=2,
        game_end=3,
        results_declared=4

    }
    private MPLGameState mPLGameState;
    void Start()
    {
        
    }

    void Awake()
    {
        instance = this;
    }

    public void SetGameState(MPLGameState state)
    {
        mPLGameState = state;
    }

    public MPLGameState GetGameState()
    {
        return mPLGameState;
    }
    void OnEnable()
    {
        readyUsers = new List<UserProfile>();
        mPLController = MPLController.Instance;
        userIdToCandidate = new Dictionary<int, Candidate>();
        userIdToPlayerIndex = new Dictionary<int, int>();
        rewardObjects = new List<GameObject>();
        

    }

    public void AddSfxListeners()
    {
        SmartFoxManager.Instance.MatchFound += OnMatchFound;
        SmartFoxManager.Instance.MatchUserIn += MatchUserIn;
        SmartFoxManager.Instance.MatchUserOut += OnMatchUserOut;
        SmartFoxManager.Instance.MatchGroupChange += OnMatchGroupChange;
        SmartFoxManager.Instance.KnockoutMatchUserIn += OnKnockoutMatchUserIn;
        SmartFoxManager.Instance.OpponentDidnotJoinKnockout += ShowKnockoutEnd;
        
        SmartFoxManager.Instance.OpponentFinished += OnOpponentFinished;
    }
    string resultDataOnDisconnection="";
    
    void OnOpponentFinished(int finishedUserId)
    {
        if(finishedUserId== roomOwner)
        {
        }
    }
    public void RemoveSfxListeners()
    {
        SmartFoxManager.Instance.MatchFound -= OnMatchFound;
        SmartFoxManager.Instance.MatchUserIn -= MatchUserIn;
        SmartFoxManager.Instance.MatchUserOut -= OnMatchUserOut;
        SmartFoxManager.Instance.MatchGroupChange -= OnMatchGroupChange;
        SmartFoxManager.Instance.KnockoutMatchUserIn -= OnKnockoutMatchUserIn;
        SmartFoxManager.Instance.OpponentDidnotJoinKnockout -= ShowKnockoutEnd;
        
        SmartFoxManager.Instance.OpponentFinished -= OnOpponentFinished;
    }

    public void SetGameEndUIReferences()
    {

    }


    

    
  
    public void ShowKnockoutEnd(List<UserProfile>users,int winnerId, List<UserProfile> joinedUsers)
    {
        
        
            
        
        if (!MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            return;
        }
        knockoutDisclaimerText.SetActive(false);
        goToLobby.GetComponent<Button>().onClick.RemoveAllListeners();
        goToLobby.GetComponent<Button>().onClick.AddListener(() => QuitUnity());
        if (cr != null)
        {
            StopCoroutine(cr);
        }
        bool isJoined=false;
        foreach (KeyValuePair<int, GameObject> entry in knockOutPlayers)
        {
            int userId = entry.Key;
            bool found = false;
            for(int i=0;i<joinedUsers.Count;i++)
            {
                if(userId == joinedUsers[i].id)
                {
                    found = true;
                    if (MPLController.Instance.gameConfig.Profile.id == userId)
                    {
                        isJoined = true;
                    }
                }

            }

            if(!found)
            {
                UpdateKnockoutJoinedUser(userId, false,false);
            }
            else
            {
                UpdateKnockoutJoinedUser(userId, true, false);
            }
        }
       
        if (isJoined)
        {
            TimerLoaderImage.color = new Color32(25, 190, 0, 179);
            loaderBG.color = new Color32(25, 190, 0, 179);
            startingBattleText.text = "Opponent did not join";
            loadingSubtitleText.text = "Min "+knockoutMinPlayers+" players needed to start match";
            

        }
        else
        {
            TimerLoaderImage.color= new Color32(248, 103, 29, 179);
            loaderBG.color = new Color32(248, 103, 29, 179);
            startingBattleText.text = "You did not join in time!";
            //loadingSubtitleText.text = "You lost this round";
            loadingSubtitleText.gameObject.SetActive(false);
        }

        knockoutLobbyButton.onClick.RemoveAllListeners();
        Session.Instance.SetQuitReason(Session.MPLQuitReason.knockout_quit);
        knockoutLobbyButton.onClick.AddListener(() => QuitUnity());
        knockoutLobbyButton.gameObject.transform.parent.gameObject.SetActive(true);

        timerText.gameObject.SetActive(false);
        KnockoutErrorObject.SetActive(true);
        SubmitBattleMatchingEnded(SmartFoxManager.SmartFoxManagerReasons.OpponentDidNotJoinKnockout.ToString(), false);
        SubmitBattleStartedEvent(false, SmartFoxManager.SmartFoxManagerReasons.OpponentDidNotJoinKnockout.ToString());
        TotalTime = 0;
        SmartFoxManager.Instance.mmRetryAttempt = 0;
        
        

    }
    bool isItFirstKnockoutEntry;
    bool isItFirstMatchMakinhIteration = true;
    public void SetUIReferences(GameObject GoToLobby,Text TimerText,Text StartingBattleText,Transform PlayerListParent,Transform PrizeObjectParent,GameObject PrizeObject,GameObject localPlayer,Text loadingSubtitleText,GameObject ChallengeWaitingPopup,GameObject StartingBattleObject,GameObject timerParent,GameObject battleMatchedObject, List<GameObject> rowParents, GameObject contentItem, GameObject viewPort,bool isPortrait, ScrollRect playersScrollView,Transform scrollViewContent,Transform prizeObjectParentWithAd, GameObject prizeObjectLandscapeWithAd,GameObject blurBG,GameObject knockoutErrorObject,Image timerLoaderImage,Button knockoutLobbyButton,Image loaderBG, GameObject knockoutDisclaimerText,Image fairPlayImage=null)
    {
        isConnectionFlagsSet = false;
        battleStartedSent = false;
        
		 if (isItFirstMatchMakinhIteration)
        {
            SetGameState(MPLGameState.before_match_making);
            isItFirstMatchMakinhIteration = false;
        }
        if (!MPLController.Instance.smartFoxManager.activeSelf)
        {
            MPLController.Instance.smartFoxManager.SetActive(true);
        }
      
        challengeWaitingPopup = ChallengeWaitingPopup;
        dataAlreadySent = false;
        knockOutPlayers = new Dictionary<int, GameObject>();
        knockOutPlayers.Add(MPLController.Instance.gameConfig.Profile.id, localPlayer);
        isItFirstKnockoutEntry = true;
        this.knockoutDisclaimerText = knockoutDisclaimerText;

        if(MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            this.knockoutDisclaimerText.SetActive(true);
        }
        this.loaderBG = loaderBG;
        this.loaderBG.color = new Color32(196, 196, 196, 51);
        this.localPlayer = localPlayer;
        this.blurBG= blurBG;
        this.knockoutLobbyButton = knockoutLobbyButton;
        EnableBlurBG(true);
        this.loadingSubtitleText = loadingSubtitleText;
        //if (MPLController.Instance.isItAnIndoBuild)
        //    isAdEnabled = MPLController.Instance.gameConfig.sponsorBattle;
        //else
        isAdEnabled = MPLController.Instance.gameConfig.IsAdsEnabled;
        userEntered = true;
        startingBattleObject = StartingBattleObject;
        startingBattleText = StartingBattleText;
        KnockoutErrorObject = knockoutErrorObject;
        TimerLoaderImage = timerLoaderImage;
        this.rowParents = rowParents;
        this.contentItem = contentItem;
        this.viewPort = viewPort;
        this.isPortrait = isPortrait;
        this.playersScrollView = playersScrollView;
        this.scrollViewContent = scrollViewContent;
        this.fairPlayImage = fairPlayImage;
        fairPlayImage.color = new Color(1, 1, 1, 0);
        loadingSubtitleText.color = new Color(1, 1, 1, 0.7f);

        LeanTween.cancel(loadingSubtitleText.gameObject);
        LeanTween.cancel(fairPlayImage.gameObject);
        AnimateLoadinMessage();
        if (startingBattleText.gameObject.activeSelf)
        {
            startingBattleText.gameObject.SetActive(false);
        }
        forceDisconnect = false;
        //loaderImage = LoaderImage;
        //errorIcon = ErrorIcon;
        
        

        totalPlayers = mPLController.gameConfig.TotalPlayers != 0 ? mPLController.gameConfig.TotalPlayers : 2;
        
        isItFirstEntry = true;
        Debug.Log("MPL: Setting up matchmaking screen");
        
        goToLobby = GoToLobby;
  
        goToLobby.GetComponent<Button>().onClick.RemoveAllListeners();

        if(MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            goToLobby.SetActive(true);
            goToLobby.GetComponent<Button>().onClick.AddListener(() => CancelChallenge(true));

            Session.Instance.SetQuitReason(Session.MPLQuitReason.knockout_quit);
        }
        
       
        timerText = TimerText;
        //timerText.text = "Finding Players";
        
        playerListParent = PlayerListParent;
        
        if (isAdEnabled)
        {
           if(!isPortrait)
                prizeObject = prizeObjectLandscapeWithAd;
           else
                prizeObject = PrizeObject;
         
           
            prizeObjectParent = prizeObjectParentWithAd;
            prizeObjectParent.gameObject.SetActive(true);
            //dummyAdImage.SetActive(true);
        }
        else
        {

            prizeObject = PrizeObject;
        
        prizeObjectParent = PrizeObjectParent;
            prizeObjectParent.gameObject.SetActive(true);
            //dummyAdImage.SetActive(false);
        }
        

        this.timerParent = timerParent;
        this.battleMatchedObject = battleMatchedObject;
        this.timerParent.SetActive(false);
        this.battleMatchedObject.SetActive(false);
        this.timerText.gameObject.SetActive(false);

        if (this.loadingSubtitleText != null && mPLController.gameConfig.LoadingMessages1v1 != null && mPLController.gameConfig.LoadingMessages1v1.Length > 0)
        {
            System.Random random = new System.Random();

            int index = random.Next(0, mPLController.gameConfig.LoadingMessages1v1.Length - 1);
            this.loadingSubtitleText.text = mPLController.gameConfig.LoadingMessages1v1[index];
            if (mPLController.gameConfig.IsKnockoutLobby)
            {
                loadingSubtitleText.gameObject.SetActive(false);
            }
                 
        }


        Debug.Log("loadingSubtitleText");
        userIdToCandidate.Clear();

        userIdToPlayerIndex.Clear();
        currentActiveChildIndex =1;

        if(!playersSpawned.Contains(this.localPlayer))
        playersSpawned.Add(this.localPlayer);
        SetPlayerProperties(this.localPlayer, mPLController.gameConfig.Profile.avatar, mPLController.gameConfig.Profile.tier, mPLController.gameConfig.Profile.displayName, mPLController.gameConfig.Profile.id);
        int childs = GetPlayersSpawnedCount();

        var v_rowCount = GetRowCount(totalPlayers);
        var v_coloumnCount = GetColoumnCount(totalPlayers, v_rowCount);
       
        //rowParents.Add(playerListParent.gameObject);
      
        if (childs<totalPlayers)
        {
            int childObjectsRequired = totalPlayers - childs;
            int childObjectsSpawned = 0;
            for(int i=0;i<v_rowCount && childObjectsSpawned < childObjectsRequired; i++)//row
            {
               
                for (int j=rowParents[i].transform.childCount;j<v_coloumnCount && childObjectsSpawned<childObjectsRequired; j++,childObjectsSpawned++)//coloumn
                {
                    
                    GameObject _opponentListItem = Instantiate(playerListItemPrefab);
                    _opponentListItem.transform.SetParent(rowParents[i].transform);
                    _opponentListItem.transform.localScale = Vector3.one;
                    playersSpawned.Add(_opponentListItem);

                }
            }
            //for (int i = 0; i < childObjectsRequired; i++)
            //{
            //    GameObject _opponentListItem = Instantiate(playerListItemPrefab);
            //    _opponentListItem.transform.SetParent(playerListParent);
            //    _opponentListItem.transform.localScale = Vector3.one;
            //    //_opponentListItem.SetActive(false);
            //}
        }
     
        SetPlayerProfileSize(totalPlayers);
        for (int j = 0; j < rowParents.Count; j++)
        {
            childs = rowParents[j].transform.childCount;
            for (int i = 0; i < childs; i++)
            {
                Debug.Log("Getting Child Match=" + i);
                if (i == 0 && j == 0)
                {
                    //first item will be players and no need to reset
                    continue;
                }
                else
                {
                    rowParents[j].transform.GetChild(i).gameObject.GetComponent<PlayerItemController>().ResetPlayerProfile();
                }
                //playerListParent.GetChild(i).gameObject.SetActive(false);
                //if(totalPlayers>=5)
                //playerListParent.GetChild(i).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            rowParents[j].GetComponent<HorizontalLayoutGroup>().padding.right = 0;
        }

        for (int i = 0;i<v_rowCount;i++)
            rowParents[i].gameObject.SetActive(true);

        //scrollViewContent.GetComponent<RectTransform>().sizeDelta =
        if ( totalPlayers >= 5)
        {
            //playersScrollView.horizontal = true;
            StartCoroutine(AdjustScrollViewContentSize(v_rowCount, v_coloumnCount, totalPlayers));
        }
        else
        {
            playersScrollView.horizontal = false;
        }

        viewPort.SetActive(true);
        childs = rewardObjects.Count;

        
        if (childs < totalPlayers)
        {

           

            childs = rewardObjects.Count;

            for (int i = 0; i < totalPlayers - childs; i++)
                {
                    GameObject _prizeItem = Instantiate(prizeObject);
                
                _prizeItem.transform.SetParent(prizeObjectParent);
                    _prizeItem.transform.localScale = Vector3.one;
                    rewardObjects.Add(_prizeItem);

                }
            
        }
        Debug.Log("childs rewardObjects2=" + rewardObjects.Count);
        for (int i=0;i< rewardObjects.Count;i++)
        {
            rewardObjects[i].SetActive(false);
        }
        
       

        
        prizeObjectParent.gameObject.SetActive(false);
        Debug.Log("prizeObjectParent");

        isMatchFound = false;
        
        Time.timeScale = 1.0f;
        playerJoinedCount = 0;
        if (startingBattleText.gameObject.activeSelf)
        {
            startingBattleText.gameObject.SetActive(false);
        }
        startingBattleText.gameObject.SetActive(true);

        startingBattleText.text = MPLController.Instance.gameConfig.IsKnockoutLobby ? mPLController.localisationDetails.GetLocalizedText("Waiting for your opponents to join") : mPLController.localisationDetails.GetLocalizedText("Finding Players");
        Debug.Log("startingBattleText");
        
       
            Debug.Log("Is Challenge Not Enable");
            //goToLobby.GetComponent<Button>().onClick.AddListener(() => QuitUnity());

            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = mPLController.localisationDetails.GetLocalizedText("Leave Battle?");
            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = mPLController.localisationDetails.GetLocalizedText("Are you sure you want to leave now?");
            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = mPLController.localisationDetails.GetLocalizedText("Leave Battle");
            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = mPLController.localisationDetails.GetLocalizedText("Wait for players");
            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitUnity());
            challengeWaitingPopup.gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => CancelChallenge(false));
            Debug.Log("challengeWaitingPopup not null");
            //goToLobby.SetActive(true);
            goToLobby.GetComponent<Button>().onClick.AddListener(() => CancelChallenge(true));
        
        

        SubmitBattleMatchingStartedEvent(mPLController.gameConfig.EntryPoint, false);
        maximumMatchMakingRetries--;
        SmartFoxManager.Instance.Connect();
    }
    string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        req.Timeout = 2000;
        req.ReadWriteTimeout = 2000;
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {

                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }
    public void CancelChallenge(bool show)
    {
        challengeWaitingPopup.SetActive(show);
    }
    bool isConnectionFlagsSet = false;
    public void SetConnectionFlags(string extraReason)
    {
        if(isConnectionFlagsSet)
        {
            return;
        }
        isConnectionFlagsSet = true;
        Debug.Log("SetConnectionFlags AA");
        if (!MPLController.Instance.gameConfig.IsPingCheckEnabled)
        {
            Debug.Log("Ping check disabled");
            Session.Instance.isConnectedToSmartFox = Session.Instance.isConnectedToInternet = Session.Instance.isConnectionModeSwitched = "DISABLED";
            return;
        }
        
       
        Session.Instance.isConnectedToInternet = MultiplayerGamesHandler.Instance.checkIfConnectedToInternet() ? "YES" : "NO";
        Session.Instance.extraDisconnectionReason = extraReason;

        string currConnectionMode = Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ? "Data" : "WIFI";
        Session.Instance.isConnectionModeSwitched = (currConnectionMode != Session.Instance.modeOfConnection) ? "YES" : "NO";
        Debug.Log("SetConnectionFlags BB");
    }
    public bool checkIfConnectedToInternet()
    {

        string HtmlText = GetHtmlFromUri("https://ping.mpl.live");
        if (HtmlText == "")
        {
            return false;
        }

        else
        {
            return true;
        }
    }
    public void QuitUnity()
    {
        if(MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            Session.Instance.SetQuitReason(Session.MPLQuitReason.knockout_quit);
        }
        Session.Instance.Quit();
    }
    void OnMatchFound(List<UserProfile> userList)
    {
        Session.Instance.playersInTheRoom = "";

        foreach(UserProfile user in userList)
        {
            Session.Instance.playersInTheRoom += (user.id + ",");
        }

        SubmitBattleMatchingEnded("", true);
        if (cr != null)
        {
            StopCoroutine(cr);
        }
        goToLobby.SetActive(false);
        
       
            //startingBattleText.gameObject.transform.parent.gameObject.SetActive(false);
            startingBattleText.gameObject.SetActive(true);
            //timerText.text = "Starting Battle";
            timerText.gameObject.SetActive(false);
            battleMatchedObject.SetActive(true);
            loaderBG.color = new Color32(219, 1, 254, 255);
            startingBattleText.text = MPLController.Instance.gameConfig.IsKnockoutLobby ? (mPLController.localisationDetails.GetLocalizedText("Starting ") + roundName) : mPLController.localisationDetails.GetLocalizedText("Starting Battle");
            if (MPLController.Instance.gameConfig.IsKnockoutLobby)
            {
                this.knockoutDisclaimerText.SetActive(false);
            }
            this.loadingSubtitleText.gameObject.SetActive(false);
            LeanTween.cancel(loadingSubtitleText.gameObject);
            LeanTween.cancel(fairPlayImage.gameObject);




        
       

        
        ResetMatchMakingRetriesValue(false);
        isThisFindAgain = false;
        userListOnMatch = userList;
        TotalTime = 0;
        Debug.Log("userList=" + userList.Count);
       
        EnableBlurBG(false);
        MPLController.Instance.LoadGameSceneFromAssetBundle(MPLController.Instance.gameConfig.GameId);
        isMatchFound = true;
        SetGameState(MPLGameState.in_game);
    }
    void MatchUserIn(UserProfile profile, List<UserProfile> userList, int timeLeft)
    {
        StartCoroutine(EnqueueMatchInEvent(profile, userList, timeLeft));
    }
    string roundName;
    
    void OnKnockoutMatchUserIn(List<UserProfile> userList, int timeLeft, string roundName,int minPlayers, List<UserProfile> joinedUsersList,UserProfile joinedUser)
    {
        this.roundName = roundName;
        knockoutMinPlayers = minPlayers;
        if (userList.Count > 2)
        {
            loadingSubtitleText.text = "Min " + knockoutMinPlayers + " players needed to start match";
        }
        else
        {
            loadingSubtitleText.text = "You'll proceed to next round if your opponent doesn't join";
        }
        loadingSubtitleText.gameObject.SetActive(true);
        if (!isMatchFound)
                {
                    //timerText.text = "Finding Players" + "(" + minutes + ":" + seconds + ")";
                    timerText.text = "";

                }
                timerParent.SetActive(true);
                if (!battleMatchedObject.gameObject.activeSelf)
                    timerText.gameObject.SetActive(true);


                if (cr != null)
                {
                    StopCoroutine(cr);
                }
                cr = LoseTime(timeLeft);
                StartCoroutine(cr);
                
                playerJoinedCount = userList.Count;

        if (isItFirstKnockoutEntry)
        {
            isItFirstKnockoutEntry = false;

            if (playerJoinedCount > 1)
            {
                for (int i = 0; i < playerJoinedCount; i++)
                {
                    if (userList[i].id != MPLController.Instance.gameConfig.Profile.id)
                    {
                        AddOpponnetPlayer(userList[i]);
                    }
                }

                for(int j=0;j<joinedUsersList.Count;j++)
                {
                    UpdateKnockoutJoinedUser(joinedUsersList[j].id,true,false);
                }


            }


        }
        else
        {
            UpdateKnockoutJoinedUser(joinedUser.id,true,false);
        }
                
            
            
        
    }

    public void UpdateKnockoutJoinedUser(int userId,bool enable,bool left)
    {
        if(!knockOutPlayers.ContainsKey(userId))
        {
            return;
        }
        GameObject player = knockOutPlayers[userId];
        PlayerItemController playerItemController = player.gameObject.GetComponent<PlayerItemController>();
        if(enable)
        {
            if (playerItemController.loader != null && playerItemController.playerDp != null)
            {
                playerItemController.loader.SetActive(false);
                playerItemController.playerDp.color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            if (playerItemController.loader != null && playerItemController.playerDp != null)
            {
                playerItemController.loader.SetActive(left);

                playerItemController.playerDp.color = new Color32(255, 255, 255, 102);
            }
        }
    }
    public IEnumerator EnqueueMatchInEvent(UserProfile profile, List<UserProfile> userList, int timeLeft)
    {
        Debug.Log("Enqueued Match User In");
        yield return new WaitWhile(() => !userEntered);
        Debug.Log("Calling Match User In");
        OnMatchUserIn(profile, userList, timeLeft);
    }

    void OnMatchUserIn(UserProfile profile, List<UserProfile> userList, int timeLeft)
    {
        Debug.Log("On Match User In");
        userEntered = false;
       
        if(MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            userEntered = true;
            return;
        }
        playerJoinedCount = userList.Count;
        prizeObjectParent.gameObject.SetActive(true);
        Debug.Log("is It first entry=" + isItFirstEntry);
        if (isItFirstEntry)
        {
            SetGameState(MPLGameState.match_making);
            string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
            string seconds = Mathf.Floor(timeLeft % 60).ToString("00");
            if (!isMatchFound)
            {
                //timerText.text = "Finding Players" + "(" + minutes + ":" + seconds + ")";
                timerText.text = "";
                
            }
                timerParent.SetActive(true);
            if (!battleMatchedObject.gameObject.activeSelf)
                timerText.gameObject.SetActive(true);
           
               
            if (cr != null)
            {
                StopCoroutine(cr);
            }
            cr = LoseTime(timeLeft);
            StartCoroutine(cr);
            Debug.Log("MPL First entry");
            playerJoinedCount = userList.Count;
            if (playerJoinedCount > 1)
            {
                for (int i = 0; i < playerJoinedCount; i++)
                {
                    if (userList[i].id != MPLController.Instance.gameConfig.Profile.id)
                    {
                        AddOpponnetPlayer(userList[i]);
                    }
                }


            }
            isItFirstEntry = false;
        }
        else
        {
            if (profile.id != MPLController.Instance.gameConfig.Profile.id)
            {
                AddOpponnetPlayer(profile);
            }
        }
        SpawnPrizeObject(playerJoinedCount);
       
    }
    public void SetPlayerProperties(GameObject player,string avatar, string tier, string displayName, int userID)
    {

        Debug.Log("tier=" + tier + "displayName=" + displayName);
        Debug.Log("Setting Player Properties");

        if (player.activeSelf && player != null)
        {
            Image playerDp, playerTier;
            Text playerName;

            PlayerItemController playerItemController = player.gameObject.GetComponent<PlayerItemController>();
            playerDp = playerItemController.playerDp;
            playerName = playerItemController.playerName;
            playerTier = playerItemController.tier;

            if (!MPLController.Instance.gameConfig.isTierEnabled)
            {
                playerTier.gameObject.SetActive(false);
            }
            else
            {
                playerTier.gameObject.SetActive(true);
            }
            Candidate candidate = new Candidate();
           

            Debug.Log("MPLController.Instance.gameConfig.isTierEnabled=" + MPLController.Instance.gameConfig.isTierEnabled);
            StartCoroutine(GetDpAsyncInGame(avatar, (sprite) =>
            {
                playerDp.sprite = sprite != null ? sprite : defaultimagecircle;
                candidate.displayPic = sprite;
            }));
            playerName.text = Truncate(displayName);
            playerName.gameObject.SetActive(true);
            candidate.name = playerName.text;

            if(MPLController.Instance.gameConfig.IsKnockoutLobby)
            {
                if (playerItemController.loader != null)
                {
                    playerItemController.loader.SetActive(true);
                    playerItemController.playerDp.color = new Color32(255, 255, 255, 102);
                    
                    knockOutPlayers.Add(userID, player);
                }
            }
            if (userIdToCandidate.ContainsKey(userID))
            {
                userIdToCandidate[userID] = candidate;
            }
            else
            {
                userIdToCandidate.Add(userID, candidate);
            }

        }
    }
    public string Truncate(string value)
    {
        return value.Length <= 12 ? value : value.Substring(0, 12) + "...";
    }

    void AddOpponnetPlayer(UserProfile profile)
    {
        Debug.Log("adding player");
        Debug.Log("currentActiveChildIndex=" + currentActiveChildIndex);
        Debug.Log("playerListParent.transform.childCount=" + playerListParent.transform.childCount);
        if (currentActiveChildIndex < playersSpawned.Count)
        {
            GameObject _opponentListItem = playersSpawned[currentActiveChildIndex]; //playerListParent.GetChild(currentActiveChildIndex).gameObject;
            _opponentListItem.SetActive(true);
            SetPlayerProperties(_opponentListItem, profile.avatar, profile.tier, profile.displayName, profile.id);


            if (userIdToPlayerIndex.ContainsKey(profile.id))
            {
                userIdToPlayerIndex[profile.id] = currentActiveChildIndex;
            }
            else
            {
                userIdToPlayerIndex.Add(profile.id, currentActiveChildIndex);
            }
            currentActiveChildIndex++;
        }
    }

    public void SpawnPrizeObject(int count)
    {
        if (rewardObjects == null || prizeObjectParent == null)
        {
            return;
        }
        Debug.Log("Spawn Prize Object");

        for (int i = 0; i < rewardObjects.Count; i++)
        {
            if (rewardObjects[i].activeSelf)
            {
                rewardObjects[i].SetActive(false);
            }
        }
        prizeObjectParent.gameObject.SetActive(true);
        double prizeAmount = 0;
        if (count == 1)
        {
            if (MPLController.Instance != null && MPLController.Instance.gameConfig != null)
            {
                prizeAmount = MPLController.Instance.gameConfig.TotalPlayers > 2 && MPLController.Instance.gameConfig.DynamicRewards != null && MPLController.Instance.gameConfig.DynamicRewards.Length > 0 ? MPLController.Instance.gameConfig.DynamicRewards[0].rankWiseWinning[0] : MPLController.Instance.gameConfig.WinningAmount;
                if (0 < rewardObjects.Count)
                {
                    SetPrizeProperties(rewardObjects[0], 1, prizeAmount);
                    rewardObjects[0].SetActive(true);
                }
            }

        }
        else
        {
            if (MPLController.Instance != null && MPLController.Instance.gameConfig != null)
            {
                if (MPLController.Instance.gameConfig.TotalPlayers > 2 && MPLController.Instance.gameConfig.DynamicRewards != null && MPLController.Instance.gameConfig.DynamicRewards.Length > 0)
                {
                    for (int i = 0; i < MPLController.Instance.gameConfig.DynamicRewards.Length; i++)
                    {

                        if (MPLController.Instance.gameConfig.DynamicRewards[i].playerCount == count)
                        {

                            for (int j = 0; j < MPLController.Instance.gameConfig.DynamicRewards[i].rankWiseWinning.Length; j++)
                            {
                                if (j < rewardObjects.Count)
                                {
                                    SetPrizeProperties(rewardObjects[j], j + 1, MPLController.Instance.gameConfig.DynamicRewards[i].rankWiseWinning[j]);
                                    rewardObjects[j].SetActive(true);
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (0 < rewardObjects.Count)
                    {
                        SetPrizeProperties(rewardObjects[0], 1, mPLController.gameConfig.WinningAmount);
                        rewardObjects[0].SetActive(true);
                    }
                }
            }
        }
        userEntered = true;
    }

    public void SetPrizeProperties(GameObject prizeItem, int rank, double prizeAmount)
    {


        PrizeObjectController prizeObjectController = prizeItem.gameObject.GetComponent<PrizeObjectController>();
       
        if (prizeObjectController == null)
        {
            return;
        }
        Text prizeWinText = prizeObjectController.countText;
        Text prizeWinAmount = prizeObjectController.winText;

        prizeObjectController.EnableCrownimage(rank);
        if (prizeWinAmount != null)
        {
            prizeWinAmount.text = prizeAmount.ToString();

            prizeWinAmount.gameObject.SetActive(false);
        }
        if (rank > 3)
        {
            prizeWinText.text = rank + "th";
        }
        else
        {
            switch (rank)
            {
                case 1:
                    {
                        prizeWinText.text = rank + "st";
                        break;
                    }
                case 2:
                    {
                        prizeWinText.text = rank + "nd";
                        break;
                    }
                case 3:
                    {
                        prizeWinText.text = rank + "rd";
                        break;
                    }
            }
        }

        if (prizeWinAmount != null)
        {
            prizeWinAmount.gameObject.SetActive(true);
        }

    }
    IEnumerator LoseTime(int challengeTimeOut)
    {
       
       
        while (challengeTimeOut >= 0)
        {
            MultiplayerGamesHandler.Instance.TotalTime++;
            yield return new WaitForSeconds(1);
          
            string minutes = Mathf.Floor(challengeTimeOut / 60).ToString("00");
            string seconds = Mathf.Floor(challengeTimeOut % 60).ToString("00");
            if (challengeTimeOut > 99)
            {
                timerText.GetComponent<Text>().fontSize = 26;
            }
            else
            {
                timerText.GetComponent<Text>().fontSize = 30;
            }
            if (!isMatchFound)
            {
               
                    //timerText.GetComponent<Text>().text = "Finding Players" + "(" + minutes + ":" + seconds + ")";
                    
                    timerText.GetComponent<Text>().text = challengeTimeOut.ToString();
                
               
            }
            challengeTimeOut--;
        }
        

        if (playerJoinedCount > 1)
        {
            timerText.gameObject.SetActive(false);
            loaderBG.color = new Color32(219, 1, 254, 255);
            if (!MPLController.Instance.gameConfig.IsKnockoutLobby)
            {
                battleMatchedObject.SetActive(true);
                startingBattleText.text = mPLController.localisationDetails.GetLocalizedText("Starting Battle");
            }
        }
        



    }
    void OnMatchGroupChange(List<UserProfile> userList, int timeLeft)
    {
        
        playerJoinedCount = 0;
        string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        string seconds = Mathf.Floor(timeLeft % 60).ToString("00");
        if (!isMatchFound)
        {
            //timerText.text = "Finding Players" + "(" + minutes + ":" + seconds + ")";
            timerText.text = "";
        }
        if (!battleMatchedObject.gameObject.activeSelf)
            timerText.gameObject.SetActive(true);

        if (cr != null)
            StopCoroutine(cr);

        playerJoinedCount = userList.Count;

        cr = LoseTime(timeLeft);
        StartCoroutine(cr);

        Dictionary<int, int> newPlayers = new Dictionary<int,int>();
        newPlayers = userIdToPlayerIndex;

        foreach (KeyValuePair<int,int> entry in newPlayers)
        {
            if (entry.Key != MPLController.Instance.gameConfig.Profile.id)
            {
                GameObject player = playersSpawned[userIdToPlayerIndex[entry.Key]];// playerListParent.GetChild(userIdToPlayerIndex[entry.Key]).gameObject;
                player.GetComponent<PlayerItemController>().ResetPlayerProfile();
                //player.SetActive(false);
            }
        }
        currentActiveChildIndex = 1;

        for (int i = 0; i < userList.Count; i++)
        {
            if (userList[i].id != MPLController.Instance.gameConfig.Profile.id)
            {
                AddOpponnetPlayer(userList[i]);
            }
        }

        SpawnPrizeObject(userList.Count);



    }
    void OnMatchUserOut(UserProfile profile, List<UserProfile> userList, int timeLeft)
    {
      
        if (MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            UpdateKnockoutJoinedUser(profile.id, false, true);

        }
        else
        {
            if (userIdToPlayerIndex.ContainsKey(profile.id))
            {

                GameObject player = playersSpawned[userIdToPlayerIndex[profile.id]]; //playerListParent.GetChild(userIdToPlayerIndex[profile.id]).gameObject;
                userIdToPlayerIndex.Remove(profile.id);
                player.GetComponent<PlayerItemController>().ResetPlayerProfile();
                //player.SetActive(false);
                currentActiveChildIndex--;
                playerJoinedCount = userList.Count;
                SpawnPrizeObject(playerJoinedCount);


            }
        }

    }
    public void OverrideGameConfig()
    {
        if (isItUpsell)
        {

            if (!string.IsNullOrEmpty(nextLobbyConfig))
            {
                
                JsonUtility.FromJsonOverwrite(nextLobbyConfig, MPLController.Instance.gameConfig);
                
                MPLController.Instance.SetSessionConfig(nextLobbyConfig);
            }
        }
    }
    public static MultiplayerGamesHandler Instance
    {
        get { return instance; }
    }
    public void SetBackgroundImage(GameObject backgroundImage)
    {
        
        backgroundImage.GetComponent<Image>().sprite = MPLController.Instance.backgroundImage;

             

    }

    public IEnumerator GetDpAsyncInGame(string avatar, UnityAction<Sprite> callback)
    {
        Sprite displayPicture = defaultimagecircle;

        


        if (string.IsNullOrEmpty(avatar))
        {
            Debug.Log("***!!!***");

            Debug.Log("Returning default dp");
            callback(displayPicture);
        }
        else
        {
            Debug.Log("***&&&***");


            avatar += ".square3x";


            using (WWW www = new WWW(avatar))
            {
                // Wait for download to complete
                yield return www;
                if (www.texture == null)
                {

                }
                else
                {
                    displayPicture = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                    Debug.Log("Returning dpwnloaded dp = " + avatar + " = " + www.texture + " = " + displayPicture);
                    callback(displayPicture);
                }
            }
        }
    }

    public void ResetMatchMakingRetriesValue(bool resetStatic)
    {

        maximumMatchMakingRetries= MPLController.Instance.gameConfig.MaxMatchMakingRetries == 0 ? 3 : MPLController.Instance.gameConfig.MaxMatchMakingRetries;
        if(resetStatic)
        {
            maximumMatchMakingRetriesStatic= MPLController.Instance.gameConfig.MaxMatchMakingRetries == 0 ? 3 : MPLController.Instance.gameConfig.MaxMatchMakingRetries;
        }
        
    }

    BattleOpponentEventProperties GetOpponentEventProperties()
    {
        
        if (readyUsers == null) return null;
        List<String> opponentsId = new List<String>();
        List<String> opponentsName = new List<String>();
        List<String> opponentsNumber = new List<string>();
        List<String> opponentsTier = new List<string>();
        for (int i = 0; i < readyUsers.Count; i++)
        {
            if (readyUsers[i].id != MPLController.Instance.gameConfig.Profile.id)
            {
                opponentsId.Add(readyUsers[i].id.ToString());
                opponentsName.Add(readyUsers[i].displayName);
                opponentsNumber.Add(readyUsers[i].mobileNumber);
                opponentsTier.Add(readyUsers[i].tier);
            }
        }
        return new BattleOpponentEventProperties(MPLController.Instance.battleSessionEventProperties, MPLController.Instance.battleRoomEventProperties, string.Join(",", opponentsId.ToArray()), string.Join(",", opponentsName.ToArray()), string.Join(",", opponentsNumber.ToArray()), string.Join(",", opponentsTier.ToArray()));
    }

    private void AnimateLoadinMessage()
    {
     
     
        Color startingColorLoadingMessage = loadingSubtitleText.color;
        Color endingColorLoadingMessage = new Color(0, startingColorLoadingMessage.g, startingColorLoadingMessage.b, 0);

        Color startingColorFairPlayImage = fairPlayImage.color;
        Color endingColorFairPlayImage = new Color(startingColorFairPlayImage.r, startingColorFairPlayImage.g, startingColorFairPlayImage.b, 1);

        //Debug.Log("startingColorLoadingMessage : " + startingColorLoadingMessage + " endingColorLoadingMessage : " + endingColorLoadingMessage + " startingColorFairPlayImage : " + startingColorFairPlayImage + " endingColorFairPlayImage : " + endingColorFairPlayImage);

      
        LeanTween.textColor(loadingSubtitleText.rectTransform, endingColorLoadingMessage, 0.5f).setDelay(5).setOnComplete(() =>
        {
         
            //loadingSubtitleText.color = endingColorLoadingMessage;
            LeanTween.color(fairPlayImage.rectTransform, endingColorFairPlayImage, 0.5f).setOnComplete(() =>
                 {

                    
                     //fairPlayImage.color = endingColorFairPlayImage;
                     LeanTween.color(fairPlayImage.rectTransform, startingColorFairPlayImage, 0.5f).setDelay(5).setOnComplete(() =>
                     {
                        
                         //fairPlayImage.color = startingColorFairPlayImage;
                         LeanTween.textColor(loadingSubtitleText.rectTransform, startingColorLoadingMessage, 0.5f).setOnComplete(() =>
                         {
                            
                             //loadingSubtitleText.color = startingColorLoadingMessage;
                             AnimateLoadinMessage();
                         });
                     });
                 });

        });
    }


    bool battleStartedSent = false;
    public void SubmitBattleStartedEvent(bool isSuccess, string failReason)
    {
        if(battleStartedSent)
        {
            return;
        }
        battleEndedSent = false;
        battleStartedSent = true;
        BattleOpponentEventProperties battleOpponentEventProperties = GetOpponentEventProperties();
        if (battleOpponentEventProperties != null)
        {
            if (isSuccess)
            {
                StartCoroutine(SubmitBattleStartedOnLoadingTimeCalc(isSuccess, failReason));
            }
            else
            {
                BattleStartedEventProperties battleStartedEventProperties = new BattleStartedEventProperties(GetOpponentEventProperties(), MPLController.Instance.battleRoomEventProperties, MPLController.Instance.battleSessionEventProperties, isSuccess, failReason, true, true, true, 0, 0, MPLController.Instance.gameConfig.IsAutoStartEnabled);
                MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_STARTED, battleStartedEventProperties.ToString());
            }
        }
    }

    IEnumerator SubmitBattleStartedOnLoadingTimeCalc(bool isSuccess, string failReason)
    {
        yield return new WaitWhile(() => !MPLController.Instance.gameLoadingTimeCaluclated);

        BattleStartedEventProperties battleStartedEventProperties = new BattleStartedEventProperties(GetOpponentEventProperties(), MPLController.Instance.battleRoomEventProperties, MPLController.Instance.battleSessionEventProperties, isSuccess, failReason, true, true, true, MPLController.Instance.gameLoadingTime, MPLController.Instance.gameLoadingMinimizes, MPLController.Instance.gameConfig.IsAutoStartEnabled);
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_STARTED, battleStartedEventProperties.ToString());
    }

    public void SubmitBattleEndedEvent(int myScore, string sessionResult, int opponentScore, string resultType, string gameEndReason, double totalCashBalance, double totalTokenBalance, bool isEnoughToRematch, int duration)
    {
        if (battleEndedSent) return;

        Debug.Log("SubmitBattleEndedEvent 1vn :: battleEndedSent " + battleEndedSent);

        BattleOpponentEventProperties battleOpponentEventProperties = GetOpponentEventProperties();
        if (battleOpponentEventProperties != null)
        {
            BattleEndedEventProperties battleEndedEventProperties = new BattleEndedEventProperties(GetOpponentEventProperties(), MPLController.Instance.battleRoomEventProperties, MPLController.Instance.battleSessionEventProperties, myScore, sessionResult, opponentScore, resultType, gameEndReason, totalCashBalance, totalTokenBalance, isEnoughToRematch, duration);
            MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_ENDED, battleEndedEventProperties.ToString());
            battleEndedSent = true;
        }
    }


    public void SubmitBattleRematchRequestedEvent(bool isWinner, bool isInitiator)
    {
        BattleOpponentEventProperties battleOpponentEventProperties = GetOpponentEventProperties();
        if (battleOpponentEventProperties != null)
        {
            BattleRematchRequestedEventProperties battleRematchRequestedEventProperties = new BattleRematchRequestedEventProperties(GetOpponentEventProperties(), MPLController.Instance.battleRoomEventProperties, MPLController.Instance.battleSessionEventProperties, isWinner, isInitiator);
            MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_REMATCH_REQUESTED, battleRematchRequestedEventProperties.ToString());
        }
    }

    public void SubmitBattleRematchRespondedEvent(bool isWinner, bool isAccepted, bool isResponder)
    {
        BattleOpponentEventProperties battleOpponentEventProperties = GetOpponentEventProperties();
        if (battleOpponentEventProperties != null)
        {
            BattleRematchRespondedEventProperties battleRematchRespondedEventProperties = new BattleRematchRespondedEventProperties(GetOpponentEventProperties(), MPLController.Instance.battleRoomEventProperties, MPLController.Instance.battleSessionEventProperties, isWinner, isAccepted, isResponder);
            MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_REMATCH_RESPONDED, battleRematchRespondedEventProperties.ToString());
        }
    }
    bool connectionLostEventSent = false;
    public void SubmitBattlePopupShownEvent(string popupName, string title, string message)
    {
        if (popupName != "Connection Lost")
        {
            
            BattlePopupShownEventProperties battlePopupShownEventProperties = new BattlePopupShownEventProperties(MPLController.Instance.battleRoomEventProperties, popupName, title, message);
            MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_POPUP_SHOWN, battlePopupShownEventProperties.ToString());
            
        }
        else
        {
            if (connectionLostEventSent)
            {
                return;
            }

            connectionLostEventSent = true;
            string gameType = MPLController.Instance.IsThirdPartyGame() ? "Third" : "First";
            BattleConnectionLostEventProperties battleConnectionLostEventProperties = new BattleConnectionLostEventProperties(MPLController.Instance.battleRoomEventProperties, popupName, title, message, GetPingState(), Session.Instance.userMinimised, Session.Instance.modeOfConnection, Session.Instance.extraDisconnectionReason, Session.Instance.isConnectedToInternet, Session.Instance.isConnectedToSmartFox, Session.Instance.isConnectionModeSwitched, gameType, GetAvgPing(),SmartFoxManager.Instance.gReconnect, mPLGameState.ToString(),MPLController.Instance.gameConfig.CountryCode);
            MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_CONNECTION_LOST, battleConnectionLostEventProperties.ToString());
        }
    }

    public string GetPingState()
    {
        string state = "";
        if (MPLController.Instance.gameConfig.IsPingCheckEnabled)
        {
            string isConnectedToInternet = SmartFoxManager.Instance.IsConnectedToInternet();
            state = (isConnectedToInternet == "YES") ? "CONNECTED" : "DISCONNECTED";
         }
        else
        {
            state = "DISABLED";
        }

        return state;
    }

    public double GetAvgPing()
    {
        double averagePing = 0;
        if (Session.Instance.pingDurations != null)
        {
            double totalPing = 0;
            for (int i = 0; i<Session.Instance.pingDurations.Count; i++)
            {
                totalPing += Session.Instance.pingDurations[i];
            }
            averagePing = totalPing / Session.Instance.pingDurations.Count;
        }
        return averagePing;
    }

    public void SubmitBattleMatchingStartedEvent(string entryPoint, bool isRetry)

    {
        BattleMatchingStartedEventProperties battleMatchingStartedEventProperties = new BattleMatchingStartedEventProperties(MPLController.Instance.battleRoomEventProperties, "", MPLController.Instance.gameConfig.CreateGameTimeout.ToString(), MPLController.Instance.gameConfig.PingInterval.ToString(), MPLController.Instance.gameConfig.BattleAgainTimeout.ToString(), entryPoint, isRetry, (Math.Abs(maximumMatchMakingRetriesStatic - maximumMatchMakingRetries)) + 1);
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_MATCHING_STARTED, battleMatchingStartedEventProperties.ToString());
    }

    public void SubmitBattleMatchingEnded(string failReason, bool isSuccess)
    {
        BattleMatchingEndedEventProperties battleMatchingStartedEventProperties = new BattleMatchingEndedEventProperties(MPLController.Instance.battleRoomEventProperties, "", MPLController.Instance.gameConfig.CreateGameTimeout.ToString(), MPLController.Instance.gameConfig.PingInterval.ToString(), MPLController.Instance.gameConfig.BattleAgainTimeout.ToString(), failReason, isSuccess, Math.Abs(maximumMatchMakingRetriesStatic - maximumMatchMakingRetries), TotalTime);
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_MATCHING_ENDED, battleMatchingStartedEventProperties.ToString());

        TotalTime = 0;
    }

    public void SubmitBattleButtonClickedEvent(string buttonName)
    {

        BattleButtonClickedEventProperties battleButtonClickedEventProperties = new BattleButtonClickedEventProperties(MPLController.Instance.battleRoomEventProperties, buttonName);
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_BATTLE_BUTTON_CLICKED, battleButtonClickedEventProperties.ToString());
    }

    public void SubmitAppScreenViewedEvent(string screenName)
    {
        AppScreenViewedEventProperties appScreenViewedEventProperties = new AppScreenViewedEventProperties(MPLController.Instance.battleRoomEventProperties, screenName);
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_APP_SCREEN_VIEWED, appScreenViewedEventProperties.ToString());
    }

    /// <summary>
    /// To get the coloumn count of the player profiles view based on the number of players as per the design.
    /// </summary>
    /// <param name="noOfPlayers"></param>
    /// <param name="rowCount"></param>
    /// <returns></returns>
    private int GetColoumnCount(int noOfPlayers,int rowCount)
    {
       
        int _coloumnCount;
        var v_numberToBeAdded = 0;

        var v_reminder = noOfPlayers % rowCount;
        if(v_reminder>0)
         v_numberToBeAdded = rowCount - v_reminder;
        _coloumnCount = (noOfPlayers + v_numberToBeAdded) / rowCount;
        return _coloumnCount;
    }

    /// <summary>
    /// To get the row count of the player profiles view based on the number of players as per the design.
    /// </summary>
    /// <param name="noOfPlayers"></param>
    /// <returns></returns>
    private int GetRowCount(int noOfPlayers)
    {
        int _rowCount = 0;
        if(isPortrait)
        {
            if (noOfPlayers <= 2)
            {
                _rowCount = 1;
            }
            else if (noOfPlayers <= 6)
            {
                _rowCount = 2;
            }
            else
            {
                _rowCount = 3;
            }
        }
        else
        {

     if(noOfPlayers<=4)
        {
            _rowCount = 1;
        }
     else
        {
            _rowCount = 2;
        }
        }

        return _rowCount;
            
    }

    /// <summary>
    /// To set the player profile sizes accordingly based on the number of players as per the design
    /// </summary>
    /// <param name="noOfPlayers"></param>
    private void SetPlayerProfileSize(int noOfPlayers)
    {
       
            if(noOfPlayers>=5)
            {
                if (isPortrait)
                {
                    foreach (var v_row in rowParents)
                    {
                        v_row.transform.localScale = new Vector3(REDUCED_SCALE_PORTRAIT, REDUCED_SCALE_PORTRAIT, REDUCED_SCALE_PORTRAIT); //0.83
                    v_row.GetComponent<HorizontalLayoutGroup>().spacing = REDUCED_HORIZONTAL_SPACING_PORTRAIT;// 15;
                    }
                    viewPort.GetComponent<VerticalLayoutGroup>().spacing = REDUCED_VERTICAL_SPACING_PORTRAIT; // 0
                }
                else
                {
                    foreach (var v_row in rowParents)
                    {
                        v_row.transform.localScale = new Vector3(REDUCED_SCALE_LANDSCAPE, REDUCED_SCALE_LANDSCAPE, REDUCED_SCALE_LANDSCAPE); //0.7F
                    v_row.GetComponent<HorizontalLayoutGroup>().spacing = REDUCED_HORIZONTAL_SPACING_LANDSCAPE;// 32;
                    }
                viewPort.GetComponent<VerticalLayoutGroup>().spacing = REDUCED_VERTICAL_SPACING_LANDSCAPE;// -15;
                }
            }
            else
            {
                foreach (var v_row in rowParents)
                {
                    v_row.transform.localScale = Vector3.one;
                v_row.GetComponent<HorizontalLayoutGroup>().spacing = NORMAL_HORIZONTAL_SPACING; //32
                }
            viewPort.GetComponent<VerticalLayoutGroup>().spacing = NORMAL_VERTICAL_SPACING;// 24;

        }
            if(!isPortrait)
        {
            if(noOfPlayers>=5)
            {
                viewPort.GetComponent<VerticalLayoutGroup>().padding.top = 15;
            }
            else
            {
                viewPort.GetComponent<VerticalLayoutGroup>().padding.top = 0;
            }
        }
       
        }

    /// <summary>
    /// coroutine to adjust the player profile content when the number of players exceeds the maximum number screen can afford
    /// </summary>
    /// <returns></returns>
    IEnumerator  AdjustScrollViewContentSize(int rowCount, int coloumnCount, int noOfPlayers)
    {
        var v_horizontalLayout = rowParents[rowParents.Count - 1].GetComponent<HorizontalLayoutGroup>();
        v_horizontalLayout.enabled = false;
        yield return new WaitForEndOfFrame();

      
        //var v_screenWidth = Camera.main.scaledPixelWidth;
        var v_screenWidth = Screen.width/3; //3 is the reference scale in canvas scaler

        Debug.LogError("Sahmil - cam width - " + v_screenWidth);

        var v_width = rowParents[0].GetComponent<RectTransform>().rect.width;
      
        if (!isPortrait)
        {
            v_width = v_width * REDUCED_SCALE_LANDSCAPE;
            if ((v_width + PLAYER_PROFILES_HORIZONTAL_PADDING) <= 640)
            {
                playersScrollView.horizontal = false;
                v_horizontalLayout.enabled = true;
                yield break;

            }
        }
        else 
        {
            v_width = v_width * REDUCED_SCALE_PORTRAIT;
            if((v_width + PLAYER_PROFILES_HORIZONTAL_PADDING) <= 360)
            {
                playersScrollView.horizontal = false;
                v_horizontalLayout.enabled = true;
                yield break;

            }
        }

       
        if ((v_width+ PLAYER_PROFILES_HORIZONTAL_PADDING) < v_screenWidth)
        {
            playersScrollView.horizontal = false;
            v_horizontalLayout.enabled = true;
            yield return null;
        }
        else
        {


            var v_scrollViewHeight = scrollViewContent.GetComponent<RectTransform>().rect.height;
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v_width, v_scrollViewHeight);
            var v_height = viewPort.GetComponent<RectTransform>().rect.height;
            viewPort.GetComponent<RectTransform>().sizeDelta = new Vector2(v_width, v_height);
            playersScrollView.horizontal = true;
            AlignTheLastRow(rowCount, coloumnCount);

        }

        v_horizontalLayout.enabled = true;

    }

     IEnumerator ResetScrollViewContentSize(Action OnResetDone)
    {
        yield return new WaitForEndOfFrame();
        foreach (var v_row in rowParents)
        {
            v_row.transform.localScale = Vector3.one;
            v_row.GetComponent<HorizontalLayoutGroup>().spacing = NORMAL_HORIZONTAL_SPACING; //32
            v_row.GetComponent<HorizontalLayoutGroup>().padding.right = 0; //32
            v_row.SetActive(false);
        }
        if (rowParents.Count > 0) 
        rowParents[0].SetActive(true); 
        if (isPortrait)
        {
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(312, 365);
            viewPort.GetComponent<RectTransform>().sizeDelta = new Vector2(312, 118);
          
        }
        else
        {
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(592, 206);
            viewPort.GetComponent<RectTransform>().sizeDelta = new Vector2(592, 118);
        }
       
        viewPort.GetComponent<VerticalLayoutGroup>().padding.top = 0;
        viewPort.GetComponent<VerticalLayoutGroup>().spacing = NORMAL_VERTICAL_SPACING;// 24;
                                                                                       //var v_scrollViewHeight = scrollViewContent.GetComponent<RectTransform>().rect.height;

        //var v_height = viewPort.GetComponent<RectTransform>().rect.height;

        OnResetDone.Invoke();

    }
    private int GetPlayersSpawnedCount()
    {
        var v_childCount = 0;
        foreach(var v_row in rowParents)
        {
            v_childCount += v_row.transform.childCount;
        }
        return v_childCount;
    }

    private void AlignTheLastRow(int rowCount, int coloumnCount)
    {
       
            var v_lastActiveRow = rowParents[rowCount - 1];
        var v_lastActiveRowChildsCount = v_lastActiveRow.transform.childCount;
        var v_playerWidth = playerListItemPrefab.gameObject.GetComponent<RectTransform>().rect.width;
        var v_horizontalLayout = v_lastActiveRow.GetComponent<HorizontalLayoutGroup>();
        if (v_lastActiveRowChildsCount < coloumnCount)
        {

            int paddingToBeAdded = (int)((coloumnCount - v_lastActiveRowChildsCount) * (v_playerWidth + v_horizontalLayout.spacing));
            v_horizontalLayout.padding.right = paddingToBeAdded;
        }
        else
        {
            v_horizontalLayout.padding.right = 0;
        }
    }

    public void EnableBlurBG(bool toggle)
    {
        blurBG.SetActive(toggle);
    }

    [ContextMenu("destroyPlayerProfiles")]
    public void DestroyPlayerProfiles(Action OnResetDone)
    {
        var v_count = playersSpawned.Count;
        Debug.LogError("sahmil--" + v_count);
        for (int i = 1;i< v_count; i++)
        {
          
            Destroy(playersSpawned[i]);
            //playersSpawned.RemoveAt(i);
        }
        playersSpawned.Clear();
        //ResetScrollViewContentSize();
        StartCoroutine(ResetScrollViewContentSize(() => { OnResetDone.Invoke(); }));
    }

   
}

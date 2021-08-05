using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;
using System;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class BattleEndController1vN : MonoBehaviour
{
    
    bool amITheWinner;
    
    public GameObject[] fraudItems;
    public Text rankLabel;
    bool gameOverShown;
    bool disableAutoStart = false;
    public GameObject interactiveButton, interactiveToolTip, interactivePopup;
    public Text interactivePopupTitle;
    public Dictionary<int, GameObject> TempPlayerProfiles;
    public GameObject gameQuitPopup;
    private ButtonController findNewController, battleAgainController, upsellController,autoStartController;
    private bool canIPlayAgain=true;
    public GameObject startBattle;
    public GameObject fraudPopUp;
    private MultiplayerGamesHandler multiplayerGamesHandler;
    public GameObject autoStart;
    private GameObject cantPlayMessage, playAgainMessage1, playAgainMessage2;
    private GameObject localPlayerObject, opponentPlayerObject;
    private bool battleAgainRequested, battleAgainRecieved,isBattleAgainEnabled,isIt1v1;
    public Button addMoneyButton,addCashButton,goToLobby,walletOpenButton,walletCloseButton, exitAddMoney,fraudPolicyButton;
    public Button fraudClose;
    private Button battleAgain;
    public GameObject exitText;
    private Image dpImage,tierImage;
    private Text  nameText,scoreText;
    public Transform playerResultParent;
    public NextLobbyConfig nextConfig { get; private set; }
    public GameObject playerListEndPrefab;
    public GameObject buttonPanel;
    public Text bonusCashPopTitleText;
    
    public Text tokenAmtText, cashAmtText;
    public GameObject findnewplayerbutton,battleAgainButton;
    public MatchMakingController1vN matchMakingManager;
    private MPLController mPLController;
    public MultiPlayerCanvasController1vN multiplayerCanvasController;
    private string mpluserID1, mpluserID2;
    //public Image ResultImage;
    public Sprite cashImage,tokenImage,diamondImage;
    double tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt;
    public Text bonusCashAmountText, winningsCashAmountText, depositCashAmountText;
    private bool hasBalanceAvailable, hasBalanceAvailableForUpsell;
    bool isWinner;
    public GameObject exitButton,knockOutOkayButton;
    
    
    public Text bonusCashSubtitle, bonusCashCanBeUsedText;
    int winnerIndex;
    public double bonusLimit, availableCashAmt;
    public GameObject rankLoader;
    public Text fraudTextSubtitle;
   
    public GameObject bonusCashPopUp, bonusCashCanBeUsed, bonusCashCannotBeUsed;
    public GameObject walletIcon, AccountBalanceCard;
    public Button autoStartNextButton, cancelAutoStartButton, upsellButton;

    public Text playerScore;
    public Text playerRank;

 
    private bool isFraudEnabled;
    public GameObject playerInvalidScoreParent;
    public GameObject playerRankInfoParent;
    public Text viewPolicyButtonText;
    public Button viewPolicyButton;
    public RectTransform playerListRectTransform;

    public bool isPortrait;
    private const int NORMAL_Y_POS  = 0;
    private const int NORMAL_HEIGHT  = 225;
    private const int AlIGNED_Y_POS = 34;
    private const int ITEM_HEIGHT = 48;

    public GameObject playerListScoreHeader, rankWidgetScoreParent;

    public GameObject scoreLabelParent;
    public Text scoreLabel;
    public GameObject fairplayObject;
    public GameObject scoreVerifiedImage;
    public Text scoreVerificationLabel;

    public GameObject _ColleactableUI;
    public GameObject _ColletibleUpdateUI;
    public GameObject _TrialsUI,_TrialsProgressionUI;
    public GameObject _2DImage, _3DImage;

    public GameObject _SelectionUI, _SelectedUI;
    public Text _ColleactableText;
    


    public FraudPopupController fraudPopupController;
    public GameObject gamePlayCancelledObject;

    private bool isFraudster;

    public ApplicationCollectibleData application;

    public Text waitingPlayersText;

    public GameObject _TrialsBG;

    #region FestiveTheme


    public GameObject festiveThemeChristmas;
    public GameObject festiveThemeSnowFallEffect;
    #endregion
    int mTutoInfo1 = 2, mTutoInfo2 = 4, mTutoInfoReset = 5;
    private void OnEnable()
    {

        disableAutoStart = false;
        multiplayerGamesHandler = MultiplayerGamesHandler.Instance;
        SmartFoxManager.Instance.OpponentFinished += OpponentFinished;


        CollectableDataUpdate();
        GameBattaleIdScreenHandler.mInstance.HideBattleId();
        multiplayerGamesHandler.SetGameState(MultiplayerGamesHandler.MPLGameState.game_end);

    }
    private void OpponentFinished(int userId)
    {
        DisableSyncLoader(userId);
    }
    
    void CollectableDataUpdate()
    {
        if(CollectableHandler.mInstance.NudgeOn())
        CollectableHandler.mInstance.KeysMatch++;
        if (CollectableHandler.mInstance.Collectable == eNotificationType.None )
        {
            if (CollectableHandler.mInstance.EnableProgressionBar() && CollectableHandler.mInstance.NudgeOn())
            {
                _ColletibleUpdateUI.SetActive(true);
                disableAutoStart = true;
                CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
            }
            else if (CollectableHandler.mInstance.KeysMatch == mTutoInfo1 && CollectableHandler.mInstance.TrialsOn())
            {
                if(CollectableHandler.mInstance.ShowPopUp2DGameOver() != null)
                {
                    _TrialsUI.SetActive(true);
                    disableAutoStart = true;
                }
                else
                {
                    CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
                }
               
            }
            else if (CollectableHandler.mInstance.KeysMatch == mTutoInfo2 && CollectableHandler.mInstance.TrialsOn())
            {
                if (CollectableHandler.mInstance.ShowPopUp2DGameOver() != null)
                {
                    _TrialsProgressionUI.SetActive(true);
                    disableAutoStart = true;
                    CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
                }
                else
                {
                    CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
                }
               
            }
            
            return;
        }
        if(CollectableHandler.mInstance.TrialsOn())
        CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
        try
        {
            switch (CollectableHandler.mInstance.Collectable)
            {
                case eNotificationType.ImageType:

                    _2DImage.GetComponent<Image>().sprite = CollectableHandler.mInstance.ShowPopUp2DGameOver();
                    break;
                case eNotificationType.ModelType:
                    _3DImage.GetComponent<RawImage>().texture = CollectableHandler.mInstance.ShowPopUp3DGameOver();
                    break;
            }


            _SelectionUI.SetActive(true);
            _SelectedUI.SetActive(false);
            _ColleactableUI.SetActive(true);
            _2DImage.SetActive(true);
            _ColleactableText.text = CollectableHandler.mInstance._CollectableName.ToUpper();
            CollectableHandler.mInstance.KeysMatch = mTutoInfoReset;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            disableAutoStart = true;
            CollectableHandler.mInstance.Collectable = eNotificationType.None;
        }
       
      
       
      
    }

    public void UpdateSelectionUI()
    {
       
        CollectableHandler.mInstance.ChangeCollectableOnMPL(ChangeColleacbleResponse);
    }

    private void ChangeColleacbleResponse(bool status, string eventId)
    {
        if (status)
        {
            Debug.Log("ChangeCollectableOnMPL Success Ravi Ranjan" + eventId);
            _SelectionUI.SetActive(false);
            _SelectedUI.SetActive(true);
              CollectableHandler.mInstance.UpdateTaskSelection();

           // var jobject = JsonConvert.DeserializeObject<JObject>(eventId);
           // Debug.Log("Mintu "+ jobject);
           //// Application application = new Application();
           // application = JsonUtility.FromJson<ApplicationCollectibleData>(eventId);

           // CollectableHandler.mInstance.allTasks = application.payload.collectibles;
           // for(int mCount = 0; mCount < CollectableHandler.mInstance.allTasks.Count; mCount++)
           // {
           //     Debug.LogError(CollectableHandler.mInstance.allTasks[mCount].unlocked + " "+ application.payload.collectibles[mCount].unlocked);
           //     Debug.LogError(CollectableHandler.mInstance.allTasks[mCount].selected + " " + application.payload.collectibles[mCount].selected);
           // }

        }
        else
        {

            Debug.Log("ChangeCollectableOnMPL Failed");
        }
    }

    public void Rematch()
    {
        if (MPLController.Instance.gameConfig.IsAutoStartEnabled)
        {
            autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
        }

        if (hasBalanceAvailable)
        {
            

            battleAgain.interactable = false;
            if (battleAgainRecieved == false)
            {
                battleAgainRequested = true;
                //Event
                MultiplayerGamesHandler.Instance.SubmitBattleRematchRequestedEvent(isWinner, true);
            }
            else
            {
                //Events
                MultiplayerGamesHandler.Instance.SubmitBattleRematchRequestedEvent(isWinner, false);
                MultiplayerGamesHandler.Instance.SubmitBattleRematchRespondedEvent(isWinner, true, true);
            }

            BattleAgain(true);
        }
        else
        {
            bonusCashSubtitle.text = "Add ₹" + Math.Ceiling(Math.Abs(double.Parse(battleAgainController.lobbyAmountText.text) - availableCashAmt)) + " or more Cash Balance to keep playing in Multiplayer Battles!";
           
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
                
           
            Session.Instance.SetMoneyToAdd(Math.Ceiling(Math.Abs(double.Parse(battleAgainController.lobbyAmountText.text) - availableCashAmt)));
            bonusCashPopUp.SetActive(true);
            if (autoStartNextButton.gameObject.activeSelf)
            {
                autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
            }
            BattleAgain(false);
            MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Battle Insufficient Funds", "Insufficient Deposit and Winnings Cash", bonusCashSubtitle.text);
        }
    }

    void BattleAgain(bool wantsTo)
    {
        //StopBattleAgainTimer();
        if(MPLController.Instance.IsThirdPartyGame())
        {
            return;
        }
        if (isIt1v1)
        {
            Debug.Log("Wants to battle again = " + wantsTo);
            multiplayerCanvasController.gameSceneLoaded = false;
            SmartFoxManager.Instance.BattleAgain(wantsTo);
        }
    }
    public void ShowBalance(bool show)
    {
        AccountBalanceCard.SetActive(show);
        if (autoStartNextButton.gameObject.activeSelf)
        {
            autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
        }
    }

    [ContextMenu("debug fraud pup")]
    public void DebugFraudPopup()
    {
        ShowFraudPopup(true);
    }
    public void ShowFraudPopup(bool show, string message="", bool isFraudster = false)
    {
        if (!MPLController.Instance.gameConfig.FraudBlockEnabled)
        {


            if (show)
            {
                fraudTextSubtitle.text = message;
                fraudTextSubtitle.text = message;
                if (isFraudster)
                    multiplayerGamesHandler.SubmitBattlePopupShownEvent("Fraudster Pop Up", "Score Rejected!", message);
                else
                    multiplayerGamesHandler.SubmitBattlePopupShownEvent("Genuine Player Pop Up", "Score Rejected!", message);
            }

            fraudPopUp.SetActive(show);
        }
        else
        {

        fraudPopupController.SetActivePopup(show);
        }
        if (show)
        {


            if (autoStartNextButton.gameObject.activeSelf)
            {
                autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
            }
        }


    }
  

    public void GoToFraudPolicy(bool isFraudster = false)
    {
        if(MPLController.Instance.gameConfig.FraudBlockEnabled)
        {
            if(isFraudster == false)
            {
                isFraudster = fraudPopupController.IsFraudster();
            }
            if(isFraudster)
            {
                multiplayerGamesHandler.SubmitBattleButtonClickedEvent("Fairplay Policy Score rejected Fraudster");
            }
            else
            {
                multiplayerGamesHandler.SubmitBattleButtonClickedEvent("Fairplay Policy Score rejected Other");
            }
        }
        Session.Instance.SetQuitReason(Session.MPLQuitReason.view_fraud_policy);
        Session.Instance.Quit();
        BattleAgain(false);
    }
    public void UpdateBalance(double tokenBal, double totalBal, double bonusBal, double depositBal, double winningsBal)
    {
        tokenAmtText.text = "" + (int)tokenBal;
        cashAmtText.text = "" + totalBal;
        bonusCashAmountText.text = bonusBal.ToString();
        depositCashAmountText.text = depositBal.ToString();
        winningsCashAmountText.text = winningsBal.ToString();
        mPLController.gameConfig.Profile.TokenBalance = tokenBal;
        mPLController.gameConfig.Profile.TotalBalance = totalBal;
        mPLController.gameConfig.Profile.BonusBalance = bonusBal;
        mPLController.gameConfig.Profile.DepositBalance = depositBal;
        mPLController.gameConfig.Profile.WithdrawableBalance = winningsBal;
    }

    public void AddCash()
    {
        
            Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
       
        Session.Instance.SetMoneyToAdd(mPLController.gameConfig.EntryFee);
        BattleAgain(false);
        Session.Instance.Quit();
    }
    void ShowGameQuitPopup(bool show)
    {
        gameQuitPopup.SetActive(show);
        
    }
    void StartNewBattleFromGameOver()
    {
        if (!MPLController.Instance.IsThirdPartyGame())
        {
            SmartFoxManager.Instance.GetAccountBalance((accountBalance) =>
            {
                bool hasEnough = true;
                Debug.Log("MPL: Account Balance=" + accountBalance);
                if (accountBalance != null)
                {
                    tokenAmt = accountBalance.tokenBalance;
                    cashAmt = accountBalance.totalBalance;
                    bonusCashAmt = accountBalance.bonusBalance;
                    depositCashAmt = accountBalance.depositBalance;
                    winningsCashAmt = accountBalance.withdrawableBalance;
                }

                UpdateBalance(tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt);

                BalanceCheck();
                walletIcon.SetActive(!mPLController.gameConfig.IsKnockoutLobby);
                FindMatchAgain(false);

            });
        }
        else
        {
            MPLSdkBridgeController.Instance.GetAccountBalance((accountBalance) =>
            {
                bool hasEnough = true;
                Debug.Log("MPL: Account Balance=" + accountBalance);
                if (accountBalance != null)
                {
                    tokenAmt = accountBalance.tokenBalance;
                    cashAmt = accountBalance.totalBalance;
                    bonusCashAmt = accountBalance.bonusBalance;
                    depositCashAmt = accountBalance.depositBalance;
                    winningsCashAmt = accountBalance.withdrawableBalance;
                }

                UpdateBalance(tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt);

                BalanceCheck();
                walletIcon.SetActive(!mPLController.gameConfig.IsKnockoutLobby);
                FindMatchAgain(false);

            });
        }

        
        
    }
    public static List<Transform> GetActiveChilds(Transform trans)
    {
        var childs = new List<Transform>();

        foreach (Transform child in trans)
        {
            if (child.gameObject.activeSelf)
            {
                childs.Add(child);
            }
        }

        return childs;
    }

   
    public void StartTrainingAgain()
    {
        MPLController.Instance.SetTutorialEntryPoint("Game Over Screen");
        MPLSdkBridgeController.Instance.StartTutorialAgain();
        gameObject.SetActive(false);
        
        
    }

    public void ShowGameEnd()
    {
        mPLController = MPLController.Instance;
   
      
        if(interactivePopup.activeSelf)
        {
            interactivePopup.SetActive(false);
        }
        scoreVerifiedImage.gameObject.SetActive(false);
        fairplayObject.gameObject.SetActive(false);
        scoreVerificationLabel.gameObject.SetActive(false);
        scoreLabel.gameObject.SetActive(false);

        playerInvalidScoreParent.SetActive(false);
        playerRankInfoParent.SetActive(true);
        gamePlayCancelledObject.SetActive(false);

        rankLabel.text = mPLController.localisationDetails.GetLocalizedText("Syncing Scores...");
        waitingPlayersText.gameObject.SetActive(true);
        isFraudEnabled = false;
        battleAgainRequested = battleAgainRecieved = false;
        battleAgainButton.SetActive(false);
        battleAgain = battleAgainButton.gameObject.GetComponent<Button>();
        battleAgainController = battleAgainButton.gameObject.GetComponent<ButtonController>();
        battleAgain.interactable = true;
        battleAgain.onClick.RemoveAllListeners();
        battleAgain.onClick.AddListener(() => Rematch());
        SmartFoxManager.Instance.OnRoomJoined += OnRoomJoined;
        SmartFoxManager.Instance.OnUserVarsUpdated += OnUserVarsUpdated;
        SmartFoxManager.Instance.OnFightAgainStateChange += OnFightAgainStateChange;
      
        startBattle.SetActive(false);

        //ResultImage.sprite = GameOverSprite;
        
        
        upsellButton.gameObject.SetActive(false);
        findnewplayerbutton.SetActive(false);
        autoStart.SetActive(false);
        autoStartNextButton.gameObject.SetActive(false);
        cancelAutoStartButton.gameObject.SetActive(false);
        buttonPanel.SetActive(false);
        TempPlayerProfiles = new Dictionary<int, GameObject>();


        
        
        bonusCashCannotBeUsed.SetActive(false);
        bonusCashCanBeUsed.SetActive(false);
        Debug.Log("Is upsell enabled=" + MPLController.Instance.gameConfig.IsUpsellEnabled);


        findNewController = findnewplayerbutton.gameObject.GetComponent<ButtonController>();
        autoStartController = autoStartNextButton.gameObject.GetComponent<ButtonController>();

        upsellController = upsellButton.gameObject.GetComponent<ButtonController>();

        fraudClose.onClick.AddListener(() => ShowFraudPopup(false, ""));


        findnewplayerbutton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        findnewplayerbutton.gameObject.GetComponent<Button>().onClick.AddListener(() => FindMatchAgain(false));
        fraudPolicyButton.onClick.RemoveAllListeners();
        fraudPolicyButton.onClick.AddListener(() => GoToFraudPolicy());
        fraudPopupController.ResetPopup();
        fraudPopupController.policyButton.onClick.AddListener(() => GoToFraudPolicy());
        autoStartNextButton.onClick.RemoveAllListeners();
        autoStartNextButton.onClick.AddListener(() => FindMatchAgain(false));
        upsellButton.onClick.RemoveAllListeners();
        upsellButton.onClick.AddListener(() => FindMatchAgain(true));
        exitButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButton.gameObject.GetComponent<Button>().onClick.AddListener(() => GoToLobby());

        knockOutOkayButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        knockOutOkayButton.gameObject.GetComponent<Button>().onClick.AddListener(() => GoToLobby());
        goToLobby.onClick.RemoveAllListeners();
        if(MPLController.Instance.gameConfig.GameId==1000040)
        {
            
            goToLobby.onClick.AddListener(() => GoToLobby());
        }
        else
        {
            goToLobby.onClick.AddListener(() => ShowGameQuitPopup(true));
        }
        
        if(mPLController.gameConfig.FestiveThemeOn)
        {

            festiveThemeChristmas.SetActive(true);
            festiveThemeSnowFallEffect.gameObject.SetActive(true);
               

        }
        gameQuitPopup.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameQuitPopup.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(()=> GoToLobby());

        gameQuitPopup.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameQuitPopup.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => StartNewBattleFromGameOver());

        addMoneyButton.onClick.RemoveAllListeners();
        addMoneyButton.onClick.AddListener(() => GoToBattleRoom());
        addCashButton.onClick.RemoveAllListeners();
        addCashButton.onClick.AddListener(() => AddCash());
        walletOpenButton.onClick.RemoveAllListeners();
        walletOpenButton.onClick.AddListener(() => ShowBalance(true));
        walletIcon.SetActive(!mPLController.gameConfig.IsKnockoutLobby);
        walletCloseButton.onClick.RemoveAllListeners();
        walletCloseButton.onClick.AddListener(() => ShowBalance(false));

        exitAddMoney.onClick.RemoveAllListeners();
        exitAddMoney.onClick.AddListener(() => GoToLobby());

        tokenAmt = mPLController.gameConfig.Profile.TokenBalance;
        cashAmt = mPLController.gameConfig.Profile.TotalBalance;
        bonusCashAmt = mPLController.gameConfig.Profile.BonusBalance;
        depositCashAmt = mPLController.gameConfig.Profile.DepositBalance;
        winningsCashAmt = mPLController.gameConfig.Profile.WithdrawableBalance;
        UpdateBalance(tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt);
        
           
            SetButtonProperties();
            //BalanceCheck();
           // buttonPanel.SetActive(true);
            //findnewplayerbutton.SetActive(true);
        
        if (MPLController.Instance.IsAsyncGame())
        {


            playerListScoreHeader.SetActive(true);
            rankWidgetScoreParent.SetActive(true);
            scoreLabelParent.SetActive(true);

        }
        else
        {
            playerListScoreHeader.SetActive(false);
            rankWidgetScoreParent.SetActive(false);
            scoreLabelParent.SetActive(false);

        }
        if (MPLController.Instance.gameConfig.FraudCheckEnabledGameIds != null)
        {
            for (int i = 0; i < MPLController.Instance.gameConfig.FraudCheckEnabledGameIds.Length; i++)
            {
                if (MPLController.Instance.gameConfig.FraudCheckEnabledGameIds[i] == MPLController.Instance.gameConfig.GameId.ToString())
                {
                    isFraudEnabled = true;
                   
                    //for (int j = 0; j < fraudItems.Length; j++)
                    //{
                    //    fraudItems[j].SetActive(true);
                    //}
                }
            }
        }
        if(isFraudEnabled)
        {

            scoreLabel.gameObject.SetActive(false);
            scoreVerificationLabel.gameObject.SetActive(true);
            fairplayObject.SetActive(true);
            scoreVerificationLabel.text = mPLController.localisationDetails.GetLocalizedText("Verifying Scores") + "...";
        }
        else
        {

            scoreLabel.gameObject.SetActive(true);
            scoreVerificationLabel.gameObject.SetActive(false);
            fairplayObject.SetActive(false);
        }
        Debug.Log("MultiplayerCanvasController.userListOnMatch=" + multiplayerGamesHandler.userListOnMatch.Count);

        int childs = playerResultParent.childCount;
        if (childs < multiplayerGamesHandler.userListOnMatch.Count)
        {
            int childObjectsRequired = multiplayerGamesHandler.userListOnMatch.Count - childs;
            for (int i = 0; i < childObjectsRequired; i++)
            {
                GameObject _opponentListItem = Instantiate(playerListEndPrefab);
                _opponentListItem.transform.SetParent(playerResultParent);
                _opponentListItem.gameObject.GetComponent<PlayerItemController>().nameTierGroup.spacing += 0.01f;
                _opponentListItem.gameObject.GetComponent<PlayerItemController>().nameTierGroup.spacing -= 0.01f;
                _opponentListItem.transform.localScale = Vector3.one;

                _opponentListItem.SetActive(false);
                
            }
        }
        childs = playerResultParent.childCount;
        for (int i = 0; i < childs; i++)
        {
            Debug.Log("Getting Child Match=" + i);
            if (playerResultParent.GetChild(i).gameObject.activeSelf)
            {
                playerResultParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        playerResultParent.gameObject.SetActive(false);
        multiplayerGamesHandler.userIdToCandidate[MPLController.Instance.gameConfig.Profile.id].isScoreSubmitted = true;

        for (int i = 0; i < multiplayerGamesHandler.userListOnMatch.Count; i++)
        {
            Debug.Log("Adding player");
            GameObject _playerListItem = playerResultParent.transform.GetChild(i).gameObject;


            Text scorePlayer = _playerListItem.GetComponent<PlayerItemController>().score;
            scorePlayer.text = "";
            Sprite userDp = _playerListItem.GetComponent<PlayerItemController>().playerDp.sprite;
            Sprite tier = _playerListItem.GetComponent<PlayerItemController>().tier.sprite;
            string playerName = "";
            bool isScoreSubmitted=false;
            if (multiplayerGamesHandler.userIdToCandidate != null && multiplayerGamesHandler.userIdToCandidate.ContainsKey(multiplayerGamesHandler.userListOnMatch[i].id))
            {
                Debug.Log("Adding player2");
                Candidate candidate = multiplayerGamesHandler.userIdToCandidate[multiplayerGamesHandler.userListOnMatch[i].id];
                if (candidate.displayPic != null)
                {
                    userDp = candidate.displayPic;
                }
                if (candidate.scoreText != null)
                {
                    scorePlayer = candidate.scoreText;
                }
                if (candidate.tier != null)
                {
                    tier = candidate.tier;
                }
                if (candidate.name != null)
                {
                    playerName = candidate.name;
                }

                isScoreSubmitted = candidate.isScoreSubmitted;
            }
            _playerListItem.SetActive(true);
          
            SetPlayerEndProperties(_playerListItem, playerName, userDp, tier, scorePlayer, multiplayerGamesHandler.userListOnMatch[i].id,isScoreSubmitted);
            

            

            if (TempPlayerProfiles.ContainsKey(multiplayerGamesHandler.userListOnMatch[i].id))
            {
                TempPlayerProfiles[multiplayerGamesHandler.userListOnMatch[i].id] = _playerListItem;
            }
            else
            {
                TempPlayerProfiles.Add(multiplayerGamesHandler.userListOnMatch[i].id, _playerListItem);
            }
            //TempPlayerProfiles[]
            if (isIt1v1)
            {
                Debug.Log("Setting up players for battle again");
                if (multiplayerGamesHandler.userListOnMatch[i].id == MPLController.Instance.gameConfig.Profile.id)
                {
                    if (TempPlayerProfiles.ContainsKey(multiplayerGamesHandler.userListOnMatch[i].id))
                    {
                        localPlayerObject = _playerListItem;
                        mpluserID1 = multiplayerGamesHandler.userListOnMatch[i].id.ToString();
                        playAgainMessage1 = localPlayerObject.gameObject.GetComponent<PlayerItemController>().playAgain1Message;
                    }

                }
                else
                {
                    if (TempPlayerProfiles.ContainsKey(multiplayerGamesHandler.userListOnMatch[i].id))
                    {
                        opponentPlayerObject = _playerListItem;
                        mpluserID2 = multiplayerGamesHandler.userListOnMatch[i].id.ToString();
                        playAgainMessage2 = opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().playAgain2Message;
                        cantPlayMessage = opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().cantPlayMessage;
                    }
                }

            }


        }

        AdjustResultListContent(GetActiveChilds(playerResultParent).Count);
        playerResultParent.gameObject.SetActive(true);





        Time.timeScale = 1;



        StartCoroutine(WaitAndPlayAnims());
        Debug.Log("OnBatOnBattleFinish AA ####");

        


        rankLoader.SetActive(true);
        gameOverShown = false;
    }

    IEnumerator WaitAndPlayAnims()
    {
        yield return null;
        Debug.Log("WHYYYYY NO GAME END ((***))????");

        if (!gameOverShown) MultiplayerGamesHandler.Instance.SubmitAppScreenViewedEvent("Battle Game Over");
        gameOverShown = true;
    }

    private void OnDisable()
    {
        
        SmartFoxManager.Instance.OnUserVarsUpdated -= OnUserVarsUpdated;

        SmartFoxManager.Instance.OpponentFinished -= OpponentFinished;
        SmartFoxManager.Instance.OnRoomJoined -= OnRoomJoined;
        SmartFoxManager.Instance.OnFightAgainStateChange -= OnFightAgainStateChange;
        if (autoStartNextButton.gameObject.activeSelf)
        {
            autoStartNextButton.gameObject.GetComponent<FillButton>().CancellLoader -= OnCancelFill;
            autoStartNextButton.gameObject.GetComponent<FillButton>().AutoStartBattle -= OnAutoStartBattle;
        }
        if(_ColleactableUI != null)
        _ColleactableUI.SetActive(false);

        if (_TrialsBG != null)
        _TrialsBG.SetActive(false);
     
  
    }

    void OnFightAgainStateChange(Dictionary<string, string> state)
    {
        if(!isIt1v1 || MPLController.Instance.gameConfig.IsUpsellEnabled)
        {
            return;
        }
        Debug.Log("mpluserID1 = " + mpluserID1 + ", mpluserID2 = " + mpluserID2);
        //I accepted

        if(state==null)
        {
            return;
        }
        if (!state.ContainsKey(mpluserID1) || !state.ContainsKey(mpluserID2))
        {
            Debug.Log("MPL: State Dictionary is null");
            return;
        }


        if (state[mpluserID1] == "ACCEPTED")
        {
            if (mpluserID1 == "" + mPLController.gameConfig.Profile.id)
            {
                if (battleAgainRequested)
                {
                    playAgainMessage1.SetActive(true);
                    
                }
                
            }
            else
            {
                battleAgainRecieved = true;
                if (battleAgainRequested == false)
                {
                    
                    playAgainMessage2.SetActive(true);
                    
                }
                else
                {
                    
                    MultiplayerGamesHandler.Instance.SubmitBattleRematchRespondedEvent(isWinner, true, false);

                   
                }
            }
        }
        //opponent accepted
        if (state[mpluserID2] == "ACCEPTED")
        {
            if (mpluserID2 == "" + mPLController.gameConfig.Profile.id)
            {

                if (battleAgainRequested)
                {
                    playAgainMessage1.SetActive(true);
                    
                    
                }
            }
            else
            {
                battleAgainRecieved = true;
                if (battleAgainRequested == false)
                {
                    
                    playAgainMessage2.SetActive(true);

                    
                }
                else
                {


                    //I requested, opponent accepted
                    //Event
                    MultiplayerGamesHandler.Instance.SubmitBattleRematchRespondedEvent(isWinner, true, false);
                }
            }
        }
        if (state[mpluserID2] == "REJECTED")
        {
            if (mpluserID2 == "" + mPLController.gameConfig.Profile.id)
            {
                //rematchcloud1.SetActive(true);
                if (battleAgainRecieved)
                {
                    
                    cantPlayMessage.SetActive(true);
                    

                    battleAgain.interactable = false;
                }
                else
                {
                    
                    cantPlayMessage.SetActive(true);
                    

                    battleAgain.interactable = false;
                }
            }
            else
            {
                if (battleAgainRequested)
                {
                    
                    cantPlayMessage.SetActive(true);
                    

                    battleAgain.interactable = false;
                }
                else
                {
                    //I requested, opponent rejected
                    //Event
                    MultiplayerGamesHandler.Instance.SubmitBattleRematchRespondedEvent(isWinner, false, false);

                    playAgainMessage2.SetActive(false);
                    cantPlayMessage.SetActive(true);
                    

                    //multiplayerCanvasController.gameStartTimer.Stop();
                    battleAgain.interactable = false;
                }
            }
        }
        if (state[mpluserID1] == "REJECTED")
        {
            if (mpluserID1 == "" + mPLController.gameConfig.Profile.id)
            {
                if (battleAgainRecieved)
                {
                    
                    cantPlayMessage.SetActive(true);
                    
                    
                    battleAgain.interactable = false;

                }
                else
                {
                    
                    cantPlayMessage.SetActive(true);
                    
                    
                    battleAgain.interactable = false;
                }
            }
            else
            {
                if (battleAgainRequested)
                {
                    
                    cantPlayMessage.SetActive(true);
                    battleAgain.interactable = false;
                }
                else
                {
                    //I requested, opponent reject
                    //Event
                    MultiplayerGamesHandler.Instance.SubmitBattleRematchRespondedEvent(isWinner, false, false);

                    playAgainMessage2.SetActive(false);
                    cantPlayMessage.SetActive(true);
                    battleAgain.interactable = false;
                }
            }
        }
    }
   
    void OnRoomJoined(UserProfile profile)
    {
        if(!isIt1v1)
        {
            return;
        }

        Debug.Log("on room joined");
        if (cantPlayMessage.gameObject.activeSelf == true)
        {
            cantPlayMessage.gameObject.SetActive(false);
        }
        
        if (playAgainMessage1.gameObject.activeSelf == true)
        {
            playAgainMessage1.gameObject.SetActive(false);
        }
        if (playAgainMessage2.gameObject.activeSelf == true)
        {
            playAgainMessage2.gameObject.SetActive(false);
        }

        if (autoStartNextButton.gameObject.activeSelf)
        {
            autoStartNextButton.gameObject.SetActive(false);
        }

        if (autoStart.gameObject.activeSelf)
        {
            autoStart.SetActive(false);
        }
        buttonPanel.SetActive(false);
        if(cancelAutoStartButton.gameObject.activeSelf)
        {
            cancelAutoStartButton.interactable = true;
            cancelAutoStartButton.gameObject.SetActive(false);
        }
        if (findnewplayerbutton.gameObject.activeSelf)
        {
            findnewplayerbutton.gameObject.SetActive(false);
        }
        if (upsellButton.gameObject.activeSelf)
        {
            upsellButton.gameObject.SetActive(false);
        }
        battleAgain.gameObject.SetActive(false);
        buttonPanel.SetActive(false);
        localPlayerObject.gameObject.GetComponent<PlayerItemController>().rewardsParent.gameObject.SetActive(false);
        localPlayerObject.gameObject.GetComponent<PlayerItemController>().score.gameObject.SetActive(false);
        //localPlayerObject.gameObject.GetComponent<PlayerItemController>().scoreLabel.gameObject.SetActive(false);

        opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().rewardsParent.gameObject.SetActive(false);
        opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().score.gameObject.SetActive(false);
        //opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().scoreLabel.gameObject.SetActive(false);
        //if (localPlayerObject.gameObject.GetComponent<PlayerItemController>().winnerObject.gameObject.activeSelf)
        //{
        //    localPlayerObject.gameObject.GetComponent<PlayerItemController>().winnerObject.gameObject.SetActive(false);
        //    localPlayerObject.gameObject.GetComponent<Image>().color = new Color32(216, 240, 250, 255);
        //}

        //if (opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().winnerObject.gameObject.activeSelf)
        //{
        //    opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().winnerObject.gameObject.SetActive(false);
        //    opponentPlayerObject.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //}
        localPlayerObject.gameObject.GetComponent<PlayerItemController>().rankText.gameObject.SetActive(false);
        opponentPlayerObject.gameObject.GetComponent<PlayerItemController>().rankText.gameObject.SetActive(false);

        //ResultImage.gameObject.SetActive(false);

        startBattle.SetActive(true);

        StartCoroutine(RestartGame());
    }
    IEnumerator RestartGame()
    {
        //yield return new WaitForSeconds(2.5f);
        yield return new WaitForSeconds(0f);
        Debug.Log("BattleEndController RestartGame called");
        multiplayerCanvasController.gameSceneLoaded = true;
        mPLController.LoadGameSceneFromAssetBundle(mPLController.gameConfig.GameId);
    }
    public void GoToBattleRoom()
    {
        if(MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            Session.Instance.SetQuitReason(Session.MPLQuitReason.knockout_quit);
        }

        BattleAgain(false);
        Session.Instance.Quit();
    }
    public void GoToLobby()
    {
        BattleAgain(false);
        
            if (mPLController.gameConfig.IsKnockoutLobby)
            {
                Session.Instance.SetQuitReason(Session.MPLQuitReason.knockout_quit);
            }
            else
            {
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
            }
        
       
       
        Session.Instance.Quit();
        
    }
    public void OnUserScoresUpdate(Dictionary<int, int> usersToScore)
    {
        foreach (KeyValuePair<int, int> item in usersToScore)
        {
            if (multiplayerGamesHandler.userIdToCandidate.ContainsKey(item.Key))
            {
                if (multiplayerGamesHandler.userIdToCandidate[item.Key].scoreText != null)
                {
                    multiplayerGamesHandler.userIdToCandidate[item.Key].scoreText.text = "" + MPL.utility.shortForm.ShortForm.GetFormattedString(usersToScore[item.Key]);
                }
            }
        }
    }
    public IEnumerator EnqueueBattleFinished(List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList, bool isFightAgainDisabled)
    {
        Debug.Log("Enqueued battle finish data");
        yield return new WaitWhile(() => !gameOverShown);
        Debug.Log("Calling battle finished");
        BattleFinished(mPLBattleFinishPlayersInfoList, isFightAgainDisabled);
    }



    void BattleFinished(List<MPLSdkBattleFinishPlayersInfo> mPLBattleFinishPlayersInfoList, bool isFightAgainDisabled)
    {

        SetBattleFinish();
        isBattleAgainEnabled = !isFightAgainDisabled;
        isIt1v1 = isBattleAgainEnabled;
        //gameOverTextSubtitle.gameObject.SetActive(false);
        Debug.Log("OnBatOnBattleFinish DD ####");
        Debug.Log("MPL: BattleFinishData=" + mPLBattleFinishPlayersInfoList);

        MPLSdkBattleFinishPlayersInfo myData = new MPLSdkBattleFinishPlayersInfo();
        MPLSdkBattleFinishPlayersInfo winnerData = new MPLSdkBattleFinishPlayersInfo();
        for (int i = 0; i < mPLBattleFinishPlayersInfoList.Count; i++)
        {
            if (MPLSdkBridgeController.Instance.profile.id == mPLBattleFinishPlayersInfoList[i].userId)
            {
                playerScore.text = MPL.utility.shortForm.ShortForm.GetFormattedString(mPLBattleFinishPlayersInfoList[i].score);
                if (mPLBattleFinishPlayersInfoList[i].rank != 0)
                    playerRank.text = mPLBattleFinishPlayersInfoList[i].rank.ToString();
                else
                    playerRank.text = mPLController.localisationDetails.GetLocalizedText("TIED");// "TIED";

                playerRank.gameObject.SetActive(true);
                myData = mPLBattleFinishPlayersInfoList[i];

              
            }

            if (mPLBattleFinishPlayersInfoList[i].rank == 1)
            {
              
                winnerData = mPLBattleFinishPlayersInfoList[i];
            }



        }









        if (myData.rank == 1)
        {
            
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_won);
            
            if (MPLController.Instance.gameConfig.IsInteractiveTrainingEnabled && MPLController.Instance.gameConfig.IsIntractiveFtueEnabled)
            {
                ShowInteractiveTooltip(false);
            }

        }
        else if (myData.rank == 0)
        {
            if (MPLController.Instance.gameConfig.IsInteractiveTrainingEnabled && MPLController.Instance.gameConfig.IsIntractiveFtueEnabled)
            {
                ShowInteractiveTooltip(false);
            }
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
            
        }
        else
        {
            if(MPLController.Instance.gameConfig.IsInteractiveTrainingEnabled && MPLController.Instance.gameConfig.IsIntractiveFtueEnabled)
            {
                ShowInteractiveTooltip(true);
            }
            
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_lost);
           
        }



        // string nextLobbyConfigSdk = JsonUtility.ToJson(winnerData.nextLobbyConfig);
        string nextLobbyConfigSdk = winnerData.nextLobbyConfig;
        Debug.Log("(string)myData.nextLobbyConfig=" + nextLobbyConfigSdk);

        if (!String.IsNullOrEmpty(nextLobbyConfigSdk))
        {
            //MultiplayerGamesHandler.Instance.nextLobbyConfig = JsonUtility.ToJson(winnerData.nextLobbyConfig);
            MultiplayerGamesHandler.Instance.nextLobbyConfig = winnerData.nextLobbyConfig;
            nextConfig = JsonUtility.FromJson<NextLobbyConfig>(MultiplayerGamesHandler.Instance.nextLobbyConfig);
        }
        else
        {
            MultiplayerGamesHandler.Instance.nextLobbyConfig = "";
        }



        //gameOverThings.SetActive(false);



        
            canIPlayAgain = myData.canPlayAgain;
            if (mPLController.gameConfig.GameId == 1000040)
            {
                canIPlayAgain = false;
            }



            
            
            amITheWinner = false;

            Debug.Log("My Rank=" + myData.rank);
            if (myData.rank == 1)
            {
                //ResultImage.sprite = YouWonSprite;
                amITheWinner = true;

            }
            
            findnewplayerbutton.SetActive(false);
            

            Debug.Log("mPLBattleFinishPlayersInfoList.Count=" + mPLBattleFinishPlayersInfoList.Count);
            Debug.Log("TempPlayerProfiles.count" + TempPlayerProfiles.Count);
            for (int i = 0; i < mPLBattleFinishPlayersInfoList.Count; i++)
            {
                Debug.Log("Profile update=");
                Debug.Log("ID=" + mPLBattleFinishPlayersInfoList[i].userId);
                Debug.Log("profile=" + TempPlayerProfiles[mPLBattleFinishPlayersInfoList[i].userId].ToString());

                SetPlayerResultProperties(TempPlayerProfiles[mPLBattleFinishPlayersInfoList[i].userId], mPLBattleFinishPlayersInfoList[i].rank, mPLBattleFinishPlayersInfoList[i].cashWinnings, mPLBattleFinishPlayersInfoList[i].tokenWinnings, mPLBattleFinishPlayersInfoList[i].score, mPLBattleFinishPlayersInfoList[i].userId, mPLBattleFinishPlayersInfoList[i].extraInfo, mPLBattleFinishPlayersInfoList[i].isCashReward, mPLBattleFinishPlayersInfoList[i].extReward);
            }

            List<GameObject> finalPlayerList = new List<GameObject>();

            for (int i = 0; i < TempPlayerProfiles.Count; i++)
            {
                finalPlayerList.Add(playerResultParent.GetChild(i).gameObject);

            }

            finalPlayerList = sortPlayers(finalPlayerList);
            for (int i = 0; i < finalPlayerList.Count; i++)
            {

                finalPlayerList[i].transform.SetSiblingIndex(i);

            }
            //gameOverImage.SetActive(false);
            //ResultImage.gameObject.SetActive(true);
            rankLoader.SetActive(false);
            waitingPlayersText.gameObject.SetActive(false);
            FetchBalance();
            
            
        

    }


    void FetchBalance()
    {

        if (!MPLController.Instance.IsThirdPartyGame())
        {
            SmartFoxManager.Instance.GetAccountBalance((accountBalance) =>
            {

                bool hasEnough = true;
                Debug.Log("MPL: Account Balance=" + accountBalance);
                if (accountBalance != null)
                {
                    tokenAmt = accountBalance.tokenBalance;
                    cashAmt = accountBalance.totalBalance;
                    bonusCashAmt = accountBalance.bonusBalance;
                    depositCashAmt = accountBalance.depositBalance;
                    winningsCashAmt = accountBalance.withdrawableBalance;
                }

                UpdateBalance(tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt);

                BalanceCheck();
                walletIcon.SetActive(!mPLController.gameConfig.IsKnockoutLobby);
                ShowBattleEndButtons();

            });
        }
        else
        {
            MPLSdkBridgeController.Instance.GetAccountBalance((accountBalance) =>
            {
                bool hasEnough = true;
                Debug.Log("MPL: Account Balance=" + accountBalance);
                if (accountBalance != null)
                {
                    tokenAmt = accountBalance.tokenBalance;
                    cashAmt = accountBalance.totalBalance;
                    bonusCashAmt = accountBalance.bonusBalance;
                    depositCashAmt = accountBalance.depositBalance;
                    winningsCashAmt = accountBalance.withdrawableBalance;
                }

                UpdateBalance(tokenAmt, cashAmt, bonusCashAmt, depositCashAmt, winningsCashAmt);

                BalanceCheck();
                walletIcon.SetActive(!mPLController.gameConfig.IsKnockoutLobby);
                ShowBattleEndButtons();

            });
        }
    }
    public void ShowInteractiveTooltip(bool status)
    {
        interactiveToolTip.SetActive(status);

    }
    public void ShowInteractivePopup(bool status)
    {
        interactivePopup.SetActive(status);
        if(status)
        {
            if (autoStartNextButton.gameObject.activeSelf)
            {
                autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
            }
        }
    }
    void SetBattleFinish()
    {
        if(MPLController.Instance.gameConfig.IsInteractiveTrainingEnabled && MPLController.Instance.gameConfig.IsIntractiveFtueEnabled)
        {
            interactiveButton.SetActive(true);
            interactivePopupTitle.text = "Play " + MPLController.Instance.gameConfig.GameName + " Training?";

        }
        multiplayerGamesHandler.SetGameState(MultiplayerGamesHandler.MPLGameState.results_declared);
        isFraudster = false;
        if (gameQuitPopup.activeSelf)
        {
            gameQuitPopup.SetActive(false);
        }
        if (isFraudEnabled)
        {
            scoreVerificationLabel.text = mPLController.localisationDetails.GetLocalizedText("Verified");
            scoreVerifiedImage.gameObject.SetActive(true);
        }
        rankLabel.text = mPLController.localisationDetails.GetLocalizedText("RANK");

        goToLobby.onClick.RemoveAllListeners();
        goToLobby.onClick.AddListener(() => GoToBattleRoom());
        multiplayerCanvasController.onStartBattleCalled = false;
        rankLoader.SetActive(false);
        SetButtonProperties();
    }
    




    List<GameObject> sortPlayers(List<GameObject> arr)
    {
        arr.Sort((x, y) => x.gameObject.GetComponent<PlayerItemController>().rank.CompareTo(y.gameObject.GetComponent<PlayerItemController>().rank));
        
        return arr;
    }









   





    
    void OnUserVarsUpdated(User user, List<string> changed)
    {
        if (user.ContainsVariable("mplUserId"))
        {
            if (user.ContainsVariable("score"))
            {
                Debug.Log("BattleEndController1vN User ID=" + (int)user.GetVariable("mplUserId").Value);
                if (multiplayerGamesHandler.userIdToCandidate.ContainsKey((int)user.GetVariable("mplUserId").Value))
                {
                    if (multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText != null)
                    {
                        multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText.text = "" +  user.GetVariable("score").Value;
                     
                        multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText.text = "" + MPL.utility.shortForm.ShortForm.GetFormattedString(multiplayerGamesHandler.userIdToCandidate[(int)user.GetVariable("mplUserId").Value].scoreText.text);
                     
                    }
                }


            }
        }

       
        

    }

    public void FindMatchAgain(bool isItUpsell)
    {
        //Event
       
        SmartFoxManager.Instance.mmRetryAttempt = 0;
        if (MPLController.Instance.timerCanvas.activeSelf)
        {
            if (TimerCanvasController.Instance.CheckAndShowPopupAtEnd())
            {
                if (autoStartNextButton.gameObject.activeSelf)
                {
                    autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                }
                return;
            }
        }
        if (gameQuitPopup.activeSelf)
        {
            gameQuitPopup.SetActive(false);
        }
        BattleAgain(false);
        bool isBalanceAvailable;

                if (isItUpsell)
                {
                    MultiplayerGamesHandler.Instance.isItUpsell = true;

                    isBalanceAvailable = hasBalanceAvailableForUpsell;
                    if (!hasBalanceAvailableForUpsell)
                    {
                        bonusCashSubtitle.text = string.Format(mPLController.localisationDetails.GetLocalizedText("Add ₹{0} or more Cash Balance to keep playing in Multiplayer Battles!"), Math.Abs(nextConfig.EntryFee - cashAmt));
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
                        Session.Instance.SetMoneyToAdd(Math.Ceiling(Math.Abs(nextConfig.EntryFee - cashAmt)));
                    }
                    MPLController.Instance.gameConfig.EntryCurrency = "CASH";
                }
                else
                {
                    MultiplayerGamesHandler.Instance.isItUpsell = false;
                    isBalanceAvailable = hasBalanceAvailable;
                    if (!hasBalanceAvailable)
                    {
                        bonusCashSubtitle.text = string.Format(mPLController.localisationDetails.GetLocalizedText("Add ₹{0} or more Cash Balance to keep playing in Multiplayer Battles!"), Math.Ceiling(Math.Abs(double.Parse(findNewController.lobbyAmountText.text) - availableCashAmt)));
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
                        Session.Instance.SetMoneyToAdd(Math.Ceiling(Math.Abs(double.Parse(findNewController.lobbyAmountText.text) - availableCashAmt)));
                    }
                }
                multiplayerCanvasController.isGameRunning = false;


                Debug.Log("hasBalanceAvailable=" + hasBalanceAvailable);
                if (isBalanceAvailable)
                {


            if(isItUpsell)
            {
                multiplayerGamesHandler.DestroyPlayerProfiles(() =>
                {
                    multiplayerGamesHandler.isThisFindAgain = true;


                    gameObject.SetActive(false);


                    matchMakingManager.gameObject.SetActive(true);
                });
            }
            else
            {


                    Debug.Log("Finding Maatttch");
            multiplayerGamesHandler.isThisFindAgain = true;


                    gameObject.SetActive(false);


                    matchMakingManager.gameObject.SetActive(true);
            }
                }
                else
                {
                    bonusCashPopUp.SetActive(true);
                    if (autoStartNextButton.gameObject.activeSelf)
                    {
                        autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                    }
            MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Battle Insufficient Funds", "Insufficient Deposit and Winnings Cash", bonusCashSubtitle.text);

                }

            }
        








    
    public void SetPlayerEndProperties(GameObject player, string name, Sprite dp,Sprite tier,Text score,int userID,bool isScoreSubmitted)
    {
        if (player != null && player.activeSelf)
        {
            
            Debug.Log("SettingPlayerProperties");
            //player.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255,255);
            //if (userID == MPLController.Instance.gameConfig.Profile.id)
            //{
            //    player.gameObject.GetComponent<Image>().color = new Color32(216, 240, 250, 255);
            //}


            PlayerItemController playerItemController = player.gameObject.GetComponent<PlayerItemController>();
            nameText = playerItemController.playerName;
            dpImage = playerItemController.playerDp;
            tierImage = playerItemController.tier;
            scoreText = playerItemController.score;
            playerItemController.ResetResultPlayerProfile();

            if(isScoreSubmitted)
            {
                if(playerItemController.syncLoader.activeSelf)
                {
                    playerItemController.syncLoader.SetActive(false);
                }
            }
            if (userID == MPLController.Instance.gameConfig.Profile.id)
            {
                playerScore.text = score.text;
                playerItemController.EnablePlayerHighlighter(true);
                playerRank.gameObject.SetActive(false);
            }
            else
            {
                playerItemController.EnablePlayerHighlighter(false);
            }
            //    if (playerItemController.winnerObject.activeSelf)
            //{
            //    playerItemController.winnerObject.SetActive(false);
            //}
            if(playerItemController.rewardsParent.activeSelf)
            {
                playerItemController.rewardsParent.SetActive(false);
            }

            if(playerItemController.rankText.gameObject.activeSelf)
            {
                playerItemController.rankText.gameObject.SetActive(false);
            }
            if (!MPLController.Instance.gameConfig.isTierEnabled)
            {
                tierImage.gameObject.SetActive(false);
            }
            else
            {
                
                tierImage.sprite = tier;
                tierImage.gameObject.SetActive(true);
            }

            


            


            dpImage.sprite = dp;


            nameText.text = multiplayerGamesHandler.Truncate(name);
            
            


            // rankText.gameObject.SetActive(true);
            if (multiplayerGamesHandler.userIdToCandidate != null)
            {
                if (multiplayerGamesHandler.userIdToCandidate.ContainsKey(userID))
                {
                    multiplayerGamesHandler.userIdToCandidate[userID].scoreText = scoreText;
                }
            }
            if (MPLController.Instance.IsAsyncGame())
            {
                playerItemController.score.gameObject.SetActive(true);
                //playerItemController.scoreLabel.gameObject.SetActive(true);
                scoreText.text = score.text;
                
            }
            else
            {
                scoreText.gameObject.SetActive(false);
                if (playerItemController.syncLoader.activeSelf)
                {
                    playerItemController.syncLoader.SetActive(false);
                }
                //playerItemController.scoreLabel.gameObject.SetActive(false);
            }
            
               // playerItemController.score.gameObject.SetActive(true);

            if (playerItemController.invalidScore.gameObject.activeSelf)
            {
                playerItemController.invalidScore.gameObject.SetActive(false);
            }
            if (playerItemController.fraudScoreRejctedButton.gameObject.activeSelf)
            {
                playerItemController.fraudScoreRejctedButton.gameObject.SetActive(false);
            }



            //player.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            //player.gameObject.GetComponent<Button>().interactable = false;
        }
    }
    private void DisableSyncLoader(int userId)
    {
        if (TempPlayerProfiles.ContainsKey(userId))
        {
            GameObject player = TempPlayerProfiles[userId];
            if (player != null && player.activeSelf)
            {
                PlayerItemController playerItemController = player.GetComponent<PlayerItemController>();
                playerItemController.syncLoader.SetActive(false);

            }
        }
    }
    public void SetPlayerResultProperties(GameObject player, int rank, double cashWinnings, double tokenWinnings, int score, int id, string extraInfo, bool isCashReward, string extReward)
    {
        PlayerItemController playerItemController = player.gameObject.GetComponent<PlayerItemController>();
        ExtraInfo extraInfo1 = null;
        FraudInfo fraudInfo1 = null;
        if (!string.IsNullOrEmpty(extraInfo))
        {
            extraInfo1 = JsonUtility.FromJson<ExtraInfo>(extraInfo);
            if (!string.IsNullOrEmpty(extraInfo1.fraudInfo))
            {
                fraudInfo1 = JsonUtility.FromJson<FraudInfo>(extraInfo1.fraudInfo);
            }

        }
        if (player.activeSelf && player != null)
        {
            //if (rank == 1)
            //{
            //    playerItemController.winnerObject.SetActive(true);
            //    player.gameObject.GetComponent<Image>().color = new Color32(255, 241, 114, 255);
            //}

            if (playerItemController.score.gameObject.activeSelf)
            {
                playerItemController.score.text = "" + score;
            }
            GameObject rewardsParent = playerItemController.rewardsParent;
            Text rankText = playerItemController.rankText;
            GameObject rewards1 = playerItemController.rewards1;
            GameObject rewards2 = playerItemController.rewards2;
            GameObject plus = playerItemController.plus;
            Text CashAmount = playerItemController.cashWinnings;
            Text TokenAmount = playerItemController.tokenWinnings;
            GameObject syncLoader = playerItemController.syncLoader;

            if (syncLoader.activeSelf)
            {
                syncLoader.SetActive(false);
            }

            playerItemController.rank = rank;
            if (rank != 0)
            {

                rankText.text = rank.ToString();
            }
            else
            {
                rankText.text = "--";
            }

            if (MPLController.Instance.gameConfig.MultiWinners)
            {
                rankText.gameObject.SetActive(true);
            }

            if (isCashReward)
            {
                if (cashWinnings > 0 && tokenWinnings > 0)
                {
                    CashAmount.text = "" + cashWinnings;
                    TokenAmount.text = "" + tokenWinnings;


                    rewardsParent.SetActive(true);
                    rewards1.SetActive(true);
                    rewards2.SetActive(true);
                    plus.SetActive(true);
                }
                else if (cashWinnings == 0 && tokenWinnings == 0)
                {
                    Debug.Log("This player Got No Rewards");
                    rewardsParent.SetActive(false);
                }

                else
                {
                    if (tokenWinnings > 0)
                    {
                        TokenAmount.text = "" + tokenWinnings;
                        CashAmount.text = "" + 0;
                        rewards1.SetActive(false);
                        rewards2.SetActive(true);

                    }
                    else
                    {

                        CashAmount.text = "" + cashWinnings;
                        TokenAmount.text = "" + 0;
                        rewards2.SetActive(false);
                        rewards1.SetActive(true);

                    }


                    rewardsParent.SetActive(true);
                    plus.SetActive(false);
                }
            }
            else
            {
                CashAmount.text = "" + extReward;
                TokenAmount.text = "" + 0;

                rewards2.SetActive(false);
                rewards1.SetActive(true);
                rewardsParent.SetActive(true);
                plus.SetActive(false);
                rewards1.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }




            if (MPLController.Instance.gameConfig.FraudBlockEnabled && extraInfo1 != null && fraudInfo1 != null)


            {
                Debug.Log("sahmil -- >isGamePlayCorrected : " + fraudInfo1.isGamePlayCorrected);
                rewardsParent.SetActive(false);
                if (playerItemController.score.gameObject.activeSelf)
                {
                    playerItemController.score.gameObject.SetActive(false);
                }
                playerItemController.SetGamePlayCorrected(fraudInfo1.isGamePlayCorrected);
                if (fraudInfo1.isGamePlayCorrected)
                {
                    if (!MPLController.Instance.IsAsyncGame())
                    {

                        scoreLabelParent.gameObject.SetActive(false);
                        rankWidgetScoreParent.SetActive(false);
                        gamePlayCancelledObject.SetActive(true);
                        if (playerItemController.syncLoader.activeSelf)
                        {
                            playerItemController.syncLoader.SetActive(false);
                        }
                    }

                }
            }

            if (MPLController.Instance.gameConfig.FraudBlockEnabled)
            {
                //add the new flow here
                if (extraInfo1 != null && extraInfo1.isFraudGamePlay)
                {
                    //player.gameObject.GetComponent<Image>().color = new Color32(240, 240, 240, 255);

                    Debug.Log("sahmil -- >extraInfo1.isGamePlayCorrected : " + fraudInfo1.isGamePlayCorrected + " extraInfo1.blockType : " + fraudInfo1.blockType + " extraInfo1.blockStatus : " + fraudInfo1 + " extraInfo1.blockDuration : " + fraudInfo1.blockDuration + " extraInfo1.fraudDetectionMessage : " + extraInfo1.fraudDetectionMessage);
                    player.gameObject.GetComponent<Button>().interactable = true;
                    rewardsParent.SetActive(false);
                    if (playerItemController.score.gameObject.activeSelf)
                    {
                        playerItemController.score.gameObject.SetActive(false);
                    }
                    playerItemController.SetFraudDetails(id, extraInfo1, fraudPopupController);
                    playerItemController.SetFraudDetected();





                    if (id == MPLController.Instance.gameConfig.Profile.id)
                    {

                        playerRankInfoParent.SetActive(false);
                        viewPolicyButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("View Policy");
                        playerInvalidScoreParent.SetActive(true);
                        if (extraInfo1.blockStatus == "BLOCK")
                        {
                            fraudPopupController.exitButton.onClick.AddListener(() => { GoToBattleRoom(); });
                        }
                        fraudPopupController.Initialise(playerItemController.playerDp.sprite, true, extraInfo1.blockStatus, fraudInfo1.blockType, fraudInfo1.blockDuration, extraInfo1.fraudDetectionMessage);
                        // if (extraInfo1.blockStatus == "BLOCK")
                        {
                           
                            ShowFraudPopup(true);
                        }
                        //playerItemController.invalidScore.onClick.AddListener(() => ShowFraudPopup(true, extraInfo1.fraudDetectionMessage,true));
                    }

                    viewPolicyButton.onClick.AddListener(() => GoToFraudPolicy(true));

                }
            }
            else
            {
                //add the old flow here
                if (extraInfo1 != null && extraInfo1.isFraudGamePlay)
                {
                    //player.gameObject.GetComponent<Image>().color = new Color32(240, 240, 240, 255);
                    viewPolicyButton.onClick.AddListener(() => ShowFraudPopup(true, extraInfo1.fraudDetectionMessage, true));

                    //player.gameObject.GetComponent<Button>().interactable = true;
                    rewardsParent.SetActive(false);
                    if (playerItemController.score.gameObject.activeSelf)
                    {
                        playerItemController.score.gameObject.SetActive(false);
                    }

                    if (id == MPLController.Instance.gameConfig.Profile.id)
                    {
                        playerRankInfoParent.SetActive(false);
                        viewPolicyButtonText.text = "Know More";
                        playerInvalidScoreParent.SetActive(true);

                        playerItemController.invalidScore.onClick.AddListener(() => ShowFraudPopup(true, extraInfo1.fraudDetectionMessage, true));
                    }
                    else
                    {
                        if (extraInfo1.blockStatus == "WARNING")
                            playerItemController.invalidScore.onClick.AddListener(() => ShowFraudPopup(true, MPLController.Instance.gameConfig.FraudWarningMessage, false));
                        else if (extraInfo1.blockStatus == "BLOCK")
                            playerItemController.invalidScore.onClick.AddListener(() => ShowFraudPopup(true, MPLController.Instance.gameConfig.FraudBlockMessage, false));
                    }
                    playerItemController.invalidScore.gameObject.SetActive(true);



                }
            }

        }
        Debug.Log("Players are set");
    }

    
    void ShowBattleEndButtons()
    {
        buttonPanel.SetActive(true);
        if (!canIPlayAgain || MPLController.Instance.gameConfig.GameId == 1000040)
        {
            
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
            

            exitButton.SetActive(true);
            exitText.SetActive(true);
        }
        else
        {
            
                if (!mPLController.gameConfig.IsKnockoutLobby)
                {
                    if (MPLController.Instance.gameConfig.SingleEntryBattle)
                    {
                        autoStart.SetActive(true);
                        autoStartNextButton.gameObject.SetActive(true);
                        cancelAutoStartButton.gameObject.SetActive(true);
                        autoStartController.lobbyCurrencyImage.gameObject.SetActive(false);
                        autoStartController.lobbyAmountText.gameObject.SetActive(false);
                        findNewController.lobbyAmountText.gameObject.SetActive(false);
                        findNewController.lobbyCurrencyImage.gameObject.SetActive(false);
                        if (autoStartNextButton.gameObject.activeSelf)
                        {
                            autoStartNextButton.gameObject.GetComponent<FillButton>().CancellLoader += OnCancelFill;
                            autoStartNextButton.gameObject.GetComponent<FillButton>().AutoStartBattle += OnAutoStartBattle;
                        }

                        if(disableAutoStart)
                        {
                            autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                        }
                    }
                    else
                    {
                        if (MPLController.Instance.gameConfig.IsAutoStartEnabled)
                        {
                            autoStart.SetActive(true);
                            autoStartNextButton.gameObject.SetActive(true);
                            cancelAutoStartButton.gameObject.SetActive(true);
                            if (autoStartNextButton.gameObject.activeSelf)
                            {
                                autoStartNextButton.gameObject.GetComponent<FillButton>().CancellLoader += OnCancelFill;
                                autoStartNextButton.gameObject.GetComponent<FillButton>().AutoStartBattle += OnAutoStartBattle;
                            }
                            if (disableAutoStart)
                            {
                                autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                            }
                        }
                        else
                        {
                            findnewplayerbutton.gameObject.SetActive(true);
                        }
                        if (MPLController.Instance.gameConfig.IsUpsellEnabled)
                        {
                            if (!String.IsNullOrEmpty(MultiplayerGamesHandler.Instance.nextLobbyConfig))
                            {
                                if (amITheWinner)
                                {
                                    upsellController.lobbyAmountText.text = nextConfig.EntryFee.ToString();
                                    upsellController.nextLobbyAmountText.text = nextConfig.WinningAmount.ToString();
                                    if (nextConfig.EntryFee > cashAmt)
                                    {
                                        hasBalanceAvailableForUpsell = false;
                                    }
                                    else
                                    {
                                        hasBalanceAvailableForUpsell = true;
                                    }
                                    Debug.Log("IsUpsellEnabled enabled");
                                    upsellButton.gameObject.SetActive(true);
                                    StartCoroutine(ResetUpsellButton());
                                    if (autoStartNextButton.gameObject.activeSelf)
                                    {
                                        autoStartNextButton.gameObject.GetComponent<FillButton>().CancelFill();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!MPLController.Instance.IsThirdPartyGame() && isBattleAgainEnabled && isIt1v1)
                            {
                                battleAgainButton.SetActive(true);
                            }
                        }
                    }


                }
                else
                {
                    knockOutOkayButton.SetActive(true);
                }
            
            
        }
        buttonPanel.SetActive(true);
    }

    void OnAutoStartBattle()
    {
        FindMatchAgain(false);
    }

    void OnCancelFill()
    {
       autoStart.SetActive(false);
       cancelAutoStartButton.gameObject.SetActive(false);
       autoStartNextButton.gameObject.SetActive(false);
       findnewplayerbutton.gameObject.SetActive(true);
    }
    void BalanceCheck()
    {


        if (MPLController.Instance.gameConfig.EntryFee == 0)
        {
            hasBalanceAvailable = true;
        }
        else
        {
            if (MPLController.Instance.gameConfig.EntryCurrency == "TOKEN")
            {
                if (tokenAmt > MPLController.Instance.gameConfig.EntryFee)
                {
                    hasBalanceAvailable = true;
                }
                else
                {
                    hasBalanceAvailable = false;
                    bonusCashSubtitle.text = string.Format(mPLController.localisationDetails.GetLocalizedText("Earn {0} or more Tokens to keep playing in MPL Battles!"), Math.Abs(mPLController.gameConfig.EntryFee - tokenAmt));
                    bonusCashPopTitleText.text = mPLController.localisationDetails.GetLocalizedText("Insufficient Tokens");
                    
                        Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_token);
                    
                }
            }
            else
            {
                
                if (!MPLController.Instance.gameConfig.ApplyBonusLimit)
                {
                    availableCashAmt = bonusCashAmt + winningsCashAmt + depositCashAmt;

                }
                else
                {
                    bonusLimit = MPLController.Instance.gameConfig.BonusLimit;
                    if (bonusLimit == 0)
                    {
                        bonusCashCannotBeUsed.SetActive(true);
                    }
                    else
                    {
                        bonusCashCanBeUsedText.text = string.Format(mPLController.localisationDetails.GetLocalizedText("Upto ₹{0} can be used from Bonus Cash"), bonusLimit);
                        bonusCashCanBeUsed.SetActive(true);
                    }

                    
                    if (bonusCashAmt < bonusLimit)
                    {
                        availableCashAmt = winningsCashAmt + depositCashAmt + bonusCashAmt;
                    }
                    else
                    {
                        availableCashAmt = winningsCashAmt + depositCashAmt + bonusLimit;
                    }
                }

                if (availableCashAmt >=double.Parse(findNewController.lobbyAmountText.text))
                {
                    hasBalanceAvailable = true;
                }
                else
                {
                    hasBalanceAvailable = false;
                    bonusCashSubtitle.text = "Add " + Math.Abs(double.Parse(findNewController.lobbyAmountText.text) - availableCashAmt) + " or more Cash to keep playing in MPL Battles!";
                    bonusCashPopTitleText.text = "Insufficient Cash";
                    
                    Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
                    Session.Instance.SetMoneyToAdd(Math.Ceiling(Math.Abs((double.Parse(findNewController.lobbyAmountText.text) - availableCashAmt))));
                }
            }
        }
        MPLController.Instance.SetAvailableBalance(availableCashAmt);
        MPLController.Instance.SetAvailableTokenBalance(tokenAmt);
        MPLController.Instance.SetIsBalanceAvailable(hasBalanceAvailable);
    }

    void SetButtonProperties()
    {
        if (MultiplayerGamesHandler.Instance.isItUpsell)
        {
            findNewController.lobbyCurrencyImage.gameObject.SetActive(true);
            findNewController.lobbyCurrencyImage.sprite = cashImage;
            findNewController.lobbyAmountText.text = upsellController.lobbyAmountText.text;
            autoStartController.lobbyCurrencyImage.sprite = findNewController.lobbyCurrencyImage.sprite;
            autoStartController.lobbyAmountText.text = findNewController.lobbyAmountText.text;
            battleAgainController.lobbyCurrencyImage.sprite = cashImage;
            battleAgainController.lobbyAmountText.text = upsellController.lobbyAmountText.text;

        }
        else
        {

            if (mPLController.gameConfig.EntryFee != 0)
            {

                findNewController.lobbyAmountText.text = "" + mPLController.gameConfig.EntryFee;

                Debug.Log("MPL Currency Multi:" + mPLController.gameConfig.EntryCurrency);
                if (mPLController.gameConfig.EntryCurrency == "TOKEN")
                {
                    findNewController.lobbyCurrencyImage.sprite = tokenImage;

                }
                else
                {

                    findNewController.lobbyCurrencyImage.sprite = cashImage;


                }
                autoStartController.lobbyCurrencyImage.sprite = findNewController.lobbyCurrencyImage.sprite;
                autoStartController.lobbyAmountText.text = findNewController.lobbyAmountText.text;

                battleAgainController.lobbyCurrencyImage.sprite = findNewController.lobbyCurrencyImage.sprite;
                battleAgainController.lobbyAmountText.text= findNewController.lobbyAmountText.text;
            }
            else
            {
                findNewController.lobbyAmountText.text = "FREE";
                autoStartController.lobbyAmountText.text = "FREE";
                battleAgainController.lobbyAmountText.text = "FREE";

                findNewController.lobbyCurrencyImage.gameObject.SetActive(false);

                autoStartController.lobbyCurrencyImage.gameObject.SetActive(false);
                battleAgainController.lobbyCurrencyImage.gameObject.SetActive(false);
            }


        }


    }

    IEnumerator ResetUpsellButton()
    {
        var horizontalLayOut = upsellButton.GetComponentInChildren<HorizontalLayoutGroup>();
        if (horizontalLayOut == null) yield return null;
        upsellButton.gameObject.SetActive(false);
        var spacingVal = horizontalLayOut.spacing <= 3.78f ? 4f : 3.78f;
        yield return new WaitForEndOfFrame();
        upsellButton.gameObject.SetActive(true);
        horizontalLayOut.spacing = spacingVal;
    }

    private IEnumerator PlayFireWorks(GameObject[] fireWorksList)
    {
        int index = 0;
        int size = fireWorksList.Length-1;
        while(true)
        {


            //if (index == 0)
            //{
            //    fireWorksList[size].gameObject.SetActive(false);

            //}
            //else
            //{
            //    fireWorksList[index - 1].gameObject.SetActive(false);
            //}
            if (index == 0)
            {
                fireWorksList[size - 1].gameObject.SetActive(false);

            }
            else if (index == 1)
            {
                fireWorksList[size].gameObject.SetActive(false);

            }
            else
            {
                fireWorksList[index - 2].gameObject.SetActive(false);
            }

            fireWorksList[index++].gameObject.SetActive(true);
            if (index > size)
            {
                index = 0;
            }

            yield return new WaitForSeconds(0.6f);
            if (index == 0)
            {
                fireWorksList[size - 1].gameObject.SetActive(false);

            }
            else if (index == 1)
            {
                fireWorksList[size].gameObject.SetActive(false);

            }
            else
            {
                fireWorksList[index - 2].gameObject.SetActive(false);
            }
            fireWorksList[index++].gameObject.SetActive(true);
            if (index > size)
            {
                index = 0;
            }
          

            yield return new WaitForSeconds(0.6f);
          
           


        }
    }

    private void AdjustResultListContent(int childCount)
    {
        if (isPortrait)
            return;
        var v_width = playerListRectTransform.rect.width;
        if (childCount>3)
        {
            playerListRectTransform.sizeDelta = new Vector2(v_width, NORMAL_HEIGHT);
            playerListRectTransform.anchoredPosition = new Vector2(playerListRectTransform.anchoredPosition.x, NORMAL_Y_POS);
        }
        else
        {
            var v_height = ITEM_HEIGHT * childCount;
            playerListRectTransform.sizeDelta = new Vector2(v_width, v_height);
            playerListRectTransform.anchoredPosition = new Vector2(playerListRectTransform.anchoredPosition.x, AlIGNED_Y_POS);
        }
    }


}

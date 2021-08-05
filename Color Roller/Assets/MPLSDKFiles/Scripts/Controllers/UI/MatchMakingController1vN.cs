using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Sfs2X.Entities;

public class MatchMakingController1vN : MonoBehaviour
{
    public MPLController mPLController;
    public MultiPlayerCanvasController1vN multiplayerCanvasController1vN;
    public BattleEndController1vN battleEndManager1vN;
    public GameObject localPlayer, matchFailed;
    public GameObject challengeWaitingPopup,knockOutDisclaimerText;
    public Text startingBattleText;
    public Text timerText;
    public Image loaderBG;

    public GameObject prizeObject,startingBattleObject;
    
    public GameObject goToLobby;
    public Text loadingSubtitleText;
    
    
    private MultiplayerGamesHandler multiplayerGamesHandler;
    public bool disconnectedOnPause;
    public Button knockoutLobbyButton;
    public Transform playerListParent, prizeObjectParent;
    public GameObject timerParent,battleMatchedObject,knockoutErrorObject;
    public Image timerLoaderImage;
  

    public List<GameObject> rowParents = new List<GameObject>();
    public GameObject contentItem;
    public GameObject viewPort;
    public bool isPortrait;
    public ScrollRect playersScrollView;
    public Transform scrollViewContent;

    public Transform prizeParentWithAd;
    
    public GameObject prizeObjectLandscapelWithAd;
    public GameObject blurBG;

    public Image fairPlayImage;

    #region FestiveTheme
  
    public GameObject festiveThemeDecos;


    public GameObject festiveThemeChristmas;
    public ParticleSystem festiveThemeSnowFallEffect;

    #endregion

    private MPLSdkBridgeController _mPLSdkBridgeController;
    public MPLSdkBridgeController mPLSdkBridgeController
    {
        get
        {
            if (_mPLSdkBridgeController == null)
            {
                _mPLSdkBridgeController = MPLSdkBridgeController.Instance;
            }
            return _mPLSdkBridgeController;
        }
        set
        {
            _mPLSdkBridgeController = value;
        }
    }

    [ContextMenu("Debug snow fall")]
    public void DebugSnowFall()
    {
        festiveThemeSnowFallEffect.gameObject.SetActive(true);
    }
    public void OnEnable()
    {
        blurBG.SetActive(true);

        mPLSdkBridgeController = MPLSdkBridgeController.Instance;
        multiplayerGamesHandler = MultiplayerGamesHandler.Instance;
        multiplayerGamesHandler.AddSfxListeners();
        SmartFoxManager.Instance.OnRoomJoinFailed += OnRoomJoinFailed;
        SmartFoxManager.Instance.ConnectionFailed += ConnectionLost;
        SmartFoxManager.Instance.ConnectionLost += ConnectionLost;


        multiplayerGamesHandler.OverrideGameConfig();
        if (mPLSdkBridgeController != null)
        {
            mPLSdkBridgeController.mPLGameConfig = MPLController.Instance.gameConfig;
        }
        multiplayerCanvasController1vN.DisableWentInBackgroundPopup();
        festiveThemeChristmas.gameObject.SetActive(false);
        festiveThemeSnowFallEffect.gameObject.SetActive(false);
        festiveThemeDecos.gameObject.SetActive(false);
      
        InitAnims();
    }
   
    public void StopMatchMaking(bool showTryAgain)
    {
        gameObject.SetActive(false);
        
        SmartFoxManager.Instance.mmRetryAttempt = 0;
        if (showTryAgain)
        {
            multiplayerGamesHandler.ResetMatchMakingRetriesValue(false);
            matchFailed.SetActive(true);
            goToLobby.gameObject.SetActive(false);
            
        }
        else
        {
            multiplayerCanvasController1vN.isItConnectionLostPopup = true;
            multiplayerCanvasController1vN.connectionLost.SetActive(true);
        }
    }

    private void OnDisable()
    {
        
        SmartFoxManager.Instance.OnRoomJoinFailed -= OnRoomJoinFailed;
        SmartFoxManager.Instance.ConnectionFailed -= ConnectionLost;
        SmartFoxManager.Instance.ConnectionLost -= ConnectionLost;

        multiplayerGamesHandler.RemoveSfxListeners();

        if (matchFailed.activeSelf)
        {
            matchFailed.SetActive(false);
        }
    }

    bool stoppdAfterHacked;
    private void Update()
    {
        if (!stoppdAfterHacked && MPLController.Instance.hackDetector.Hacked())
        {
            Debug.Log("Hack detected, disconnecting");
            stoppdAfterHacked = true;
            multiplayerGamesHandler.maximumMatchMakingRetries = 0;

            SmartFoxManager.Instance.mmRetryAttempt = 0;
            OnRoomJoinFailed(SmartFoxManager.SmartFoxManagerReasons.Hacked);
            SmartFoxManager.Instance.SetConnectionFlags("Disconnected Due To Hacking");
            multiplayerCanvasController1vN.Disconnect(true, MPLGameEndReason.GameEndReasons.DISCONNECTED_DUE_TO_HACKING);
        }

        
    }


    
    
    
    IEnumerator AnimateMandala(Transform mandalaTransform)
    {
        while (true)
        {


            float rotationsPerMinute = 10.0f;

            mandalaTransform.transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
        }
        yield return null;
      
    }

  

    public void OnRoomJoinFailed(SmartFoxManager.SmartFoxManagerReasons reason)
    {

        // timerText.gameObject.SetActive(false);
       
        if (multiplayerGamesHandler.cr != null)
        {
            StopCoroutine(multiplayerGamesHandler.cr);
        }
        

        
        multiplayerCanvasController1vN.StopGameStartTimer();

        
        multiplayerGamesHandler.isMatchFound = false;
        
        int maxRetries;
        if(reason==SmartFoxManager.SmartFoxManagerReasons.LoginError)
        {

            multiplayerGamesHandler.maximumMatchMakingRetries = 0;
            SmartFoxManager.Instance.mmRetryAttempt = 0;
            MPLController.Instance.SetPlayerProperties();
        }
        if (MPLController.Instance.gameConfig.MaxMatchMakingRetries == 0)
        {
            maxRetries = 3;
        }
        else
        {
            maxRetries = MPLController.Instance.gameConfig.MaxMatchMakingRetries;
        }
        if (!MPLController.Instance.gameConfig.IsKnockoutLobby)
        {
            if (multiplayerGamesHandler.maximumMatchMakingRetries != 0)
            {
                if (multiplayerGamesHandler.maximumMatchMakingRetries == maxRetries - 1)
                {
                    goToLobby.SetActive(true);
                }
                Debug.Log("Retrying matchmaking=" + multiplayerGamesHandler.maximumMatchMakingRetries);
                SmartFoxManager.Instance.mmRetryAttempt = Mathf.Abs(maxRetries - multiplayerGamesHandler.maximumMatchMakingRetries);
                multiplayerCanvasController1vN.FindNewPlayer();

            }
            else
            {
                multiplayerGamesHandler.SubmitBattleMatchingEnded(reason.ToString(), false);
                multiplayerGamesHandler.SubmitBattleStartedEvent(false, reason.ToString());
                Debug.Log("Retries over");

                StopMatchMaking(true);

                multiplayerGamesHandler.TotalTime = 0;
            }
        }
        

       
        
        //  }

       
    }



    public void InitAnims()
    {
        if(MPLController.Instance.gameConfig.FestiveThemeOn)
        {
         
            festiveThemeChristmas.gameObject.SetActive(true);
            festiveThemeSnowFallEffect.gameObject.SetActive(true);
            festiveThemeSnowFallEffect.Play();

            festiveThemeDecos.gameObject.SetActive(true);

        }

            
        
        multiplayerGamesHandler.SetUIReferences(goToLobby, timerText, startingBattleText, playerListParent, prizeObjectParent, prizeObject, localPlayer, loadingSubtitleText, challengeWaitingPopup, startingBattleObject, timerParent, battleMatchedObject,  rowParents,  contentItem,  viewPort,  isPortrait, playersScrollView, scrollViewContent, prizeParentWithAd, prizeObjectLandscapelWithAd,blurBG,knockoutErrorObject,timerLoaderImage,knockoutLobbyButton,loaderBG, knockOutDisclaimerText,fairPlayImage);


    }
    void ConnectionLost(MPLGameEndReason.GameEndReasons reason)
    {

        //Event
        if (MPLController.Instance.IsThirdPartyGame())
        {
            Debug.Log("Connection lost sdk=" + reason.ToString());


            Debug.Log("multiplayerCanvasController1vN.onStartBattleCalled=" + multiplayerCanvasController1vN.onStartBattleCalled);
            Debug.Log("!multiplayerGamesHandler.forceDisconnect=" + multiplayerGamesHandler.forceDisconnect);

            if (reason == MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND)
            {
                multiplayerCanvasController1vN.EnableWentInBackgroundPopup();
            }
            else
            {
                if (!multiplayerCanvasController1vN.onStartBattleCalled && !multiplayerGamesHandler.forceDisconnect)
                {
                    multiplayerCanvasController1vN.errorTitle.text = MPLController.Instance.gameConfig.ICLostTitle;
                    multiplayerCanvasController1vN.errorSubtitle.text = MPLController.Instance.gameConfig.ICLostMessage;
                    multiplayerCanvasController1vN.isItConnectionLostPopup = true;
                    multiplayerCanvasController1vN.connectionLost.SetActive(true);
                }
            }
        }
        else
        {
            if (multiplayerCanvasController1vN.gameStartTimer.Enabled)
            {
                //Event
                multiplayerGamesHandler.SubmitBattleStartedEvent(false, reason.ToString());
            }



            StopMatchMaking(false);
        }
    }

    Thread _thread;
    MPLGameEndReason.GameEndReasons reason;
    public void Disconnect(MPLGameEndReason.GameEndReasons reason)
    {
        this.reason = reason;
        _thread = new Thread(DisconnectOnSeparateThread);
        _thread.Start();




    }

    void DisconnectOnSeparateThread()
    {
        SmartFoxManager.Instance.Disconnect(reason);
        _thread.Join();
    }

    void OnApplicationPause(bool status)
    {
        if (status == true)
        {

            multiplayerCanvasController1vN.StopGameStartTimer();
            //UNCOMMENT
            //SmartFoxManager.Instance.Disconnect();

            Debug.Log("OnApplicationPaue=MatchMakingController1vN");
            disconnectedOnPause = true;
            //TODO:
            Session.Instance.userMinimised = "YES";
            if (multiplayerCanvasController1vN.onStartBattleCalled || multiplayerGamesHandler.isMatchFound)
            {
                SmartFoxManager.Instance.SetConnectionFlags("Went In BackGround After the game started");
                multiplayerCanvasController1vN.Disconnect(true, MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND);
                multiplayerCanvasController1vN.errorTitle.text = mPLController.localisationDetails.GetLocalizedText("Game Ended!");
                multiplayerCanvasController1vN.errorSubtitle.text = mPLController.localisationDetails.GetLocalizedText("You got disconnected as MPL app was minimized. Please do not minimize app during a game.");
                mPLSdkBridgeController.SendEventToGame("userGameEnd");
            }
            else
            {
                SmartFoxManager.Instance.SetConnectionFlags("Went In BackGround");
                multiplayerCanvasController1vN.Disconnect(false, MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND);
            }

            if (multiplayerGamesHandler.cr != null)
            {
                StopCoroutine(multiplayerGamesHandler.cr);
            }
            multiplayerGamesHandler.maximumMatchMakingRetries = 0;
            SmartFoxManager.Instance.mmRetryAttempt = 0;
           
                if (!mPLController.gameConfig.IsKnockoutLobby)
                {
                    OnRoomJoinFailed(SmartFoxManager.SmartFoxManagerReasons.WentInBackgroundMM);
                }
            
        }
        else
        {
            if(mPLController.gameConfig.IsKnockoutLobby)
            {
                SmartFoxManager.Instance.mmRetryAttempt = 0;
                multiplayerCanvasController1vN.FindNewPlayer();
            }
        }
    }
    



}


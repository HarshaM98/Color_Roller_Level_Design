using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class InteractiveFTUEController : MonoBehaviour
{
    // Start is called before the first frame update
    enum TutorialState
    {
        Start,
        InGame,
        End
    }

    TutorialState tutorialState;
    string tutorialEndReason = "";
    
    public static string EVENT_TRAINING_STARTED = "Training Started";
    public static string EVENT_TRAINING_ENDED = "Training Ended";
    public GameObject startScreen, inGameScreen, endScreen,newBattleButton,freeBattleButton;
    public Image backGround, logoType, logoTypeEnd;
    public Text entryFee;
    public Text countDownText,freeBattleText;
    public GameObject panel;
    public IEnumerator cr;
    public GameObject balancePopup;
    public Text balanceText;
    bool isItTrainingAgain=false;

    public Button endTraining;
    
    void Start()
    {
        
    }
   
    public void SetTutorialEndReason(string tutorialEndReason)
    {
        this.tutorialEndReason = tutorialEndReason;
    }
    void EnableStartScreen(bool state)
    {
        if (state)
        {
            tutorialState = TutorialState.Start;
        }
        backGround.gameObject.SetActive(state);
        panel.SetActive(state);
        startScreen.SetActive(state);
    }
    private void OnEnable()
    {
        
        Session.Instance.OnTutorialStart += TutorialStart;
        Session.Instance.OnTutorialEnd += EndTutorial;
        GetTutorialInfo();
        StartTutorial();
    }
    void StartTutorial()
    {
        Session.Instance.SetInteractiveTutorial(true);
        EnableEndScreen(false);
        EnableStartScreen(true);
        Debug.Log("Loading async tutorial");
        MPLController.Instance.LoadGameSceneFromAssetBundle(MPLController.Instance.gameConfig.GameId);
        StartCountdown();
    }
    private void OnDisable()
    {

        
        Session.Instance.OnTutorialStart -= TutorialStart;
        Session.Instance.OnTutorialEnd -= EndTutorial;
    }

    void EndTutorial()
    {
        tutorialEndReason = "Training Completed";
        TutorialEnd();
    }
    IEnumerator CountDown(int count)
    {
        countDownText.text =count.ToString();
        countDownText.gameObject.SetActive(true);
        while (count >= 0)
        {
            
            yield return new WaitForSeconds(1);
            countDownText.text = count.ToString();

            count--;
        }
        StartTraining();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void EndTraining()
    {
        tutorialEndReason = "End Training Button";
        TutorialEnd();
        Session.Instance.EndGame(MPLGameEndReason.GameEndReasons.END_TRAINING, "Ending Training");
    }
    void GetTutorialInfo()
    {
        
        


         
                    backGround.GetComponent<Image>().sprite = MPLController.Instance.backgroundImage;

                
               
                    logoType.sprite = MPLController.Instance.gameLogoTypeSprite;
                    logoTypeEnd.sprite = logoType.sprite;
               

          
        
       
    }
    void StopCountdown()
    {
        if (cr != null)
        {
            StopCoroutine(cr);
        }
    }
    void StartCountdown()
    {
        StopCountdown();
        cr = CountDown(3);
        StartCoroutine(cr);

    }
    public void StartTraining()
    {
        StopCountdown();
        countDownText.gameObject.SetActive(false);
        Debug.Log("activating scene async tutorial");
        StartCoroutine(MPLController.Instance.StartAsyncLoadedScene());
    }
    void TutorialStart()
    {
        EnableStartScreen(false);
        EnableInGameScreen(true);
        SendTrainingStartEvent(MPLController.Instance.GetTutorialEntryPoint());
    }
    void SendTrainingStartEvent(string entryPoint)
    {
        TrainingStartedEventProperties trainingStartedEventProperties = new TrainingStartedEventProperties(MPLController.Instance.gameConfig.GameName, MPLController.Instance.gameConfig.GameId.ToString(), entryPoint);
        MPLSdkBridgeController.Instance.SubmitEvent(EVENT_TRAINING_STARTED, trainingStartedEventProperties.ToString());
    }
    void SendTrainingEndedEvent(string endReason)
    {
        TrainingEndedEventProperties trainingEndedEventProperties = new TrainingEndedEventProperties(MPLController.Instance.gameConfig.GameName, MPLController.Instance.gameConfig.GameId.ToString(), endReason);
        MPLSdkBridgeController.Instance.SubmitEvent(EVENT_TRAINING_ENDED, trainingEndedEventProperties.ToString());
    }
    void TutorialEnd()
    {
        EnableEndScreen(true);
        EnableInGameScreen(false);
        freeBattleButton.SetActive(false);
        newBattleButton.SetActive(false);
        if (MPLController.Instance.GetTutorialEntryPoint()=="Game Over Screen" || MPLController.Instance.GetTutorialType()==MPLController.MPLTutorialType.GameEndScreen||MPLController.Instance.gameConfig.IsInteractiveTrainingLobby)
        {
            if (MPLController.Instance.gameConfig.EntryFee == 0)
            {
                freeBattleText.text = "Play a Free Battle";
                freeBattleButton.SetActive(true);
                if(MPLController.Instance.GetTutorialEntryPoint() == "Game Over Screen" || MPLController.Instance.GetTutorialType() == MPLController.MPLTutorialType.GameEndScreen)
                {
                    freeBattleButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    freeBattleButton.gameObject.GetComponent<Button>().onClick.AddListener(() => StartNewMatchFromResult());
                }
            }
            else
            {
                entryFee.text = MPLController.Instance.gameConfig.EntryFee.ToString();
                newBattleButton.SetActive(true);
            }
            
        }
        else
        {
            if(MPLController.Instance.gameConfig.Is1v1)
            {
                freeBattleText.text = "Continue To Battle";
            }
            else
            {
                freeBattleText.text = "Continue To Tournament";
            }
            freeBattleButton.SetActive(true);
           
        }
        SendTrainingEndedEvent(tutorialEndReason);
    }
    void EnableInGameScreen(bool status)
    {
        if(status)
        {
            tutorialState = TutorialState.InGame;
        }
        inGameScreen.SetActive(status);
        backGround.gameObject.SetActive(!status);
        panel.SetActive(!status);
    }
    void EnableEndScreen(bool status)
    {
        if (status)
        {
            tutorialState = TutorialState.End;
        }
        endScreen.SetActive(status);
        backGround.gameObject.SetActive(status);
        panel.SetActive(status);
    }
    public void StartBattle()
    {
        Session.Instance.SetInteractiveTutorial(false);
        MPLController.Instance.StartGame();
        this.gameObject.SetActive(false);
    }
    public void TrainAgain()
    {
        MPLController.Instance.SetTutorialEntryPoint("Play Training Again");
        StartTutorial();
    }
    public void QuitUnity()
    {
        StopCountdown();
        Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_quit);
        Session.Instance.Quit();
    }
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            if(tutorialState ==TutorialState.InGame)
            {
                EndTraining();
            }
                 
        }
    }
    public void StartNewMatchFromResult()
    {

        if (!MPLController.Instance.IsBalanceAvailable())
        {
            if (MPLController.Instance.gameConfig.EntryCurrency.ToUpper() == "CASH")
            {
                balanceText.text = string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("Add ₹{0} or more Cash Balance to keep playing in Multiplayer Battles!"), Math.Ceiling(Math.Abs(double.Parse(entryFee.text) - MPLController.Instance.GetAvailableBalance())));
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_money_insufficient);
                Session.Instance.SetMoneyToAdd(Math.Ceiling(Math.Abs(double.Parse(entryFee.text) - MPLController.Instance.GetAvailableBalance())));
            }
            else
            {
                balanceText.text = string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("Add {0} or more Tokens to keep playing in Multiplayer Battles!"), Math.Ceiling(Math.Abs(double.Parse(entryFee.text) - MPLController.Instance.GetAvailableTokenBalance())));
                Session.Instance.SetQuitReason(Session.MPLQuitReason.battle_add_token);
            }
            balancePopup.SetActive(true);

        }
        else
        {
            Session.Instance.SetInteractiveTutorial(false);
            MPLController.Instance.mplSdkObject.SetActive(true);
            MPLSdkBridgeController.Instance.StartMultiplayerGame();
            this.gameObject.SetActive(false);

        }
    }
        
  
    public void AddMoney()
    {
        Session.Instance.Quit();
    }
}

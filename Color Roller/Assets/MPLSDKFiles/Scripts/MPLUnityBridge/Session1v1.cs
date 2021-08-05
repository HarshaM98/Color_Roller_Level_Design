using System.Collections.Generic;
using UnityEngine;
using Sfs2X.Entities.Variables;
using System.Threading;
using System;
using System.Timers;

public class Session1v1: Session
{
    private float scoreSendingInterval;
    private float lastSyncedScoreTime;
    private System.Timers.Timer gameplayTimer;

    /**************************************** Constructors & Destructor ****************************************/
    static Session1v1() { }

    public Session1v1()
    {      
        gameplayTimer = new System.Timers.Timer(1000);
        gameplayTimer.Elapsed += HandleGamePlayTimer;
    }

  
    public override void InitData()
    {
        base.InitData();
        //vars.Clear();

       
                scoreSendingInterval = 0f;
            

        
        
        Debug.Log("MPL: Score Sync Interval for game id=" + mplController.gameConfig.GameId+"="+ scoreSendingInterval);
        AudioListener.volume = 1f;
    }
   
    public override void UpdateData(string data)
    {
        //base.UpdateData(data);
        SendData(data);
    }
    void SendData(string data)
    {
        List<UserVariable> vars = new List<UserVariable>();
        vars.Add(new SFSUserVariable("updatedData", data));
        vars.Add(new SFSUserVariable("mplUserId", MPLController.Instance.gameConfig.Profile.id));
        Debug.Log("Sending Data");
        SmartFoxManager.Instance.SendUserVariables(vars);
    }
    public override SI GetSessionInfo<SI>()
    {
        Debug.LogError("StartGameplayTimer called");
        StartGameplayTimer();
        //throw new NotImplementedException();
        return base.GetSessionInfo<SI>();
    }

    public override List<MPLDisableType> GetObjectsToDisable()
    {
        return new List<MPLDisableType>() { MPLDisableType.PAUSE_BUTTON, MPLDisableType.ON_ESCAPE };
    }

    void SendScore(int scoreToSend)
    {
        if ((mplController.timeSpent - lastSyncedScoreTime) <= scoreSendingInterval)
        {
            return;
        }

        lastSyncedScoreTime = mplController.timeSpent;

        List<UserVariable>vars = new List<UserVariable>();
        vars.Add(new SFSUserVariable("score", scoreToSend));
        vars.Add(new SFSUserVariable("mplUserId", MPLController.Instance.gameConfig.Profile.id));
        SmartFoxManager.Instance.SendUserVariables(vars);
    }

    void SendField(string fieldName, double valueToSend)
    {
        ///*if ((mplController.timeSpent - lastSyncedScoreTime) <= scoreSendingInterval)
        //{
        //    return;
        //}*/

        ////lastSyncedScoreTime = mplController.timeSpent;

        ////Debug.Log(fieldName + valueToSend);
        //List<UserVariable> vars = new List<UserVariable>();
        ////Debug.Log("Send Field Vars" + vars);
        
        //vars.Add(new SFSUserVariable(fieldName, valueToSend));
        //SmartFoxManager.Instance.SendUserVariables(vars);
    }

    Thread _thread;
    void SubmitResultToSfx()
    {
        //Task.Factory.StartNew(() => SmartFoxManager.Instance.SubmitScore(scoreToSend));
        _thread = new Thread(SubmitScoreOnSeparateThread);
        _thread.Start();
    }

    void SubmitScoreOnSeparateThread()
    {
        SmartFoxManager.Instance.SubmitResult(this.result);
        _thread.Join();
    }

    public override void SubmitResult(SessionResult result)
    {
        if (this.result != null)
        {
            Debug.Log("MPL: 1v1 result already submitted, can't submit result");
            return;
        }

        AudioListener.volume = 0f;

        StopGameplayTimer();
        this.result = CleanSubmitScore(result, false);
     
        lastScoreLogAddedTime = gameplayDuration + mplController.gameConfig.SequenceInterval;
        AddScoreLog(score - randomAdder);
        result.SetSequenceList(scoreLog);
        /*  if (mplController.gameConfig.GameId == 1000049)
          {
              result.SetSequenceList(chessTimerLog);
          }*/
        if (result.GameEndReason == MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND.ToString() || result.GameEndReason == Session.Instance.GetReasonString(MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND))
        {
           
            InvokeWentInBG();
        }

        this.result = FraudCheck(this.result);
        Debug.Log("MPL: 1v1 submitting result to SmartFoxManager = " + this.result);
        SubmitResultToSfx();
    }
    public override void InvokeWentInBG()
    {
        base.InvokeWentInBG();
    }

    public override void UpdateField(string fieldName, double previousField, double newField)
    {
        if (this.result != null)
        {
            Debug.Log("MPL: 1v1 result already submitted, can't update field");
            return;
        }
       

        base.UpdateField(fieldName, previousField, newField);
        SendField(fieldName, newField);
    }

    public override void UpdateScore(int previousScore, int score)
    {
        if (this.result != null)
        {
            Debug.Log("MPL: 1v1 result already submitted, can't update score");
            return;
        }

        base.UpdateScore(previousScore, score);
        SendScore(GetScore());
    }

    //public override void PauseSession()
    //{
    //    if (this.result != null)
    //    {
    //        Debug.Log("MPL: 1v1 result already submitted, can't pause");
    //        return;
    //    }

    //    Debug.Log("MPL: 1v1 session can't be paused: ending game");

    //    //UNCOMMENT
    //    SubmitResult(new SessionResult("", 0, 0, this.score, MPLGameEndReason.USER_QUIT));
    //    EndGame(MPLGameEndReason.PAUSE_TIME_LIMIT_EXCEEDED, "Youuuu Quit");
    //}

    public override void ForceEndGame(MPLGameEndReason.GameEndReasons reason, string message, SessionResult result)
    {
        if (this.result != null)
        {
            Debug.Log("MPL: 1v1 result already submitted, can't ForceEndGame");
            return;
        }

        result.Score = GetScore();
        result.GameplayDurationSDK = gameplayDuration;
        //result.GameEndReason = reason.ToString();
        Debug.Log("MPL: Forcefully ending game with score = " + GetScore() + " : "+ result.Score + " : " + gameplayDuration);
        SubmitResult(result/*new SessionResult("", 0, 0, GetScore(), reason)*/);
        EndGame(reason, message);
        //mplController.StartCoroutine(Util.WaitAndExecute(2, () =>
        //{
        //    EndGame(reason, message);
        //}));
    }

    public override void FraudDetected(string type, string proof)
    {
        SmartFoxManager.Instance.FraudDetected(type, proof); 
    }

    public override void EndGame(MPLGameEndReason.GameEndReasons reason, string message)
    {
        if (reason == MPLGameEndReason.GameEndReasons.TOURNAMENT_ENDED) return;
        base.EndGame(reason, message);
    }

    public override void StartGameplayTimer()
    {
        Debug.LogError("StartGamePlayTimer call came: "+ gameplayTimer.Enabled);
        if (gameplayTimer.Enabled) return;
        gameplayDuration = 0;
        gameplayTimer.Start();

        Debug.LogError("*********** GameplayDurationSDK value reset from StartGameplayTimer ");
    }

    void StopGameplayTimer()
    {
        if (!gameplayTimer.Enabled) return;
        gameplayTimer.Stop();
        Debug.LogError("GamePlay timer made off");
    }


    void HandleGamePlayTimer(object sender, EventArgs e)
    {
        if (StartAddingGameplayDuration)
            gameplayDuration++;

        AddScoreLog(GetScore());
        //Debug.LogError("GameplayDuration increased from 1v1: "+ gameplayDuration);
    }

    protected override void AddScoreLog(int gameScore)
    {
        
        if (Mathf.Abs(lastScoreLogAddedTime - gameplayDuration) >= mplController.gameConfig.SequenceInterval)
        {
            
            scoreLog.Add(gameScore);
            lastScoreLogAddedTime = gameplayDuration;
        }
    }
   

    
}

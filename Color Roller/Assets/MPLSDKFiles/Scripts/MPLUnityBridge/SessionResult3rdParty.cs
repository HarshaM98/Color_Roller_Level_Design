using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SessionResult3rdParty
{
    /******************************** Global Variables & Constants ********************************/
    /// <summary>
    /// Active session id
    /// </summary>
    public String SessionId;



    /// <summary>
    /// GameId 
    /// </summary>
    public int GameId;

    public int GamePauseDuration;

    public string GameVersion;
    public string AppVersionCode;
    public string AppVersionName;
    public string AppType;

  

    /// Screen Touch Press Data
     /// </summary>
     
    public int[] ScreenTouchPressCount = { 0,0,0,0};


    /// Screen Touch Relese Data
    /// </summary>
    public int[] ScreenTouchReleaseCount = { 0, 0, 0, 0 };

    /// Device Is Mobile Or Not
    /// </summary>
    /// 
    public bool Mobile;


  //  public List<AllTask> collectibles;

    /**************************************** Constructors ****************************************/
    public SessionResult3rdParty()
    {

        GameId = MPLController.Instance.gameConfig.GameId;
        GamePauseDuration = MPLController.Instance.gameConfig.MaxPauseDuration;
        SessionId = MPLController.Instance.gameConfig.SessionId;
        AppVersionName = MPLController.Instance.gameConfig.AppVersionName;
        AppType = MPLController.Instance.gameConfig.AppType;
        AppVersionName = MPLController.Instance.gameConfig.AppVersionName;
        AppVersionCode = MPLController.Instance.gameConfig.AppVersionCode;
        ScreenTouchPressCount = InputHandler.mInstance._TouchCountPress;
        ScreenTouchReleaseCount = InputHandler.mInstance._TouchCountRelease;
        Mobile = InputHandler.mInstance.isMobile;
      //  collectibles = CollectableHandler.mInstance.DataParse();
       


    }




 

  



}
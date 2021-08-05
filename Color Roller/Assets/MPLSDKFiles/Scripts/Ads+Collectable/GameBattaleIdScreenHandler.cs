using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameBattaleIdScreenHandler : MonoBehaviour
{
    public static GameBattaleIdScreenHandler mInstance;
    public Text _PortraitText, _LandScapeText;
    public Text _PortraitTournamentText, _LandScapeTournamentText;
    string mBattleName;
    public float _StartTime;


    string mBattaleId;
    string mDay;
    string mMonth;
    string mGameType;
    string mBattaleIdCustomise;
    string NormalizedString;
    int mMaxSize = 10;


    // Start is called before the first frame update
    void Awake()
    {
        mInstance = this;
        _StartTime = Time.time;

        Invoke("ShowSessionId", 5);
    }

    readonly List<int> RUMMY_GAME_ID = new List<int>
    {
       1000125, //  Rummy
       1000126, //  Rummy
       1000127, //  Rummy
       128,//Rummy Tournaments
       1000141, //  Rummy
       1000142, //  Rummy
       1000143 //  Rummy
    };

    public void ShowSessionId()
    {
        try
        {
            Debug.LogError("SessionId " + MPLController.Instance.gameConfig.unityUid);
            if (!MPLController.Instance.gameConfig.unityUid || MPLController.Instance.gameConfig.GameId > 1000000 || RUMMY_GAME_ID.Contains(MPLController.Instance.gameConfig.GameId))
                return;

            mGameType = "0.";
            CustomizedBattleId(MPLController.Instance.gameConfig.SessionId);
            Debug.LogError(MPLController.Instance.gameConfig.SessionId);
            if (!MPLController.Instance.isLandscape)
                _PortraitTournamentText.text = NormalizedString;
            else
                _LandScapeTournamentText.text = NormalizedString;
        }
        catch (Exception ex)
        {

        }
    }
    void CustomizedBattleId(string pBattleId)
    {
        mBattaleId = pBattleId;
        mDay = System.DateTime.Now.Date.Day.ToString();
        mMonth = System.DateTime.Now.Date.Month.ToString();
      
        mDay = ExtraValuesAdd(mDay);
        mMonth = ExtraValuesAdd(mMonth);
        mBattaleIdCustomise = mDay + mMonth + mGameType  + NormalizeLength(mBattaleId, mMaxSize);
        NormalizedString = mBattaleIdCustomise;
    }
    public void ShowBattleId()
    {
        try
        {
            Debug.LogError("BattleId " + MPLController.Instance.gameConfig.unityUid+ SmartFoxManager.Instance.GetJoinedRoom().Name);
        if (!MPLController.Instance.gameConfig.unityUid || RUMMY_GAME_ID.Contains(MPLController.Instance.gameConfig.GameId) || MPLController.Instance.gameConfig.GameId <= 1000000)
            return;
        mGameType = "1.";
        CustomizedBattleId(SmartFoxManager.Instance.GetJoinedRoom().Name);
       
        if (!MPLController.Instance.isLandscape)
            _PortraitText.text = NormalizedString;
        else
            _LandScapeText.text = NormalizedString;

        }
        catch(Exception ex)
        {

        }

       
    }
    public void ShowBattleIdThirdParty(string pName)
    {
        try
        {
            Debug.LogError("BattleId " + MPLController.Instance.gameConfig.unityUid + pName);
        if (!MPLController.Instance.gameConfig.unityUid || RUMMY_GAME_ID.Contains(MPLController.Instance.gameConfig.GameId) || MPLController.Instance.gameConfig.GameId <= 1000000)
            return;
        mGameType = "1.";
        CustomizedBattleId(pName);
        if (!MPLController.Instance.isLandscape)
            _PortraitText.text = NormalizedString;
        else
            _LandScapeText.text = NormalizedString;

        }
        catch (Exception ex)
        {

        }

      
    }
    public void HideBattleId()
    {

        try
        {
            if (!MPLController.Instance.isLandscape)
                _PortraitText.text = "";
            else
                _LandScapeText.text = "";
        }
        catch (Exception ex)
        {

        }


       
    }
    public void TextUpdate(Text pPortraitText, Text pLandScapeText)
    {

        try
        {
            _PortraitText = pPortraitText;
            _LandScapeText = pLandScapeText;
        }
        catch (Exception ex)
        {

        }
      
    }

   

    string ExtraValuesAdd(string pValues)
    {
        if (pValues.Length < 2)
            pValues = "0" + pValues;
        return pValues;
    }



    string NormalizeLength(string value, int maxLength)
    {
        return value.Substring(value.Length - maxLength, maxLength);
    }
}





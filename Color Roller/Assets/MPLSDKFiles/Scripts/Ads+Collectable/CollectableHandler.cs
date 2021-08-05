using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;
using UnityEngine.UI;


public enum eNotificationType
{
    None,
    ImageType,
    ModelType
};
// Later Update required for 3D TODO
[System.Serializable]
public class UiObject
{
    public string _Orientations;
    public RectTransform _Object;
    public RectTransform _Target;
}
public class CollectableHandler : MonoBehaviour
{
    // Later Update required for 3D TODO
    public List<UiObject> uiObjects;

    public RectTransform _Object;
    public RectTransform _Target;

    public Image _2DElement;
    public GameObject _3DElement;
    public Image _BG;
    public Text _CollectableText;
    public eNotificationType _eNotificationType;
 

    Vector3 mStartPosition;

    public Transform _Reference;

    Transform mLoadedObject;

    [SerializeField] int _AnimationSpeed = 200;



    private Sprite gameLogoSprite;
    private Sprite gameIcon;

    private GameObject AssetsBundleElement;

    public List<Sprite> gameNotificationSprite;
    public List<Sprite> gameIconSprite;

    public List<GameObject> gameNotification3D;

    public static CollectableHandler mInstance;

    eNotificationType _eNotificationGameOver;

    public RenderTexture _3DRendrer;

    public string _ColleactableText;


    int orientationType = 0;

    public string _CollectableName;
    float mReverseTimeDelay = 5;
    public bool _DataUpdated = false;
    public List<AllTask> allTasks;

    public delegate void SendCollectibles(int id);
    public event SendCollectibles sendCollectibles;
    public int _Sequence = 0;
    public Vector2 _SequenceVector2 = new Vector2(0,0);

    Vector3 mStartScaling;
    float mScalingMul =1;
    Rect LastSafeArea = new Rect(0, 0, 0, 0);
    bool mShiftedUI = false;
    float mFractionValues = 0.001f;

    Vector3 mNotificationTarget;
    int mShiftDistance = 30;

    public bool collectiblesOn = false;
    public bool CollectiblesOnHansel = false;
  
    public bool collectiblesEnable()
    {

        TrailInit();
        if (MPLController.Instance.gameConfig.collectiblesOn && MPLController.Instance.gameConfig.CollectiblesOnHansel)
            return true;
        else
            return
                false;
    }
    public bool TrialsOn()
    {

        if (MPLController.Instance.gameConfig.collectiblesOn && MPLController.Instance.gameConfig.CollectiblesOnHansel && DataParse().Count != 0 && MPLController.Instance.gameConfig.TrialEnable && TrailEnableId.Contains(MPLController.Instance.gameConfig.GameId) && MPLController.Instance.gameConfig.EntryFee > 0 && MPLController.Instance.gameConfig.EntryCurrency != "TOKEN")
            return true;
        else
            return
                false;
    }
    public bool NudgeOn()
    {

        if (MPLController.Instance.gameConfig.collectiblesOn && MPLController.Instance.gameConfig.CollectiblesOnHansel && DataParse().Count != 0 && MPLController.Instance.gameConfig.NudgePercentage >=1 && MPLController.Instance.gameConfig.NudgePercentage  < 100 && NudgeEnableId.Contains(MPLController.Instance.gameConfig.GameId) && MPLController.Instance.gameConfig.EntryFee > 0 && MPLController.Instance.gameConfig.EntryCurrency != "TOKEN")
            return true;
        else
            return
                false;
    }

    readonly List<int> NudgeEnableId = new List<int>
    {
        1000065,
        1000121,
        1000136,
        1000144,
        1000161,
        1000162,
        1000153

    };
    readonly List<int> TrailEnableId = new List<int>
    {
        1000065,
        1000121,
        1000136,
        1000144,
        1000161

    };
    bool TrialGiven()
    {

      //  return Convert.ToBoolean(PlayerPrefs.GetInt("Trails" + MPLController.Instance.gameConfig.GameId.ToString()));

        if (KeysMatch == 0)
            return false;
        else
            return true;
    }

    public string TrailKeysActivate()
    {
      
        if (TrialsOn() && KeysMatch == 2)
        {
            ShowPopUp2DTrials(int.Parse(this.allTasks[0].id), this.allTasks[0].name + " Trial Applied", true);
           // KeysMatch = 3;
            Invoke("DelayUpdate", 2);
            return allTasks[0].id;
        }
        else
        {
           
            return "";
        }

    }
    void DelayUpdate()
    {
        KeysMatch = 3;

    }

    private void OnGUI()
    {
        //  PlayerPrefs.DeleteAll();
        //if (GUI.Button(new Rect(0, 0, 100, 100), "Nonfatals"))
        //{

        //    //Trail();
        //    throw new System.Exception("NonFatal Test exception please ignore");
        //}
        //if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 100), "Fatals"))
        //{

        //    TrailKeysActivate();
          
        //}

    }

    bool mTrialsEnable = false;
    public bool TrialsEnable
    {
        set
        {
            mTrialsEnable = value;
        }
        get
        {
            return mTrialsEnable;
        }
    }

 
    public int KeysMatch
    {
        
        get
        {
            return (PlayerPrefs.GetInt("Trails" + MPLController.Instance.gameConfig.GameId.ToString()));
        }
        set
        {
            PlayerPrefs.SetInt("Trails" + MPLController.Instance.gameConfig.GameId.ToString(), value);
            PlayerPrefs.Save();
        }

    }
    public void TrailInit()
    {
        

            if (TrialsOn())
            {
                if ( !TrialGiven())
                {
                    // Check task Progress..
                    foreach (AllTask allTask in DataParse())
                    {
                        if (int.Parse(allTask.tasks[0].currentValue) > 0)
                        {
                            return;
                        }
                    }
                   

                    KeysMatch = 1;
                    Debug.LogError("Trail Enable ...");
                    ShowPopUp2DTrials(int.Parse(this.allTasks[0].id),  this.allTasks[0].name + " Trial Applied", false);
                    TrialsEnable = true;
   


                }

            }
    
           
    }

    public void ShowPopUp2DTrials(int pId, string ptext , bool pStates)
    {
       
        _Sequence = IdPlace(pId);
        _CollectableName = allTasks[_Sequence].name;
        //_CollectableName = ptext;
        _ColleactableText = MPLController.Instance.localisationDetails.GetLocalizedText(ptext);
        CollectionId = pId;
    

        try
        {
            gameLogoSprite = gameNotificationSprite[IdSprite(pId.ToString())];
            gameIcon = gameIconSprite[IdSprite(pId.ToString())];
        }
        catch (Exception ex)
        {
            Debug.Log("Collectible Sprite Issues" + ex);
        }
        if(pStates)
        {
            if (gameIcon != null)
                CollectableShowPopUp(gameIcon, new Color(0, 1, 0, 1), ptext, eNotificationType.ImageType, null);
            else
                CollectableShowPopUp(gameLogoSprite, new Color(0, 1, 0, 1), ptext, eNotificationType.ImageType, null);
        }
      

      
    }


    public List<AllTask> DataParse()
    {
        return allTasks;
    }
    public List<AllTask> UpdatedTaskData
    {
        get
        {
            return allTasks;
        }
        set
        {
            MPLController.Instance.gameConfig.collectibles = value;
            allTasks = value;
        }
    }
 
    public void UpdateTaskSelection()
    {
        foreach (AllTask task in allTasks)
            task.selected = false;
        allTasks[_Sequence].selected = true;
    }
    void CollectableUIUpdate()
    {
        
        mScalingMul = (MPLController.Instance.isLandscape) ? 1 : 1.8f;
       // _Object = uiObjects[orientationType]._Object;
        _Object.localScale = mScalingMul * mStartScaling;
        if (!MPLController.Instance.isLandscape)
            Refresh();
    }
    IEnumerator StartAnimation()
    {
         CollectableUIUpdate();
        _Object.gameObject.SetActive(true);
        _Object.GetComponent<RectTransform>().localPosition = mStartPosition;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            _Object.localPosition = Vector3.MoveTowards(_Object.localPosition, _Target.localPosition, _AnimationSpeed * Time.deltaTime);

        }
    }

  
    void Refresh()
    {
        ApplySafeArea(mShiftDistance);
    }


    void ApplySafeArea(float mDifference)
    {
        mShiftedUI = true;
        _Target.position = mNotificationTarget - new Vector3(0, Screen.height * mDifference * mFractionValues, 0);

    }


    public void Init(List<Sprite> pgameNotification, List<Sprite> pgameIcon)
    {
        if (pgameNotification.Count != 0)
        {
            gameIconSprite = pgameIcon;
            gameNotificationSprite = pgameNotification;
            allTasks = MPLController.Instance.gameConfig.collectibles;
           // ShowPreNotification();
        }
        
    }
    IEnumerator BackAnimation()
    {

        yield return new WaitForSeconds(mReverseTimeDelay);
        Debug.Log("Return Back..");
        StopCoroutine("StartAnimation");
        while (true)
        {
            yield return new WaitForEndOfFrame();
            _Object.localPosition = Vector3.MoveTowards(_Object.localPosition, mStartPosition, _AnimationSpeed * Time.deltaTime);
            if (Vector3.Distance(_Object.localPosition, mStartPosition) == 0)
            {
                Debug.Log("UI Reset...");
                _Object.gameObject.SetActive(false);
                StopCoroutine("BackAnimation");
            }

        }

    }

    private void Reset()
    {
        StopCoroutine("StartAnimation");
        StopCoroutine("BackAnimation");
        if (mLoadedObject != null)
            Destroy(mLoadedObject.gameObject);
    }
    private void OnDisable()
    {
        Reset();
    }
    public void CollectableShowPopUp(Sprite pImage, Color pColor, string pText, eNotificationType pNotificationType, Transform pTransform)
    {

        Reset();
       // _BG.color = pColor;
        _CollectableText.text = pText;
        if(pText.Length > 30)
        {
            _CollectableText.fontSize = 30;
        }


        _2DElement.sprite = pImage;
        Object_2D_3D(true);

        if (pTransform != null)
            mLoadedObject = Instantiate(pTransform, _Reference.position, pTransform.rotation);



        StartCoroutine("StartAnimation");
        StartCoroutine("BackAnimation");
    }

    void Object_2D_3D(bool pStates)
    {
        _2DElement.gameObject.SetActive(pStates);
        _3DElement.SetActive(!pStates);
    }
    private void Awake()
    {
        mInstance = this;

       
    }

    // Start is called before the first frame update
    private void Start()
    {

        mStartPosition = _Object.localPosition;
        mStartScaling = _Object.localScale;
        mNotificationTarget = _Target.position;
    
    
        // ShowPreNotification();
    }

    public eNotificationType Collectable
    {

        set
        {
            _eNotificationGameOver = value;
        }
        get
        {
            return _eNotificationGameOver;
        }
    }
    public Sprite ShowPopUp2DGameOver()
    {
        return gameLogoSprite;
    }
    public RenderTexture ShowPopUp3DGameOver()
    {
        return _3DRendrer;
    }
    int CollectionId;
 

    public void ShowPopUp2D(int pId, Color pColor, string ptext, eNotificationType pNotificationType, eNotificationType pNotificationGameOver)
    {
        allTasks[IdPlace(pId)].unlocked = true;
        _Sequence = IdPlace(pId);
        _CollectableName = allTasks[_Sequence].name;
        //_CollectableName = ptext;
        _ColleactableText = MPLController.Instance.localisationDetails.GetLocalizedText(ptext);
        CollectionId = pId;
        _eNotificationGameOver = pNotificationGameOver;

        try
        {
            gameLogoSprite = gameNotificationSprite[IdSprite(pId.ToString())];
           // gameIcon = gameIconSprite[IdSprite(pId.ToString())];
        }
        catch (Exception ex)
        {
            Debug.Log("Collectible Sprite Issues" + ex);
        }

        //if (gameIcon != null)
        //    CollectableShowPopUp(gameIcon, new Color(0, 1, 0, 1), ptext, eNotificationType.ImageType, null);
        //else
            CollectableShowPopUp(gameLogoSprite, new Color(0, 1, 0, 1), ptext, eNotificationType.ImageType, null);

        SendInGameUnlockEvent(pId.ToString(), _CollectableName, _Sequence.ToString(), allTasks[IdPlace(pId)].tasks[0].targetValue);
    }
    public void SendInGameUnlockEvent(string _virtualGoodID, string _virtualGoodName, string _tier, string _goodCount)
    {
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_INGAME_COLLECTIBLES, new CollectiblesEventProperties(_virtualGoodID,
                       _virtualGoodName,
                       MPLController.Instance.gameConfig.GameName,
                       MPLController.Instance.gameConfig.GameId.ToString(),
                       _tier,
                       _goodCount,
                       "Unlocked",
                       "In Game"
                       ).ToString());
    }


   public int IdSprite(string pName)
    {
        int mCount = 0;
        foreach(Sprite sprite in gameNotificationSprite)
        {
            if(sprite.name == pName)
            {
                return mCount;
            }
            mCount++;
        }
        return 0;
    }

 

    int IdPlace(int pId)
    {
        int mCount = 0;
        foreach (AllTask task in allTasks)
        {
            if (int.Parse(task.id) == pId)
            {
                return mCount;
            }
            mCount++;
        }
        return 0;
    }

    bool mChildEnable = false;



    // Later Update required for 3D TODO
    int IdGameObject(string pName)
    {
        int mCount = 0;
        foreach (GameObject game in gameNotification3D)
        {
            if (game.name == pName)
            {
                return mCount;
            }
            mCount++;
        }
        return 0;
    }


    public void ChangeCollectableOnMPL(Action<bool, string> callback)
    {

        Debug.Log("Ravi " );
        string url = "/game-profiles/" + MPLController.Instance.gameConfig.GameId + "/collectibles";

        Debug.Log("ranjan ");
        string requestBody = "{\"selectedCollectible\":" + CollectionId + "}";

        Debug.Log("Ravi ");

        StartCoroutine(MplSdkApiHandler.Request(url, requestBody, MPL_SDK_REQUEST_TYPE.POST, null, (MPLSdkRequestInfo status) =>
        {

            Debug.Log("ChangeCollectableOnMPL Status:" + status.ToString());
            if (status.isSuccess)
            {
                string callbackDataInString = System.Text.Encoding.UTF8.GetString(status.callBackData);
                MPLController.Instance.PrintExtraLog("ChangeCollectableOnMPL ="+callbackDataInString);

                Debug.LogError("callback" + callback);
                if (callback != null)
                {
                    callback(true, callbackDataInString);
                }


                SendGameEndSelectionCollectibles(CollectionId);
                //sendCollectibles.Invoke(CollectionId);
            }
            else
            {
                Debug.LogError("callback" + callback);
                Debug.LogError("ChangeCollectableOnMPL failed  = " + status.errorDescription);
                if (callback != null)
                {
                    callback(false, "");
                }
            }
        }));


    }
    void SendGameEndSelectionCollectibles(int id)
    {
        Debug.LogError("SendGameEndSelectionCollectibles received...");
        AllTask task = new AllTask();
        int index = 0;

        for (int i = 0; i < MPLController.Instance.gameConfig.collectibles.Count; i++)
        {
            if (MPLController.Instance.gameConfig.collectibles[i].id == id.ToString())
            {
                task = MPLController.Instance.gameConfig.collectibles[i];
                index = i;
                break;
            }
        }
        MPLSdkBridgeController.Instance.SubmitEvent(MPLController.EVENT_INGAME_COLLECTIBLES, new CollectiblesEventProperties(task.id,
                   task.name,
                   MPLController.Instance.gameConfig.GameName,
                   MPLController.Instance.gameConfig.GameId.ToString(),
                   index.ToString(),
                   task.tasks[0].currentValue,
                   "Applied",
                   "Game End Screen"
                   ).ToString());
    }

  
    int pCount = 0;
    int pIncrementedValues;
    float mfixedValues = 0;
    float mCompleteRatio;
    string mNotificationText;

    public bool CanShowPopUp = false;
    int  mColletibleCount = 0;

    float nudgePercentage;

    string mSavingName;
    public int ShowPreNotification()
    {
        
        Debug.LogError("Ravi ranjan...1");
        pIncrementedValues = 0;
        pCount = 0;
        mfixedValues = 0;
        mCompleteRatio = 0;
        nudgePercentage = (float)MPLController.Instance.gameConfig.NudgePercentage;
        foreach (AllTask task in allTasks)
        {
            mSavingName = "Collectible" + MPLController.Instance.gameConfig.GameId.ToString() + task.id;
            if (!task.unlocked)
            {
                Debug.LogError("Ravi ranjan...2"+ float.Parse(task.tasks[pCount].currentValue) +" "+ float.Parse(task.tasks[pCount].targetValue) + ((float.Parse(task.tasks[pCount].currentValue) / float.Parse(task.tasks[pCount].targetValue)) ));
                mCompleteRatio = (float.Parse(task.tasks[pCount].currentValue) / float.Parse(task.tasks[pCount].targetValue))*100;
                Debug.LogError("% "+ mCompleteRatio  + (PlayerPrefs.GetInt("Collectible" + MPLController.Instance.gameConfig.GameId + task.id) == 0) + PlayerPrefs.GetInt("Collectible" + MPLController.Instance.gameConfig.GameId + task.id));
                if (mCompleteRatio >= nudgePercentage && mCompleteRatio < 100.0f && PlayerPrefs.GetInt(mSavingName) == 0)
                {
                    Debug.LogError("%2  " + mCompleteRatio);
                   // if (mCompleteRatio > mfixedValues)
                    {
                        mfixedValues = mCompleteRatio;
                        mNotificationText = task.tasks[pCount].description + "\n" + task.tasks[pCount].currentValue + "/" + task.tasks[pCount].targetValue;
                        mColletibleCount = pIncrementedValues;
                        PlayerPrefs.SetInt(mSavingName, 1);
                        PlayerPrefs.Save();
                    }

                    Debug.LogError("Ravi ranjan...3");
                    return mColletibleCount;
                }
            }
            pIncrementedValues++;
          
        }
       
        return mColletibleCount;
        
    }

    public bool EnableProgressionBar()
    {

       
        CanShowPopUp = false;
        pCount = 0;
        mCompleteRatio = 0;
        nudgePercentage = (float)MPLController.Instance.gameConfig.NudgePercentage;
        
        if (!collectiblesEnable() || nudgePercentage <= 1 || nudgePercentage >= 100 || !NudgeEnableId.Contains(MPLController.Instance.gameConfig.GameId))
            return false;

        foreach (AllTask task in allTasks)
        {
            mSavingName = "Collectible" + MPLController.Instance.gameConfig.GameId.ToString() + task.id;
            if (!task.unlocked)
            {
                Debug.LogError("SaveData " + PlayerPrefs.GetInt("Collectible" + MPLController.Instance.gameConfig.GameId + task.id) + " " + MPLController.Instance.gameConfig.GameId);
                mCompleteRatio = (float.Parse(task.tasks[pCount].currentValue) / float.Parse(task.tasks[pCount].targetValue))*100;
                if (mCompleteRatio >= nudgePercentage && mCompleteRatio < 100.0f && PlayerPrefs.GetInt(mSavingName) == 0)
                {
                    CanShowPopUp = true;
                    return CanShowPopUp;

                }
            }
        

        }


        return false;
    }
 


}

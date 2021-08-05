using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TimerCanvasController : MonoBehaviour
{
	public Text timerText;
	public Image timerImage;
	public GameObject popUp;
    public int numberOfSessions,maxSessions;
    public int thresholdTime,currentTime,timeOutDuration;
    private static TimerCanvasController instance;
    public bool continueTimer=true;
    public bool continueGame = true;
    public System.DateTime dateTime;
    public int currDateTime,lastDateTime;
    public GameObject timerObject;
    public Text subtitle;
    public bool isItBattleEnd=false;
    public CanvasGroup timerCanvasGroup;
    public bool isItFirstTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static TimerCanvasController Instance
    {
        get { if (instance == null) instance = FindObjectOfType<TimerCanvasController>(); return instance; }
    }
    IEnumerator cr;
    void OnEnable()
	{
        if (PlayerPrefs.HasKey("SessionsTillNow"))
        {
            Debug.Log("has key sessions till now");
            isItFirstTime = false;
        }
        else
        { 
            Debug.Log("doesn't have key sessions till now");
        isItFirstTime = true;

        }
        instance = this;

        if (MPLController.Instance.gameConfig.RealityCheckTimerDuration > 0)
        {
            thresholdTime = MPLController.Instance.gameConfig.RealityCheckTimerDuration;
        }
        else
        {
            thresholdTime = 120;
        }

        if (MPLController.Instance.gameConfig.RealityCheckSessions > 0)
        {
            maxSessions = MPLController.Instance.gameConfig.RealityCheckSessions;
        }
        else
        {
            maxSessions = 3;
        }

        dateTime = System.DateTime.Now;
        
        currentTime = getPrefernce("TimeTillNow");
        numberOfSessions = getPrefernce("SessionsTillNow");

        
        numberOfSessions++;

        setPrefernce("SessionsTillNow", numberOfSessions);

        Debug.Log("isItFirstTimeENable=" + isItFirstTime);
        Debug.Log("Number of sessionsEnable=" + numberOfSessions);
        Debug.Log("Current TimeEnable=" + currentTime);
        if ((isItFirstTime || numberOfSessions >= maxSessions) && (currentTime >= thresholdTime))
        {
            
           showPopup(true);
           continueGame = false;

            
        }
        else
        {
            cr = addTime();
            StartCoroutine(cr);
        }
        Debug.Log("Current Time=" + currentTime);
        timeOutDuration = currentTime + thresholdTime;

        subtitle.text = string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("You have been playing on MPL for more than  {0}h and {1}m.How would you like to proceed?"), Mathf.Floor(thresholdTime / 3600).ToString("00"), Mathf.Floor(thresholdTime % 3600 / 60).ToString("00"));

    }

    public bool CheckAndShowPopupAtEnd()
    {
        Debug.Log("Check Battle End Popup called");
        isItBattleEnd = true;
        Debug.Log("isItFirstTime=" + isItFirstTime);
        Debug.Log("Number of sessions=" + numberOfSessions);
        Debug.Log("Current Time=" + currentTime);

        int sessions = getPrefernce("SessionsTillNow");

      //  int time = getPrefernce("TimeTillNow");
        if ((isItFirstTime || sessions >= maxSessions) && (currentTime >= thresholdTime))
        {

            showPopup(true);
            continueGame = false;
            return true;

        }
        return false;
    }
    // Update is called once per frame
    void Update()
    { 
        
    }
    int timeToInt(System.DateTime dateTime)
    {
        string timeString = ""+dateTime.Year + dateTime.Month + dateTime.Day;
        //Debug.Log("Time String=" + timeString);
        return int.Parse(timeString);
    }

    void convertTime(int timeInseconds)
    {
        string hours= Mathf.Floor(timeInseconds / 3600).ToString("00");
        timeInseconds = timeInseconds % 3600;
        string minutes = Mathf.Floor(timeInseconds / 60).ToString("00");
        string seconds = Mathf.Floor(timeInseconds % 60).ToString("00");


        timerText.GetComponent<Text>().text = hours+":"+minutes + ":" + seconds;
        if(timeInseconds> thresholdTime)
        {
            timerText.GetComponent<Text>().color = new Color32(255, 30, 70, 255);
            timerImage.color= new Color32(255, 30, 70, 255);
        }
        else
        {
            timerText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            timerImage.color = new Color32(255, 255, 255, 255);
        }
    }

    public void showPopup(bool show)
    {
        if (show)
        {
            isItFirstTime = false;
            Debug.Log("isItBattleEnd=" + isItBattleEnd);
            if (!isItBattleEnd)
            {
                timerObject.SetActive(false);
            }
            else
            {
                timerCanvasGroup.alpha = 0f;
            }
            popUp.SetActive(true);
            Time.timeScale = 0f;
            setPrefernce("SessionsTillNow", 0);
            SetValues();
            if (cr != null)
            {
                StopCoroutine(cr);
            }

        }
        else
        {
            popUp.SetActive(false);
            Time.timeScale = 1f;
            continueGame = true;
            if (!isItBattleEnd)
            {
                timerObject.SetActive(true);

                
                MPLController.Instance.StartGame();
            }
            else
            {
                Debug.Log("Setting Alpha to 0");
                timerCanvasGroup.alpha = 1f;
               
            }
            cr = addTime();
            StartCoroutine(cr);
        }
    }

    IEnumerator addTime()
    {
        while (continueTimer)
        {
            lastDateTime = getPrefernce("LastDateTime");

            currDateTime = timeToInt(System.DateTime.Now);
            setPrefernce("LastDateTime", currDateTime);

            if (currDateTime - lastDateTime > 0)
            {
                currentTime = 0;
            }
            convertTime(currentTime);
            yield return new WaitForSeconds(1);
            currentTime++;
            //Debug.Log("Current Time addTime=" + currentTime);
        }


    }
    public void Quit()
	{
        Session.Instance.Quit();
	}

    public void GoToTransactionHistory()
    {
        Session.Instance.SetQuitReason(Session.MPLQuitReason.view_transaction_history);
        Session.Instance.Quit();
    }
    public void setPrefernce(string key,int value)
    {
        if (true)
        {
            PlayerPrefs.SetInt(key, value);
        }
        

        
    }
    public int getPrefernce(string key)
    {
        int Value = 0;
        if (true)
        {
            if(PlayerPrefs.HasKey(key))
            {
                Value = PlayerPrefs.GetInt(key);
            }
            
        }
        
        return Value;
    }

    private void OnApplicationQuit()
    {
        SetValues();
    }


    public void SetValues()
    {
        setPrefernce("TimeTillNow", TimerCanvasController.Instance.currentTime);
        setPrefernce("LastDateTime", timeToInt(System.DateTime.Now));
        
        //continueTimer = false;
    }

}

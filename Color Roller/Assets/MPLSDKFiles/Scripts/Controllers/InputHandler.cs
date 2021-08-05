using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    // Use this for initialization
    public static InputHandler instance;
    public bool isMobile;
    float mMiddleX;
    float mMiddleY;
    public int[] _TouchCountPress = { 0, 0, 0, 0 };
    public int[] _TouchCountRelease = { 0, 0, 0, 0 };
    public int[] _IntervalTouchCount = { 0,0,0,0};
  
    private List<int> touchCountPressList = new List<int>();
    private int interval; //Read from Lobby config
    //public int valReq = 10;
    //public List<int[]> finalTouchPressList = new List<int[]>();
    public IEnumerator intervalCort;
    public IEnumerator recordCort;

    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.TizenPlayer)
            isMobile = true;
        Init();
        Session.Instance.Restart += Reset;
        Session.Instance.GameEnd += SubmitScoreOnMPLGameEnd;
        if (recordCort == null)
        {
            intervalCort = WaitForIntervalValue();
            StartCoroutine(intervalCort);
        }

        //interval = MPLController.Instance.gameConfig.TouchTimeInterval;
        //recordCort = RecordTouchCountValues(interval);
        
        // if (interval != 0)
        //  StartCoroutine(recordCort);
    }
    int touchCount;
    int dataCount = 0;
    int maxDataCount = 100;

    IEnumerator WaitForIntervalValue()
    {
        
        yield return new WaitForSeconds(1f);
        
        interval = MPLController.Instance.gameConfig.TouchTimeInterval;
        //Debug.LogError("-------Pulkit------WaitForIntervalValue touch data check interval------ interval: " + interval);

        if (recordCort == null)
        {
            recordCort = RecordTouchCountValues(interval);

            if (interval != 0)
                StartCoroutine(recordCort);
        }
    }
    IEnumerator RecordTouchCountValues(float t)
    {
        //Debug.LogError("-------Pulkit------RecordTouchCountValues touch data");
        touchCountPressList.Clear();
        while (true)
        {
            yield return new WaitForSeconds(t);
            //Debug.LogError("-------Pulkit------touch data check interval inside------- dataCount: " + dataCount + " touchCount: " + touchCount);
            dataCount++;
            touchCount = 0;
            for (int mCount = 0; mCount < _IntervalTouchCount.Length; mCount++)
            {
                touchCount += _IntervalTouchCount[mCount];
                _IntervalTouchCount[mCount] = 0;
            }
            if(dataCount < maxDataCount)
            touchCountPressList.Add(touchCount);
           
            //touchCountReleaseList.Add(_TouchCountRelease);
        }
    }

    //lobbyconfig 0, default 1

    public List<int> GetTouchCountPressList()
    {
        //if (touchCountPressList.Count >= valReq)
        //{
        //    Debug.Log("------------touchCountPressList Count-------------" + touchCountPressList.Count);

        //    finalTouchPressList.Clear();
        //    int valAdd = (touchCountPressList.Count / valReq);
        //    int i;
        //    for (i = 0; i < touchCountPressList.Count; i += valAdd)
        //    {
        //        finalTouchPressList.Add(touchCountPressList[i]);

        //        if (finalTouchPressList.Count == valReq - 1)
        //        {
        //            finalTouchPressList.Add(touchCountPressList[touchCountPressList.Count - 1]);
        //            break;
        //        }
        //    }

        //    Debug.Log("------------touchCountPressList Count Final-------------" + finalTouchPressList.Count);
        //    return finalTouchPressList;
        //}
        //else
        //{
        //    //Debug.Log("------------ Sending touchCountPress Count ------------- " + _TouchCountPress[0] + " " + _TouchCountPress[1]
        //    //+ " " + _TouchCountPress[2] + " " + _TouchCountPress[3]);
        //    return new List<int[]>() { _TouchCountPress };
        //}
        if (recordCort != null)
        {
            //Debug.LogError("-------Pulkit------Stopping record CORT ");
            StopCoroutine(recordCort);
        }
        if (intervalCort != null)
        {
            //Debug.LogError("-------Pulkit------Stopping interval CORT ");
            StopCoroutine(intervalCort);
        }
        recordCort = null;
        return touchCountPressList;
    }

    public void Init()
    {
        Debug.Log("InitRavi..");

        mMiddleX = Screen.width / 2;
        mMiddleY = Screen.height / 2;
        for (int i = 0; i < _TouchCountPress.Length; i++)
        {
            _TouchCountPress[i] = 0;
            _TouchCountRelease[i] = 0;
        }
    }
    private void SubmitScoreOnMPLGameEnd(MPLNotificationEventArgs mplNotification)
    {
        //dataCount = 0;

        //StopCoroutine(recordCort);

    }
    public void Reset()
    {
        //if(recordCort!=null)
        //{
        //    StopCoroutine(recordCort);
        //}
        //if (intervalCort != null)
        //{
        //    StopCoroutine(intervalCort);
        //}
        touchCountPressList.Clear();
        dataCount = 0;
        Init();
        if (recordCort == null)
        {
            intervalCort = WaitForIntervalValue();
            StartCoroutine(intervalCort);
        }
        //Debug.LogError("-------Pulkit------touch data check RESET called");
        //recordCort = RecordTouchCountValues(interval);

        //if (interval != 0)
        //StartCoroutine(recordCort);
    }
    private void OnDisable()
    {
        Session.Instance.Restart -= Reset;
        if (recordCort != null)
        {
            StopCoroutine(recordCort);
        }
    }
    void Update()
    {
        if (isTouchBegan())
        {
            if (touchPositions().x < mMiddleX)
            {
                if (touchPositions().y < mMiddleY)
                {
                    _TouchCountPress[3]++;
                    _IntervalTouchCount[3]++;
                }
                else
                {
                    _TouchCountPress[0]++;
                    _IntervalTouchCount[3]++;
                }
            }
            else
            {
                if (touchPositions().y < mMiddleY)
                {
                    _TouchCountPress[2]++;
                    _IntervalTouchCount[3]++;
                }
                else
                {
                    _TouchCountPress[1]++;
                    _IntervalTouchCount[3]++;
                }
            }

        }

        if (isTouchEnded())
        {
            if (touchPositions().x < mMiddleX)
            {
                if (touchPositions().y < mMiddleY)
                {
                    _TouchCountRelease[3]++;
                   
                }
                else
                {
                    _TouchCountRelease[0]++;
                  
                }
            }
            else
            {
                if (touchPositions().y < mMiddleY)
                {
                    _TouchCountRelease[2]++;
                 
                }
                else
                {
                    _TouchCountRelease[1]++;
                 
                }
            }

        }


    }
    public static InputHandler mInstance
    {

        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "InputHandlerMPL";
                gameObject.AddComponent<InputHandler>();
               
            }
           
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public bool isTouchBegan()
    {

        if (isMobile)
        {
            if (Input.touchCount >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
                return true;
            else
                return false;

        }

        else
            return Input.GetMouseButtonDown(0);

    }


    public bool isTouchEnded()
    {
        if (isMobile)
        {
            if (Input.touchCount >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Ended)
                return true;
            else
                return false;
        }

        else
            return Input.GetMouseButtonUp(0);
    }

    public bool isTouchMoving()
    {
        if (isMobile)
        {
            if (Input.touchCount >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Moved)
                return true;
            else
                return false;
        }

        else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            return true;

        else
            return false;
    }

    public bool isTouchStationary()
    {

        if (isMobile)
        {
            if (Input.touchCount >= 1)
                return true;
            else
                return false;
        }

        else
            return Input.GetMouseButton(0);

    }

    public Vector2 deltaXAndDeltaY()
    {
        Vector2 deltaPositions;

        if (isMobile)
            deltaPositions = new Vector2(Input.GetTouch(Input.touchCount -1).deltaPosition.x, Input.GetTouch(Input.touchCount - 1).deltaPosition.y);

        else
            deltaPositions = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        return deltaPositions;
    }


    public Vector3 touchPositions()
    {

        Vector3 touchPos;

        if (isMobile)
            touchPos = Input.GetTouch(Input.touchCount -1).position;
        else
            touchPos = Input.mousePosition;

        return touchPos;
    }







    public Vector2 DeviceTild()
    {
        Vector2 deltaPositions;

        if (isMobile)
            deltaPositions = new Vector2(Input.acceleration.x, Input.acceleration.y);

        else
            deltaPositions = new Vector2(Input.GetAxis("Mouse X") * 0.08f, Input.GetAxis("Mouse Y") * 0.1f);

        return deltaPositions;
    }




}

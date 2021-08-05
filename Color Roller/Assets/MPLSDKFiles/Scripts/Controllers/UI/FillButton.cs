using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillButton : MonoBehaviour {
    float timeOut;
    public GameObject cancelButton;
    
    
    public Transform buttonFilltransform;
    private Image buttonFillImage;
    public delegate void FillButtonGameEvents();
    public event FillButtonGameEvents CancellLoader,AutoStartBattle;
    private bool cancelPressed;

    // Use this for initialization
    void Start () {
        
	}
    private void OnEnable()
    {
        //buttonFillImage.localScale = new Vector3(0f, 1f,1f);
        cancelButton.gameObject.GetComponent<Button>().interactable = true;
        buttonFillImage = buttonFilltransform.gameObject.GetComponent<Image>();
        StartCoroutine(fillTheBar());
        Debug.Log("Enable Fill Image Button");
    }

    // Update is called once per frame
    void Update () {
		
	}
    IEnumerator fillTheBar()
    {
        cancelPressed = false;
        Debug.Log("Fill Meter");
        // timeOut = MPLController.Instance.gameConfig.AutoStartTimer;
        if(MPLController.Instance.gameConfig.AutoStartTimer>0)
        {
            timeOut = MPLController.Instance.gameConfig.AutoStartTimer;
        }
        else
        {
            timeOut = 15f;
        }
        
       
        float mstartTime = 0.0f;
        while (mstartTime < timeOut)
        {
            // Debug.Log("Fill Meter +="+ timeOut);

            mstartTime += Time.deltaTime;

            //inc = 1 * Time.deltaTime / timeOut;
            ////Debug.Log("inc=" + inc);
            //currentScale += inc;
            //Debug.Log("currentScale=" + currentScale);
            // buttonFillImage.localScale = new Vector3(currentScale, 1f,1f);
            buttonFillImage.fillAmount = mstartTime/ timeOut;


          //  timeOut -= Time.deltaTime;
            yield return new WaitForEndOfFrame(); //WaitForSeconds(1f);
        }
        //timeOut = 15f;

        //StopCoroutine(fillTheBar());
        cancelButton.GetComponent<Button>().interactable = false;
        if (!cancelPressed)
        {

            AutoStartBattle?.Invoke();


        }


    }
    public void CancelFill()
    {
        // buttonFillImage.transform.localScale = new Vector3(1f, 1f, 1f);
        StopCoroutine(fillTheBar());
        timeOut = 0f;
        buttonFillImage.fillAmount = 1f;
        cancelPressed = true;
        // timeOut = MPLController.Instance.gameConfig.AutoStartTimer;

        cancelButton.GetComponent<Button>().interactable = false;
        CancellLoader?.Invoke();
    }
}

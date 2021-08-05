using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePageType
{
    NudgePage,
    TrailsPage
}
public class CollectibleUpdateBar : MonoBehaviour
{
    public int _CollectableDetailsCount;
    string mCollectibleName;
    string mDiscription;
    float mCurrentValue;
    float targetValue;

    [SerializeField] Text _CollectibleNameText, _CollectibleNameText2;
    [SerializeField] Text _CollectibleDetailsText, _CollectibleDetailsText2;
    [SerializeField] Image _TopBar;
    public GameObject _2DImage, _3DImage;

    public ePageType _ePageType;
    // Start is called before the first frame update
    void Start()
    {

       
      
    }
    private void OnEnable()
    {
        switch (_ePageType)
        {
            case ePageType.NudgePage:
                NudgePage();
                break;
            case ePageType.TrailsPage:
                TrailsPage();
                break;
        }
    }


    string mTemp;
    int mTargetValue;
    int mValue;
    string UpdatedtText(string pName)
    {


        if (pName.Length > 25)
        {
            mTargetValue = pName.Length / 2;
        }
        //  mTargetValue = 0;
        char mUpdatedValues;
        bool mShiftedNextLine = false;
        foreach (char mCharName in pName)
        {
            if (mValue > mTargetValue && mValue > 25 && !mShiftedNextLine)
            {

                if (mCharName == ' ')
                {
                    mUpdatedValues = '\n';
                    mShiftedNextLine = true;
                }

                else
                    mUpdatedValues = mCharName;
                mTemp += mUpdatedValues;

            }
            else
            {

                mUpdatedValues = mCharName;
                mTemp += mUpdatedValues;
            }
            mValue++;

        }

        return mTemp;
    }
    void NudgePage()
    {
        _CollectableDetailsCount = CollectableHandler.mInstance.ShowPreNotification();
        Debug.LogError(_CollectableDetailsCount);
        mCollectibleName = CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].name;

      //  mDiscription = CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].description;
        mDiscription = UpdatedtText(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].description);

        mCurrentValue = float.Parse(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].currentValue);
        targetValue = float.Parse(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].targetValue);
        Debug.LogError("CollectibleUpdate : " + _CollectableDetailsCount);

        _CollectibleNameText.text = mCollectibleName;
        _CollectibleDetailsText.text = mDiscription;

        _CollectibleDetailsText2.text = string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("Only {0} more to go !"), (targetValue - mCurrentValue).ToString());
        //_CollectibleDetailsText2.text = "Only " + (targetValue - mCurrentValue).ToString() + " more to go !";
        _TopBar.fillAmount = mCurrentValue / targetValue;

        _2DImage.GetComponent<Image>().sprite = CollectableHandler.mInstance.gameNotificationSprite[CollectableHandler.mInstance.IdSprite(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].id)];
    }
    void TrailsPage()
    {
        _CollectableDetailsCount = 0;
        mCollectibleName = CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].name;
        mDiscription = CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].description;

        mCurrentValue = float.Parse(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].currentValue);
        targetValue = float.Parse(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].tasks[0].targetValue);
        Debug.LogError("CollectibleUpdate : " + _CollectableDetailsCount);
        _CollectibleNameText.text = mCollectibleName;
        _CollectibleNameText2.text = mCollectibleName + "?";
        _CollectibleDetailsText.text = mDiscription;
        _TopBar.fillAmount = mCurrentValue / targetValue;



      //  _2DImage.GetComponent<Image>().sprite = CollectableHandler.mInstance.gameNotificationSprite[_CollectableDetailsCount];

        _2DImage.GetComponent<Image>().sprite = CollectableHandler.mInstance.gameNotificationSprite[CollectableHandler.mInstance.IdSprite(CollectableHandler.mInstance.allTasks[_CollectableDetailsCount].id)];
    }

}

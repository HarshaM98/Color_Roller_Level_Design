using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image playerDp, tier;
    public HorizontalLayoutGroup nameTierGroup;
    public Text playerName,cashWinnings,tokenWinnings;
    //public GameObject winnerObject;
    //public Text scoreLabel;
    public Text score,rankText;
    public GameObject rewards1, rewards2, rewardsParent, plus,cantPlayMessage,playAgain1Message,playAgain2Message;
    public Button invalidScore;
    public Button fraudScoreRejctedButton;
    public GameObject loader;
    public int rank;
    public GameObject playerHighlighter;
    public Sprite defaultDp;
    public Text fraudButtonText;
    public Image profileImageOverlay;
    public Image profileImageStroke;

    public Color white;
    public Color Red;
    public GameObject syncLoader;

    private ExtraInfo fraudDetails;
    private FraudInfo fraudInfo;
    FraudPopupController fraudPopupController;
    private int playerID;
    void Start()
    {
        
    }
    public void EnablePlayerHighlighter(bool toggle)
    {
        playerHighlighter.SetActive(toggle);
    }
   

    public void ResetPlayerProfile()
    {
        playerName.text = "";
        playerDp.sprite = defaultDp;
       
    }
    public void ResetResultPlayerProfile()
    {
       
        profileImageOverlay.gameObject.SetActive(false);
        profileImageStroke.color = white;
        fraudScoreRejctedButton.onClick.RemoveListener(OnFraudButtonClick);
    }

    public void SetFraudDetails(int id,ExtraInfo extraInfo,FraudPopupController fraudPopupController)
    {
        fraudDetails = extraInfo;
        fraudInfo = null;

        if (!string.IsNullOrEmpty(fraudDetails.fraudInfo))
        {

            fraudInfo = JsonUtility.FromJson<FraudInfo>(extraInfo.fraudInfo);

        }

        this.fraudPopupController = fraudPopupController;
        playerID = id;
        fraudScoreRejctedButton.onClick.AddListener(OnFraudButtonClick);
    }

    public void SetGamePlayCorrected(bool isGamePlayCorrected)
    {
        if (isGamePlayCorrected)
        {
            if (!MPLController.Instance.IsAsyncGame())
                return;

            fraudButtonText.text = "Score Rejected for Disconnection";
            fraudScoreRejctedButton.gameObject.SetActive(true);
            fraudScoreRejctedButton.interactable = false;
            profileImageStroke.color = Red;
        }
    }
    public void SetFraudDetected()
    {
     
 
            fraudButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("MPL FAIRPLAY Rejected Score");
        fraudScoreRejctedButton.gameObject.SetActive(true);
            profileImageOverlay.gameObject.SetActive(true);
        fraudScoreRejctedButton.interactable = true;
            profileImageStroke.color = Red;
        
    }

    private void OnFraudButtonClick()
    {
        if (playerID == MPLController.Instance.gameConfig.Profile.id)
        {


            fraudPopupController.Initialise(playerDp.sprite, true, fraudDetails.blockStatus, fraudInfo.blockType, fraudInfo.blockDuration, fraudDetails.fraudDetectionMessage);
        }
        else
        {
            fraudPopupController.Initialise(playerDp.sprite, false, fraudDetails.blockStatus, fraudInfo.blockType, fraudInfo.blockDuration, fraudDetails.fraudDetectionMessage, playerName.text);
        }
        fraudPopupController.SetActivePopup(true);
    }
}

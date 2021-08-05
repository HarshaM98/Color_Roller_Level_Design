using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FraudPopupController : MonoBehaviour
{

    public Image profileImage;
    public Image warningImage;

    public Text popupTitle;
    public Text popupContent_1;
    public GameObject popupContentParent_1;
    public GameObject popupContentParent_2;
    public Text popupContent_2;

    public GameObject warningMessage;

    public Button policyButton;

    public Button exitButton;
    public Text exitButtonText;

    private bool isFraudster;

    [ContextMenu("DEBUG LOCALISATION")]
    public void DebugLocalisation()
    {
        popupTitle.text = string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("You are banned for {0} days for cheating"), 1);
    }
    public void Initialise(Sprite playerDP, bool isFraudster,string blockStatus ,string blockType,int blockDuration,string messageContent,string playerName = "")
    {
     
        this.isFraudster = isFraudster;
          warningMessage.gameObject.SetActive(true);
        profileImage.sprite = playerDP;
        profileImage.gameObject.SetActive(true);
        warningImage.gameObject.SetActive(false);
        popupContentParent_2.gameObject.SetActive(true);
        if (isFraudster)
        {
            if (blockStatus!=null &&  blockStatus.ToUpper() == "WARNING")
            {
                popupTitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("Warning for Cheating!");
                profileImage.gameObject.SetActive(false);
                warningImage.gameObject.SetActive(true);
                exitButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("Back To Game");
                exitButton.onClick.AddListener(() => { SetActivePopup(false); });

            }
            else if (blockStatus!=null && blockStatus.ToUpper() == "BLOCK" )
            {

             
                if (blockType != null && blockType.ToUpper() == "TEMPORARY" )
                {
                  
                    popupTitle.text =   string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("You are banned for {0} days for cheating"), blockDuration);
                    //popupTitle.text = "You are banned for " + blockDuration + " days for cheating";

                }
                else if (blockType!=null && blockType.ToUpper() == "PERMANENT")
                {
                    popupTitle.text = MPLController.Instance.localisationDetails.GetLocalizedText("You have been banned from MPL");
                    warningMessage.gameObject.SetActive(false);
                }
                exitButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("Exit Game");
            }
            
            popupContent_2.text = messageContent;
            popupContentParent_1.gameObject.SetActive(true);
        }
        else
        {
            exitButton.onClick.AddListener(() => { SetActivePopup(false); });
            exitButtonText.text = MPLController.Instance.localisationDetails.GetLocalizedText("Back To Game");
            popupTitle.text =   string.Format(MPLController.Instance.localisationDetails.GetLocalizedText("{0}'s score has been rejected for cheating"), playerName);
            //popupTitle.text = playerName + "'s score has been rejected for cheating";
            if (blockStatus != null && blockStatus.ToUpper() == "WARNING")
            {
                //popupContentParent_2.gameObject.SetActive(false);
                popupContent_2.text = MPLController.Instance.localisationDetails.GetLocalizedText("The player has been given a warning for cheating");
            }
            else if (blockStatus != null && blockStatus.ToUpper() == "BLOCK")
            {
                if (blockType != null && blockType.ToUpper() == "TEMPORARY")
                {
                    popupContent_2.text = MPLController.Instance.localisationDetails.GetLocalizedText("The player has been temporarily banned from MPL");

                }
                else if (blockType != null && blockType.ToUpper() == "PERMANENT")
                {
                    popupContent_2.text = MPLController.Instance.localisationDetails.GetLocalizedText("The player has been permanently banned from MPL");
                    warningMessage.gameObject.SetActive(false);
                }
                //popupContent_2.text = MPLController.Instance.gameConfig.FraudBlockMessage;
            }

            popupContentParent_1.gameObject.SetActive(false);
          
        }
    }

    public bool IsFraudster()
    {
        return isFraudster;

    }

    public void ResetPopup()
    {
        if(gameObject.activeSelf)
             gameObject.SetActive(false);
    }
    public void SetActivePopup(bool toggle)
    {
         gameObject.SetActive(toggle);
        if (toggle)
        {


            if (isFraudster)
                MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Fraudster Pop Up", popupTitle.text, popupContent_1.text + popupContent_2.text);
            else
                MultiplayerGamesHandler.Instance.SubmitBattlePopupShownEvent("Genuine Player Pop Up", popupTitle.text, popupContent_1.text + popupContent_2.text);
        }

    }
}

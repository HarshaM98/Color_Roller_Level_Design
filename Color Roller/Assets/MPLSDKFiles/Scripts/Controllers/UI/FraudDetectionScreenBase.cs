using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FraudDetectionScreenBase : MonoBehaviour
{
 
    public Button exitButton;
    public Button popupPolicyButton;

    private void OnEnable()
    {
        exitButton.onClick.AddListener(MPLController.Instance.ExitButtonClick);
        popupPolicyButton.onClick.AddListener(MPLController.Instance.GoToFraudPolicyButtonClick);
    }
    private void OnDisable()
    {
        exitButton.onClick.RemoveAllListeners();
        popupPolicyButton.onClick.RemoveAllListeners();
    }
}

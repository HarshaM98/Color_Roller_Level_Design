using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyImageTranslator : MonoBehaviour {

    // Use this for initialization
    public bool shouldBeDiamond = false;
    void Start () {
		
	}

    private void OnEnable()
    {
        if (MPLController.Instance.IsItIndo())
        {

            if (shouldBeDiamond/*gameObject.name == "UpsellBattleCurrencyImage1" || gameObject.name == "UpsellBattleCurrencyImage2"*/)
            {
                gameObject.GetComponent<Image>().sprite = MPLController.Instance.diamondImage;
            }
            else if (MPLController.Instance.gameConfig.EntryCurrency == "TOKEN" && MPLController.Instance.gameConfig.WinningCurrency!=null && MPLController.Instance.gameConfig.WinningCurrency == "CASH") {
                gameObject.GetComponent<Image>().sprite = MPLController.Instance.diamondImage;
            }
            else
            {
                if (MPLController.Instance.gameConfig.EntryCurrency == "TOKEN")
                {
                    gameObject.GetComponent<Image>().sprite = MPLController.Instance.tokenImage;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = MPLController.Instance.diamondImage;
                }
            }

        }
    }
}

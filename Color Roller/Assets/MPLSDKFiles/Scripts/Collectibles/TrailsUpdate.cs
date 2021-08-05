using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailsUpdate : MonoBehaviour
{
    public Image _image;
    public Text _PlayerName,_PlayerTopName;

    public GameObject _TrialsBG;
    public GameObject _GameOverIcon;
    public GameObject _GameOverPlayerName;
    // Start is called before the first frame update
    void OnEnable()
    {
        _image.GetComponent<Image>().sprite = CollectableHandler.mInstance.ShowPopUp2DGameOver();
        _PlayerName.text = CollectableHandler.mInstance._CollectableName.ToUpper();
        _PlayerTopName.text = CollectableHandler.mInstance._CollectableName + "!";

        if(_TrialsBG != null)
        _TrialsBG.SetActive(true);
        _GameOverIcon.GetComponent<Image>().sprite = CollectableHandler.mInstance.ShowPopUp2DGameOver();
        _GameOverPlayerName.GetComponent<Text>().text = CollectableHandler.mInstance._CollectableName + " Applied";

    }
  

    // Update is called once per frame
    void Update()
    {
        
    }
}

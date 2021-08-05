using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class MPLCanvasController : MonoBehaviour
{
    public MPLTutorialScreenController tutorialScreenController;
    public Image sponser,sponserSquare;
   
    



    public void ShowTutorial(MPLGameConfig gameConfig)
    {

        tutorialScreenController.gameObject.SetActive(true);
        tutorialScreenController.ShowTutorial(gameConfig);
    }
    public void DontShowTutorial(MPLGameConfig gameConfig)
    {
       
        tutorialScreenController.DontShowTutorial(gameConfig);
    }
 
            
}
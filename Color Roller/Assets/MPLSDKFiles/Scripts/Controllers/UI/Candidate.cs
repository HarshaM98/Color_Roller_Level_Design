using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Candidate
{
    public Sprite displayPic,tier;
    public Text scoreText;
    public string name;
    public string gameLog;
    public bool isScoreSubmitted = false;
    


    public Candidate()
    {
        this.displayPic = MultiplayerGamesHandler.Instance.defaultimagecircle;
        this.tier= MultiplayerGamesHandler.Instance.defaultimagecircle;
        this.name = "";
        this.isScoreSubmitted = false;
    }

    public Candidate(Sprite Dp, Sprite Tier, Text ScoreText)
    {
        this.displayPic = Dp;
        this.tier = Tier;
        this.scoreText = ScoreText;
        this.isScoreSubmitted = false;
    }

    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorRoller { 
    public class TimeBar : MonoBehaviour
    {
        public Text TimeText;
        public AudioSource FastTicking, FinalAlarm;
        public Image Mask;

        private bool tenSecMarkReached = false;
        private bool endReached = false;

        private void Start()
        {
            GetComponent<Image>().color = BackgroundColorChanger.selectedColor;
        }

        private void Update()
        {
            if (GameManager.singleton.StartGame && !GameManager.singleton.PauseGame)
            {
                if(GameManager.singleton.TimeLeft <= 10 && !tenSecMarkReached)
                {
                    FastTicking.Play();
                    tenSecMarkReached = true;
                }
                if (GameManager.singleton.TimeLeft > 0)
                {
                    GameManager.singleton.TimeLeft -= Time.deltaTime;
                    GameManager.singleton.TimeLeft = GameManager.singleton.TimeLeft < 0 ? 0 : GameManager.singleton.TimeLeft;
                    Mask.fillAmount = (float)GameManager.singleton.TimeLeft /(float)GameManager.singleton.MaxTime;
                    TimeText.text = GameManager.singleton.TimeLeft.ToString("F2") + "s";

                    //string minutes = Mathf.Floor(GameManager.singleton.TimeLeft / 60).ToString("#0");
                    //string seconds = Mathf.Floor(GameManager.singleton.TimeLeft % 60).ToString("00");

                    //TimeText.text = string.Format("{0}:{1}", minutes, seconds);
                }
                else
                {
                    if (!GameManager.singleton.endReached)
                    {
                        FastTicking.Stop();
                        FinalAlarm.Play();
                        GameManager.singleton.endReached = true;
                        GameManager.singleton.StartGame = false;
                    }
                }
            }
        }
    }
}

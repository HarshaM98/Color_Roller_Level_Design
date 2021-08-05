using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorRoller { 
    public class points : MonoBehaviour
    {
        public Text PointsTxt;

        private void Update()
        {
            PointsTxt.text = GameManager.points.ToString();
        }
    }
}
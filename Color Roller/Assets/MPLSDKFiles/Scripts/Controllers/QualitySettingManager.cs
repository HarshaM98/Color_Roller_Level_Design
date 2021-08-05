using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualitySettingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        

        if (MPLController.Instance.gameConfig.GameId == 5 || MPLController.Instance.gameConfig.GameId == 1000005) // RunOut
        {

            Time.fixedDeltaTime = 0.008f;

        }
        else if (MPLController.Instance.gameConfig.GameId == 1000084) // Cricket Clash
        {


            QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadowProjection = ShadowProjection.StableFit;
            QualitySettings.shadowDistance = 15;
            QualitySettings.shadowNearPlaneOffset = 3;
            QualitySettings.shadowCascades = 0;



        }
    

    }
    // Update is called once per frame
     void Update()
    {
        
    }
}

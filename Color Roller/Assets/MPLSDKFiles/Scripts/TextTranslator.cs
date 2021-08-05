using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextTranslator : MonoBehaviour {


 //public string TextId;
	// Use this for initialization

	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
    void OnEnable()
    {
        if (MPLController.Instance.IsItIndo())
        {
            var textObj = gameObject.GetComponent<Text>();

            textObj.text = MPLController.Instance.localisationDetails.GetLocalizedText(textObj.text);//Translations.INDO_STRINGS[TextId];

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFrameUpdate : MonoBehaviour
{
    public List<Sprite> sprites;
    public int mStartCount = 0;
    public Image _AnimationImage;
    // Start is called before the first frame update
    void Awake()
    {
        _AnimationImage = gameObject.GetComponent<Image>();
    }
    private void OnEnable()
    {
        StartCoroutine("StartAnimation");
    }


    IEnumerator StartAnimation()
    {
       
        while(true)
        {
            yield return new WaitForSeconds(0.05f);
            mStartCount++;
            if (mStartCount >= sprites.Count)
                mStartCount = 0;
            _AnimationImage.sprite = sprites[mStartCount];
            
        }
      
      
    }
    private void OnDisable()
    {
        StopCoroutine("StartAnimation");
    }
}

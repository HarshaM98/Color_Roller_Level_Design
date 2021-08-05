using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeObjectController : MonoBehaviour
{
	// Start is called before the first frame update
	public Text countText, winText;
    public Image currencyImage;
    public Sprite cashImage, tokenImage;
    public GameObject crownImage;

    private RectTransform winTextRectTransform;
    private RectTransform itemRectTransform;
    private HorizontalLayoutGroup crownParentHorizontalLayout;
  
    const int PRIZE_OFFSET = 44;

    public bool isAdPrizeObjectLandscape;

    private void OnEnable()
    {
        if (MPLController.Instance.gameConfig.EntryCurrency == "TOKEN")
        {
            currencyImage.sprite = tokenImage;
        }
        else
        {
            currencyImage.sprite = cashImage;
        }
        AdjustPrizeSize();
    }
    void Start()
    {

        winTextRectTransform = winText.GetComponent<RectTransform>();
        itemRectTransform = gameObject.GetComponent<RectTransform>();

        crownParentHorizontalLayout = crownImage.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();
    }
    public void EnableCrownimage(int rank)
    {
       
        if (rank == 1)
            crownImage.SetActive(true);
        else
        {
            countText.color = Color.white;
            crownImage.SetActive(false);
            countText.gameObject.transform.position = new Vector3(0, countText.gameObject.transform.position.y, countText.gameObject.transform.position.z);
            countText.alignment = TextAnchor.MiddleCenter;
        }
      
    }

    [ContextMenu("DEBUG ADJUST SIZE")]
    public void AdjustPrizeSize()
    {
        //crownImage.transform.parent.gameObject.SetActive(false);
            StartCoroutine(AdjustPrizeSizeCoroutine());

    }
   
    IEnumerator  AdjustPrizeSizeCoroutine()
    {
       
        yield return new WaitForEndOfFrame();
        crownParentHorizontalLayout.enabled = true;
        if (isAdPrizeObjectLandscape)
        {
            this.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
        else
        {
           
        var v_amountWidth = winTextRectTransform.rect.width;
        var v_itemHeight = itemRectTransform.rect.height;
        v_amountWidth += PRIZE_OFFSET;
        itemRectTransform.sizeDelta = new Vector2(v_amountWidth, v_itemHeight);
           
        }

    }
}

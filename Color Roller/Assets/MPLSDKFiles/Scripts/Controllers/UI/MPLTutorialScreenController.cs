using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class MPLTutorialScreenController : MonoBehaviour
{
    public Canvas mainCanvas;
    public CanvasScaler mainCanvasScaler;
    public GameObject multiplayerCanvas;
    public GameObject tutorialscreenObject, dotObjectPrefab;
    private TutorialTextDetails tutorialTextDetails;
    public GameObject tutorialContainerPrefab;
    public string[] allAssets;
    public ScrollRect tutorialScrollRect;
    public CanvasScaler canvasScaler;
    private List<string> tutStrings;
    public GameObject portraitStuff, landscapeStuff;
    public Sprite nextButtonWhiteSprite, nextButtonGreenSprite;
    public Image overlay;
    public Text portraitInfo, landscapeInfo;
    private Transform dotsParent;
    private Text skipButtonText;
    private Image gameLogoImg, gamePreviewImg;
    private Button skipButton;
    private List<RectTransform> screens;
    private List<float> screenPoints;
    private float snapScreenPoint;
    private bool snap;
    private List<Image> dots;

    private string gameAssetsDirectory;
    private Color nextButtonTextDefaultColor, nextButtonTextFinalColor;
    private Sprite gameLogoSprite;
    private Sprite gameBgPreviewSprite;
    private List<Sprite> gameTutorialSprites;

    public List<Sprite> gameNotificationSprite;
    public List<Sprite> gameIconSprite;

    public List<GameObject> gameNotification3D;

    public AssetBundle gamePreviewAssetBundle;
    private bool tutorialShown;
    private MPLGameConfig gameConfig;

    private static readonly Vector2 PORTRAIT_CARD_RES = new Vector2(336f, 488f);
    private static readonly Vector2 LANDSCAPE_CARD_RES = new Vector2(544f, 280f);

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
           

            Swipe(MPLGestureRecognizer.SwipeDirection.Right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            Swipe(MPLGestureRecognizer.SwipeDirection.Left);
        }
        if (!snap) return;
        ScrollTutorialScrollViewTo(snapScreenPoint);

        

    }

    void InitData()
    {
        
        mainCanvasScaler = mainCanvas.GetComponent<CanvasScaler>();
        dotsParent = GameObject.Find("Dots").transform;
        


        skipButton = GameObject.Find("SkipButton").GetComponent<Button>();

        skipButtonText = skipButton.transform.Find("SkipButtonText").GetComponent<Text>();

        gameLogoImg = GameObject.Find("GameLogo").GetComponent<Image>();
        gamePreviewImg = transform.Find("PreviewBG").GetComponent<Image>();

        nextButtonTextDefaultColor = skipButtonText.color;
        nextButtonTextFinalColor = Color.red;

        gameTutorialSprites = new List<Sprite>();

        MPLGestureRecognizer.Swipe += Swipe;
        if (MPLController.Instance.IsItIndo())
        {
            portraitInfo.text = "Cara Bermain";
            landscapeInfo.text = "Cara Bermain";
            
        }
        else
        {
            portraitInfo.text = "How To Play";
            landscapeInfo.text = "How To Play";
            
        }
    }

    void Swipe(MPLGestureRecognizer.SwipeDirection direction)
    {
        //Next card
        if (direction == MPLGestureRecognizer.SwipeDirection.Left)
        {
            Scroll(true);
        }
        //Prev card
        if (direction == MPLGestureRecognizer.SwipeDirection.Right)
        {
            Scroll(false);
        }
    }

    void ExtractPreviewData(int gameId)
    {
       
        
        
        

        

        
        
        
            //Logo
            //if (allAssets[i].ToLower().Contains(previewAssetsLogoPath.ToLower())) gameLogoSprite = gamePreviewAssetBundle.LoadAsset<Sprite>(allAssets[i]);

            //Logo Type
            gameLogoSprite = MPLController.Instance.gameLogoTypeSprite;

            //BG Preview
            
                gameBgPreviewSprite = MPLController.Instance.backgroundImage;


        //Tutorials
        gameTutorialSprites = MPLController.Instance.tutorialCards;

            //TutorialText
            
            
           

           

        


       
            
   
        CollectableHandler.mInstance.Init(gameNotificationSprite, gameIconSprite);


    }
    
    void SetGameInfo(MPLGameConfig gameConfig)
    {
        gamePreviewImg.sprite = gameBgPreviewSprite;
        if (gameLogoSprite != null)
        {
            gameLogoImg.sprite = gameLogoSprite;
        }
        else
        {
            gameLogoImg.gameObject.SetActive(false);
        }
        
        gamePreviewImg.GetComponent<AspectRatioFitter>().aspectRatio = (float)gameBgPreviewSprite.texture.width / (float)gameBgPreviewSprite.texture.height;
    }

    public void ShowTutorial(MPLGameConfig gameConfig)
    {
        
        RectTransform rt = tutorialscreenObject.GetComponent<RectTransform>();

       // multiplayerCanvas.SetActive(false);
        this.gameConfig = gameConfig;

       

        if (!MPLController.Instance.isLandscape) ShowTutorialPortrait();
        else ShowTutorialLandscape();

        StartCoroutine(ScaleAndShowTutorial());
    }
    public void DontShowTutorial(MPLGameConfig gameConfig)
    {
       RectTransform rt = tutorialscreenObject.GetComponent<RectTransform>();

        //multiplayerCanvas.SetActive(false);
        
        this.gameConfig = gameConfig;
        

        if (!MPLController.Instance.isLandscape) DontShowTutorialPortrait();
        else DontShowTutorialLandscape();

        StartCoroutine(ScaleAndDontShowTutorial());
    }

    public void SkipTutorial()
    {
        tutorialScrollRect.gameObject.SetActive(false);
        portraitStuff.SetActive(false);
        landscapeStuff.SetActive(false);
        MPLController.Instance.StartGame();
    }
    IEnumerator ScaleAndDontShowTutorial()
    {
        yield return new WaitForSeconds(1f);
        
      
        MPLController.Instance.StartGame();

    }
    IEnumerator ScaleAndShowTutorial()
    {
        yield return new WaitForSeconds(1f);
        
         overlay.gameObject.SetActive(false);
/*           if(!MPLController.Instance.isGameLoadedSent)
        {
            MPLController.Instance.OnGameLoaded();
        }*/
        
            InitData();
        ExtractPreviewData(gameConfig.GameId);
            SetGameInfo(gameConfig);
            screens = new List<RectTransform>();
            screenPoints = new List<float>();
            dots = new List<Image>();
            Vector2 canvasRes = canvasScaler.GetComponent<RectTransform>().sizeDelta;
        //Spawn Dots
        
        for (int i = 0; i < gameTutorialSprites.Count; i++)
            {
            //Spawn and transform Tutorial Containers appropriately
            GameObject instance = Instantiate(tutorialContainerPrefab) as GameObject;
            instance.transform.parent = tutorialScrollRect.content;

                //Tutorial container
                RectTransform tutorialContainer = instance.GetComponent<RectTransform>();
                tutorialContainer.sizeDelta = canvasRes;
                Vector3 pos = new Vector3(i * tutorialContainer.sizeDelta.x, 0);
                tutorialContainer.anchoredPosition = pos;
                tutorialContainer.localScale = Vector3.one;

                RectTransform card;
                if (!MPLController.Instance.isLandscape)
                {
                    tutorialContainer.Find("LandscapeCard").gameObject.SetActive(false);
                    card = tutorialContainer.Find("PortraitCard").GetComponent<RectTransform>();
                }
                else
                {
                    tutorialContainer.Find("PortraitCard").gameObject.SetActive(false);
                    card = tutorialContainer.Find("LandscapeCard").GetComponent<RectTransform>();
                }

                Image tutorialImg = card.Find("Image").GetComponent<Image>();
                tutorialImg.sprite = gameTutorialSprites[i];

                screens.Add(tutorialContainer);
            GameObject dotInstance = Instantiate(dotObjectPrefab) as GameObject;

            dotInstance.transform.parent = dotsParent;
            dotInstance.transform.localScale = Vector3.one;
            dots.Add(dotInstance.GetComponent<Image>());

        }

            //Resize tutorials scroll rect size x to fit all Tutorial Containers
            Vector2 size = tutorialScrollRect.content.GetComponent<RectTransform>().sizeDelta;
            size.x = tutorialScrollRect.content.childCount * canvasRes.x;
            tutorialScrollRect.content.GetComponent<RectTransform>().sizeDelta = size;

            float step = 1 / (float)(screens.Count - 1);
            for (int i = 0; i < screens.Count; i++)
            {
                screenPoints.Add(i * step);
            }

            ScrollTutorialScrollViewTo(0);
        
        }

   


    public void ShowTutorialLandscape()
    {
       

       
            canvasScaler.referenceResolution = Globals.LANDSCAPE_RES;
            portraitStuff.SetActive(false);
            landscapeStuff.SetActive(true);

    }

    public void ShowTutorialPortrait()
    {
        

       
       
            canvasScaler.referenceResolution = Globals.PORTRAIT_RES;
            landscapeStuff.SetActive(false);
            portraitStuff.SetActive(true);

    }
    public void DontShowTutorialLandscape()
    {

      
            canvasScaler.referenceResolution = Globals.LANDSCAPE_RES;

    }

    public void DontShowTutorialPortrait()
    {


        canvasScaler.referenceResolution = Globals.PORTRAIT_RES;
    }
    
    void ScrollTutorialScrollViewTo(float snapPoint)
    {
        tutorialScrollRect.horizontalNormalizedPosition = Mathf.Lerp(tutorialScrollRect.horizontalNormalizedPosition, snapScreenPoint, 100 * tutorialScrollRect.elasticity * Time.deltaTime);
        if (Mathf.Abs(tutorialScrollRect.horizontalNormalizedPosition - snapScreenPoint) < 0.0001f)
        {
            snap = false;
        }

        //Update dots
        int snapPointDotIndex = screenPoints.IndexOf(snapPoint);

        

        
        for (int i = 0; i < dots.Count; i++)
        {
            if (snapPointDotIndex == i) continue;
            Color color = Color.white;
            color.a = 0.25f;
            dots[i].color = color;
        }
        
        //Update next text
        dots[snapPointDotIndex].color = Color.white;
        //dots[2].gameObject.SetActive(true);


        skipButtonText.color = nextButtonTextDefaultColor;
        
        skipButtonText.text = "SKIP";
        //Final page
        if (snapPointDotIndex == dots.Count - 1)
        {
            
            if (MPLController.Instance.IsItIndo())
            {
                skipButtonText.text = "MAIN";
            }
            else
            {
                skipButtonText.text = "PLAY";
            }
            
            
            skipButtonText.color = nextButtonTextFinalColor;
            //dots[2].gameObject.SetActive(false);
        }

        if (snapPointDotIndex == 0)
        {
           // dots[0].gameObject.SetActive(false);
            dots[snapPointDotIndex].color = Color.white;

        }
        else
        {
           // dots[0].gameObject.SetActive(true);
        }
    }

    public void NextTutorialAction()
    {
        if (screens == null || screens.Count == 0) return;

        int snapPointDotIndex = screenPoints.IndexOf(snapScreenPoint);

        //START GAME
        if (snapPointDotIndex == dots.Count - 1)
        {
           
            try
            {
                MPLGestureRecognizer.Swipe -= Swipe;
            }
            catch (Exception e)
            {
                Debug.Log("Can't deregister from MPLGestureRecognizer.Swipe: " + e);
            }

            MPLController.Instance.StartGame();
            return;
        }
        Scroll(true);
    }

    public void BackTutorialAction()
    {
        if (screens == null || screens.Count == 0) return;
        Scroll(false);
    }

    private void Scroll(bool right)
    {
        if (screens == null || screens.Count == 0) return;

        int snapPointDotIndex = screenPoints.IndexOf(snapScreenPoint);

        if ((!right && snapPointDotIndex == 0) || (right && snapPointDotIndex == screenPoints.Count - 1))
        {
            return;
        }

        if (right) snapPointDotIndex++;
        else snapPointDotIndex--;
        snap = true;
        snapScreenPoint = screenPoints[snapPointDotIndex];
    }
}
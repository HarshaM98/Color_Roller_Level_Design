using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColorRoller { 
    public class GameManager : MonoBehaviour
    {
        public static GameManager singleton;

        [HideInInspector]
        public bool PauseGame, StartGame, endReached;
        [HideInInspector]
        public float TimeLeft;
        [HideInInspector]
        public static int points = 0;

        public float MaxTime = 120;
        public Animator MainPanel;
        public Text StartSequenceText;

        private GroundPiece[] allGroundPieces;
        private AudioSource MyAudio;
        private ParticleSystem Confetti;
        private bool resultSubmitted = false;
        private long startTime, endTime;
        private int noOfLevelsCompleted = 0;
        private const int screenTouchCount = 1;
        private List<levelData> lvlData = new List<levelData>();

        //Level data
        private int levelNo;
        private int blocksAvailable;
        private int blocksColored;
        private int blocksLeft;
        private long timeAlloted;
        private long timeTaken;
        public static int levelScore;
        private long levelStartTime;
        private long levelEndTime;


        [Header("FOR DEVELOPERS: ")]
        public bool devMode = false;
        public bool startParticularLevel = false;
        [Range(1,100)]
        public int startLevel = 1;

        public ColorRollerSessionInfo SessionInfo { get; private set; }

        private void Start()
        {
            devMode = startParticularLevel ? startParticularLevel : devMode;

            if(!devMode) MPLFunctions();

            TimeLeft = MaxTime = (SessionInfo != null && SessionInfo.MaxTime != 0) ? SessionInfo.MaxTime : MaxTime;
            startLevel = (startParticularLevel) ? startLevel : 1;

            if (SessionInfo != null && SessionInfo.QATestingMode)
            {
                startLevel = (SessionInfo.LoadParticularLevel) ? SessionInfo.LevelToLoad : startLevel;
            }

            MyAudio = GetComponent<AudioSource>();
            Confetti = GetComponent<ParticleSystem>();
            SetupNewLevel();
            StartCoroutine("StartSequence");
        }

        private void Update()
        {
            if (!devMode && endReached && !resultSubmitted)
            {
                levelEndTime = (long)Time.time;
                timeTaken = levelEndTime - levelStartTime;
                levelData lvl = new levelData(levelNo, blocksAvailable, blocksColored, blocksLeft, timeAlloted, timeTaken, levelScore);
                lvlData.Add(lvl);

                foreach(levelData l in lvlData)
                {
                    Debug.Log(l.ToString());
                }

                endTime = (long)Time.time;
                Session.Instance.SubmitResult(GetSessionResult(MPLGameEndReason.GameEndReasons.OUT_OF_TIME));
                resultSubmitted = true;
            }

        }

        private void MPLFunctions()
        {
            SessionInfo = Session.Instance.GetSessionInfo<ColorRollerSessionInfo>();

            Session.Instance.Restart += reStartGame;
            Session.Instance.GameEnd += EndGame;
            Session.Instance.OnCreateSessionResult += GetSessionResult;
        }

        private void SetupNewLevel()
        {
            allGroundPieces = FindObjectsOfType<GroundPiece>();
            blocksAvailable = allGroundPieces.Length;
        }

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            SetupNewLevel();
        }

        public void CheckComplete()
        {
            bool isFinished = true;
            blocksLeft = 0;
            blocksColored = 0;

            for (int i = 0; i < allGroundPieces.Length; i++)
            {
                if (allGroundPieces[i].isColored == false)
                {
                    blocksLeft++;
                    isFinished = false;
                    //break;
                }
                else
                {
                    blocksColored++;
                }
            }

            if (isFinished) {
                StartCoroutine("WinSequence");
            }
        }

        private SessionResult GetSessionResult(MPLGameEndReason.GameEndReasons gameEndReason)
        {

            return new ColorRollerSessionResult(
                    SessionInfo.SessionId,
                    SessionInfo.StartTime,
                    startTime,
                    endTime,
                    points,
                    screenTouchCount,
                    noOfLevelsCompleted,
                    lvlData,
                    gameEndReason,
                    (endTime - startTime)
                );
        }

        private void NextLevel()
        {
            levelNo = Random.Range(1, 101);
            levelStartTime = (long)Time.time;
            timeAlloted = (long)TimeLeft;
            levelScore = 0;

            string sceneName = "Level" + levelNo.ToString();
            Debug.Log("Loaded scene " + sceneName);
            SceneManager.LoadScene(sceneName);
        }

        private void ContinousLevels()
        {
            //MaxTime = TimeLeft = 500;
            string sceneName = "Level" + startLevel.ToString();
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator WinSequence()
        {
            MyAudio.Play();
            //PauseGame = true;
            Confetti.Play();
            noOfLevelsCompleted++;
            yield return new WaitForSeconds(0.75f);
            //PauseGame = false;

            levelEndTime = (long)Time.time;
            timeTaken = levelEndTime - levelStartTime;
            levelData lvl = new levelData(levelNo, blocksAvailable, blocksColored, blocksLeft, timeAlloted, timeTaken, levelScore);
            lvlData.Add(lvl);

            if (!devMode || (SessionInfo != null && !SessionInfo.QATestingMode))
                NextLevel();
            else
            {
                startLevel++;
                ContinousLevels();
            }
        }

        IEnumerator StartSequence()
        {
            StartSequenceText.text = 3.ToString();
            yield return new WaitForSeconds(1f);
            StartSequenceText.text = 2.ToString();
            yield return new WaitForSeconds(1f);
            StartSequenceText.text = 1.ToString();
            yield return new WaitForSeconds(1f);
            StartSequenceText.gameObject.SetActive(false);
            StartGame = true;
            PauseGame = false;
            MainPanel.SetTrigger("start");

            if (devMode || (SessionInfo != null && SessionInfo.QATestingMode)) {
                if (startParticularLevel || (SessionInfo != null && SessionInfo.LoadParticularLevel))
                    SceneManager.LoadScene("Level"+startLevel.ToString());
                else ContinousLevels();
            }
            else { 
                startTime = (long) Time.time;
                NextLevel();
            }

        }

        void EndGame(MPLNotificationEventArgs GameEndInfo)
        {
            MPLGameEndReason.GameEndReasons EndReason = GameEndInfo.notification;
            if (GameEndInfo.notification == MPLGameEndReason.GameEndReasons.WENT_IN_BACKGROUND)
            {
                endTime = (long)Time.time;
                levelData lvl = new levelData(levelNo, blocksAvailable, blocksColored, blocksLeft, timeAlloted, timeTaken, levelScore);
                lvlData.Add(lvl);
                Debug.LogError("Reason to end the game: " + EndReason);
                endReached = true;
            }
        }

        void reStartGame()
        {
            points = 0;
            endReached = false;
            Session.Instance.Restart -= reStartGame;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene("GameScene");
        }

        private void OnValidate()
        {
            if (devMode)
            {
                Debug.LogError("devMode is turned on in ColorRoller.GameManager. Please turn it off when building");
            }
        }

    }
}

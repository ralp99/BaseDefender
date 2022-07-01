using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RAGameManager : MonoBehaviour
{

    public static RAGameManager Instance;
    [HideInInspector]
    public EnemyMarchingController enemyMarchingController;
    [HideInInspector]
    public BulletManager bulletManager;
    [HideInInspector]
    public PoolManager poolManager;
    [HideInInspector]
    public UIController uIController;
    [HideInInspector]
    HeroShipController heroShipController;
    [HideInInspector]
    BonusShipManager bonusShipManager;

    public enum CharacterType { Hero, StandardEnemy, BonusShip, Shield}

    public bool RunAtStart;
    public UnityAction FireButtonAction;
    public bool FireButtonPressed;
    public SoManagerValues soManagerValues;

    // ----------------------------

    [HideInInspector]
    public float ShotSpeedMultiplierHero = 1.0f;
    [HideInInspector]
    public float ShotSpeedMultiplierEnemy = 1.0f;
    private float MovementSpeedMultiplierHero = 1.0f;
    private float MovementSpeedMultiplierEnemy = 1.0f;

    private float BeginEnemyMarchSpeed = 0.025f;
    [Space]
    public float CurrentEnemyMarchSpeed = 0.0f;
    [HideInInspector]
    public float EnemyDescendAmount = 0.0f;
    [HideInInspector]
    public float HeroTravelSpeed = 1.0f;
    [HideInInspector]
    public float BonusShipSpeed = 1.0f;


    private float PcntSpdIncOnEnemyDeath = 5.0f;
    private float PcntSpdIncOnRowAdvance = 5.0f;
    private float PcntSpdIncOnEnemyDeathIncs = 0.002f;
    private float PcntSpdIncOnRowAdvanceIns = 0.002f;

    private float EnemySpeedLimit = 1000.0f;

    ///game settings

    public bool GameIsPaused;
    public bool GameplayAvailable;

    // sources
    public GameObject EnemyObjectSource;
    public GameObject EnemyBonusShipObjectSource;
    public GameObject HeroShipTransformSource;
    public GameObject HeroBulletSource;
    public GameObject EnemyBulletSource;
    public GameObject ShieldSource;
    public SoInvaderColumn EnemyColumnSOs;

    //ingame entities
    public Transform GameParent;
    public Transform HeroShipTransform;
    public GameObject RegisteredBonusShip;

    // shields
    private int ShieldAmount = 4;
    private float ShieldPlacementY;
    [HideInInspector]
    public float BonusShipPlacementY;
    public List<GameObject> ShieldList;

    public float GamefieldXMin;
    public float GamefieldXMax;
    public float GamefieldYMin;
    public float GamefieldYMax;

    [HideInInspector]
    public float HorizontalRangeBorder = 5.0f;
    [HideInInspector]
    public float CeilingBorder = 5.0f;
    private float FloorBorder = 5.0f;
    [HideInInspector]
    public int HeroShotLimit = 1;
    [HideInInspector]
    public int EnemyShotLimit = 2;
    [HideInInspector]
    public float EnemyShotDelay = 1.0f;
    [HideInInspector]
    public float EnemyShotJitter = 0.13f;
    [HideInInspector]
    public float EnemiesBecomeSmashers = 1.0f;

    [HideInInspector]
    public float PaddingEnemyX;
    [HideInInspector]
    public float PaddingEnemyY;
    [HideInInspector]
    public Vector2 EnemySpawnBegin;

    [HideInInspector]
    public int EnemyColumns;
    [HideInInspector]
    public int EnemyRows;

    // stats
    private int LivesStart = 3;
    [HideInInspector]
    public float BonusShipDelay;
    [HideInInspector]
    public float BonusShipJitter;
    [HideInInspector]
    public int BonusShipAppearancesPerRound;
    [HideInInspector]
    public int BonusLifeAt = 1000;
    [Space]
    public int LivesRemaining;
    public int CurrentScore;
    public int HiScore;
    public int CurrentGameLevel;
    public int TopGameLevel;

    // pools
    [Space]
    public List<GameObject> EnemyPoolActive = new List<GameObject>();
    public List<GameObject> EnemyPoolDead = new List<GameObject>();
    public List<GameObject> EnemyBulletPoolActive = new List<GameObject>();
    public List<GameObject> EnemyBulletPoolDead = new List<GameObject>();
    public List<GameObject> HeroBulletPoolActive = new List<GameObject>();
    public List<GameObject> HeroBulletPoolDead = new List<GameObject>();

    public List<SoInvaderColumn> InvaderColumns = new List<SoInvaderColumn>();

    public bool CanRunGameLoop = false;
    public bool ResetHiScore;
    [HideInInspector]
    public bool SoValuesInjected;
    private bool awardedBonusLife;

    /// <summary>
    /// notes
    /// 5 rows x 11 columns
    /// </summary>

   
    void GameLoop()
    {

     if (!CanRunGameLoop)
        {
            return;
        }

        enemyMarchingController.PerformAllEnemyPosChecks(true);
        bulletManager.MoveAllBullets();
        CheckForRoundEnd();
    }

    void InjectSoValues()
    {
        ShotSpeedMultiplierHero =  soManagerValues.ShotSpeedMultiplierHero;
        ShotSpeedMultiplierEnemy = soManagerValues.ShotSpeedMultiplierEnemy;
        MovementSpeedMultiplierHero = soManagerValues.MovementSpeedMultiplierHero;
        MovementSpeedMultiplierEnemy = soManagerValues.MovementSpeedMultiplierEnemy;

        BeginEnemyMarchSpeed = soManagerValues.BeginEnemyMarchSpeed;
        EnemyDescendAmount = soManagerValues.EnemyDescendAmount;
        HeroTravelSpeed = soManagerValues.HeroTravelSpeed;
        BonusShipSpeed = soManagerValues.BonusShipSpeed;

        PcntSpdIncOnEnemyDeath = soManagerValues.PcntSpdIncOnEnemyDeath;
        PcntSpdIncOnRowAdvance = soManagerValues.PcntSpdIncOnRowAdvance;
        PcntSpdIncOnEnemyDeathIncs = soManagerValues.PcntSpdIncOnEnemyDeathIncs;
        PcntSpdIncOnRowAdvanceIns = soManagerValues.PcntSpdIncOnRowAdvanceIns;
        
        EnemySpeedLimit = soManagerValues.EnemySpeedLimit;

        ShieldAmount = soManagerValues.ShieldAmount;
        ShieldPlacementY = soManagerValues.ShieldPlacementY;
        BonusShipPlacementY = soManagerValues.BonusShipPlacementY;

        HorizontalRangeBorder = soManagerValues.HorizontalRangeBorder;
        CeilingBorder = soManagerValues.CeilingBorder;
        FloorBorder = soManagerValues.FloorBorder;
        HeroShotLimit = soManagerValues.HeroShotLimit;
        EnemyShotLimit = soManagerValues.EnemyShotLimit;
        EnemyShotDelay = soManagerValues.EnemyShotDelay;
        EnemyShotJitter = soManagerValues.EnemyShotJitter;
        EnemiesBecomeSmashers = soManagerValues.EnemiesBecomeSmashers;
        PaddingEnemyX = soManagerValues.PaddingEnemyX;
        PaddingEnemyY = soManagerValues.PaddingEnemyY;
        EnemySpawnBegin = soManagerValues.EnemySpawnBegin;
        EnemyColumns = soManagerValues.EnemyColumns;
        EnemyRows = soManagerValues.EnemyRows;
        BonusShipDelay = soManagerValues.BonusShipDelay;
        BonusShipJitter = soManagerValues.BonusShipJitter;
        BonusShipAppearancesPerRound = soManagerValues.BonusShipAppearancesPerRound;
        BonusLifeAt = soManagerValues.BonusLifeAt;

        LivesStart = soManagerValues.LivesStart;
        SoValuesInjected = true;
    }


    public void UnlockGameLoop()
    {
        CanRunGameLoop = true;
        bulletManager.DefineBounds();
        bonusShipManager.StartBonusShipTimer();
    }

    void CheckForRoundEnd()
    {
        if (!CanRunGameLoop)
        {
            return;
        }

        if (EnemyPoolActive.Count == 0)
        {
            BeginRound();
        }
    }

    public void GameOver()
    {
        CanRunGameLoop = false;
        int currentHiscore = HiscorePPrefCheck();
        int currentTopGameLevel = TopGameLevelPPrefCheck();

        if (CurrentScore > currentHiscore)
        {
            HiScore = CurrentScore;
            PlayerPrefs.SetInt("baseDefenderHiscore", HiScore);
        }

        if (CurrentGameLevel > currentTopGameLevel)
        {
            TopGameLevel = CurrentGameLevel;
            PlayerPrefs.SetInt("baseDefenderTopGameLevel", TopGameLevel);
        }

        enemyMarchingController.KillShotCache();
        ClearEntities();

        if(RegisteredBonusShip)
        {
            if (RegisteredBonusShip.activeSelf)
            {
                RegisteredBonusShip.SetActive(false);
            }
        }
     
        uIController.UIVisibility(true);
        CurrentGameLevel = 0;
        GameplayAvailable = false;
    }

    void ClearEntities()
    {
        List<GameObject> clearObjectsList = new List<GameObject>();

        for (int i = 0; i < EnemyPoolActive.Count; i++)
        {
            clearObjectsList.Add(EnemyPoolActive[i]);
        }

        for (int i = 0; i < EnemyBulletPoolActive.Count; i++)
        {
            clearObjectsList.Add(EnemyBulletPoolActive[i]);
        }

        for (int i = 0; i < HeroBulletPoolActive.Count; i++)
        {
            clearObjectsList.Add(HeroBulletPoolActive[i]);
        }

        for (int i = 0; i < clearObjectsList.Count; i++)
        {
            clearObjectsList[i].SetActive(false);
        }

        DestroyHeroTransform();
        bonusShipManager.CancelBonusShipTimer();
    }

    void DestroyHeroTransform()
    {
        if (HeroShipTransform)
        {
            Destroy(HeroShipTransform.gameObject);
        }
    }


    public void HeroKilled()
    {
        LivesRemaining--;

        // see if game over, or if next round
        if (LivesRemaining >= 0)
        {
            CreateHeroShipTransform();
        }
        else
        {
            GameOver();
        }
    }



    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        InjectSoValues();
        enemyMarchingController = GetComponent<EnemyMarchingController>();
        bulletManager = GetComponent<BulletManager>();
        poolManager = GetComponent<PoolManager>();
        heroShipController = GetComponent<HeroShipController>();
        bonusShipManager = GetComponent<BonusShipManager>();
        uIController = GetComponent<UIController>();
        uIController.UIVisibility(true);

        if (ResetHiScore)
        {
            PlayerPrefs.SetInt("baseDefenderHiscore", 0);
            PlayerPrefs.SetInt("baseDefenderTopGameLevel", 0);
        }

        HiScore = HiscorePPrefCheck();
        TopGameLevel = TopGameLevelPPrefCheck();

        if (RunAtStart)
        {
            RestartGame();
        }

    }


    public void RestartGame()
    {
        LivesRemaining = LivesStart;
        CurrentScore = 0;
        awardedBonusLife = false;
        CurrentEnemyMarchSpeed = BeginEnemyMarchSpeed;
        BeginRound();
        heroShipController.HeroShipInit();
        uIController.UIVisibility(false);
    }



    void BeginRound()
    {
        InvaderColumns.Clear();
        StartCoroutine(enemyMarchingController.SpawnEnemySet());
        CreateHeroShipTransform();
        StartCoroutine(CreateShields());
        CurrentEnemyMarchSpeed = BeginEnemyMarchSpeed;
        bonusShipManager.ResetAppearanceCounter();
        CurrentGameLevel++;
        if (CurrentGameLevel > 1)
        {
            PcntSpdIncOnEnemyDeath = PcntSpdIncOnEnemyDeath + PcntSpdIncOnEnemyDeathIncs;
            PcntSpdIncOnRowAdvance = PcntSpdIncOnRowAdvance + PcntSpdIncOnRowAdvanceIns;
        }
    }



    private void Update()
    {
        if (FireButtonPressed)
        {
            FireButtonPressed = false;
            FireButtonAction();
        }

        if (GameIsPaused)
        {
            GameplayAvailable = false;
        }

        if (CanRunGameLoop)
        {
            GameplayAvailable = true;
        }

        if (!GameplayAvailable)
        {
            return;
        }

        GameLoop();
    }


    void CreateHeroShipTransform()
    {
        DestroyHeroTransform();
        GameObject newHeroObject = null;
        newHeroObject = Instantiate(HeroShipTransformSource) as GameObject;
        HeroShipTransform = newHeroObject.transform;
        HeroShipTransform.SetParent(GameParent);
    }

    IEnumerator CreateShields()
    {

        while (GamefieldXMax == 0)
        {
            yield return null;
        }

        // reactivate existing shields
        if (ShieldList.Count > 0)
        {
            for (int i = 0; i < ShieldList.Count; i++)
            {
                Transform currentShield = ShieldList[i].transform;
                foreach (Transform child in currentShield)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        else
        {
            // instantiating new shields

            // get shields Y position
            float useYplacement = Mathf.Lerp(GamefieldYMin, GamefieldYMax, ShieldPlacementY);

            // populate a list of shields X positions
            List<float> ShieldXposList = new List<float>();
            float wholeFieldLength = GamefieldXMax - GamefieldXMin;
            float xPos = wholeFieldLength / ShieldAmount;

            for (int i = 0; i < ShieldAmount; i++)
            {
                float newUsePos = 0;
                if (i == 0)
                {
                    newUsePos = (xPos + GamefieldXMin) - (xPos / 2);
                }
                else
                {
                    newUsePos = ShieldXposList[i - 1] + xPos;
                }

                ShieldXposList.Add(newUsePos);
            }

            for (int i = 0; i < ShieldXposList.Count; i++)
            {
                GameObject newShield = Instantiate(ShieldSource) as GameObject;
                Transform shieldTransform = newShield.transform;
                ShieldList.Add(newShield);

                shieldTransform.SetParent(GameParent);
                shieldTransform.localPosition = new Vector3(ShieldXposList[i], useYplacement, 0);
            }
        }
    }




    int HiscorePPrefCheck()
    {
        return PlayerPrefs.GetInt("baseDefenderHiscore");
    }

    int TopGameLevelPPrefCheck()
    {
        return PlayerPrefs.GetInt("baseDefenderTopGameLevel");
    }

    public void AddToScore(int addValue)
    {
        CurrentScore += addValue;
        if (CurrentScore > BonusLifeAt && !awardedBonusLife)
        {
            LivesRemaining++;
            awardedBonusLife = true;
        }

        if (HiScore < CurrentScore)
        {
            HiScore = CurrentScore;
        }

        enemyMarchingController.EvaluateBottomRowEnemies();
    }

    public void IncreaseEnemySpeedAtDeath()
    {
        float newSpeed = PcntSpdIncOnEnemyDeath;
        IncreaseEnemySpeedByPercent(newSpeed);
    }

    public void IncreaseEnemySpeedAtRowAdvance()
    {
        float newSpeed = PcntSpdIncOnRowAdvance;
        IncreaseEnemySpeedByPercent(newSpeed);
    }

    private void IncreaseEnemySpeedByPercent(float increasePercent)
    {
        float increaseAmount = (increasePercent * 0.1f) * CurrentEnemyMarchSpeed;

        CurrentEnemyMarchSpeed = CurrentEnemyMarchSpeed + increaseAmount;
        if (CurrentEnemyMarchSpeed > EnemySpeedLimit)
        {
            CurrentEnemyMarchSpeed = EnemySpeedLimit;
        }
    }



    public void ChangePauseState()
    {
        GameIsPaused = !GameIsPaused;
    }

}

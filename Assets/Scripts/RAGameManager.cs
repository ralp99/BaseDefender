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

    public enum CharacterType { Hero, StandardEnemy, BonusShip, Shield}

    public UnityAction FireButtonAction;
    public bool FireButtonPressed;

    // ----------------------------

    public float ShotSpeedMultiplierHero = 1.0f;
    public float ShotSpeedMultiplierEnemy = 1.0f;
    public float MovementSpeedMultiplierHero = 1.0f;
    public float MovementSpeedMultiplierEnemy = 1.0f;

    [Space]
    public float BeginEnemyMarchSpeed = 0.025f;
    public float CurrentEnemyMarchSpeed = 0.0f;
    public float EnemyDescendAmount = 0.0f;
    public float HeroTravelSpeed = 1.0f;

    [Space]
    public float PercentageSpeedIncOnEnemyDeath = 5.0f;
    public float PercentageSpeedIncOnRowAdvance = 5.0f;
    public float EnemySpeedLimit = 1000.0f;

    ///game settings
    ///

    public bool GameIsPaused;
    public bool GameplayAvailable;
    public bool GameIsOver;

    // sources
    public GameObject EnemyObjectSource;
    public GameObject HeroShipTransformSource;
    public GameObject HeroBulletSource;
    public GameObject EnemyBulletSource;
    public GameObject ShieldSource;
    public SoInvaderColumn EnemyColumnSOs;

    //ingame entities
    public Transform GameParent;
    public Transform HeroShipTransform;

    // shields
    public int ShieldAmount = 4;
    public float ShieldPlacementX;
    public float ShieldPlacementY;
    public float ShieldPadding;
    public List<GameObject> ShieldList;

    public float GamefieldXMin;
    public float GamefieldXMax;
    public float GamefieldYMin;
    public float GamefieldYMax;

    public float HorizontalRangeBorder = 5.0f;
    public float CeilingBorder = 5.0f;
    public float FloorBorder = 5.0f;
    public int HeroShotLimit = 1;
    public int EnemyShotLimit = 2;
    public float EnemyShotDelay = 1.0f;
    public float EnemyShotJitter = 0.13f;

    [Space]
    public float PaddingEnemyX;
    public float PaddingEnemyY;
    public Vector2 EnemySpawnBegin;

    [Space]
    public int EnemyColumns;
    public int EnemyRows;

    // stats
    [Space]
    public int LivesStart = 3;
    public int LivesRemaining;
    public int CurrentScore;
    public int HiScore;

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
        CheckForLivesEnd();
        CheckForRoundEnd();
    }


    public void UnlockGameLoop()
    {
        CanRunGameLoop = true;
        bulletManager.DefineBounds();
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

    void CheckForLivesEnd()
    {
        if (LivesRemaining == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        CanRunGameLoop = false;
        enemyMarchingController.KillShotCache();
        ClearEntities();
        uIController.UIVisibility(true);
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
        // see if game over, or if next round
        if (LivesRemaining > 0)
        {
            LivesRemaining--;
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
        enemyMarchingController = GetComponent<EnemyMarchingController>();
        bulletManager = GetComponent<BulletManager>();
        poolManager = GetComponent<PoolManager>();
        heroShipController = GetComponent<HeroShipController>();
        uIController = GetComponent<UIController>();
        uIController.UIVisibility(true);

        // this should be launched by a button press
        // RestartGame();
    }

    public void RestartGame()
    {
        LivesRemaining = LivesStart;
        CurrentScore = 0;
        CurrentEnemyMarchSpeed = BeginEnemyMarchSpeed;
        BeginRound();
        heroShipController.HeroShipInit();
        uIController.UIVisibility(false);
    }



    void BeginRound()
    {
        StartCoroutine(enemyMarchingController.SpawnEnemySet());
        CreateHeroShipTransform();
        CreateShields();
    }



    private void Update()
    {
        if (FireButtonPressed)
        {
            FireButtonPressed = false;
            FireButtonAction();
        }

        //   GameplayAvailable = GameIsPaused || !CanRunGameLoop;

        GameplayAvailable = !GameIsPaused || CanRunGameLoop;


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

    void CreateShields()
    {

       // return;

       // float fieldWidth = GamefieldXMax - GamefieldXMin;
        float lastXpos = ShieldPlacementX;
        /*
        float increaseAmount = ShieldPadding;
        if (increaseAmount < 0)
        {
            increaseAmount = increaseAmount * -1;
        }
        */
        if (ShieldList.Count>0)
        {
            for (int i = 0; i < ShieldList.Count; i++)
            {
                Destroy(ShieldList[i]);
            }
            ShieldList.Clear();
        }



        for (int i = 0; i < ShieldAmount; i++)
        {
            GameObject newShield = Instantiate(ShieldSource) as GameObject;
            Transform shieldTransform = newShield.transform;
            ShieldList.Add(newShield);

            shieldTransform.SetParent(GameParent);

            /*
            if (i == 0)
            {
                lastXpos = 
            }
            */

            shieldTransform.localPosition = new Vector3(lastXpos, ShieldPlacementY, 0);

            lastXpos += ShieldPadding;


        }
    }

    public void AddToScore(int addValue)
    {
        CurrentScore += addValue;
        if (HiScore < CurrentScore)
        {
            HiScore = CurrentScore;
        }

        enemyMarchingController.EvaluateBottomRowEnemies();
    }

    public void IncreaseEnemySpeedAtDeath()
    {
        float newSpeed = PercentageSpeedIncOnEnemyDeath;
        IncreaseEnemySpeedByPercent(newSpeed);
    }

    public void IncreaseEnemySpeedAtRowAdvance()
    {
        float newSpeed = PercentageSpeedIncOnRowAdvance;
        IncreaseEnemySpeedByPercent(newSpeed);
    }

    private void IncreaseEnemySpeedByPercent(float increasePercent)
    {
        float increaseAmount = CurrentEnemyMarchSpeed / increasePercent;
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

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

    public enum CharacterType { Hero, StandardEnemy, BonusShip}

    public UnityAction FireButtonAction;
    public bool FireButtonPressed;

    // ----------------------------

    public float ShotSpeedMultiplierHero = 1.0f;
    public float ShotSpeedMultiplierEnemy = 1.0f;
    public float MovementSpeedMultiplierHero = 1.0f;
    public float MovementSpeedMultiplierEnemy = 1.0f;

    public float EnemyMarchSpeed = 0.25f;
    public float EnemyDescendAmount = 0.0f;
    public float HeroTravelSpeed = 1.0f;


    ///game settings
    ///

    public bool GameIsPaused;

    // sources
    public GameObject EnemyObjectSource;
    public GameObject HeroShipTransformSource;
    public GameObject HeroBulletSource;
    public GameObject EnemyBulletSource;
    public SoInvaderColumn EnemyColumnSOs;

    //ingame entities
    public Transform GameParent;
    public Transform HeroShipTransform;

    public float GamefieldXMin;
    public float GamefieldXMax;
    public float GamefieldYMin;
    public float GamefieldYMax;

    public float HorizontalRangeBorder = 5.0f;
    public float CeilingBorder = 5.0f;
    public float FloorBorder = 5.0f;
    public int HeroShotLimit = 1;

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

    bool CanRunGameLoop = false;

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
        ClearEntities();

    }

    void ClearEntities()
    {
        for (int i = 0; i < EnemyBulletPoolActive.Count; i++)
        {
            EnemyBulletPoolActive[i].SetActive(false);
        }

        for (int i = 0; i < HeroBulletPoolActive.Count; i++)
        {
            HeroBulletPoolActive[i].SetActive(false);
        }

        for (int i = 0; i < EnemyPoolActive.Count; i++)
        {
            EnemyPoolActive[i].SetActive(false);
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


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        enemyMarchingController = GetComponent<EnemyMarchingController>();
        bulletManager = GetComponent<BulletManager>();
        poolManager = GetComponent<PoolManager>();

        // this should be launched by a button press
        RestartGame();
    }

    public void RestartGame()
    {
        LivesRemaining = LivesStart;
        CurrentScore = 0;
        BeginRound();
    }



    void BeginRound()
    {
        StartCoroutine(enemyMarchingController.SpawnEnemySet());
        CreateHeroShipTransform();
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

    public void AddToScore(int addValue)
    {
        CurrentScore += addValue;
        if (HiScore < CurrentScore)
        {
            HiScore = CurrentScore;
        }
    }

    public void ChangePauseState()
    {
        GameIsPaused = !GameIsPaused;
    }

}

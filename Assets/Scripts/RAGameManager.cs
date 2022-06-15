using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RAGameManager : MonoBehaviour
{

    public static RAGameManager Instance;
    private EnemyMarchingController enemyMarchingController;
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


    /// <summary>
    /// notes
    /// 5 rows x 11 columns
    /// </summary>

   
    void GameLoop()
    {
        if (GameIsPaused)
        {
            return;
        }
        enemyMarchingController.PerformAllEnemyPosChecks(true);
        bulletManager.MoveAllBullets();
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

        GameObject newHeroObject = null;
        newHeroObject = Instantiate(HeroShipTransformSource) as GameObject;
        //  currentEnemy = newEnemyObject.GetComponent<CharacterObject>();
        HeroShipTransform = newHeroObject.transform;
        HeroShipTransform.SetParent(GameParent);

    }


    public void ChangePauseState()
    {
        GameIsPaused = !GameIsPaused;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RAGameManager : MonoBehaviour
{

    public static RAGameManager Instance;
    private EnemyMarchingController enemyMarchingController;

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


    ///game settings
    ///

    public bool GameIsPaused;

    // sources
    public GameObject EnemyObjectSource;
    public GameObject HeroShipTransformSource;

    //ingame entities
    public Transform GameParent;
    public Transform HeroShipTransform;

    public float GamefieldXMin;
    public float GamefieldXMax;
    public float GamefieldYMin;
    public float GamefieldYMax;

    public float HorizontalRangeBorder = 5.0f;

    [Space]
    public float PaddingEnemyX;
    public float PaddingEnemyY;
    public Vector2 EnemySpawnBegin;

    [Space]
//    public float LastPlacedEnemyX;
//    public float LastPlacedEnemyY;
    public int EnemyColumns;
    public int EnemyRows;

    // stats
    [Space]
    public int LivesRemaining;
    public int CurrentScore;
    public int HiScore;

    // pools
    [Space]
    public List<CharacterObject> EnemyPoolActive = new List<CharacterObject>();
    public List<CharacterObject> EnemyPoolDead = new List<CharacterObject>();
    public List<Bullet> EnemyBulletPoolActive = new List<Bullet>();
    public List<Bullet> EnemyBulletPoolDead = new List<Bullet>();
    public List<Bullet> HeroBulletPoolActive = new List<Bullet>();
    public List<Bullet> HeroBulletPoolDead = new List<Bullet>();


    /// <summary>
    /// notes
    /// 5 rows x 11 columns
    /// </summary>



    public void ChangePoolMemberShipEnemyCharacter(CharacterObject thisCharacter, bool addToList)
    {
        ChangePoolMembership(EnemyPoolActive, EnemyPoolDead, thisCharacter, addToList);
    }

    public void ChangePoolMemberShipEnemyBullet(Bullet thisCharacter, bool addToList)
    {
        ChangePoolMembership(EnemyBulletPoolActive, EnemyBulletPoolActive, thisCharacter, addToList);
    }

    void ChangePoolMembership(List<CharacterObject> characterListActive, List<CharacterObject> characterListDead, CharacterObject thisCharacter, bool AddToList)
    {
        if (AddToList)
        {
            if (!characterListActive.Contains(thisCharacter))
            {
                characterListActive.Add(thisCharacter);
            }

            if (characterListDead.Contains(thisCharacter))
            {
                characterListDead.Remove(thisCharacter);
            }

        }
        else
        {
            // do I need to find index and RemoveAt instead?
            if (characterListActive.Contains(thisCharacter))
            {
                characterListActive.Remove(thisCharacter);
            }

            if (!characterListDead.Contains(thisCharacter))
            {
                characterListDead.Add(thisCharacter);
            }
        }
    }

    void ChangePoolMembership(List<Bullet> characterListActive, List<Bullet> characterListDead, Bullet thisCharacter, bool AddToList)
    {
        if (AddToList)
        {
            if (!characterListActive.Contains(thisCharacter))
            {
                characterListActive.Add(thisCharacter);
            }
        }
        else
        {
            // do I need to find index and RemoveAt instead?
            if (characterListActive.Contains(thisCharacter))
            {
                characterListActive.Remove(thisCharacter);
            }
        }
    }

   



    void GameLoop()
    {
        if (GameIsPaused)
        {
            return;
        }
        enemyMarchingController.PerformAllEnemyPosChecks(true);
    }




    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        enemyMarchingController = GetComponent<EnemyMarchingController>();
        enemyMarchingController.SpawnEnemySet();
        CreateHeroShipTransform();
    }

    private void Update()
    {
        if (FireButtonPressed)
        {
            FireButtonPressed = false;
            FireButtonAction();
        }

        GameLoop();
    }


    void CreateHeroShipTransform()
    {

        GameObject newHeroObject = null;
        newHeroObject = Instantiate(EnemyObjectSource) as GameObject;
        //  currentEnemy = newEnemyObject.GetComponent<CharacterObject>();
        HeroShipTransform = newHeroObject.transform;
        HeroShipTransform.SetParent(GameParent);

    }


    public void ChangePauseState()
    {
        GameIsPaused = !GameIsPaused;
    }



}

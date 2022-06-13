using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RAGameManager : MonoBehaviour
{

    public static RAGameManager Instance;

    public enum CharacterType { Hero, StandardEnemy, BonusShip}

    public UnityAction FireButtonAction;
    public bool FireButtonPressed;

    // ----------------------------

    public float ShotSpeedMultiplierHero = 1.0f;
    public float ShotSpeedMultiplierEnemy = 1.0f;
    public float MovementSpeedMultiplierHero = 1.0f;
    public float MovementSpeedMultiplierEnemy = 1.0f;

    public float EnemyMarchSpeed = 0.25f;
    float currentEnemyMarchSpeed = 0;
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
    public float LastPlacedEnemyX;
    public float LastPlacedEnemyY;
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

    [Space]
    public float furthestEnemyPosLeft;
    public float furthestEnemyPosRight;
    public float furthestEnemyPosDown;
    public bool EnemiesMarchLeft = true;

    bool did1st = false;

    void SpawnEnemy()
    {
        CharacterObject currentEnemy = null;
        if (EnemyPoolDead.Count<0)
        {
            currentEnemy = EnemyPoolDead[0];
            currentEnemy.CharacterActivate();
        }
        else
        {
            GameObject newEnemyObject = null;
            newEnemyObject = Instantiate(EnemyObjectSource) as GameObject;
            currentEnemy = newEnemyObject.GetComponent<CharacterObject>();
        }

        ChangePoolMemberShipEnemyCharacter(currentEnemy, true);

        // assign init characterPosition
        Transform enemyTransform = currentEnemy.GetComponent<Transform>();
        enemyTransform.SetParent(GameParent);
        float newXpos = LastPlacedEnemyX + PaddingEnemyX;

        if (!did1st)
        {
            did1st = true;
            newYpos = EnemySpawnBegin.y;
        }

        LastPlacedEnemyX = newXpos;
        LastPlacedEnemyY = newYpos;

        enemyTransform.localPosition = new Vector3(newXpos, newYpos, 0);
    }

    float newYpos = 0;
    bool shouldDescend = false;



    void EnemyPositionCheck(Transform enemyPos)
    {
        float checkXpos = enemyPos.localPosition.x;
        float checkYpos = enemyPos.localPosition.y;

        if (checkXpos < furthestEnemyPosLeft)
        {
            furthestEnemyPosLeft = checkXpos;
        }

        if (checkXpos > furthestEnemyPosRight)
        {
            furthestEnemyPosRight = checkXpos;
        }

        if (checkYpos < furthestEnemyPosDown)
        {
            furthestEnemyPosDown = checkYpos;
        }
    }




    void SpawnEnemySet()
    {
        for (int i = 0; i < EnemyColumns; i++)
        {
            LastPlacedEnemyX = EnemySpawnBegin.x - PaddingEnemyX;

            for (int j = 0; j < EnemyRows; j++)
            {
                SpawnEnemy();
            }
            newYpos += PaddingEnemyY;
        }

        PerformAllEnemyPosChecks(false);

        // define borders
        GamefieldXMax = furthestEnemyPosRight + HorizontalRangeBorder;
        GamefieldXMin = furthestEnemyPosLeft - HorizontalRangeBorder;

    }
    
    // called from GL
    void PerformAllEnemyPosChecks(bool doMarchEnemy)
    {
        shouldDescend = false;

        for (int i = 0; i < EnemyPoolActive.Count; i++)
        {
            Transform currentEnemyTransform = EnemyPoolActive[i].transform;
            EnemyPositionCheck(currentEnemyTransform);
            if (doMarchEnemy)
            {
                bool canReverseMarch = i == 0;
                MarchEnemy(currentEnemyTransform, canReverseMarch);
            }
        }
    }




    void MarchEnemy(Transform currentEnemyTransform, bool canReverseMarch)
    {
        float newXpos = currentEnemyMarchSpeed;

        if (canReverseMarch)
        {
            if (!EnemiesMarchLeft)
            {
                if (furthestEnemyPosRight > GamefieldXMax)
                {
                    EnemiesMarchLeft = true;
                    furthestEnemyPosRight = 0;
                    shouldDescend = true;
                }
            }
            else
            {
                if (furthestEnemyPosLeft < GamefieldXMin)
                {
                    EnemiesMarchLeft = false;
                    furthestEnemyPosLeft = 0;
                    shouldDescend = true;
                }
            }
        }


        if (EnemiesMarchLeft)
        {
            newXpos = newXpos * -1;
        }

        float assignXpos = currentEnemyTransform.localPosition.x + newXpos;
        float assignYpos = currentEnemyTransform.localPosition.y;
        if (shouldDescend)
        {
            assignYpos -= EnemyDescendAmount;
        }

        currentEnemyTransform.localPosition = new Vector3(assignXpos, assignYpos, 0);

    }



    void GameLoop()
    {
        if (GameIsPaused)
        {
            return;
        }

        PerformAllEnemyPosChecks(true);

    }




    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        SpawnEnemySet();
        CreateHeroShipTransform();
        currentEnemyMarchSpeed = EnemyMarchSpeed;
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

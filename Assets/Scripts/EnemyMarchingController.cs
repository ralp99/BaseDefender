using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarchingController : MonoBehaviour
{
    private int enemyColumns;
    private int enemyRows;
    private Vector2 enemySpawnBegin;
    private float lastPlacedEnemyX;
    private float lastPlacedEnemyY;
    private float paddingEnemyX;
    private float paddingEnemyY;
    public float enemyShotDelay;
    public float enemyShotJitter;

    float newYpos = 0;
    bool shouldDescend = false;

    private float furthestEnemyPosLeft;
    private float furthestEnemyPosRight;
    private float furthestEnemyPosDown;
    private bool enemiesMarchLeft = true;

    public Dictionary<GameObject, SoInvaderColumn> Ass_Enemy_ColumnSO = new Dictionary<GameObject, SoInvaderColumn>();

    public List<GameObject> BottomRowEnemies = new List<GameObject>();
    public IEnumerator EnemyShotTimerCached;

    RAGameManager rAGameManager;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        enemyColumns = rAGameManager.EnemyColumns;
        enemyRows = rAGameManager.EnemyRows;
        paddingEnemyX = rAGameManager.PaddingEnemyX;
        paddingEnemyY = rAGameManager.PaddingEnemyY;
        enemyShotDelay = rAGameManager.EnemyShotDelay;
        enemyShotJitter = rAGameManager.EnemyShotJitter;

        enemySpawnBegin = rAGameManager.EnemySpawnBegin;
    }


    public IEnumerator SpawnEnemySet()
    {

        while (!rAGameManager)
        {
            yield return null;
        }

        Ass_Enemy_ColumnSO.Clear();

        for (int i = 0; i < enemyColumns; i++)
        {
            SoInvaderColumn newColumn = Instantiate(rAGameManager.EnemyColumnSOs) as SoInvaderColumn;
            rAGameManager.InvaderColumns.Add(newColumn);
        }


        for (int i = 0; i < enemyRows; i++)
        {
            lastPlacedEnemyX = enemySpawnBegin.x - paddingEnemyX;
            if (i == 0)
            {
                newYpos = enemySpawnBegin.y;
            }

            for (int j = 0; j < enemyColumns; j++)
            {
                SpawnEnemy(j);
            }
            newYpos += paddingEnemyY;
        }

        PerformAllEnemyPosChecks(false);

        rAGameManager = RAGameManager.Instance;

        // define borders
        rAGameManager.GamefieldXMax = furthestEnemyPosRight + rAGameManager.HorizontalRangeBorder;
        rAGameManager.GamefieldXMin = furthestEnemyPosLeft - rAGameManager.HorizontalRangeBorder;
        rAGameManager.GamefieldYMax = rAGameManager.EnemyPoolActive[rAGameManager.EnemyPoolActive.Count-1].GetComponent<Transform>().localPosition.y +
            rAGameManager.CeilingBorder;

        EvaluateBottomRowEnemies();
        rAGameManager.UnlockGameLoop();
        EnemyShotTimerCached = EnemyShotTimer();
        StartCoroutine(EnemyShotTimerCached);
    }


    public void KillShotCache()
    {
        if (EnemyShotTimerCached != null)
        {
            StopCoroutine(EnemyShotTimerCached);
            EnemyShotTimerCached = null;
        }
    }


    void SpawnEnemy(int addToEnemyColumn)
    {
        GameObject newEnemy = 
        rAGameManager.poolManager.SpawnEntity(rAGameManager.EnemyObjectSource) as GameObject;

        SoInvaderColumn currentColumn = rAGameManager.InvaderColumns[addToEnemyColumn];
        currentColumn.EnemyColumn.Add(newEnemy);
        Ass_Enemy_ColumnSO.Add(newEnemy, currentColumn);

        Transform enemyTransform = newEnemy.GetComponent<Transform>();

        enemyTransform.SetParent(rAGameManager.GameParent);
        float newXpos = lastPlacedEnemyX + paddingEnemyX;

        lastPlacedEnemyX = newXpos;
        lastPlacedEnemyY = newYpos;

        enemyTransform.localPosition = new Vector3(newXpos, newYpos, 0);
    }





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

    // called from gameLoop
    public void PerformAllEnemyPosChecks(bool doMarchEnemy)
    {
        if (!rAGameManager)
        {
            return;
        }

        shouldDescend = false;
        float marchSpeed = rAGameManager.CurrentEnemyMarchSpeed;

        for (int i = 0; i < rAGameManager.EnemyPoolActive.Count; i++)
        {
            Transform currentEnemyTransform = rAGameManager.EnemyPoolActive[i].transform;
            EnemyPositionCheck(currentEnemyTransform);
            if (doMarchEnemy)
            {
                bool canReverseMarch = i == 0;
                MarchSingleEnemy(currentEnemyTransform, canReverseMarch, marchSpeed);
            }
        }
    }


    void MarchSingleEnemy(Transform currentEnemyTransform, bool canReverseMarch, float currentEnemyMarchSpeed)
    {
        float newXpos = currentEnemyMarchSpeed;

        if (canReverseMarch)
        {
            if (!enemiesMarchLeft)
            {
                if (furthestEnemyPosRight > rAGameManager.GamefieldXMax)
                {
                    enemiesMarchLeft = true;
                    furthestEnemyPosRight = 0;
                    shouldDescend = true;
                }
            }
            else
            {
                if (furthestEnemyPosLeft < rAGameManager.GamefieldXMin)
                {
                    enemiesMarchLeft = false;
                    furthestEnemyPosLeft = 0;
                    shouldDescend = true;
                }
            }
        }


        if (enemiesMarchLeft)
        {
            newXpos = newXpos * -1;
        }

        float assignXpos = currentEnemyTransform.localPosition.x + newXpos;
        float assignYpos = currentEnemyTransform.localPosition.y;
        if (shouldDescend)
        {
            assignYpos -= rAGameManager.EnemyDescendAmount;
        }

        currentEnemyTransform.localPosition = new Vector3(assignXpos, assignYpos, 0);

    }

    public void EvaluateBottomRowEnemies()
    {
        BottomRowEnemies.Clear();
        for (int i = 0; i < rAGameManager.InvaderColumns.Count; i++)
        {
            SoInvaderColumn currentColumn = rAGameManager.InvaderColumns[i];

            //  if (currentColumn.EnemyColumn != null)
              if (currentColumn.EnemyColumn.Count > 0)
            {
                GameObject currentEnemy = currentColumn.EnemyColumn[0];
                BottomRowEnemies.Add(currentEnemy);
            }
        }
    }

    public IEnumerator EnemyShotTimer()
    {
        float delay = Random.Range(0, enemyShotJitter) + enemyShotDelay;
        yield return new WaitForSeconds(delay);

        int randomShooterID = Random.Range(0, BottomRowEnemies.Count);
        GameObject currentEnemyShooter = BottomRowEnemies[randomShooterID];
        EnemyFireProjectile(currentEnemyShooter);
        EnemyShotTimerCached = EnemyShotTimer();
        StartCoroutine(EnemyShotTimerCached);
    }

    void EnemyFireProjectile(GameObject currentEnemy)
    {
        if (rAGameManager.EnemyBulletPoolActive.Count < rAGameManager.EnemyShotLimit)
        {
            rAGameManager.bulletManager.SpawnBullet(currentEnemy.GetComponent<CharacterObject>());
        }
    }

}

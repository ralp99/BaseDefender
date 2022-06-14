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

    private float currentEnemyMarchSpeed = 0;

    float newYpos = 0;
    bool shouldDescend = false;

    private float furthestEnemyPosLeft;
    private float furthestEnemyPosRight;
    private float furthestEnemyPosDown;
    private bool enemiesMarchLeft = true;

    RAGameManager rAGameManager;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        enemyColumns = rAGameManager.EnemyColumns;
        enemyRows = rAGameManager.EnemyRows;
        paddingEnemyX = rAGameManager.PaddingEnemyX;
        paddingEnemyY = rAGameManager.PaddingEnemyY;
        currentEnemyMarchSpeed = rAGameManager.EnemyMarchSpeed;

        enemySpawnBegin = rAGameManager.EnemySpawnBegin;
    }


    public IEnumerator SpawnEnemySet()
    {

        while (!rAGameManager)
        {
            yield return null;
        }

        for (int i = 0; i < enemyColumns; i++)
        {
            lastPlacedEnemyX = enemySpawnBegin.x - paddingEnemyX;
            if (i == 0)
            {
                newYpos = enemySpawnBegin.y;
            }

            for (int j = 0; j < enemyRows; j++)
            {
                SpawnEnemy();
            }
            newYpos += paddingEnemyY;
        }

        PerformAllEnemyPosChecks(false);

        rAGameManager = RAGameManager.Instance;

        // define borders
        rAGameManager.GamefieldXMax = furthestEnemyPosRight + rAGameManager.HorizontalRangeBorder;
        rAGameManager.GamefieldXMin = furthestEnemyPosLeft - rAGameManager.HorizontalRangeBorder;
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = 
        rAGameManager.poolManager.SpawnEntity(rAGameManager.EnemyObjectSource) as GameObject;

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

        for (int i = 0; i < rAGameManager.EnemyPoolActive.Count; i++)
        {
            Transform currentEnemyTransform = rAGameManager.EnemyPoolActive[i].transform;
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



}

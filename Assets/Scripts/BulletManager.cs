using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public enum BulletType { HeroStandard, EnemyStandard }
    RAGameManager rAGameManager;


    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }


   

    public void MoveAllBullets()
    {
        for (int i = 0; i < rAGameManager.HeroBulletPoolActive.Count; i++)
        {
            Transform currentBulletTransform = rAGameManager.HeroBulletPoolActive[i].transform;

            // move all bullets hero amount
        }

        for (int i = 0; i < rAGameManager.EnemyBulletPoolActive.Count; i++)
        {
            Transform currentBulletTransform = rAGameManager.EnemyBulletPoolActive[i].transform;
            // move all bullets enemy amount

        }
    }





}

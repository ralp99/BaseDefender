using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public enum UsingList { ExplosionsEnemy, BulletsEnemy, BulletsHero, EnemyA, EnemyB, Bonus }
    RAGameManager rAGameManager;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }


    public GameObject SpawnEntity(GameObject spawnObject)
    {
        GameObject newObject = null;

        UsingList usingList = spawnObject.GetComponent<PoolListMembership>().usingList;
        List<GameObject> inactiveList = UsingInactiveList(usingList, false);


        // incoming spawnObject will be listed as a SourcePrefab
        // check the appropriate inactiveList to see if there's a contained name of the same, to use
        // assign if so, otherwise instantiate it (it should auto-add to activeList)

        if (inactiveList != null)
        {
            if (inactiveList.Count > 0)
            {
                for (int i = 0; i < inactiveList.Count; i++)
                {
                    if (inactiveList[i] != null)
                    {
                        if (spawnObject.name == inactiveList[i].name)
                        {
                            newObject = inactiveList[i];
                            break;
                        }
                    }
                }
            } // END CHECKING POOL
        }

        if (!newObject)
        {
            newObject = Instantiate(spawnObject) as GameObject;
            newObject.name = spawnObject.name;
        }

        newObject.SetActive(true);
        return newObject;
    }

    public List <GameObject> UsingInactiveList(UsingList usingList, bool activeList)
    {
        List<GameObject> returnListActive = null;
        List<GameObject> returnListDead = null;
        List<GameObject> returnList = null;

        switch (usingList)
        {
            case UsingList.ExplosionsEnemy:
                break;
            case UsingList.BulletsEnemy:
                returnListActive = rAGameManager.EnemyBulletPoolActive;
                returnListDead = rAGameManager.EnemyBulletPoolDead;
                break;
            case UsingList.BulletsHero:
                returnListActive = rAGameManager.HeroBulletPoolActive;
                returnListDead = rAGameManager.HeroBulletPoolDead;
                break;
            case UsingList.EnemyA:
                returnListActive = rAGameManager.EnemyPoolActive;
                returnListDead = rAGameManager.EnemyPoolDead;
                break;
            case UsingList.EnemyB:
                break;
            case UsingList.Bonus:
                break;
            default:
                break;
        }
        
        if (activeList)
        {
            returnList = returnListActive;
        }
        else
        {
            returnList = returnListDead;
        }

        return returnList;
    }




}

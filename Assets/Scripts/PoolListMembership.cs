using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolListMembership : MonoBehaviour
{

    public enum UsingList {ExplosionsEnemy, BulletsEnemy, BulletsHero, EnemyA, EnemyB, Bonus}
    public enum PerformListMembership {None, OnDisable, OnEnable, Both}
    public UsingList usingList;
    public PerformListMembership performListMembership;

    RAGameManager rAGameManager;
    private List<GameObject> activeList;
    private List<GameObject> inactiveList;


  void SelectListType()
    {
        rAGameManager = RAGameManager.Instance;
        activeList = rAGameManager.poolManager.UsingInactiveList(usingList, true);
        inactiveList = rAGameManager.poolManager.UsingInactiveList(usingList, false);
    }

    void CheckInits()
    {
        if (activeList == null)
        {
            SelectListType();
        }
    }


    private void OnEnable()
    {
        CheckInits();

        if (performListMembership == PerformListMembership.OnEnable || performListMembership == PerformListMembership.Both)
        {
            if (inactiveList.Contains(gameObject))
            {
                inactiveList.Remove(gameObject);
            }

            if (!activeList.Contains(gameObject))
            {
                activeList.Add(gameObject);
            }
        }

    }

    private void OnDisable()
    {
        if (rAGameManager == null)
        {
            return;
        }

        if (performListMembership == PerformListMembership.OnDisable || performListMembership == PerformListMembership.Both)
        {
            if (inactiveList.Contains(gameObject))
            {
                inactiveList.Remove(gameObject);
            }

            if (!activeList.Contains(gameObject))
            {
                activeList.Add(gameObject);
            }
        }

    }


}

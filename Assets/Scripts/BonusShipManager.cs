using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusShipManager : MonoBehaviour
{
    private bool bonusShipGoingLeft = false;

    private float bonusShipDelay;
    private float bonusShipJitter;
    private float usingDelay;
    private int bonusShipAppearancesPerRound;
    private int appearanceCounter;

    private RAGameManager rAGameManager;
    private IEnumerator cachedBonusTimer;

    IEnumerator ResetDelayValue()
    {
        if (!rAGameManager)
        {
            rAGameManager = RAGameManager.Instance;
        }

        while (!rAGameManager.SoValuesInjected)
        {
            yield return null;
        }

        if (bonusShipDelay == 0)
        {
            bonusShipDelay = rAGameManager.BonusShipDelay;
            bonusShipJitter = rAGameManager.BonusShipJitter;
            bonusShipAppearancesPerRound = rAGameManager.BonusShipAppearancesPerRound;
        }

        float randomValue = Random.Range(0, bonusShipJitter);
        usingDelay = bonusShipDelay + randomValue;
    }

    void BonusShipAppear()
    {
        bonusShipGoingLeft = !bonusShipGoingLeft;
        print("02 - BSP appears -- "+bonusShipGoingLeft);
    }

    public void StartBonusShipTimer()
    {
        if (cachedBonusTimer == null)
        {
            cachedBonusTimer = BonusShipTimer();
            StartCoroutine(ResetDelayValue());
            StartCoroutine(cachedBonusTimer);
        }
    }

    public void CancelBonusShipTimer()
    {
        bonusShipGoingLeft = false;
        appearanceCounter = 0;
        if (cachedBonusTimer != null)
        {
            StopCoroutine(cachedBonusTimer);
            cachedBonusTimer = null;
        }
    }

    IEnumerator BonusShipTimer()
    {
        while (usingDelay == 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(usingDelay);
        BonusShipAppear();
        cachedBonusTimer = null;
        appearanceCounter++;
        if (appearanceCounter < bonusShipAppearancesPerRound)
        {
            StartBonusShipTimer();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusShipMover : MonoBehaviour
{
    private RAGameManager rAGameManager;
    private float bonusShipSpeed;
    public bool GoLeft = false;
    float directionMultiplier = 1;
    float fieldXmin;
    float fieldXmax;

    bool valuesSet;

    void AssignedValuesCheck()
    {
        if (valuesSet)
        {
            return;
        }

        rAGameManager = RAGameManager.Instance;
        bonusShipSpeed = rAGameManager.BonusShipSpeed;
        fieldXmax = rAGameManager.GamefieldXMax;
        fieldXmin = rAGameManager.GamefieldXMin;
        valuesSet = true;
    }


    void Update()
    {

        if (rAGameManager.GameIsPaused)
        {
            return;
        }

        if (GoLeft)
        {
            directionMultiplier = -1;
        }
        else
        {
            directionMultiplier = 1;
        }

        Vector3 currentShipPos = transform.localPosition;
        float posX = currentShipPos.x + (directionMultiplier * bonusShipSpeed);
        float posY = currentShipPos.y;
        float posZ = currentShipPos.z;

        transform.localPosition = new Vector3(posX, posY, posZ);

        if (GoLeft && posX < fieldXmin)
        {
            gameObject.SetActive(false);
        }

        if (!GoLeft && posX > fieldXmax)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        AssignedValuesCheck();
        GoLeft = !GoLeft;

        Vector3 currentShipPos = transform.localPosition;
        float posX = currentShipPos.x;
        float posY = currentShipPos.y;
        float posZ = currentShipPos.z;

        if (GoLeft)
        {
            posX = fieldXmax;
        }
        else
        {
            posX = fieldXmin;
        }

        transform.localPosition = new Vector3(posX, posY, posZ);

    }
}

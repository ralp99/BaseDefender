using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroShipController : MonoBehaviour
{
    RAGameManager rAGameManager;
    Transform HeroShipTransform;
    CharacterObject HeroShipCharacter;

    public UnityAction OnPlayerJoyleft;
  

    void OnPlayerJoyMoveShip(bool moveLeft)
    {

        if (rAGameManager.GameIsPaused || !rAGameManager.CanRunGameLoop)
        {
            return;
        }

        int moveLeftMulti = 1;
        if (moveLeft)
        {
            moveLeftMulti = -1;
        }

        if (!HeroShipTransform)
        {
            HeroShipTransform = RAGameManager.Instance.HeroShipTransform;
        }

        float newXpos = HeroShipTransform.localPosition.x - (rAGameManager.HeroTravelSpeed*moveLeftMulti);

        if (newXpos < rAGameManager.GamefieldXMin)
        {
            newXpos = rAGameManager.GamefieldXMin;
        }

        if (newXpos > rAGameManager.GamefieldXMax)
        {
            newXpos = rAGameManager.GamefieldXMax;
        }

        HeroShipTransform.localPosition = new Vector3(newXpos, HeroShipTransform.localPosition.y, HeroShipTransform.localPosition.z);
    }

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        
    }

    public void HeroShipInit()
    {
        HeroShipTransform = rAGameManager.HeroShipTransform;
        HeroShipCharacterCheck();
    }


    void HeroShipCharacterCheck()
    {
        if (!HeroShipCharacter)
        {
            HeroShipCharacter = HeroShipTransform.GetComponent<CharacterObject>();
        }
    }

    private void OnEnable()
    {
        ControllerManager.OnLeftThumbstick += MoveShipLeft;
        ControllerManager.OnRightThumbstick += MoveShipRight;
        ControllerManager.OnAbuttonPress += PlayerFireProjectile;
        ControllerManager.OnYbuttonPress += TogglePause;
    }

    private void OnDisable()
    {
        ControllerManager.OnLeftThumbstick -= MoveShipLeft;
        ControllerManager.OnRightThumbstick -= MoveShipRight;
        ControllerManager.OnAbuttonPress -= PlayerFireProjectile;
        ControllerManager.OnYbuttonPress -= TogglePause;
    }

    void MoveShipLeft()
    {
        OnPlayerJoyMoveShip(false);
    }

    void MoveShipRight()
    {
        OnPlayerJoyMoveShip(true);
    }

    void PlayerFireProjectile()
    {
        if (!rAGameManager.GameplayAvailable)
        {
            return;
        }



        if (rAGameManager.HeroBulletPoolActive.Count < rAGameManager.HeroShotLimit)
        {
            HeroShipCharacterCheck();
            rAGameManager.bulletManager.SpawnBullet(HeroShipCharacter);
        }
    }

    void TogglePause()
    {
        rAGameManager.ChangePauseState();
    }


}

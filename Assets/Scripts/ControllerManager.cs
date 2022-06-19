using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllerManager : MonoBehaviour
{
    public float ButtonThrottleDelay = 0.1f;
    enum ButtonsToLock { ButtonA, ButtonB, ButtonX, ButtonY}

    public bool LB;
    public bool RB;
    public bool Dup;
    public bool Ddown;
    public bool Dleft;
    public bool Dright;

    public bool ButtonB;
    public bool ButtonA;
    public bool ButtonY;
    public bool ButtonX;

    public float Ltrigger;
    public float Rtrigger;
    public float LstickX;
    public float LstickY;
    public float RstickX;
    public float RstickY;

    [Header("User Defined")]
    public float LstickHLimitNeg;
    public float LstickHLimitPos;
    public float LstickVLimitNeg;
    public float LstickVLimitPos;

    public float RstickHLimitNeg;
    public float RstickHLimitPos;
    public float RstickVLimitNeg;
    public float RstickVLimitPos;

    private string XbuttonString = "360_X";
    private string YbuttonString = "360_Y";
    private string AbuttonString = "360_A";
    private string BbuttonString = "360_B";

    private string triggerLeft = "360_TriggerLeft";
    private string triggerRight = "360_TriggerRight";

    private string bumperLeft = "360_LB";
    private string bumperRight = "360_RB";


    private string lJoyLeftX = "360_LeftJoystickX";
    private string lJoyLeftY = "360_LeftJoystickY";
    private string rJoyLeftX = "360_RightJoystickX";
    private string rJoyLeftY = "360_RightJoystickY";

    public static event Action OnLeftThumbstick;
    public static event Action OnRightThumbstick;
    public static event Action OnAbuttonPress;
    public static event Action OnYbuttonPress;



    void Start()
    {
        
    }


    void Update()
    {

        LB = Input.GetAxis(bumperLeft) != 0;
        RB = Input.GetAxis(bumperRight) != 0;

        ButtonX = Input.GetAxis(XbuttonString) > 0;
        ButtonY = Input.GetAxis(YbuttonString) > 0;
        ButtonA = Input.GetAxis(AbuttonString) > 0;
        ButtonB = Input.GetAxis(BbuttonString) > 0;

        Ltrigger = Input.GetAxis(triggerLeft);
        Rtrigger = Input.GetAxis(triggerRight);

        LstickX = Input.GetAxis(lJoyLeftX);
        LstickY = Input.GetAxis(lJoyLeftY);
        RstickX = Input.GetAxis(rJoyLeftX);
        RstickY = Input.GetAxis(rJoyLeftY);

        if (LstickX <0)
        {
            OnLeftThumbstick?.Invoke();
        }

        if (LstickX > 0)
        {
            OnRightThumbstick?.Invoke();
        }

        if (ButtonA)
        {
            if (buttonAdelaying)
            {
                return;
            }

            StartCoroutine(ButtonLocker(ButtonsToLock.ButtonA));
            OnAbuttonPress?.Invoke();
        }

        if (ButtonY)
        {
            if (buttonYdelaying)
            {
                return;
            }

            StartCoroutine(ButtonLocker(ButtonsToLock.ButtonY));
            OnYbuttonPress?.Invoke();
        }


        if (Input.GetKey(KeyCode.Z))
        {
            OnLeftThumbstick?.Invoke();
        }



        if (Input.GetKey(KeyCode.X))
        {
            OnRightThumbstick?.Invoke();
        }


        if (Input.GetKey(KeyCode.Space))
        {
            if (buttonAdelaying)
            {
                return;
            }

            StartCoroutine(ButtonLocker(ButtonsToLock.ButtonA));
            OnAbuttonPress?.Invoke();
        }



        if (Input.GetKey(KeyCode.B))
        {
            if (buttonYdelaying)
            {
                return;
            }
            print("y button");
            StartCoroutine(ButtonLocker(ButtonsToLock.ButtonY));
            OnYbuttonPress?.Invoke();
        }

        /// kbm
        /// 

        // z x spc





    }

    bool buttonAdelaying;
    bool buttonBdelaying;
    bool buttonXdelaying;
    bool buttonYdelaying;


    IEnumerator ButtonLocker (ButtonsToLock lockButton)
    {
        ChangeButtonLockState(lockButton, true);
        yield return new WaitForSeconds(ButtonThrottleDelay);
        ChangeButtonLockState(lockButton, false);
    }

    void ChangeButtonLockState(ButtonsToLock lockButton, bool lockState)
    {
        switch (lockButton)
        {
            case ButtonsToLock.ButtonA:
                buttonAdelaying = lockState;
                break;
            case ButtonsToLock.ButtonB:
                buttonBdelaying = lockState;
                break;
            case ButtonsToLock.ButtonX:
                buttonXdelaying = lockState;
                break;
            case ButtonsToLock.ButtonY:
                buttonYdelaying = lockState;
                break;
            default:
                break;
        }
    }


}

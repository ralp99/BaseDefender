using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RAGameManager : MonoBehaviour
{

    public static RAGameManager Instance;

    public UnityAction FireButtonAction;
    public bool FireButtonPressed;


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        
    }

    private void Update()
    {
        if (FireButtonPressed)
        {
            FireButtonPressed = false;
            FireButtonAction();
        }
    }



}

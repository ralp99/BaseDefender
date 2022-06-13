using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSettings : MonoBehaviour
{

    RAGameManager rAGameManager;

    public RAGameManager.CharacterType CharacterType;



    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }


    void Update()
    {
        
    }
}

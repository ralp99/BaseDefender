using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{

    RAGameManager rAGameManager;

    public RAGameManager.CharacterType CharacterType;

    public Health MyHealth;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        MyHealth = GetComponent<Health>();
    }


    public void CharacterActivate()
    {
        MyHealth.RestartCharacter();
    }

    void CharacterDead()
    {
    //    rAGameManager.ChangePoolMemberShipEnemyCharacter(this, false);

    }


    void Update()
    {
        
    }
}

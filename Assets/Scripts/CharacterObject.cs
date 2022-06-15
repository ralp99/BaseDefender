using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{

    RAGameManager rAGameManager;

    public RAGameManager.CharacterType CharacterType;
    public BulletManager.BulletType MyBulletType;
    public GameObject MyBulletSpawnSourceOverride;
    [HideInInspector]
    public GameObject MyBulletSpawnSource;
    public Transform BulletSpawnPoint;

    public Health MyHealth;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        MyHealth = GetComponent<Health>();
        if (MyBulletType == BulletManager.BulletType.Override)
        {
            MyBulletSpawnSource = MyBulletSpawnSourceOverride;
        }
        else
        {
            switch (CharacterType)
            {
                case RAGameManager.CharacterType.Hero:
                    MyBulletSpawnSource = rAGameManager.HeroBulletSource;
                    break;
                case RAGameManager.CharacterType.StandardEnemy:
                    MyBulletSpawnSource = rAGameManager.EnemyBulletSource;
                    break;
                case RAGameManager.CharacterType.BonusShip:
                    break;
                default:
                    break;
            }
        }

    }


    public void CharacterActivate()
    {
        MyHealth.RestartCharacter();
    }

    void CharacterDead()
    {

    }


    void Update()
    {
        
    }
}

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
    public int PointValue;

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

    public void CharacterDead()
    {
        if (CharacterType == RAGameManager.CharacterType.Hero)
        {
            rAGameManager.HeroKilled();
        }

        if (rAGameManager.enemyMarchingController.SmasherEnemies.Contains(gameObject))
        {
            rAGameManager.enemyMarchingController.SmasherEnemies.Remove(gameObject);
          
            if (gameObject.GetComponent<Rigidbody>())
            {
                Destroy(gameObject.GetComponent<Rigidbody>());
            }

            if (gameObject.GetComponent<Projectile>())
            {
                Destroy(gameObject.GetComponent<Projectile>());
            }
        }
    }

}

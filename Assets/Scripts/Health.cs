using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int CurrentHealth;
    public int MaxHealth;
    public BulletManager.Vulnerability BulletVulnerability;
    private CharacterObject characterObject;
    private Projectile projectile;
    private RAGameManager rAGameManager;

    private void Start()
    {
        if (GetComponent<CharacterObject>())
        {
            characterObject = GetComponent<CharacterObject>();
        }

        if (GetComponent<Projectile>())
        {
            projectile = GetComponent<Projectile>();
        }

        rAGameManager = RAGameManager.Instance;
        RestartCharacter();
    }

    public void HealthChange(int healthValue)
    {
        CurrentHealth -= healthValue;
        if (CurrentHealth < 1)
        {
            KillCharacter();
        }
    }

    void KillCharacter()
    {
        gameObject.SetActive(false);
    
        if (rAGameManager.enemyMarchingController.Ass_Enemy_ColumnSO.ContainsKey(gameObject))
        {
            // get list val in index
            rAGameManager.enemyMarchingController.Ass_Enemy_ColumnSO[gameObject].EnemyColumn.Remove(gameObject);
        }

        if (characterObject)
        {
            rAGameManager.AddToScore(characterObject.PointValue);
            characterObject.CharacterDead();
            if (characterObject.CharacterType == RAGameManager.CharacterType.StandardEnemy)
            {
                rAGameManager.IncreaseEnemySpeedAtDeath();
            }
        }

        if (projectile)
        {

        }
    }

    public void RestartCharacter()
    {
        CurrentHealth = MaxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (!rAGameManager.GameplayAvailable)
        {
            return;
        }

        CollisionInteraction(collision.gameObject);
    }

    void CollisionInteraction(GameObject col)
    {

        /*
        if (!col.gameObject.GetComponent<Projectile>())
        {
            return;
        }
        */


        GameObject thisObjectForNameCheck = gameObject;


        if (col.gameObject.GetComponent<Projectile>())
        {

            Projectile hittingProjectile = col.gameObject.GetComponent<Projectile>();

            if (BulletVulnerability == BulletManager.Vulnerability.ToHero &&
                hittingProjectile.MyBulletType == BulletManager.BulletType.HeroStandard)
            {
                HitHappens(col);
            }

            if (BulletVulnerability == BulletManager.Vulnerability.ToEnemy &&
                hittingProjectile.MyBulletType == BulletManager.BulletType.EnemyStandard)
            {
                HitHappens(col);
            }

            if (BulletVulnerability == BulletManager.Vulnerability.ToAll)
            {
                if ((hittingProjectile.MyBulletType == BulletManager.BulletType.EnemyStandard) ||
                    (hittingProjectile.MyBulletType == BulletManager.BulletType.HeroStandard))

                {
                    HitHappens(col);
                }

            }
        }
    }

    void HitHappens(GameObject col)
    {
        col.gameObject.SetActive(false);
        DealDamage(col.GetComponent<Projectile>());
    }


    void DealDamage(Projectile hittingProjectile)
    {
        HealthChange(hittingProjectile.DealDamage);
    }



}

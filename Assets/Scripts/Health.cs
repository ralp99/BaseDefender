﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int CurrentHealth;
    public int MaxHealth;
    public BulletManager.Vulnerability BulletVulnerability;
    private CharacterObject characterObject;
    private RAGameManager rAGameManager;

    private void Start()
    {
        characterObject = GetComponent<CharacterObject>();
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
        rAGameManager.AddToScore(characterObject.PointValue);
        if (rAGameManager.enemyMarchingController.Ass_Enemy_ColumnSO.ContainsKey(gameObject))
        {
            // get list val in index
            rAGameManager.enemyMarchingController.Ass_Enemy_ColumnSO[gameObject].EnemyColumn.Remove(gameObject);
        }
        characterObject.CharacterDead();
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

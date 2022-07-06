using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public BulletManager.BulletDirection MyBulletDirection = BulletManager.BulletDirection.NA;
    public BulletManager.BulletType MyBulletType = BulletManager.BulletType.HeroStandard;
    public BulletManager.BulletSpeed MyBulletSpeed = BulletManager.BulletSpeed.ByType;
    public float OverrideSpeed = 0.0f;
    [HideInInspector]
    public float MyUsingSpeed = 0.0f;
    public bool DestroyIfOutOfBounds = true;
    public int DealDamage = 1;

    public bool DontKillSelfAtCollision;

    public Transform SpawnedFrom;
    public bool FollowSpawner;

    RAGameManager rAGameManager;
    AudioSource audioSource;
    AudioBank audioBank;

    void AudioBankCheck()
    {
        if (audioBank)
        {
            return;
        }

        if (!rAGameManager)
        {
            rAGameManager = RAGameManager.Instance;
        }

        audioBank = rAGameManager.audioBank;
    }


    void Start()
    {
        rAGameManager = RAGameManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioBank = rAGameManager.audioBank;

        if (MyBulletType == BulletManager.BulletType.HeroStandard)
        {
            MyUsingSpeed = rAGameManager.ShotSpeedMultiplierHero;
            MyBulletDirection = BulletManager.BulletDirection.Above;
        }

        if (MyBulletType == BulletManager.BulletType.EnemyStandard)
        {
            MyUsingSpeed = rAGameManager.ShotSpeedMultiplierEnemy;
            MyBulletDirection = BulletManager.BulletDirection.Below;
        }

        if (MyBulletSpeed == BulletManager.BulletSpeed.Override)
        {
            MyUsingSpeed = OverrideSpeed;
        }
    }


    private void OnEnable()
    {
        StartCoroutine(EnableAudioFX());
    }

    IEnumerator EnableAudioFX()
    {
        while (audioBank == null || audioSource == null)
        {
            yield return null;
        }

        if (MyBulletType == BulletManager.BulletType.HeroStandard)
        {
            audioSource.PlayOneShot(audioBank.HeroBullet_fire);
        }
    }



}

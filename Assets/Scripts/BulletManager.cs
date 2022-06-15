using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public enum BulletType { HeroStandard, EnemyStandard, Override }
    public enum BulletDirection { NA, Forward, Backward, Left, Right, Above, Below }
    public enum BulletSpeed { ByType, Override}

    // associate bullet and direction
    // associate bullet and speed

    public Dictionary<Projectile, BulletDirection> Ass_Projectile_Bdirect = new Dictionary<Projectile, BulletDirection>();
    public Dictionary<Projectile, float> Ass_Projectile_Speed = new Dictionary<Projectile, float>();
    public Dictionary<Projectile, bool> Ass_Projectile_BoundsDestroy = new Dictionary<Projectile, bool>();

    RAGameManager rAGameManager;

    float boundsMinX;
    float boundsMinY;
    float boundsMaxX;
    float boundsMaxY;


    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }

    public void DefineBounds()
    {
        boundsMaxX = rAGameManager.GamefieldXMax;
        boundsMaxY = rAGameManager.GamefieldYMax;
        boundsMinX = rAGameManager.GamefieldXMin;
        boundsMinY = rAGameManager.GamefieldYMin;
    }

   public void SpawnBullet(CharacterObject spawningCharacter)
    {
        GameObject newBullet =
       rAGameManager.poolManager.SpawnEntity(spawningCharacter.MyBulletSpawnSource) as GameObject;

        Transform bulletTransform = newBullet.GetComponent<Transform>();
        bulletTransform.SetParent(rAGameManager.GameParent);

        if (spawningCharacter.BulletSpawnPoint != null)
        {
            bulletTransform.localPosition = spawningCharacter.BulletSpawnPoint.localPosition;
        }
        else
        {
            bulletTransform.localPosition = spawningCharacter.GetComponent<Transform>().localPosition;
        }

    }

    public void MoveAllBullets()
    {
        List<Transform> AllBulletTransforms = new List<Transform>();
 
        // dictionary checks
        for (int i = 0; i < rAGameManager.HeroBulletPoolActive.Count; i++)
        {
            GameObject currentBullet = rAGameManager.HeroBulletPoolActive[i];
            DictionaryChecks(currentBullet, AllBulletTransforms);
        }

        for (int i = 0; i < rAGameManager.EnemyBulletPoolActive.Count; i++)
        {
            GameObject currentBullet = rAGameManager.HeroBulletPoolActive[i];
            DictionaryChecks(currentBullet, AllBulletTransforms);
        }

        // move all bullets
        for (int i = 0; i < AllBulletTransforms.Count; i++)
        {
            Transform currentBulletTransform = AllBulletTransforms[i];
            Projectile currentBulletProjectile = currentBulletTransform.GetComponent<Projectile>();
            float currentBulletPosX = currentBulletTransform.localPosition.x;
            float currentBulletPosY = currentBulletTransform.localPosition.y;
            float currentBulletPosZ = currentBulletTransform.localPosition.z;

            BulletDirection currentBulletDirection = Ass_Projectile_Bdirect[currentBulletProjectile];
            float currentBulletSpeed = Ass_Projectile_Speed[currentBulletProjectile];

            switch (currentBulletDirection)
            {
                case BulletDirection.NA:
                    break;
                case BulletDirection.Forward:
                    // x+;
                    currentBulletPosX += currentBulletSpeed;
                    break;
                case BulletDirection.Backward:
                    // x-
                    currentBulletPosX -= currentBulletSpeed;
                    break;
                case BulletDirection.Left:
                    break;
                case BulletDirection.Right:
                    break;
                case BulletDirection.Above:
                    // y+
                    currentBulletPosY += currentBulletSpeed;
                    break;
                case BulletDirection.Below:
                    // y-
                    currentBulletPosY -= currentBulletSpeed;
                    break;
                default:
                    break;
            }

            currentBulletTransform.localPosition = new Vector3(currentBulletPosX, currentBulletPosY, currentBulletPosZ);
            bool outOfBounds = false;

            if (!Ass_Projectile_BoundsDestroy[currentBulletProjectile])
            {
                continue;
            }

            if (currentBulletPosX < boundsMinX)
            {
                outOfBounds = true;
            }

            if(currentBulletPosX > boundsMaxX)
            {
                outOfBounds = true;
            }

            if (currentBulletPosY < boundsMinY)
            {
                outOfBounds = true;
            }

            if (currentBulletPosY > boundsMaxY)
            {
                outOfBounds = true;
            }

            if (outOfBounds)
            {
                currentBulletProjectile.gameObject.SetActive(false);
            }

        }




    }

    void DictionaryChecks(GameObject bulletGameObject, List<Transform> AllBulletTransforms)
    {
        Transform currentBulletTransform = bulletGameObject.transform;
        Projectile currentProjectile = currentBulletTransform.GetComponent<Projectile>();
        AllBulletTransforms.Add(currentBulletTransform);

        if (!Ass_Projectile_Bdirect.ContainsKey(currentProjectile))
        {
            Ass_Projectile_Bdirect.Add(currentProjectile, currentProjectile.MyBulletDirection);
        }

        if (!Ass_Projectile_Speed.ContainsKey(currentProjectile))
        {
            Ass_Projectile_Speed.Add(currentProjectile, currentProjectile.MyUsingSpeed);
        }

        if (!Ass_Projectile_BoundsDestroy.ContainsKey(currentProjectile))
        {
            Ass_Projectile_BoundsDestroy.Add(currentProjectile, currentProjectile.DestroyIfOutOfBounds);
        }
    }



}

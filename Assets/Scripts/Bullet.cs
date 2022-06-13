using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    RAGameManager rAGameManager;

    public float MyBulletSpeed = 0f;
    public BulletManager.BulletType BulletType;

    void Start()
    {
        rAGameManager = RAGameManager.Instance;
    }


    void Update()
    {
        
    }
}

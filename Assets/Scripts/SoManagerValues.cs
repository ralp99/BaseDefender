using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewManagerValues", menuName = "sos/NewManagerValues")]

public class SoManagerValues : ScriptableObject
{


    public float ShotSpeedMultiplierHero = 1.0f;
    public float ShotSpeedMultiplierEnemy = 1.0f;
    public float MovementSpeedMultiplierHero = 1.0f;
    public float MovementSpeedMultiplierEnemy = 1.0f;

    [Space]
    public float BeginEnemyMarchSpeed = 0.025f;
    public float EnemyDescendAmount = 0.0f;
    public float HeroTravelSpeed = 1.0f;

    [Space]
    public float PercSpdIncOnEnemyDeath = 5.0f;
    public float PercSpdIncOnRowAdvance = 5.0f;
    public float EnemySpeedLimit = 1000.0f;
    [Space]
    public int ShieldAmount = 4;
    [Range(0,1)]
    public float ShieldPlacementY;
    [Space]
    [Range(0, 1)]
    public float BonusShipPlacementY;

    [Space]
    public float HorizontalRangeBorder = 5.0f;
    public float CeilingBorder = 5.0f;
    public float FloorBorder = 5.0f;
    public int HeroShotLimit = 1;
    public int EnemyShotLimit = 2;
    public float EnemyShotDelay = 1.0f;
    public float EnemyShotJitter = 0.13f;
    public float EnemiesBecomeSmashers = 1.0f;
    [Space]
    public float PaddingEnemyX;
    public float PaddingEnemyY;
    public Vector2 EnemySpawnBegin;

    public int EnemyColumns;
    public int EnemyRows;

    public int LivesStart = 3;

    public float BonusShipDelay;
    public float BonusShipJitter;
    public int BonusShipAppearancesPerRound;

}

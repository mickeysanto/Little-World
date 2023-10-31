using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Stats
{
    public float damage;
    public float speed;
    public float attackCool;
    public float retreatChance;
    public float attackSpeed;

    private Vector2 damageRange;
    private Vector2 speedRange;
    private Vector2 attackCoolRange;
    private Vector2 retreatChanceRange; //a percentage expressed as a whole number
    private Vector2 attackSpeedRange;

    public Stats(Vector2 dam, Vector2 spd, Vector2 attktime, Vector2 retChance, Vector2 attkspd)
    {
        damageRange = dam;
        speedRange = spd;
        attackCoolRange = attktime;
        retreatChanceRange = retChance;
        attackSpeedRange = attkspd;
    }

    public void rollStats()
    {
        damage = UnityEngine.Random.Range(damageRange.x, damageRange.y);
        speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
        attackCool = UnityEngine.Random.Range(attackCoolRange.x, attackCoolRange.y);
        retreatChance = UnityEngine.Random.Range(retreatChanceRange.x, retreatChanceRange.y);
        attackSpeed = UnityEngine.Random.Range(attackSpeedRange.x, attackSpeedRange.y);
    }
}

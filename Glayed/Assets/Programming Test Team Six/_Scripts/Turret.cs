using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Machine gun; Rapid fire ====== + OverCharge (fire rate increases over time, tot overheat)
 * Lazer; Piercing -----------
 * Rocket launcher; AoE  %#@^&%$ kaboom
 */

[Serializable]
public class Turret
{
    public int damage;
    public float fireRate, projectileSpeed;
    public GameObject turretObject;
    public ParticleSystem projectile;

    public virtual void OnHit(EnemyUnit enemyUnit, BulletScript bullet)
    {
        enemyUnit.ReceiveDamage(damage);
        MonoBehaviour.Destroy(bullet.gameObject);
    }
}

[Serializable]
public class MachineGun : Turret
{

}

[Serializable]
public class Lazer : Turret
{
    public override void OnHit(EnemyUnit enemyUnit, BulletScript bullet)
    {
        enemyUnit.ReceiveDamage(damage);
    }
}

[Serializable]
public class RocketLauncher : Turret
{

}

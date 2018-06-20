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
    public string turretSwapKey;

    public int damage;
    public float fireRate, projectileSpeed;
    public GameObject turretObject;
    public ParticleSystem projectile;

    // This will be added to the projectiles shot with this turret
    public virtual void OnHit(EnemyUnit enemyUnit, BulletScript bullet)
    {
        enemyUnit.ReceiveDamage(damage);
        MonoBehaviour.Destroy(bullet.gameObject);
    }
}

[Serializable]
public class Lazer : Turret
{
    // This turret's bullets don't get destroyed if they collide
    public override void OnHit(EnemyUnit enemyUnit, BulletScript bullet)
    {
        enemyUnit.ReceiveDamage(damage);
    }
}

[Serializable]
public class RocketLauncher : Turret
{
    // This turret's bullet cause an explosion on impact
    public float boomRadius;
    public override void OnHit(EnemyUnit enemyUnit, BulletScript bullet)
    {
        Vector3 center = bullet.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, boomRadius / 2);

        foreach(Collider col in hitColliders)
        {
            if (col.tag == "Enemy")
            {
                col.GetComponent<EnemyUnit>().ReceiveDamage(damage);
            }
        }
        MonoBehaviour.Destroy(bullet.gameObject);
    }
}

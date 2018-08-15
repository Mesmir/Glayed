using System;
using UnityEngine;

[Serializable]
public class Turret
{
    public string turretSwapKey;

    public int level = 1, damage, magazine;
    public float fireRate, projectileSpeed, reloadTime;
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
public class MachineGun : Turret
{

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
    //Turret specific; Core explosion dmg, rocket projectile speed.
    //This turret's bullet cause an explosion on impact
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

[Serializable]
public class GlaiveThrower : Turret
{

}

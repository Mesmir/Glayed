using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    // This is where the turret's effect will be stored in
    public delegate void HitFunc(EnemyUnit enemyUnit, BulletScript bullet);
    public HitFunc hitFunc;

    public void OnTriggerEnter(Collider trig)
    {
        if (trig.tag == "Enemy")
        { 
            if (hitFunc != null)
            {
                // Activate turret's effect
                hitFunc(trig.GetComponent<EnemyUnit>(), this);
            }
        }
    }
}

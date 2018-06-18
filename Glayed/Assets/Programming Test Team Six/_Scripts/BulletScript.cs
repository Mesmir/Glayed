using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public delegate void HitFunc(EnemyUnit enemyUnit, BulletScript bullet);
    public HitFunc hitFunc;

    public void OnTriggerEnter(Collider trig)
    {
        if (trig.tag == "Enemy")
        { 
            if (hitFunc != null)
            {
                hitFunc(trig.GetComponent<EnemyUnit>(), this);
            }
        }
    }
}

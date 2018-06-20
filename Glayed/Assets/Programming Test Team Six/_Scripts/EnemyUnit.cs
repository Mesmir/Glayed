using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour {

    public int health;
    public int damage;
    // The powerlevel is used to balance the combined power level of spawned enemies
    public int powerLevel; // It's over 9000!!

    private Transform target;
    public float moveSpeed;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if (Vector3.Distance(transform.position, target.position) <= 4f)
        {
            // Deals damage when closed in on the player and destroys itself
            target.GetComponent<TurretControls>().GetDamaged(damage);
            OnDeath();
        }
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDeath();
    }

    private void OnDeath()
    {
        // Cleaning the spawned units list
        EnemySpawner.instance.spawnedUnits.Remove(this);
        Destroy(gameObject);
    }
}

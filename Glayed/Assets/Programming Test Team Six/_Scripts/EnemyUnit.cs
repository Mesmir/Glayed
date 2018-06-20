using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour {

    public int health;
    public int damage;
    public int scoreValue;
    // The powerlevel is used to balance the combined power level of spawned enemies
    public int powerLevel;

    private Transform target;
    public float moveSpeed;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        float speed = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
        if (Vector3.Distance(transform.position, target.position) <= 3.5f)
        {
            // Deals damage when closed in on the player and destroys itself
            target.GetComponent<TurretManager>().GetDamaged(damage);
            OnDeath();
        }
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            target.GetComponent<TurretManager>().AddScore(scoreValue);
            OnDeath();
        }
    }

    private void OnDeath()
    {
        // Cleaning the spawned units list
        EnemySpawner.enemySpawner.spawnedUnits.Remove(this);
        Destroy(gameObject);
    }
}

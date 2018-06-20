using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
lijst met spawnpoints
elke wave, spawn karakter tot y powerlevel
move to turret
hou bij hoeveel er nog zijn en start nieuwe wave als het afgelopen is
 */

public class EnemySpawner : MonoBehaviour {

    // Static reference to itself, so that the enemy units can easily access it
    public static EnemySpawner instance;

    private GameObject[] spawnPoints;
    // The starting maximum powerlevel of all enemies combined.
    public int maxCombinedPowerLevel;
    // Amount of time before the maximum powerlevel is raised
    public float levelTimer;
    // Amount the power is raised per "wave"
    public int powerLevelIncrease;

    [HideInInspector]
    public List<EnemyUnit> spawnedUnits = new List<EnemyUnit>();
    public List<EnemyUnit> spawnableUnits = new List<EnemyUnit>();

    //Returns the combined power level of all currently spawned enemies
    private int CurrentPowerLevel
    {
        get
        {
            int ret = 0;
            foreach (EnemyUnit enemy in spawnedUnits)
                ret += enemy.powerLevel;
            return ret;
        }
    }

    private void Awake()
    {
        instance = this;
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");

        StartCoroutine(EnemySpawning());
        StartCoroutine(LevelIncrease());
    }

    private IEnumerator EnemySpawning()
    {
        while (true)
        {
            yield return null;

            int currentPowerLevel = CurrentPowerLevel;

            // If the current power level is just right
            if (currentPowerLevel >= maxCombinedPowerLevel)
                continue;

            // The maximum power level a spawned enemies can have
            int powerLevelDifference = maxCombinedPowerLevel - currentPowerLevel;

            // Add all enemies that are below the maximum power level difference
            List<EnemyUnit> unitsWithRightPowerLevel = new List<EnemyUnit>();
            foreach (EnemyUnit unit in spawnableUnits)
                if (unit.powerLevel <= powerLevelDifference)
                    unitsWithRightPowerLevel.Add(unit);

            // If no units weak enough exist
            if (unitsWithRightPowerLevel.Count == 0)
                continue;
            int chosenUnit = Random.Range(0, unitsWithRightPowerLevel.Count - 1);
            int chosenSpawnPos = Random.Range(0, spawnPoints.Length - 1);

            // Randomly choose unit and spawning position
            EnemyUnit spawned = Instantiate(unitsWithRightPowerLevel[chosenUnit], spawnPoints[chosenSpawnPos].transform.position, Quaternion.identity);
            spawnedUnits.Add(spawned);
        }
    }

    private IEnumerator LevelIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(levelTimer);
            // Slightly increase power level
            maxCombinedPowerLevel += powerLevelIncrease;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform spawnpointsParent;
    [SerializeField] private Transform player;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject greenEnemy;
    [SerializeField] private GameObject bloodEyesEnemy;
    [SerializeField] private GameObject wizardEnemy;

    private Transform[] spawnpoints;
    private float spawnCooldown = 0;
    private float spawnRate = 5;

    void Start()
    {
        //initialize spawnpoints array
        spawnpoints = new Transform[spawnpointsParent.childCount];

        for (int i = 0; i < spawnpoints.Length; i++)
        {
            spawnpoints[i] = spawnpointsParent.GetChild(i);
        }
    }

    void Update()
    {
        //when cooldown hits 0, enemy spawn is triggered
        if(spawnCooldown <= 0)
        {
            spawnCooldown = spawnRate; //<--reset spawn cooldown
            if (spawnRate > 2) 
            { 
                spawnRate -= 0.2f; //<--incremently decrease spawnRate if not at 2 yet
            }

            TriggerSpawn(); //<--trigger actual spawn event
        }
        else
        {
            spawnCooldown -= Time.deltaTime;
        }
    }

    private void TriggerSpawn()
    {
        //first determine what type of enemy to spawn (15% chance for blood eyes, 25% chance for wizard, 60% chance for green)
        float enemyType = Random.Range(0f, 1f);
        if (enemyType > 0.4f)
        {
            //spawn green enemy

            if (spawnRate > 3.5f)
            {
                SpawnEnemies(greenEnemy, Random.Range(2, 4)); //<--spawn 2 or 3 green enemies
            }
            else if (spawnRate > 2.5f)
            {
                SpawnEnemies(greenEnemy, Random.Range(3, 5)); //<--spawn 3 or 4 green enemies
            }
            else
            {
                SpawnEnemies(greenEnemy, 4); //<--spawn 4 green enemies
            }
        }
        else if (enemyType > 0.15f)
        {
            //spawn wizard enemy

            if (spawnRate > 3)
            {
                SpawnEnemies(wizardEnemy, 1); //<--spawn 1 wizard enemy
            }
            else
            {
                SpawnEnemies(wizardEnemy, Random.Range(1, 3)); //<--spawn 1 or 2 wizard enemies
            }
        }
        else
        {
            //spawn blood eyes enemy

            if (spawnRate > 2)
            {
                SpawnEnemies(bloodEyesEnemy, 1); //<--spawn 1 blood eyes enemy
            }
            else
            {
                SpawnEnemies(bloodEyesEnemy, 2); //<--spawn 2 blood eyes enemies
            }
        }
    }

    //this will be used after spawn algorithm decides on type and amount of enemies to spawn
    private void SpawnEnemies(GameObject enemyType, int enemyAmount)
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            Transform randomSpawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            var newEnemy = Instantiate(enemyType, randomSpawnpoint.position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().player = player; //<--remember to give each enemy a reference to the player
        }
    }
}

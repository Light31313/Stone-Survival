using System.Collections.Generic;
using GgAccel;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float ONE_MINUTE = 60f;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private WaveInfo[] waveInfo;
    [SerializeField] private float spawnDelay = 1f;
    private float spawnTimer;
    [SerializeField] private float spawnRadius = 20;
    [SerializeField] private int numberOfEnemies = 10;
    [SerializeField] private int increaseEnemyEachWave = 5;
    [SerializeField] private List<Enemy> enemyPrefabs;
    private int waveIndex = 0;
    private float countDownForNextWave;
    private bool hasSummonBoss = false;

    private List<Enemy> activeEnemies = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnDelay;
        countDownForNextWave = ONE_MINUTE;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null)
            return;

        countDownForNextWave -= Time.deltaTime;
        if (countDownForNextWave <= 0)
        {
            if (waveIndex < waveInfo.Length - 1)
                waveIndex++;
            hasSummonBoss = false;
            numberOfEnemies += increaseEnemyEachWave;
            countDownForNextWave = ONE_MINUTE;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnDelay;
        }
    }

    private void FixedUpdate()
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.ChaseTarget(playerPosition);
        }
    }

    public void SpawnEnemy()
    {
        var enemies = waveInfo[waveIndex].Enemies;
        var bosses = waveInfo[waveIndex].Bosses;

        if (!hasSummonBoss && bosses.Length > 0)
        {
            hasSummonBoss = true;
            foreach (var boss in bosses)
            {
                var randomPos = new Vector3(playerPosition.position.x, playerPosition.position.y, 0) +
                                (Vector3)RandomExtension.OnUnitCircle() * spawnRadius;
                SpawnEnemyFromPool(boss, randomPos);
            }
        }
        else
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                var randomPos = new Vector3(playerPosition.position.x, playerPosition.position.y, 0) +
                                (Vector3)RandomExtension.OnUnitCircle() * spawnRadius;
                var enemy = enemies[Random.Range(0, enemies.Length)];
                SpawnEnemyFromPool(enemy, randomPos);
            }
        }
    }

    private void SpawnEnemyFromPool(EnemyType type, Vector3 position)
    {
        var enemyPrefab = enemyPrefabs.Find(enemy => enemy.Type == type);
        var enemy = Pool.Get(enemyPrefab);

        activeEnemies.Add(enemy);
        enemy.initDieAction(() =>
        {
            Pool.Release(enemy);
            activeEnemies.Remove(enemy);
        });
        enemy.transform.position = position;
    }
}
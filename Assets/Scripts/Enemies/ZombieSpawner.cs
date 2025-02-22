using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public ZombiePool normalPool;
    public ZombiePool bossPool;

    public Transform[] spawnPoints;
    public Transform player;

    public ZombieType normalType;
    public ZombieType bossType;

    private LevelData currentLevel;
    private int currentWaveIndex = 0;
    private int totalZombiesAlive = 0;

    void Start()
    {
        LevelManager lm = FindObjectOfType<LevelManager>();
        if (lm == null)
            return;

        currentLevel = lm.GetCurrentLevel();
        if (currentLevel == null)
            return;

        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        for (currentWaveIndex = 1; currentWaveIndex <= currentLevel.totalWaves; currentWaveIndex++)
        {
            int enemiesInThisWave = currentLevel.totalEnemies / currentLevel.totalWaves;
            
            int bossesInThisWave = Random.Range(currentLevel.minBossCountPerWave, currentLevel.maxBossCountPerWave);

            yield return StartCoroutine(SpawnWave(enemiesInThisWave, bossesInThisWave));

            yield return new WaitUntil(() => totalZombiesAlive <= 0);

        }
        FindObjectOfType<GameManager>()?.CompleteLevel();
    }

    private IEnumerator SpawnWave(int normalCount, int bossCount)
    {
        for (int i = 0; i < normalCount; i++)
        {
            SpawnZombie(false);
            yield return new WaitForSeconds(currentLevel.spawnRate);
        }

        for (int b = 0; b < bossCount; b++)
        {
            SpawnZombie(true);
            yield return new WaitForSeconds(currentLevel.spawnRate);
        }
    }

    private void SpawnZombie(bool isBoss)
    {
        GameObject zObj = isBoss ? bossPool.GetZombieFromPool() : normalPool.GetZombieFromPool();

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        zObj.transform.position = sp.position;
        zObj.transform.rotation = sp.rotation;

        ZombieController zc = zObj.GetComponent<ZombieController>();
        if (zc != null)
        {
            zc.zombieType = isBoss ? bossType : normalType;
            zc.spawner = this;
        }

        ZombieAI ai = zObj.GetComponent<ZombieAI>();
        if (ai != null && player != null)
        {
            ai.SetTarget(player);
        }
        zObj.SetActive(true);

        totalZombiesAlive++;
    }

    public void ZombieKilled()
    {
        totalZombiesAlive--;
        if (totalZombiesAlive < 0) totalZombiesAlive = 0;
    }
}

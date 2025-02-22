using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Pools")]
    public ZombiePool normalPool;  // Normal prefab havuzu
    public ZombiePool bossPool;    // Boss prefab havuzu

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Player")]
    public Transform player;

    // Eğer sadece scriptable object parametresiyle seçecekseniz:
    public ZombieType normalType;  // isBoss = false
    public ZombieType bossType;    // isBoss = true

    [Header("Spawn Counts")]
    public int normalZombiesCount = 3;
    public int bossZombiesCount = 1;

    void Start()
    {
        // Örnek: "Sabit" sayıda 3 normal + 1 boss
        SpawnMultipleZombies(normalType, normalZombiesCount);
        SpawnMultipleZombies(bossType, bossZombiesCount);
    }

    void SpawnMultipleZombies(ZombieType zType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnZombie(zType);
        }
    }

    void SpawnZombie(ZombieType zType)
    {
        GameObject zObj = null;

        // 1) Boss mu normal mi bak
        if (zType.isBoss)
        {
            // Boss
            zObj = bossPool.GetZombieFromPool();
        }
        else
        {
            // Normal
            zObj = normalPool.GetZombieFromPool();
        }

        // 2) Konum
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        zObj.transform.position = sp.position;
        zObj.transform.rotation = sp.rotation;

        // 3) ZombieController atama
        ZombieController zc = zObj.GetComponent<ZombieController>();
        if (zc != null)
        {
            zc.zombieType = zType;   // BossType veya NormalType
            zc.spawner = this;
        }

        // 4) AI hedef
        ZombieAI ai = zObj.GetComponent<ZombieAI>();
        if (ai != null && player != null)
        {
            ai.SetTarget(player);
        }

        // 5) Aktif et
        zObj.SetActive(true);
    }

    public void ZombieKilled()
    {
        // Zombi öldüğünde eğer sayım yapacaksanız, burada yapabilirsiniz
    }
}

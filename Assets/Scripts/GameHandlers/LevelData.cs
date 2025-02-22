using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    public string sceneName;

    [Header("Level Ayarları")]
    public int totalWaves;
    public int totalEnemies;
    public float spawnRate;
    public int maxBossCountPerWave;
    public int minBossCountPerWave;

    [Header("Ödüller")]
    public int rewardCoins;

}

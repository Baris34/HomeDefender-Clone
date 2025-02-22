using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerData playerData;
    public LevelManager levelManager;
    public SaveManager saveManager;
    public WeaponManager weaponManager;
    private void Awake()
    {
        saveManager.LoadGameData(playerData, weaponManager);
    }

    void Start()
    {
        if (levelManager == null || playerData == null)
            return;
        if (SceneManager.GetActiveScene().name == levelManager.GetCurrentLevel().sceneName)
            return;

        LoadLevel(playerData.currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        LevelData level = levelManager.GetCurrentLevel();
        if (level != null)
            SceneManager.LoadScene(level.sceneName);
    }

    public void CompleteLevel()
    {
        playerData.Coin+=levelManager.GetCurrentLevel().rewardCoins;
        playerData.currentLevel++;
        saveManager.SaveGameData(playerData, weaponManager);
        levelManager.LoadNextLevel();
        LoadLevel(levelManager.currentLevelIndex);
    }

    private void OnApplicationQuit()
    {
        saveManager.SaveGameData(playerData, weaponManager);
    }
}

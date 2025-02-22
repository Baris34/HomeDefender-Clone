using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveManager : MonoBehaviour
{
    private const string COINS_KEY = "Coins";
    private const string CURRENT_WEAPON_KEY = "CurrentWeaponIndex";
    private const string UNLOCKED_WEAPONS_KEY = "UnlockedWeapons";
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    public void SaveGameData(PlayerData playerData, WeaponManager weaponManager)
    {
        PlayerPrefs.SetInt(COINS_KEY, playerData.Coin);
        
        int currentWeaponIndex = weaponManager.GetCurrentWeaponIndex();
        PlayerPrefs.SetInt(CURRENT_WEAPON_KEY, currentWeaponIndex);

        string unlockedStr = string.Join(",", playerData.unlockedWeaponNames);
        PlayerPrefs.SetString(UNLOCKED_WEAPONS_KEY, unlockedStr);
        
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, playerData.currentLevel);
        
        PlayerPrefs.Save();

        Debug.Log("SaveManager: Oyun verileri PlayerPrefs'e kaydedildi!");
    }
    public void LoadGameData(PlayerData playerData, WeaponManager weaponManager)
    {
        if (PlayerPrefs.HasKey(COINS_KEY))
        {
            int savedCoin = PlayerPrefs.GetInt(COINS_KEY);
            playerData.Coin = savedCoin;
        }

        if (PlayerPrefs.HasKey(CURRENT_WEAPON_KEY))
        {
            int wIndex = PlayerPrefs.GetInt(CURRENT_WEAPON_KEY);
            weaponManager.EquipWeapon(wIndex);
        }

        if (PlayerPrefs.HasKey(UNLOCKED_WEAPONS_KEY))
        {
            string unlockedStr = PlayerPrefs.GetString(UNLOCKED_WEAPONS_KEY);
            if (!string.IsNullOrEmpty(unlockedStr))
            {
                playerData.unlockedWeaponNames = new List<string>(unlockedStr.Split(','));
            }
        }

        if (PlayerPrefs.HasKey(CURRENT_LEVEL_KEY))
        {
            playerData.currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY);
        }

        Debug.Log("SaveManager: PlayerPrefs'ten veriler yüklendi.");
    }

    }

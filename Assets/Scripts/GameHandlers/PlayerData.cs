using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Oyuncu Temel İstatistikleri")]
    public int currentLevel = 1;

    public int selectedWeaponIndex;
    [Header("Kaynaklar")]
    [SerializeField] private int _coin = 0;
    
    public event Action<int> OnCoinChanged;

    public int Coin
    {
        get { return _coin; }
        set
        {
            _coin = value;
            OnCoinChanged?.Invoke(_coin);
        }
    }
    [Header("Oyuncu İlerlemesi ve Koleksiyonlar")]
    public List<string> unlockedWeaponNames = new List<string>();
    
    public bool HasWeaponUnlocked(string weaponName)
    {
        return unlockedWeaponNames.Contains(weaponName);
    }
    
}

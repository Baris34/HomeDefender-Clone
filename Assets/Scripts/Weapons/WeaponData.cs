using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponStats")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.2f;
    public float recoilKickBack = 0.05f;
    public RuntimeAnimatorController overrideController;
    public int unlockPrice; 
    public bool isUnlocked; 
    
    public int maxAmmoInClip;
    public int totalAmmo;
    
    [HideInInspector] public int currentAmmoInClip;
    void OnEnable()
    {
        currentAmmoInClip = maxAmmoInClip;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponStats")]
public class WeaponData : ScriptableObject
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.2f;  // Saniyede kaç atış? (veya mermi başına bekleme)
    public float recoilKickBack = 0.05f; // Pozisyon geriye gitmesi
    public RuntimeAnimatorController overrideController;
    
    public int maxAmmoInClip; // 🔥 Şarjördeki maksimum mermi (ör: 30 mermi)
    public int totalAmmo; // 🔥 Toplam taşınan mermi (ör: 90 mermi)
    
    [HideInInspector] public int currentAmmoInClip; // 🔥 Şarjörde kalan mermi (Inspector'da gizli)
    void OnEnable()
    {
        currentAmmoInClip = maxAmmoInClip; // **Silah alındığında şarjör full olacak**
    }
}


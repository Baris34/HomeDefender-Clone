using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunAmmoManager
{
    private GunController gunController;
    private WeaponData currentStats;
    private TextMeshProUGUI ammoText => gunController.ammoText;


    public GunAmmoManager(GunController controller)
    {
        gunController = controller;
        currentStats = gunController.currentStats;
    }

    public void InitializeAmmo()
    {
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentStats.currentAmmoInClip} / {currentStats.totalAmmo}";
        }
    }

    public void DecreaseAmmoInClip()
    {
        if (currentStats.currentAmmoInClip > 0)
        {
            currentStats.currentAmmoInClip--;
        }
    }

    public void ReloadAmmo()
    {
        
        int neededAmmo = currentStats.maxAmmoInClip - currentStats.currentAmmoInClip;
        int ammoToReload = Mathf.Min(neededAmmo, currentStats.totalAmmo);

        currentStats.currentAmmoInClip = currentStats.totalAmmo;
        
        UpdateAmmoUI();
    }
    
    public bool IsClipEmpty()
    {
        return currentStats.currentAmmoInClip <= 0;
    }

    public bool HasTotalAmmo()
    {
        return currentStats.totalAmmo > 0;
    }
}

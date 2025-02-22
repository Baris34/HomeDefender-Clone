using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponEntry
    {
        public string weaponName;
        public WeaponData weaponStats;
        public GameObject weaponModel;
    }
    public Animator playerAnimator;
    public List<WeaponEntry> weapons;
    public TwoBoneIKConstraint leftArmConstraint;
    public GunController gunController;
    private int currentWeaponIndex;
    private WeaponEntry currentWeapon;
    public PlayerData playerData;
    void Start()
    {
        
        if (weapons.Count > 0)
        {
            EquipWeapon(playerData.selectedWeaponIndex);
        }
    }
    
    public void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
            return;

        foreach (var weapon in weapons)
        {
            if (weapon.weaponModel != null)
                weapon.weaponModel.SetActive(false);
        }

        currentWeapon = weapons[weaponIndex];

        if (playerAnimator != null && currentWeapon.weaponStats.overrideController != null)
            playerAnimator.runtimeAnimatorController = currentWeapon.weaponStats.overrideController;
        
        currentWeapon.weaponModel.SetActive(true);
        currentWeaponIndex = weaponIndex;
        Transform gripPoint = currentWeapon.weaponModel.transform.Find("GripPoint");
        gunController.animator = currentWeapon.weaponModel.GetComponent<Animator>();
        if (gripPoint == null)
            return;

        leftArmConstraint.data.target = gripPoint;
        leftArmConstraint.data.targetPositionWeight = 1f;
        leftArmConstraint.data.targetRotationWeight = 1f;

        leftArmConstraint.GetComponentInParent<RigBuilder>().Build();

        if (gunController != null)
            gunController.SetWeaponStats(currentWeapon.weaponStats);
    }
    public GameObject GetActiveWeaponModel()
    {
        return currentWeapon.weaponModel;
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }
}
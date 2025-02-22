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
    public List<WeaponEntry> weapons; // Silahları dinamik olarak yönetmek için liste
    public TwoBoneIKConstraint leftArmConstraint;
    public GunController gunController;

    private WeaponEntry currentWeapon; // Şu anki silah

    void Start()
    {
        
        if (weapons.Count > 0)
        {
            
            EquipWeapon(0); // Varsayılan olarak ilk silahı seç
            
        }
        else
        {
            Debug.LogError("WeaponManager: Hiç silah eklenmemiş!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) EquipWeapon(0); // İlk silah (Handgun)
        if (Input.GetKeyDown(KeyCode.Y)) EquipWeapon(1); // İkinci silah (AR)
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2); // Üçüncü silah (SMG)
    }

    public void EquipWeapon(int weaponIndex)
    {
        
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
        {
            Debug.LogError($"WeaponManager: Geçersiz silah indeksi {weaponIndex}!");
            return;
        }

        // 1) Tüm silahları kapat
        foreach (var weapon in weapons)
        {
            if (weapon.weaponModel != null)
                weapon.weaponModel.SetActive(false);
        }

        // 2) Yeni silahı seç
        currentWeapon = weapons[weaponIndex];

        if (playerAnimator != null && currentWeapon.weaponStats.overrideController != null)
        {
            playerAnimator.runtimeAnimatorController = currentWeapon.weaponStats.overrideController;
            Debug.Log($"WeaponManager: {currentWeapon.weaponName} için Animator Override uygulandı.");
        }
        // 3) Yeni silah modelini ÖNCE aktif et!
        currentWeapon.weaponModel.SetActive(true);

        // 4) GripPoint'ı bul
        Transform gripPoint = currentWeapon.weaponModel.transform.Find("GripPoint");
        gunController.animator = currentWeapon.weaponModel.GetComponent<Animator>();
        if (gripPoint == null)
        {
            Debug.LogError($"WeaponManager: {currentWeapon.weaponName} içinde 'GripPoint' bulunamadı! Silah modelinin içinde 'GripPoint' adında bir Empty GameObject olduğundan emin ol.");
            return;
        }

        // 5) IK hedefini güncelle
        leftArmConstraint.data.target = gripPoint;
        leftArmConstraint.data.targetPositionWeight = 1f;
        leftArmConstraint.data.targetRotationWeight = 1f;

        // 6) Rig'i güncelle (Animation Rigging sisteminde değişiklikleri anında görmek için)
        leftArmConstraint.GetComponentInParent<RigBuilder>().Build();

        // 7) GunController’a yeni silah verilerini gönder
        if (gunController != null)
        {
            gunController.SetWeaponStats(currentWeapon.weaponStats);
        }

        Debug.Log($"WeaponManager: {currentWeapon.weaponName} başarıyla donatıldı!");
    }
    public GameObject GetActiveWeaponModel()
    {
        return currentWeapon.weaponModel;
    }
}
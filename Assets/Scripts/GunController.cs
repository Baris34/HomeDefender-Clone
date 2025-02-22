using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public Transform muzzleTransform;
    public ParticleSystem muzzleFlash;
    public float nextFireTime = 0f;
    public float headshotMultiplier = 2f;
    public Animator animator;

    public Material bulletTrailMaterial;
    public WeaponManager weaponManager;
    
    public WeaponData currentStats;
    
    public float reloadTime = 2f;
    
    public IGunState currentState;
    public bool isReloading = false;
    
    public TextMeshProUGUI ammoText;
    public LayerMask targetLayer; // 🎯 Hedeflenebilir katmanlar (Enemy, Explosive, FireTrap)

    void Start()
    {
        SwitchState(new IdleState()); // Başlangıçta "Idle" modunda başlasın
    }
    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentStats.currentAmmoInClip} / {currentStats.totalAmmo}";
        }
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }

        if (Input.GetButton("Fire1") && currentState is IdleState && Time.time >= nextFireTime) // Sürekli ateş edebilme
        {
            SwitchState(new FiringState());
        }
    }

    public void SwitchState(IGunState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void FireWeapon()
    {
        if (currentStats == null || isReloading)
            return;
        GameObject activeWeapon = weaponManager.GetActiveWeaponModel();
        
        if (activeWeapon == null)
        {
            Debug.LogError("❌ Aktif Silah Modeli Bulunamadı!");
            return;
        }
        
        if (currentStats.currentAmmoInClip <= 0)
        {
            Debug.Log("❌ Mermi bitti! Yeniden doldurulması gerekiyor.");
            return;
        }

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Ray centerRay = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        bool validTarget = Physics.Raycast(centerRay, out hit, currentStats.range, targetLayer);

        if (validTarget)
        {
            Debug.Log("🎯 Hedef vuruldu: " + hit.collider.name + " - Tag: " + hit.collider.tag);

            // **Eğer hedef geçerliyse, mermiyi azalt**
            currentStats.currentAmmoInClip--;
            UpdateAmmoUI();
            
            ZombieController zc = hit.collider.GetComponentInParent<ZombieController>();
            if (zc != null)
            {
                if (hit.collider.CompareTag("Head"))
                {
                    zc.TakeDamage(currentStats.damage * headshotMultiplier); // Headshot
                }
                else
                {
                    zc.TakeDamage(currentStats.damage); // Normal damage
                }
            }

            FireTrap fireTrap = hit.collider.GetComponentInParent<FireTrap>();
            if (fireTrap != null)
            {
                fireTrap.ActivateFireTrap();
            }

            ExplosiveBarrel explosiveBarrel = hit.collider.GetComponent<ExplosiveBarrel>();
            if (explosiveBarrel != null)
            {
                explosiveBarrel.Explode();
            }
            nextFireTime = Time.time + currentStats.fireRate;
            ApplyRecoil(activeWeapon.transform);
            animator.SetTrigger("FireTrigger");

            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            // Tracer için hedef nokta belirle
            Vector3 targetPoint = validTarget ? hit.point : centerRay.origin + centerRay.direction * currentStats.range;
            SpawnTracer(muzzleTransform.position, targetPoint);
        }
        else
        {
            Debug.Log("❌ Geçersiz hedef! Mermi sayısı değişmedi.");
        }

       
    }

    void ApplyRecoil(Transform weaponTransform)
    {
        weaponTransform.DOKill(); // Önceki DoTween animasyonlarını durdur

        float recoilAmount = currentStats.recoilKickBack; 

        Vector3 originalPos = weaponTransform.localPosition;

        weaponTransform.DOLocalMoveZ(originalPos.z + recoilAmount, 0.1f).SetEase(Ease.OutQuad);
        weaponTransform.DOLocalMove(originalPos, 0.15f).SetEase(Ease.InQuad);
    }

    void SpawnTracer(Vector3 start, Vector3 end)
    {
        GameObject tracerObj = new GameObject("Tracer", typeof(LineRenderer));
        LineRenderer lr = tracerObj.GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.02f;
        lr.endWidth = 0.01f;

        if (bulletTrailMaterial != null)
        {
            lr.material = bulletTrailMaterial;
        }
        else
        {
            Debug.LogError("❌ Bullet Trail Material eksik! Inspector’dan ataman gerekiyor.");
        }

        Destroy(tracerObj, 0.05f);
    }

    public void SetWeaponStats(WeaponData newStats)
    {
        currentStats = newStats;
        UpdateAmmoUI();
    }
}

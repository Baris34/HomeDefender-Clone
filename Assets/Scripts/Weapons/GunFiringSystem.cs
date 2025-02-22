using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GunFiringSystem 
{
    private GunController gunController;
    private WeaponData currentStats => gunController.currentStats;
    private Transform muzzleTransform => gunController.muzzleTransform;
    private ParticleSystem muzzleFlash => gunController.muzzleFlash;
    private Animator animator => gunController.animator;
    private Material bulletTrailMaterial => gunController.bulletTrailMaterial;
    private WeaponManager weaponManager => gunController.weaponManager;
    private LayerMask targetLayer => gunController.targetLayer;

    public Image crosshairImage;
    private Color defaultCrosshairColor = Color.white;
    public GunFiringSystem(GunController controller)
    {
        gunController = controller;
    }

    public void FireWeapon()
    {
        if (currentStats == null || gunController.isReloading)
            return;
        GameObject activeWeapon = weaponManager.GetActiveWeaponModel();

        if (activeWeapon == null)
            return;

        if (currentStats.currentAmmoInClip <= 0)
            return;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        Ray centerRay = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        bool validTarget = Physics.Raycast(centerRay, out hit, currentStats.range, targetLayer);

        if (validTarget)
        {
            gunController.ammoManager.DecreaseAmmoInClip();
            gunController.ammoManager.UpdateAmmoUI();

            ZombieController zc = hit.collider.GetComponentInParent<ZombieController>();
            if (zc != null)
            {
                if (hit.collider.CompareTag("Head"))
                {
                    zc.TakeDamage(currentStats.damage * gunController.headshotMultiplier);
                    OnHeadshotFeedback();
                }
                else
                {
                    zc.TakeDamage(currentStats.damage);
                }
            }

            FireTrap fireTrap = hit.collider.GetComponentInParent<FireTrap>();
            if (fireTrap != null)
            {
                fireTrap.TriggerFireTrap();
            }

            ExplosiveBarrel explosiveBarrel = hit.collider.GetComponent<ExplosiveBarrel>();
            if (explosiveBarrel != null)
            {
                explosiveBarrel.Explode();
            }
            gunController.nextFireTime = Time.time + currentStats.fireRate;
            ApplyRecoil(activeWeapon.transform);
            animator.SetTrigger("FireTrigger");

            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
            Vector3 targetPoint = validTarget ? hit.point : centerRay.origin + centerRay.direction * currentStats.range;
            SpawnTracer(muzzleTransform.position, targetPoint);
        }
    }
    public void OnHeadshotFeedback()
    {
        crosshairImage.DOColor(Color.red, 0.1f)
            .OnComplete(() => {
                crosshairImage.DOColor(defaultCrosshairColor, 0.2f);
            });
    }
    private void ApplyRecoil(Transform weaponTransform)
    {
        weaponTransform.DOKill();

        float recoilAmount = currentStats.recoilKickBack;

        Vector3 originalPos = weaponTransform.localPosition;

        weaponTransform.DOLocalMoveZ(originalPos.z + recoilAmount, 0.1f).SetEase(Ease.OutQuad);
        weaponTransform.DOLocalMove(originalPos, 0.15f).SetEase(Ease.InQuad);
    }

    private void SpawnTracer(Vector3 start, Vector3 end)
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
        Object.Destroy(tracerObj, 0.05f);
    }
}

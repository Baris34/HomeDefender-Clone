using System.Collections;
using UnityEngine;

public class ReloadingState : IGunState
{
    public void EnterState(GunController gun)
    {
        if (gun.currentStats.totalAmmo <= 0)
        {
            Debug.Log("⚠️ Mermi tamamen bitti! Reload yapılamaz.");
            gun.SwitchState(new IdleState()); // Mermi yoksa tekrar Idle state'e dön
            return;
        }

        Debug.Log("🔄 Yeniden dolduruluyor...");
        gun.StartCoroutine(ReloadCoroutine(gun));
    }

    public void UpdateState()
    {
        // Burada ekstra reload efektleri oynatılabilir.
    }

    public void ExitState()
    {
        Debug.Log("✅ Reload tamamlandı.");
    }

    private IEnumerator ReloadCoroutine(GunController gun)
    {
        gun.isReloading = true;
        if (gun.animator != null)
        {
            gun.animator.SetTrigger("ReloadTrigger"); // Animasyonu oynat
        }

        yield return new WaitForSeconds(gun.reloadTime); // Reload süresi bekleniyor

        int neededAmmo = gun.currentStats.maxAmmoInClip - gun.currentStats.currentAmmoInClip;
        int ammoToReload = Mathf.Min(neededAmmo, gun.currentStats.totalAmmo);

        gun.currentStats.currentAmmoInClip += ammoToReload;
        gun.currentStats.totalAmmo -= ammoToReload;

        gun.isReloading = false;
        gun.UpdateAmmoUI();
        gun.SwitchState(new IdleState()); // **Reload bitince Idle moda geç**
    }
}
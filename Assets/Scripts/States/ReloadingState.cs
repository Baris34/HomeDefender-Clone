using System.Collections;
using UnityEngine;

public class ReloadingState : IGunState
{
    public void EnterState(GunController gun)
    {
        if (!gun.ammoManager.HasTotalAmmo())
        {
            gun.SwitchState(new IdleState());
            return;
        }
        gun.StartCoroutine(ReloadCoroutine(gun));
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }

    private IEnumerator ReloadCoroutine(GunController gun)
    {
        gun.isReloading = true;
        if (gun.animator != null)
        {
            gun.animator.SetTrigger("ReloadTrigger");
        }

        yield return new WaitForSeconds(gun.reloadTime);

        gun.ammoManager.ReloadAmmo();

        gun.isReloading = false;
        gun.SwitchState(new IdleState());
    }
}
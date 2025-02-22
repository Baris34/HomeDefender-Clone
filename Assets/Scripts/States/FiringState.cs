    using UnityEngine;
    
    public class FiringState : IGunState
    {
        private GunController gun;
        public void EnterState(GunController gun)
        {
            this.gun = gun;
        }

        public void UpdateState()
        {
            if (gun.currentStats.currentAmmoInClip <= 0)
            {
                Debug.Log("🚨 Mermi bitti! Reload yapılıyor...");
                gun.SwitchState(new ReloadingState());
                return;
            }

            if (Input.GetButton("Fire1"))
            {
                if (Time.time >= gun.nextFireTime)
                {
                    gun.nextFireTime = Time.time + gun.currentStats.fireRate;
                    gun.FireWeapon();
                }
            }
            else
            {
                gun.SwitchState(new IdleState());
            }
        }

        public void ExitState()
        {
            Debug.Log("🔴 Ateş durdu.");
        }
    }

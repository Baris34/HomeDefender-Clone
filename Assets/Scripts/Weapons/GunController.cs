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
    private IGunState currentState;
    public bool isReloading = false;
    public TextMeshProUGUI ammoText;
    public LayerMask targetLayer;
    
    public GunFiringSystem firingMechanism { get; private set; }
    public GunAmmoManager ammoManager { get; private set; }


    void Start()
    {
        
        ammoManager = new GunAmmoManager(this);
        firingMechanism = new GunFiringSystem(this);
        
        ammoManager.InitializeAmmo(); 
        SwitchState(new IdleState());
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }

        if (Input.GetButton("Fire1") && currentState is IdleState && Time.time >= nextFireTime)
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
        firingMechanism.FireWeapon();
    }

    public void SetWeaponStats(WeaponData newStats)
    {
        currentStats = newStats;
        if (ammoManager != null)
        {
            ammoManager.InitializeAmmo();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class FireTrap : MonoBehaviour
{
    public Transform targetPivot;
    public ParticleSystem fireEffect;
    public float fallSpeed = 2f;
    public float damageRadius = 3f;
    public float fireDuration = 3f;
    public float returnDelay = 7f;
    public Vector3 fallRotation = new Vector3(-180f, -90f, 360f);

    public event Action OnFireTrapActivated;
    
    private Quaternion originalRotation;
    private bool isActivated = false;
    public List<ZombieController> burningEnemies = new List<ZombieController>();
    private BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        originalRotation = targetPivot.rotation;
        OnFireTrapActivated += ActivateFireTrap;
    }
    public void TriggerFireTrap()
    {
        
        OnFireTrapActivated?.Invoke();
        boxCollider.enabled = false;
    }
    private void ActivateFireTrap()
    {
        if (isActivated) return;
        isActivated = true;

        targetPivot.DOLocalRotate(fallRotation, fallSpeed)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                
                fireEffect.Play();
                StartCoroutine(CheckBurningEnemies());
                Invoke(nameof(ResetFireTrap), returnDelay);
            });
    }

    void ResetFireTrap()
    {
        boxCollider.enabled = true;
        fireEffect.Stop();
        targetPivot.DORotateQuaternion(originalRotation, fallSpeed).SetEase(Ease.InOutBack);
        isActivated = false;
        
        foreach (var enemy in burningEnemies)
        {
            if (enemy != null)
                enemy.StopBurning();
        }
        burningEnemies.Clear();
    }

    IEnumerator CheckBurningEnemies()
    {
        while (isActivated)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
            
            List<ZombieController> newBurningEnemies = new List<ZombieController>();

            foreach (var enemy in hitColliders)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    ZombieController enemyScript = enemy.GetComponent<ZombieController>();
                
                    if (enemyScript != null && !burningEnemies.Contains(enemyScript))
                    {
                        enemyScript.Burn(fireDuration);
                        burningEnemies.Add(enemyScript);
                    }
                    newBurningEnemies.Add(enemyScript);
                }
            }
            foreach (var enemy in burningEnemies)
            {
                if (!newBurningEnemies.Contains(enemy))
                {
                    enemy.StopBurning();
                }
            }
            burningEnemies = newBurningEnemies;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

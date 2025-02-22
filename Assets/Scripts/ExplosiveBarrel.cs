using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject[] debrisParts; // **Patlamış varil parçaları**
    public ParticleSystem explosionEffect; // **Patlama efekti**
    public float explosionRadius = 5f; // **Patlama menzili**
    public float explosionForce = 700f; // **Patlama gücü**
    public float damageAmount = 50f; // **Düşmanlara verilen hasar**
    public float destroyDelay = 3f; // **Parçaların yok olma süresi**
    
    private bool isExploded = false;
    
    public void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        Debug.Log("💥 TNT Varili Patladı!");
        
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        // **2) Patlama Efektini Çalıştır**
        if (explosionEffect != null)
        {
            explosionEffect.Play();
        }

        // **3) İçindeki parçaları aç ve fizik ekle**
        foreach (var part in debrisParts)
        {
            part.SetActive(true);
            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // **4) Yakındaki Düşmanlara Hasar Ver**
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                ZombieController enemy = hit.GetComponent<ZombieController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                }
            }
        }

        // **5) Parçaları Belirli Bir Süre Sonra Yok Et**
        StartCoroutine(DestroyDebris());
    }

    IEnumerator DestroyDebris()
    {
        yield return new WaitForSeconds(destroyDelay);
        
        foreach (var part in debrisParts)
        {
            if (part != null)
            {
                part.SetActive(false); // **Parçaları yok et**
            }
        }

        Destroy(gameObject); // **Varilin kalıntılarını da yok et**
    }
}

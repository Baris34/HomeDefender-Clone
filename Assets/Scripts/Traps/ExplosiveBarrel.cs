using System.Collections;
using UnityEngine;
public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject[] debrisParts;
    public ParticleSystem explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float damageAmount = 50f;
    public float destroyDelay = 3f;
    
    private bool _isExploded = false;
    
    public void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;
        
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        if (explosionEffect != null)
        {
            explosionEffect.Play();
        }

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
        StartCoroutine(DestroyDebris());
    }

    IEnumerator DestroyDebris()
    {
        yield return new WaitForSeconds(destroyDelay);
        
        foreach (var part in debrisParts)
        {
            if (part != null)
            {
                part.SetActive(false);
            }
        }
        Destroy(gameObject);
    }
}

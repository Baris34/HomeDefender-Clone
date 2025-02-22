using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
    public ZombieType zombieType;
    public GameObject[] bodyParts;
    public ParticleSystem burnEffect; // **Yanma efekti**
    public float burnDamagePerSecond = 5f; // **Yanarken alınan hasar**
    private bool isBurning = false;
    private Coroutine burnCoroutine;
    [HideInInspector] public ZombieSpawner spawner;

    private float currentHealth;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        // 1) SO verilerini uygula
        if (zombieType != null)
        {
            currentHealth = zombieType.health;
            if (agent != null)
                agent.speed = zombieType.speed;
        }

        // 2) Random body part
        if (bodyParts != null && bodyParts.Length > 0)
        {
            foreach (GameObject part in bodyParts)
                part.SetActive(false);

            int randIndex = Random.Range(0, bodyParts.Length);
            bodyParts[randIndex].SetActive(true);
        }
    }
    public void Burn(float duration)
    {
        if (isBurning) return;
        isBurning = true;

        Debug.Log("🔥 Düşman yanıyor: " + gameObject.name);

        burnEffect.Play(); // **Yanma efektini aç**
        burnCoroutine = StartCoroutine(BurnDamage(duration));
    }

    IEnumerator BurnDamage(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            TakeDamage(burnDamagePerSecond * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopBurning(); // **Yanma süresi dolunca durdur**
    }

    public void StopBurning()
    {
        if (!isBurning) return;
        
        Debug.Log("🔥 Düşman artık yanmıyor: " + gameObject.name);

        if (burnCoroutine != null)
            StopCoroutine(burnCoroutine);

        burnEffect.Stop(); // **Yanma efektini kapat**
        isBurning = false;
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Ölüm animasyonu vs.
        gameObject.SetActive(false);  // Havuz'a geri döner

        // Spawner'a haber verebilirsiniz (opsiyonel)
        if (spawner != null)
        {
            spawner.ZombieKilled();
        }
    }
}

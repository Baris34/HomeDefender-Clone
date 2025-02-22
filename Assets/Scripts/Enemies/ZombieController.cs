using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
    public ZombieType zombieType;
    public GameObject[] bodyParts;
    public ParticleSystem burnEffect;
    public float burnDamagePerSecond = 5f;
    private bool isBurning = false;
    private Coroutine burnCoroutine;
    [HideInInspector] public ZombieSpawner spawner;
    [SerializeField] private PlayerData playerData;
    public int coinValueOnDeath = 1;
    private float currentHealth;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        if (zombieType != null)
        {
            currentHealth = zombieType.health;
            if (agent != null)
                agent.speed = zombieType.speed;
        }
        
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
        
        burnEffect.Play();
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
    }

    public void StopBurning()
    {
        if (!isBurning) return;

        if (burnCoroutine != null)
            StopCoroutine(burnCoroutine);

        burnEffect.Stop();
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
        gameObject.SetActive(false);
        
        if (spawner != null)
        {
            spawner.ZombieKilled();
        }
        if (playerData != null)
        {
            playerData.Coin += coinValueOnDeath;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int poolSize = 10;
    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();
        for(int i = 0; i < poolSize; i++)
        {
            GameObject z = Instantiate(zombiePrefab, transform);
            z.SetActive(false);
            pool.Add(z);
        }
    }

    public GameObject GetZombieFromPool()
    {
        foreach(GameObject z in pool)
        {
            if(!z.activeInHierarchy)
                return z;
        }

        // Havuz biterse yenisi oluştur
        GameObject newZombie = Instantiate(zombiePrefab, transform);
        newZombie.SetActive(false);
        pool.Add(newZombie);
        return newZombie;
    }
}

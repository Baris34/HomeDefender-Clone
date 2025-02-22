using UnityEngine;

[CreateAssetMenu(fileName = "NewZombieType", menuName = "Zombies/ZombieType")]
public class ZombieType : ScriptableObject
{
    public float health = 50f;
    public float speed = 3.5f;
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewZombieType", menuName = "Zombies/ZombieType")]
public class ZombieType : ScriptableObject
{
    public string typeName;         // Örn: "Normal", "Boss"
    public float health = 50f;      // Can
    public float speed = 3.5f;      // NavMeshAgent hızı
    public float damage = 10f;      // Saldırı gücü (opsiyonel)
    public bool isBoss=false;
}
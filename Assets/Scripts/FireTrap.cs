using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireTrap : MonoBehaviour
{
    public Transform targetPart; // 🔥 Vurulunca düşecek olan parça
    public Transform targetPivot;
    public ParticleSystem fireEffect; // 🔥 İçindeki ateş efekti
    public float fallSpeed = 2f; // 🔥 Düşme hızı
    public float damageRadius = 3f; // 🔥 Ateşin etkilediği alan
    public float fireDuration = 3f; // 🔥 Düşmanlar kaç saniye yanacak?
    public float returnDelay = 7f;
    public Vector3 fallRotation = new Vector3(-180f, -90f, 360f); // 🔥 Hedef rotasyon

    private Quaternion originalRotation;
    private bool isActivated = false;
    public List<ZombieController> burningEnemies = new List<ZombieController>(); // **Yanmakta olan düşmanları takip et**

    private void Start()
    {
        originalRotation = targetPivot.rotation;
    }

    public void ActivateFireTrap()
    {
        if (isActivated) return;
        isActivated = true;

        Debug.Log("🔥 Fire Trap Aktif Edildi!");

        // **Parçayı döndürerek düşür**
        targetPivot.DOLocalRotate(fallRotation, fallSpeed)
            .SetEase(Ease.OutBounce) // **Düşüş sırasında esneklik efekti**
            .OnComplete(() =>
            {
                fireEffect.Play(); // **Ateş efektini aç**
                StartCoroutine(CheckBurningEnemies()); // 🔥 **Sürekli kontrol et**
                Invoke(nameof(ResetFireTrap), returnDelay); // **7 saniye sonra geri dön**
            });
    }

    void ResetFireTrap()
    {
        Debug.Log("🔄 Fire Trap Eski Pozisyonuna Geri Dönüyor.");

        fireEffect.Stop(); // **Ateş efektini kapat**
        targetPivot.DORotateQuaternion(originalRotation, fallSpeed).SetEase(Ease.InOutBack);
        isActivated = false;

        // 🔥 **Tüm yanan düşmanların yanmasını durdur**
        foreach (var enemy in burningEnemies)
        {
            if (enemy != null)
                enemy.StopBurning();
        }
        burningEnemies.Clear(); // **Listeyi temizle**
    }

    IEnumerator CheckBurningEnemies()
    {
        while (isActivated)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
            
            List<ZombieController> newBurningEnemies = new List<ZombieController>();

            foreach (var enemy in hitColliders)
            {
                if (enemy.CompareTag("Enemy")) // **Eğer düşman etkilendiyse**
                {
                    ZombieController enemyScript = enemy.GetComponent<ZombieController>();
                
                    if (enemyScript != null && !burningEnemies.Contains(enemyScript))
                    {
                        Debug.Log($"🔥 {enemy.name} ateşe yakalandı!");
                        enemyScript.Burn(fireDuration); // **Düşman yanmaya başlasın**
                        burningEnemies.Add(enemyScript);
                    }

                    newBurningEnemies.Add(enemyScript); // **Şu an alanda olan düşmanları listeye ekle**
                }
            }

            // **Alanın dışına çıkan düşmanları yakmayı durdur**
            foreach (var enemy in burningEnemies)
            {
                if (!newBurningEnemies.Contains(enemy))
                {
                    Debug.Log($"🔥 {enemy.name} ateş alanından çıktı!");
                    enemy.StopBurning();
                }
            }

            burningEnemies = newBurningEnemies; // **Yeni listeyi güncelle**
            yield return new WaitForSeconds(0.5f); // **Her 0.5 saniyede bir kontrol et**
        }
    }

}

    using UnityEngine;
    public class IdleState : IGunState
    {
        public void EnterState(GunController gun)
        {
            Debug.Log("🔵 Silah şu an bekleme modunda.");
        }

        public void UpdateState()
        {
            // Buraya gerekirse bekleme efektleri eklenebilir.
        }

        public void ExitState()
        {
            Debug.Log("🔵 Silah artık beklemede değil.");
        }
    }
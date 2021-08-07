using UnityEngine;

namespace KangJaeWook.SurvivalShooter.Weapons
{
    public class BaseAmmo : BaseItem
    {
        [SerializeField] private Rigidbody m_Rigdbody;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private float m_Damage;
        [SerializeField] private float m_FireSpeed;
        private HighSpeedAmmoChecker m_Checker;

        private float TotalDamage { get; set; } = 0;

        private void Awake()
        {
            m_Checker = GetComponent<HighSpeedAmmoChecker>();

        }

        public void Create(float Damage)
        {
            TotalDamage = m_Damage + Damage;
            m_Rigdbody.AddRelativeForce(Vector3.forward * m_FireSpeed, ForceMode.Impulse);
        }

        public void OnCheckCollision(Collider other)
        {
            m_Collider.isTrigger = false;
            m_Checker.enabled = false;

            Destroy(gameObject, 10f);

        }

    }
}

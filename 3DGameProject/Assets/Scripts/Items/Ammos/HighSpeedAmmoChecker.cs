using UnityEngine;

namespace KangJaeWook.SurvivalShooter.Weapons
{
    public class HighSpeedAmmoChecker : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Collider m_Collider;
        [SerializeField] private BaseAmmo m_Ammo;
        [SerializeField] public LayerMask m_Layermask = -1;
        [SerializeField] public float m_SkinWidth = 0.1f;
        private float m_MiniumExtent;
        private float m_PartialExtent;
        private float m_SqrtMinimumExtent;
        private Vector3 m_PreviousPosition;

        private void Start()
        {
            m_PreviousPosition = m_Rigidbody.position;
            m_MiniumExtent = Mathf.Min(
                Mathf.Min(m_Collider.bounds.extents.x, m_Collider.bounds.extents.y, m_Collider.bounds.extents.z));
            m_PartialExtent = m_MiniumExtent * (1.0f - m_SkinWidth);
            m_SqrtMinimumExtent = m_MiniumExtent * m_MiniumExtent;
        }

        private void FixedUpdate()
        {
            Vector3 movementThisStep = m_Rigidbody.position - m_PreviousPosition;
            float movementSqrMagnitude = movementThisStep.sqrMagnitude;

            if(movementSqrMagnitude > m_SqrtMinimumExtent)
            {
                float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);

                if(Physics.Raycast(m_PreviousPosition, movementThisStep
                    , out RaycastHit hitInfo,
                    movementMagnitude,
                    m_Layermask.value))
                {
                    if (hitInfo.collider.isTrigger) m_Ammo.OnCheckCollision(hitInfo.collider);
                    else
                        m_Rigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * m_PartialExtent;
                }
            }

        }
    }
}


using UnityEditor;

namespace Main.Characters
{
    using UnityEngine;
    
    public class CharacterMovementMB : MonoBehaviour
    {
        protected CharacterMB m_character;
        
        [SerializeField]
        protected CharacterMovementSettingsSO m_settings;
        
        // Movement states
        protected bool m_isGrounded;
        protected bool m_isJumping;
        protected bool m_isFalling;
        
        
        
        

        protected void Awake()
        {
            m_character = GetComponent<CharacterMB>();
        }

        public void HandleGrounding()
        {
            if (m_isJumping) return;
            
            Transform charTransform = m_character.CachedTransform;
            Vector3 charPosition = charTransform.position;
            CapsuleCollider col = m_character.Collider;
            
            Vector3 botOfColl = charPosition + col.center + Vector3.down * col.height * 0.5f;

            float rayDistance = botOfColl.y - charPosition.y + m_settings.SnapToGroundDistance - col.radius * 0.5f;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            int layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            Vector3 groundHitPos = Vector3.zero;
            
            RaycastHit hit;
            if (Physics.SphereCast(botOfColl, col.radius, Vector3.down, out hit, rayDistance, layerMask))
            {
                // Player is on the ground
                m_isGrounded = true;

                groundHitPos = hit.point;
                
                Debug.DrawLine(botOfColl, groundHitPos);
            }
            else
            {
                // Player is not on the ground
                m_isGrounded = false;
            }

            if (m_isGrounded)
            {
                // Turn off gravity for character while character is grounded to prevent sinking
                m_character.RigidBody.useGravity = false;

                // Calculate position on ground character should be at
                Vector3 targetStandingPos = charPosition;
                targetStandingPos.y = groundHitPos.y;

                charTransform.position = targetStandingPos;
            }
            else
            {
                m_character.RigidBody.useGravity = true;
            }
        }
        
        public void HandleMovement(Vector3 moveDirection)
        {
            Vector3 newVelocity = moveDirection * m_settings.Speed;

            if (!m_isGrounded)
            {
                newVelocity.y = m_character.RigidBody.velocity.y;
            }
            
            m_character.RigidBody.velocity = newVelocity;
        }

        public void HandleRotation(float targetAngle)
        {
            float currentRotateVelocity = 0;
            
            float smoothedRotationAngle = Mathf.SmoothDampAngle(
                m_character.CachedTransform.eulerAngles.y, targetAngle,
                ref currentRotateVelocity, m_settings.TurnSmoothTime * Time.deltaTime);

            m_character.CachedTransform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
        }

        public void Jump()
        {
            if (!m_isGrounded) return;

            m_isJumping = true;
            m_isGrounded = false;

            m_character.RigidBody.useGravity = true;

            Vector3 jumpVector = new Vector3(0, m_settings.JumpPower, 0);
            
            m_character.RigidBody.AddForce(jumpVector);
        }

        public void HandleJumping()
        {
            if (m_isJumping)
            {
                // TODO: Better jumping feel (rise less when not holding jump button)

                
                // If character is falling
                if (m_character.RigidBody.velocity.y < 0)
                {
                    m_isJumping = false;
                }
            }
        }
    }
}

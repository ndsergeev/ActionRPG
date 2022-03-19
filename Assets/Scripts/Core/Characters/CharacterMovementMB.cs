
using UnityEditor;

namespace Main.Characters
{
    using UnityEngine;

    using Main.Core;
    using Main.Core.Input;

    public class CharacterMovementMB : MovementMB
    {
        [SerializeField]
        protected CharacterMovementSettingsSO m_Settings;

        // MOVEMENT STATES
        [SerializeField]
        protected bool m_IsGrounded;

        [SerializeField]
        protected bool m_IsWalking;

        [SerializeField]
        protected bool m_IsRunning;

        [SerializeField]
        protected bool m_IsCrouchWalking;

        [SerializeField]
        protected bool m_IsStartingJump;

        [SerializeField]
        protected bool m_IsJumping;

        [SerializeField]
        protected bool m_IsFalling;

        public bool IsGrounded => m_IsGrounded;

        public bool IsWalking
        {
            get => m_IsWalking;
            set => m_IsWalking = value;
        }

        public bool IsRunning
        {
            get => m_IsRunning;
            set => m_IsRunning = value;
        }
        
        public bool IsCrouchWalking
        {
            get => m_IsCrouchWalking;
            set => m_IsCrouchWalking = value;
        }

        public bool IsStartingJump => m_IsStartingJump;
        public bool IsJumping => m_IsJumping;
        public bool IsFalling => m_IsFalling;

        // GROUNDING
        protected RaycastHit m_GroundHit;
        protected RaycastHit m_PrevGroundHit;
        protected Vector3 m_GroundingSphereCastPos;
        protected float m_GroundingSphereCastRad;

        // MOVEMENT
        protected Vector3 m_MoveDirection;

        public Vector3 MoveDirection => m_MoveDirection;

        // OBSTACLES


        // JUMPING
        protected float m_TimeSinceJumpStarted;
        protected float m_TimeAfterJumpToNotGroundCheck = 0.05f;
        
        // AIR VELOCITY
        protected Vector3 m_PreAirVelocity;
        protected Vector3 m_CurrentAirVelocity;

        private bool m_HasAirVelocity;

        // GUI
        protected Color m_GroundSphereColour = new Color(1f, 0.92f, 0.016f, 0.5f);

        protected virtual void Awake()
        {
            m_Character = GetComponent<CharacterMB>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = m_GroundSphereColour;
            Gizmos.DrawSphere(m_GroundingSphereCastPos, m_GroundingSphereCastRad);
        }
        
        protected virtual bool CheckIfTouchingGround()
        {
            const int layerMask = ~(1 << 8);
            
            var charTransform = m_Character.CachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider) m_Character.Col;

            var rayOrigin = charPosition + col.center;
            rayOrigin.y -= col.radius;
            m_GroundingSphereCastRad = m_Settings.SnapToGroundRadius;
            
            float rayDistance;
            if (m_IsGrounded)
            {
                // The grounding SphereCastRay distance will be longer while the character is on the ground so that
                // grounding is handled better while the character moves up and down slopes (or any variable terrain)
                rayDistance = rayOrigin.y - charPosition.y - col.radius + m_Settings.SnapToGroundDistance;
            }
            else
            {
                // The grounding SphereCastRay distance is shorter while character is in the air so that the character
                // snapping to the ground happens as the characters feet touch the ground
                rayDistance = rayOrigin.y - charPosition.y - col.radius;
            }

            m_GroundingSphereCastPos = rayOrigin + Vector3.down * rayDistance;

            var isTouchingGround = Physics.SphereCast(rayOrigin, m_GroundingSphereCastRad, Vector3.down,
                out var hit, rayDistance, layerMask);

            if (!isTouchingGround)
                return false;

            m_GroundHit = hit;
#if UNITY_EDITOR
            Debug.DrawLine(rayOrigin, hit.point);
#endif
            
            return true;
        }

        protected void PreventSinking()
        {
            var charTransform = m_Character.CachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider) m_Character.Col;

            var collCentre = charPosition + col.center;

            var rayDistance = collCentre.y - charPosition.y;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            // Check if sunken
            var isSunken = Physics.Raycast(collCentre, Vector3.down, out var hit, rayDistance, layerMask);

            if (isSunken)
            {
                // Set character's position to the ground (Unsink character)
                var characterPos = charTransform.position;
                characterPos.y = hit.point.y;
                charTransform.position = characterPos;
#if UNITY_EDITOR
                Debug.DrawLine(collCentre, hit.point, Color.green);
#endif
            }
        }

        public virtual void HandleGrounding()
        {
            if (CheckIfTouchingGround())
            {
                if (m_IsStartingJump)
                {
                    // Don't handle grounding if character just started a jump
                    // Prevents character being stuck to ground while trying to jump
                    return;
                }
                else // Set character position to ground point
                {
                    if (!IsGrounded)
                    {
                        m_Character.Input.InputReader.JumpInput = false;
                    }
                    
                    // Character is on the ground
                    m_IsGrounded = true;

                    // Turn off gravity for character while character is grounded to prevent sinking
                    m_Character.Rb.useGravity = false;

                    // Calculate position on ground character should be at
                    var characterTransform = m_Character.CachedTransform;
                    var targetStandingPos = characterTransform.position;

                    targetStandingPos.y = m_GroundHit.point.y;
                    // Set character position to position on ground
                    characterTransform.position = targetStandingPos;

                    //PreventSinking();
                }
            }
            else // Character in the air
            {
                // Character is not on the ground
                m_IsGrounded = false;

                // Activate gravity so character falls
                m_Character.Rb.useGravity = true;

                // If character falling
                if (m_Character.Rb.velocity.y < 0)
                {
                    if (m_Character.Rb.velocity.y < -m_Settings.MaxFallSpeed)
                    {
                        var newFallVelocity = m_Character.Rb.velocity;
                        newFallVelocity.y = -m_Settings.MaxFallSpeed;

                        m_Character.Rb.velocity = newFallVelocity;
                        return;
                    }

                    // Make character fall faster for better jump feel (Stops floaty looking falling)
                    m_Character.Rb.velocity +=
                        Vector3.up * (Physics.gravity.y * (m_Settings.FallMultiplier) * Time.deltaTime);
                }
            }
        }

        public virtual void HandleMovement(Vector3 moveDirection)
        {
            const float piInDeg = 180f;
            
            m_MoveDirection = moveDirection;

            float speed = 0;
            if (IsWalking) speed = m_Settings.WalkSpeed;
            else if (IsRunning) speed = m_Settings.RunSpeed;
            else if (!IsGrounded) speed = m_Settings.AirSpeed;
            else if (m_IsCrouchWalking) speed = m_Settings.CrouchWalkSpeed;

            var angleeBetweenMoveAndFacingDirections = Vector3.Angle(CachedTransform.forward, moveDirection);
            var anglePercent = angleeBetweenMoveAndFacingDirections / piInDeg;
            
            if (m_Character.isTargeting)
            {
                speed *= m_Settings.MoveAgainstFacingDirectionWhileTargetingCurve.Evaluate(1 - anglePercent);
            }
            else
            {
                speed *= m_Settings.MoveAgainstFacingDirectionCurve.Evaluate(1 - anglePercent);
            }
            
            var newVelocity = moveDirection * speed;

            // If in the air
            if (!m_IsGrounded)
            {
                // Keep vertical velocity from jump/fall
                newVelocity.y = m_Character.Rb.velocity.y;
            }

            // Apply adjusted velocity to character
            m_Character.Rb.velocity = newVelocity;

            HandleObstaclesAndSlopes();
        }

        protected virtual void HandleObstaclesAndSlopes()
        {
            const int layerMask = ~(1 << 8);
            
            // TODO: Replace this obstacle handling with something better

            var charTransform = m_Character.CachedTransform;
            var charPosition = charTransform.position;

            var rayOrigin = charPosition;
            rayOrigin.y += m_Settings.StepHeight;

            // Front
            var forward = m_MoveDirection;
            ObstacleRayCast(rayOrigin, forward, layerMask);

            // Right
            var right = Vector3.Cross(forward, Vector3.up);
            ObstacleRayCast(rayOrigin, right, layerMask);

            // Left
            var left = -right;
            ObstacleRayCast(rayOrigin, left, layerMask);
            
            // Front-Right
            var frontRight = (forward + right).normalized;
            ObstacleRayCast(rayOrigin, frontRight, layerMask);

            // Front-Left
            var frontLeft = (forward + left).normalized;
            ObstacleRayCast(rayOrigin, frontLeft, layerMask);
        }

        private void ObstacleRayCast(Vector3 rayOrigin, Vector3 direction, int layerMask)
        {
            if (!Physics.Raycast(rayOrigin, direction, out var frontLeftHit, m_Settings.ObstacleCheckSize, layerMask))
                return;

            if (m_HasAirVelocity)
            {
                m_CurrentAirVelocity = Vector3.zero;
                m_HasAirVelocity = false;
            }
            
#if UNITY_EDITOR
            Debug.DrawLine(rayOrigin, frontLeftHit.point, Color.red);
#endif
            if (!CheckIfSlopeIsTraversable(frontLeftHit.normal))
            {
                m_Character.Rb.velocity -= Vector3.Project(m_Character.Rb.velocity, frontLeftHit.point - rayOrigin);
            }
        }

        protected bool CheckIfSlopeIsTraversable(Vector3 slopeNormal)
        {
            var slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);
            return slopeAngle < m_Settings.MaxSlopeAngle;
        }

        public virtual void HandleRotation(float targetAngle)
        {
            if (m_Character.isTargeting)
                return;
            
            var currentRotateVelocity = 0f;
            var smoothTime = 0f;

            if (IsRunning)
                smoothTime = m_Settings.TurnSmoothTimeWhileRunning * Time.deltaTime;
            else if (!IsGrounded)
                smoothTime = m_Settings.TurnSmoothTimeWhileInAir * Time.deltaTime;
            else
            {
                smoothTime = m_Settings.TurnSmoothTime * Time.deltaTime;
            }

            var smoothedRotationAngle = Mathf.SmoothDampAngle(
                m_Character.CachedTransform.eulerAngles.y, targetAngle,
                ref currentRotateVelocity, smoothTime);

            m_Character.CachedTransform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
        }

        public virtual void Jump()
        {
            // Don't do jump if character is in the air
            if (!m_IsGrounded)
                return;

            // Update movement states
            m_IsStartingJump = true;
            m_IsJumping = true;
            m_IsGrounded = false;

            // Reset starting jump timer to stop player from sticking to ground
            m_TimeSinceJumpStarted = 0f;

            // Activate gravity so character falls
            m_Character.Rb.useGravity = true;

            // Calculate jump force vector
            var jumpVector = new Vector3(0, m_Settings.VerticalJumpPower, 0);
            
            // Add horizontal force (air velocity)
            if (MoveDirection == Vector3.zero)
            {
                m_CurrentAirVelocity = Vector3.zero;
            }
            else
            {
                SetAirVelocity(m_MoveDirection * m_Settings.HorizontalJumpPower);
            }

            // Remove any downward velocity on player before adding jump force to make jump work
            var currVelocity = m_Character.Rb.velocity;
            currVelocity.y = 0;
            m_Character.Rb.velocity = currVelocity;

            // Apply jump force to character
            m_Character.Rb.AddForce(jumpVector);
        }

        public virtual void HandleJumping()
        {
            if (!m_IsJumping)
                return;

            // Handle started jump timer to prevent character sticking to ground
            if (m_IsStartingJump)
            {
                m_TimeSinceJumpStarted += Time.deltaTime;

                if (m_TimeSinceJumpStarted > m_TimeAfterJumpToNotGroundCheck)
                {
                    m_IsStartingJump = false;
                }
            }

            // If character is rising upwards AND player isn't holding jump button
            if (m_Character.Rb.velocity.y > 0 && !((PlayerInputMB) m_Character.Input).jumpInput)
            {
                // Make character not rise as much
                m_Character.Rb.velocity +=
                    Vector3.up * (Physics.gravity.y * m_Settings.LowJumpMultiplier * Time.deltaTime);
            }

            // IF character is falling
            if (m_Character.Rb.velocity.y < 0)
            {
                // Character is no longer jumping
                m_IsStartingJump = false;
                m_IsJumping = false;
                m_IsFalling = true;
            }

            // If character landed on 'ground' while jumping upwards (before falling back down)
            if (m_IsGrounded)
            {
                // Character is no longer jumping
                m_IsStartingJump = false;
                m_IsJumping = false;
            }
        }

        protected virtual void SetAirVelocity(Vector3 velocity)
        {
            m_CurrentAirVelocity = velocity;
            
            m_HasAirVelocity = true;
        }
        
        public virtual void HandleAirVelocity()
        {
            if (!m_HasAirVelocity)
                return;
            
            m_Character.Rb.velocity += m_CurrentAirVelocity;
            
            m_CurrentAirVelocity =
                Vector3.MoveTowards(m_CurrentAirVelocity, Vector3.zero, m_Settings.SpeedAirVelocityIsReducedAt);

            if (Vector3.Distance(Vector3.zero, m_CurrentAirVelocity) < 0.01)
            {
                m_CurrentAirVelocity = Vector3.zero;

                m_HasAirVelocity = false;
            }
        }
    }
}
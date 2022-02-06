namespace Main.Characters
{
    using UnityEngine;
    using System;
    using System.Runtime.CompilerServices;
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
        protected bool m_IsStartingJump;

        [SerializeField]
        protected bool m_IsJumping;

        [SerializeField]
        protected bool m_IsFalling;

        public bool isGrounded => m_IsGrounded;

        public bool isWalking
        {
            get => m_IsWalking;
            set => m_IsWalking = value;
        }

        public bool isRunning
        {
            get => m_IsRunning;
            set => m_IsRunning = value;
        }

        public bool isStartingJump => m_IsStartingJump;
        public bool isJumping => m_IsJumping;
        public bool isFalling => m_IsFalling;

        // GROUNDING
        protected RaycastHit m_GroundHit;
        protected RaycastHit m_PrevGroundHit;
        protected Vector3 m_GroundingSphereCastPos;
        protected float m_GroundingSphereCastRad;

        // MOVEMENT
        protected Vector3 m_MoveDirection;

        // OBSTACLES


        // JUMPING
        protected float m_TimeSinceJumpStarted;
        protected float m_TimeAfterJumpToNotGroundCheck = 0.05f;

        // GUI
        protected Color m_GroundSphereColour = new Color(1f, 0.92f, 0.016f, 0.5f);

        protected virtual void Awake()
        {
            Character = GetComponent<CharacterMB>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = m_GroundSphereColour;
            Gizmos.DrawSphere(m_GroundingSphereCastPos, m_GroundingSphereCastRad);
        }

        protected virtual bool CheckIfTouchingGround()
        {
            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider) Character.col;

            var rayOrigin = charPosition + col.center;
            rayOrigin.y -= col.radius;
            m_GroundingSphereCastRad = m_Settings.snapToGroundRadius;

            var rayDistance = 0f;

            if (m_IsGrounded)
            {
                // The grounding SphereCastRay distance will be longer while the character is on the ground so that
                // grounding is handled better while the character moves up and down slopes (or any variable terrain)
                rayDistance = rayOrigin.y - charPosition.y - col.radius + m_Settings.snapToGroundDistance;
            }
            else
            {
                // The grounding SphereCastRay distance is shorter while character is in the air so that the character
                // snapping to the ground happens as the characters feet touch the ground
                rayDistance = rayOrigin.y - charPosition.y - col.radius;
            }

            m_GroundingSphereCastPos = rayOrigin + Vector3.down * rayDistance;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            bool isTouchingGround = Physics.SphereCast(
                rayOrigin,
                m_GroundingSphereCastRad,
                Vector3.down,
                out var hit,
                rayDistance, layerMask
            );

            if (!isTouchingGround)
                return false;

            m_GroundHit = hit;
            Debug.DrawLine(rayOrigin, hit.point);

            return true;
        }

        protected void PreventSinking()
        {
            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider) Character.col;

            var collCentre = charPosition + col.center;

            var rayDistance = collCentre.y - charPosition.y;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            // Check if sunken
            bool isSunken = Physics.Raycast(collCentre, Vector3.down, out var hit, rayDistance, layerMask);

            if (isSunken)
            {
                // Set character's position to the ground (Unsink character)
                var characterPos = charTransform.position;
                characterPos.y = hit.point.y;
                charTransform.position = characterPos;

                Debug.DrawLine(collCentre, hit.point, Color.green);
            }
            else
                return;
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
                    // Character is on the ground
                    m_IsGrounded = true;

                    // Turn off gravity for character while character is grounded to prevent sinking
                    Character.rb.useGravity = false;

                    // Calculate position on ground character should be at
                    var characterTransform = Character.cachedTransform;
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
                Character.rb.useGravity = true;

                // If character falling
                if (Character.rb.velocity.y < 0)
                {
                    if (Character.rb.velocity.y < -m_Settings.MaxFallSpeed)
                    {
                        Vector3 newFallVelocity = Character.rb.velocity;
                        newFallVelocity.y = -m_Settings.MaxFallSpeed;

                        Character.rb.velocity = newFallVelocity;
                        return;
                    }

                    // Make character fall faster for better jump feel (Stops floaty looking falling)
                    Character.rb.velocity +=
                        Vector3.up * (Physics.gravity.y * (m_Settings.FallMultiplier) * Time.deltaTime);
                }
            }
        }

        public virtual void HandleMovement(Vector3 moveDirection)
        {
            m_MoveDirection = moveDirection;

            float speed = 0;

            if (isWalking) speed = m_Settings.WalkSpeed;
            else if (isRunning) speed = m_Settings.RunSpeed;
            else if (!isGrounded) speed = m_Settings.AirSpeed;

            // Adjust speed based on direction character is facing
            float angleDifferenceBetweenMoveAndFacingDirections = Vector3.Angle(cachedTransform.forward, moveDirection);
            float anglePercent = angleDifferenceBetweenMoveAndFacingDirections / 180f;
            speed *= m_Settings.moveAgainstFacingDirectionCurve.Evaluate(1 - anglePercent);

            var newVelocity = moveDirection * speed;

            // If in the air
            if (!m_IsGrounded)
            {
                // Keep vertical velocity from jump/fall
                newVelocity.y = Character.rb.velocity.y;
            }

            // Apply adjusted velocity to character
            Character.rb.velocity = newVelocity;

            HandleObstaclesAndSlopes();
        }

        protected virtual void HandleObstaclesAndSlopes()
        {
            // TODO: Replace this obstacle handling with something better

            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider) Character.col;

            var collCentre = charPosition + col.center;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            var rayOrigin = charPosition;
            rayOrigin.y += m_Settings.StepHeight;

            // Front
            var forward = m_MoveDirection;
            if (Physics.Raycast(rayOrigin, forward, out RaycastHit frontHit, m_Settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, frontHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(frontHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, m_MoveDirection);
                }
            }

            // Right
            var right = Vector3.Cross(forward, Vector3.up);
            if (Physics.Raycast(rayOrigin, right, out RaycastHit rightHit, m_Settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, rightHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(rightHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, rightHit.point - rayOrigin);
                }
            }

            // Left
            var left = -right;
            if (Physics.Raycast(rayOrigin, left, out RaycastHit leftHit, m_Settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, leftHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(leftHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, leftHit.point - rayOrigin);
                }
            }

            // Front-Right
            var frontRight = (forward + right).normalized;
            if (Physics.Raycast(rayOrigin, frontRight, out RaycastHit frontRightHit, m_Settings.ObstacleCheckSize,
                    layerMask))
            {
                Debug.DrawLine(rayOrigin, frontRightHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(frontRightHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, frontRightHit.point - rayOrigin);
                }
            }

            // Front-Left
            var frontLeft = (forward + left).normalized;
            if (Physics.Raycast(rayOrigin, frontLeft, out RaycastHit frontLeftHit, m_Settings.ObstacleCheckSize,
                    layerMask))
            {
                Debug.DrawLine(rayOrigin, frontLeftHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(frontLeftHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, frontLeftHit.point - rayOrigin);
                }
            }
        }

        protected bool CheckIfSlopeIsTraversable(Vector3 slopeNormal)
        {
            float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);

            if (slopeAngle < m_Settings.maxSlopeAngle)
                return true;

            return false;
        }

        public virtual void HandleRotation(float targetAngle)
        {
            var currentRotateVelocity = 0f;
            var smoothTime = m_Settings.TurnSmoothTime * Time.deltaTime;

            if (isRunning) smoothTime *= m_Settings.turnSmoothTimeRunningMultiplier;

            var smoothedRotationAngle = Mathf.SmoothDampAngle(
                Character.cachedTransform.eulerAngles.y, targetAngle,
                ref currentRotateVelocity, smoothTime);

            Character.cachedTransform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
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
            Character.rb.useGravity = true;

            // Calculate jump force vector
            var jumpVector = new Vector3(0, m_Settings.JumpPower, 0);

            // Remove any downward velocity on player before adding jump force to make jump work
            Vector3 currVelocity = Character.rb.velocity;
            currVelocity.y = 0;
            Character.rb.velocity = currVelocity;

            // Apply jump force to character
            Character.rb.AddForce(jumpVector);
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
            if (Character.rb.velocity.y > 0 && !((PlayerInputMB) Character.input).jumpInput)
            {
                // Make character not rise as much
                Character.rb.velocity +=
                    Vector3.up * (Physics.gravity.y * m_Settings.LowJumpMultiplier * Time.deltaTime);
            }

            // IF character is falling
            if (Character.rb.velocity.y < 0)
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
    }
}

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
        protected CharacterMovementSettingsSO settings;
        
        // MOVEMENT STATES
        [SerializeField]
        protected bool IsGrounded;
        [SerializeField]
        protected bool IsWalking;
        [SerializeField]
        protected bool IsRunning;
        [SerializeField]
        protected bool IsStartingJump;
        [SerializeField]
        protected bool IsJumping;
        [SerializeField]
        protected bool IsFalling;

        public bool isGrounded => IsGrounded;
        public bool isWalking
        {
            get => IsWalking;
            set => IsWalking = value;
        }
        public bool isRunning
        {
            get => IsRunning;
            set => IsRunning = value;
        }
        public bool isStartingJump => IsStartingJump;
        public bool isJumping => IsJumping;
        public bool isFalling => IsFalling;

        // GROUNDING
        protected RaycastHit GroundHit;
        private Vector3 spherePos;
        private float sphereRad;
        
        // MOVEMENT
        protected Vector3 MoveDirection;
        
        // OBSTACLES
        
        
        // JUMPING
        protected float TimeSinceJumpStarted;
        protected float TimeAfterJumpToNotGroundCheck = 0.05f;
        
        // GUI
        private Color groundSphereColour = new Color(1f, 0.92f, 0.016f, 0.5f);
        
        protected virtual void Awake()
        {
            Character = GetComponent<CharacterMB>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = groundSphereColour;
            Gizmos.DrawSphere(spherePos, sphereRad);
        }

        protected virtual bool CheckIfTouchingGround()
        {
            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider)Character.col;

            var rayOrigin = charPosition + col.center;
            rayOrigin.y -= col.radius;
            sphereRad = settings.snapToGroundRadius;
            
            var rayDistance = 0f;
            
            if (IsGrounded)
            {
                // The grounding SphereCastRay distance will be longer while the character is on the ground so that
                // grounding is handled better while the character moves up and down slopes (or any variable terrain)
                rayDistance = rayOrigin.y - charPosition.y - col.radius + settings.snapToGroundDistance;
            }
            else
            {
                // The grounding SphereCastRay distance is shorter while character is in the air so that the character
                // snapping to the ground happens as the characters feet touch the ground
                rayDistance = rayOrigin.y - charPosition.y - col.radius;
            }

            spherePos = rayOrigin + Vector3.down * rayDistance;
            
            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            bool isTouchingGround = Physics.SphereCast(
                rayOrigin,
                sphereRad,
                Vector3.down,
                out var hit,
                rayDistance, layerMask
                );

            if (!isTouchingGround)
                return false;
            
            GroundHit = hit;
            Debug.DrawLine(rayOrigin, hit.point);
            
            return true;
        }

        protected void PreventSinking()
        {
            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider)Character.col;
            
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
                if (IsStartingJump)
                {
                    // Don't handle grounding if character just started a jump
                    // Prevents character being stuck to ground while trying to jump
                    return;
                }
                else // Set character position to ground point
                {
                    // Character is on the ground
                    IsGrounded = true;
                    
                    // Turn off gravity for character while character is grounded to prevent sinking
                    Character.rb.useGravity = false;

                    // Calculate position on ground character should be at
                    var characterTransform = Character.cachedTransform;
                    var targetStandingPos = characterTransform.position;
                    targetStandingPos.y = GroundHit.point.y;
                    
                    // Set character position to position on ground
                    characterTransform.position = targetStandingPos;
                    
                    PreventSinking();
                }
            }
            else // Character in the air
            {
                // Character is not on the ground
                IsGrounded = false;
                
                // Activate gravity so character falls
                Character.rb.useGravity = true;
                
                // If character falling
                if (Character.rb.velocity.y < 0)
                {
                    if (Character.rb.velocity.y < -settings.MaxFallSpeed)
                    {
                        Vector3 newFallVelocity = Character.rb.velocity;
                        newFallVelocity.y = -settings.MaxFallSpeed;

                        Character.rb.velocity = newFallVelocity;
                        return;
                    }
                    
                    // Make character fall faster for better jump feel (Stops floaty looking falling)
                    Character.rb.velocity +=
                        Vector3.up * (Physics.gravity.y * (settings.FallMultiplier) * Time.deltaTime);
                }
            }
        }

        public virtual void HandleMovement(Vector3 moveDirection)
        {
            MoveDirection = moveDirection;

            float speed = 0;

            if (isWalking) speed = settings.WalkSpeed;
            else if (isRunning) speed = settings.RunSpeed;
            else if (!isGrounded) speed = settings.AirSpeed;
            
            var newVelocity = moveDirection * speed;

            // If in the air
            if (!IsGrounded)
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
            var col = (CapsuleCollider)Character.col;
            
            var collCentre = charPosition + col.center;
            
            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            var rayOrigin = charPosition;
            rayOrigin.y += settings.StepHeight;
            
            // Front
            var forward = MoveDirection;
            if (Physics.Raycast(rayOrigin, forward, out RaycastHit frontHit, settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, frontHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(frontHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, MoveDirection);
                }
            }
            
            // Right
            var right = Vector3.Cross(forward, Vector3.up);
            if (Physics.Raycast(rayOrigin, right, out RaycastHit rightHit, settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, rightHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(rightHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, rightHit.point - rayOrigin);
                }
            }
            
            // Left
            var left = -right;
            if (Physics.Raycast(rayOrigin, left, out RaycastHit leftHit, settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, leftHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(leftHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, leftHit.point - rayOrigin);
                }
            }
            
            // Front-Right
            var frontRight = (forward + right).normalized;
            if (Physics.Raycast(rayOrigin, frontRight, out RaycastHit frontRightHit, settings.ObstacleCheckSize, layerMask))
            {
                Debug.DrawLine(rayOrigin, frontRightHit.point, Color.red);

                if (!CheckIfSlopeIsTraversable(frontRightHit.normal))
                {
                    Character.rb.velocity -= Vector3.Project(Character.rb.velocity, frontRightHit.point - rayOrigin);
                }
            }
            
            // Front-Left
            var frontLeft = (forward + left).normalized;
            if (Physics.Raycast(rayOrigin, frontLeft, out RaycastHit frontLeftHit, settings.ObstacleCheckSize, layerMask))
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

            if (slopeAngle < settings.maxSlopeAngle)
                return true;

            return false;
        }

        public virtual void HandleRotation(float targetAngle)
        {
            var currentRotateVelocity = 0f;
            
            var smoothedRotationAngle = Mathf.SmoothDampAngle(
                Character.cachedTransform.eulerAngles.y, targetAngle,
                ref currentRotateVelocity, settings.TurnSmoothTime * Time.deltaTime);

            Character.cachedTransform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
        }

        public virtual void Jump()
        { 
            // Don't do jump if character is in the air
            if (!IsGrounded)
                return;

            // Update movement states
            IsStartingJump = true;
            IsJumping = true;
            IsGrounded = false;
            
            // Reset starting jump timer to stop player from sticking to ground
            TimeSinceJumpStarted = 0f;

            // Activate gravity so character falls
            Character.rb.useGravity = true;

            // Calculate jump force vector
            var jumpVector = new Vector3(0, settings.JumpPower, 0);
            
            // Remove any downward velocity on player before adding jump force to make jump work
            Vector3 currVelocity = Character.rb.velocity;
            currVelocity.y = 0;
            Character.rb.velocity = currVelocity;
            
            // Apply jump force to character
            Character.rb.AddForce(jumpVector);
        }

        public virtual void HandleJumping()
        {
            if (!IsJumping)
                return;
            
            // Handle started jump timer to prevent character sticking to ground
            if (IsStartingJump)
            {
                TimeSinceJumpStarted += Time.deltaTime;

                if (TimeSinceJumpStarted > TimeAfterJumpToNotGroundCheck)
                {
                    IsStartingJump = false;
                }
            }
                
            // If character is rising upwards AND player isn't holding jump button
            if (Character.rb.velocity.y > 0 && !((PlayerInputMB) Character.input).jumpInput)
            {
                // Make character not rise as much
                Character.rb.velocity += Vector3.up * (Physics.gravity.y * settings.LowJumpMultiplier * Time.deltaTime);
            }

            // IF character is falling
            if (Character.rb.velocity.y < 0)
            {
                // Character is no longer jumping
                IsStartingJump = false;
                IsJumping = false;
                IsFalling = true;
            }
            
            // If character landed on 'ground' while jumping upwards (before falling back down)
            if (IsGrounded)
            {
                // Character is no longer jumping
                IsStartingJump = false;
                IsJumping = false;
            }
        }
    }
}

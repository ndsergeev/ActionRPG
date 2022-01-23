
namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.Input;
    
    public class CharacterMovementMB : MovementMB
    {
        [SerializeField]
        protected CharacterMovementSettingsSO settings;
        
        // MOVEMENT STATES
        [SerializeField]
        protected bool isGrounded;
        [SerializeField]
        protected bool isStartingJump;
        [SerializeField]
        protected bool isJumping;
        [SerializeField]
        protected bool isFalling;

        // GROUNDING
        protected RaycastHit GroundHit;
        
        // JUMPING
        protected float TimeSinceJumpStarted;
        protected float TimeAfterJumpToNotGroundCheck = 0.2f;
        
        protected virtual void Awake()
        {
            Character = GetComponent<CharacterMB>();
        }

        protected virtual bool CheckIfTouchingGround()
        {
            var charTransform = Character.cachedTransform;
            var charPosition = charTransform.position;
            var col = (CapsuleCollider)Character.col;
            
            var collCentre = charPosition + col.center;

            var rayDistance = 0f;
            
            if (isGrounded)
            {
                rayDistance = collCentre.y - charPosition.y + settings.SnapToGroundDistance - col.radius;
            }
            else
            {
                rayDistance = collCentre.y - charPosition.y - col.radius;
            }
            
            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            var layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            if (!Physics.SphereCast(collCentre, col.radius, Vector3.down, out var hit, rayDistance, layerMask))
                return false;
            
            GroundHit = hit;
            Debug.DrawLine(collCentre, hit.point);
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

            RaycastHit sinkRayHit;
            if (Physics.Raycast(collCentre, Vector3.down, out var hit, rayDistance, layerMask))
            {
                sinkRayHit = hit;

                var characterPos = charTransform.position;
                characterPos.y = hit.point.y;
                charTransform.position = characterPos;
            }
        }
        
        public virtual void HandleGrounding()
        {
            if (CheckIfTouchingGround())
            {
                if (isStartingJump)
                {
                    // Don't handle grounding if character just started a jump
                    // Prevents character being stuck to ground while trying to jump
                    return;
                }
                else // Set character position to ground point
                {
                    // Character is on the ground
                    isGrounded = true;
                    
                    // Turn off gravity for character while character is grounded to prevent sinking
                    Character.rb.useGravity = false;

                    // Calculate position on ground character should be at
                    var characterTransform = Character.cachedTransform;
                    var targetStandingPos = characterTransform.position;
                    targetStandingPos.y = GroundHit.point.y - settings.SnapToGroundDistance;

                    // Set character position to position on ground
                    characterTransform.position = targetStandingPos;
                    
                    PreventSinking();
                }
            }
            else // Character in the air
            {
                // Character is not on the ground
                isGrounded = false;
                
                // Activate gravity so character falls
                Character.rb.useGravity = true;

                // If character falling
                if (Character.rb.velocity.y < 0)
                {
                    // Make character fall faster for better jump feel (Stops floaty looking falling)
                    Character.rb.velocity +=
                        Vector3.up * (Physics.gravity.y * (settings.FallMultiplier) * Time.deltaTime);
                }
            }
        }
        
        public virtual void HandleMovement(Vector3 moveDirection)
        {
            var newVelocity = moveDirection * settings.Speed;

            // If in the air
            if (!isGrounded)
            {
                // Keep vertical velocity from jump/fall
                newVelocity.y = Character.rb.velocity.y;
            }
            
            // Apply adjusted velocity to character
            Character.rb.velocity = newVelocity;
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
            if (!isGrounded)
                return;

            // Update movement states
            isStartingJump = true;
            isJumping = true;
            isGrounded = false;
            
            // Reset starting jump timer to stop player from sticking to ground
            TimeSinceJumpStarted = 0f;

            // Activate gravity so character falls
            Character.rb.useGravity = true;

            // Calculate jump force vector
            var jumpVector = new Vector3(0, settings.JumpPower, 0);
            
            // Apply jump force to character
            Character.rb.AddForce(jumpVector);
        }

        public virtual void HandleJumping()
        {
            if (!isJumping)
                return;
            
            // Handle started jump timer to prevent character sticking to ground
            if (isStartingJump)
            {
                TimeSinceJumpStarted += Time.deltaTime;

                if (TimeSinceJumpStarted > TimeAfterJumpToNotGroundCheck)
                {
                    isStartingJump = false;
                }
            }
                
            // If character is rising upwards AND player isn't holding jump button
            if (Character.rb.velocity.y > 0 && !((PlayerInputMB) Character.Input).jumpInput)
            {
                // Make character not rise as much
                Character.rb.velocity += Vector3.up * (Physics.gravity.y * settings.LowJumpMultiplier * Time.deltaTime);
            }

            // IF character is falling
            if (Character.rb.velocity.y < 0)
            {
                // Character is no longer jumping
                isStartingJump = false;
                isJumping = false;
            }
            
            // If character landed on 'ground' while jumping upwards (before falling back down)
            if (isGrounded)
            {
                // Character is no longer jumping
                isStartingJump = false;
                isJumping = false;
            }
        }
    }
}


namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Inputs;
    
    public class CharacterMovementMB : MovementMB
    {
        [SerializeField]
        protected CharacterMovementSettingsSO settings;
        
        // Movement states
        [SerializeField]
        protected bool isGrounded;
        [SerializeField]
        protected bool isStartingJump;
        [SerializeField]
        protected bool isJumping;
        [SerializeField]
        protected bool isFalling;

        // Grounding
        protected RaycastHit GroundHit;
        
        // Jumping
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

            var rayDistance = collCentre.y - charPosition.y + settings.SnapToGroundDistance - col.radius;

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
        { }
        
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

            if (!isGrounded)
            {
                newVelocity.y = Character.rb.velocity.y;
            }
            
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
            
            // Handle started jump timer for preventing character sticking to ground
            if (isStartingJump)
            {
                TimeSinceJumpStarted += Time.deltaTime;

                if (TimeSinceJumpStarted > TimeAfterJumpToNotGroundCheck)
                {
                    isStartingJump = false;
                }
            }
                
            // If player is rising upwards AND player isn't holding jump button
            if (Character.rb.velocity.y > 0 && !((PlayerInputMB) Character.Input).jumpInput)
            {
                // Make player not rise as much / fall faster
                Character.rb.velocity += Vector3.up * (Physics.gravity.y * settings.LowJumpMultiplier * Time.deltaTime);
            }

            // Check if character is falling
            if (Character.rb.velocity.y < 0)
            {
                // Character is falling instead of jumping
                isStartingJump = false;
                isJumping = false;
            }

            if (isGrounded)
            {
                isStartingJump = false;
                isJumping = false;
            }
        }
    }
}

using System.IO.IsolatedStorage;
using Main.Input;
using UnityEngine.InputSystem;

namespace Main.Characters
{
    using UnityEngine;
    
    public class CharacterMovementMB : MonoBehaviour
    {
        protected CharacterMB character;
        
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
        protected RaycastHit groundHit;
        
        // Jumping
        protected float timeSinceJumpStarted;
        protected float timeAfterJumpToNotGroundCheck = 0.2f;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterMB>();
        }

        protected virtual bool CheckIfTouchingGround()
        {
            Transform charTransform = character.CachedTransform;
            Vector3 charPosition = charTransform.position;
            CapsuleCollider col = character.Collider;
            
            Vector3 collCentre = charPosition + col.center;

            float rayDistance = collCentre.y - charPosition.y + settings.SnapToGroundDistance - col.radius;

            // Bit shift the index of layer 8 to get a bit mask
            // Layer 8 has been set to the 'Characters' layer
            int layerMask = 1 << 8;
            // Invert bitmask to every layer other than 'Characters' layer
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.SphereCast(collCentre, col.radius, Vector3.down, out hit, rayDistance, layerMask))
            {
                groundHit = hit;
                Debug.DrawLine(collCentre, hit.point);
                return true;
            }

            return false;
        }

        protected void PreventSinking()
        {
            
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
                    character.RB.useGravity = false;

                    // Calculate position on ground character should be at
                    Transform characterTransform = character.CachedTransform;
                    Vector3 targetStandingPos = characterTransform.position;
                    targetStandingPos.y = groundHit.point.y - settings.SnapToGroundDistance;

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
                character.RB.useGravity = true;

                // If character falling
                if (character.RB.velocity.y < 0)
                {
                    // Make character fall faster for better jump feel (Stops floaty looking falling)
                    character.RB.velocity +=
                        Vector3.up * (Physics.gravity.y * (settings.FallMultiplier) * Time.deltaTime);
                }
            }
        }
        
        public virtual void HandleMovement(Vector3 moveDirection)
        {
            Vector3 newVelocity = moveDirection * settings.Speed;

            if (!isGrounded)
            {
                newVelocity.y = character.RB.velocity.y;
            }
            
            character.RB.velocity = newVelocity;
        }

        public void HandleRotation(float targetAngle)
        {
            float currentRotateVelocity = 0;
            
            float smoothedRotationAngle = Mathf.SmoothDampAngle(
                character.CachedTransform.eulerAngles.y, targetAngle,
                ref currentRotateVelocity, settings.TurnSmoothTime * Time.deltaTime);

            character.CachedTransform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
        }

        public virtual void Jump()
        { 
            // Don't do jump if character is in the air
            if (!isGrounded) return;

            // Update movement states
            isStartingJump = true;
            isJumping = true;
            isGrounded = false;
            
            // Reset starting jump timer to stop player from sticking to ground
            timeSinceJumpStarted = 0f;

            // Activate gravity so character falls
            character.RB.useGravity = true;

            // Calculate jump force vector
            Vector3 jumpVector = new Vector3(0, settings.JumpPower, 0);
            
            // Apply jump force to character
            character.RB.AddForce(jumpVector);
        }

        public virtual void HandleJumping()
        {
            if (isJumping)
            {
                // Handle started jump timer for preventing character sticking to ground
                if (isStartingJump)
                {
                    timeSinceJumpStarted += Time.deltaTime;

                    if (timeSinceJumpStarted > timeAfterJumpToNotGroundCheck)
                    {
                        isStartingJump = false;
                    }
                }
                
                // If player is rising upwards AND player isn't holding jump button
                if (character.RB.velocity.y > 0 && !(character.Input as PlayerInputMB).JumpInput)
                {
                    // Make player not rise as much / fall faster
                    character.RB.velocity += Vector3.up * Physics.gravity.y * (settings.LowJumpMultiplier) * Time.deltaTime;
                }

                // Check if character is falling
                if (character.RB.velocity.y < 0)
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
}

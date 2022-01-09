
namespace Main.Characters
{
    using UnityEngine;
    
    public class PlayerMovementMB : CharacterMovementMB
    {
        
        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void HandleGrounding()
        {
            base.HandleGrounding();
        }
        
        public virtual void HandleMovement(Vector3 moveDirection)
        {
            base.HandleMovement(moveDirection);
        }

        public void HandleRotation(float targetAngle)
        {
            base.HandleRotation(targetAngle);
        }

        public virtual void Jump()
        {
            base.Jump();
        }

        public virtual void HandleJumping()
        {
            base.HandleJumping();
        }
    }
}

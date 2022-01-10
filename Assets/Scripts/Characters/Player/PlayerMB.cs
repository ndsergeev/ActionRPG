
namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Cameras;
    using Main.Core;
    using Main.Inputs;
    
    public class PlayerMB : CharacterMB
    {
        protected PlayerInputMB PlayerInput => input as PlayerInputMB;
        protected PlayerMovementMB PlayerMovement => movement as PlayerMovementMB;
        
        protected void HandleMovementInput()
        {
            var moveDirection = Vector3.zero;

            float targetAngle = 0;
            
            // Only calculate move direction if there is movement input
            if (PlayerInput.moveInput != Vector2.zero)
            {
                // Set move direction based on move input
                moveDirection = new Vector3(PlayerInput.moveInput.x, 0, PlayerInput.moveInput.y);
                
                // Calculate rotation needed for move direction to be based on camera
                var cameraYEuler = CameraManagerMB.instance.MainCameraTransform.eulerAngles.y;
                targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraYEuler;
                
                // Set move direction based on camera
                moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
            
            PlayerMovement.HandleMovement(moveDirection);

            if (targetAngle != 0)
            {
                PlayerMovement.HandleRotation(targetAngle);
            }
        }
        
        public void JumpStart()
            => PlayerMovement.Jump();

        public override void Run()
        {
            base.Run();
            
            PlayerMovement.HandleGrounding();
            HandleMovementInput();
            PlayerMovement.HandleJumping();
        }
    }
}

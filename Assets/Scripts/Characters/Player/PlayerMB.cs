using System;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Core.Console;
    using Main.Input;
    using Main.Cameras;
    
    public class PlayerMB : CharacterMB
    {
        protected PlayerInputMB PlayerInput => Input as PlayerInputMB;
        
        protected void Update()
        {
            movement.HandleGrounding();
            HandleMovementInput();
            movement.HandleJumping();
        }
        
        protected void HandleMovementInput()
        {
            Vector3 moveDirection = Vector3.zero;

            float targetAngle = 0;
            
            // Only calculate move direction if there is movement input
            if (PlayerInput.MoveInput != Vector2.zero)
            {
                // Set move direction based on move input
                moveDirection = new Vector3(PlayerInput.MoveInput.x, 0, PlayerInput.MoveInput.y);
                
                // Calculate rotation needed for move direction to be based on camera
                float cameraYEuler = CameraManagerMB.instance.MainCameraTransform.eulerAngles.y;
                targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraYEuler;
                
                // Set move direction based on camera
                moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
            
            movement.HandleMovement(moveDirection);

            if (targetAngle != 0)
            {
                movement.HandleRotation(targetAngle);
            }
        }
        
        public void JumpStart()
            => movement.Jump();
    }
}

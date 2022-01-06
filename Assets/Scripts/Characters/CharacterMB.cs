
using Cinemachine;
using Main.Cameras;
using UnityEngine.Serialization;

namespace Main.Characters
{
    using System;
    using UnityEngine;
    using UnityEditor;
    
    using Main.Core.Console;
    using Main.Input;
    
    public class CharacterMB : MonoBehaviour
    {
        // TRANSFORM
        protected Transform cachedTransform;
        public Transform CachedTransform => cachedTransform;
        
        // MOVEMENT
        protected CharacterMovementMB movement;
        public CharacterMovementMB MovementMB => m_movement;
        
        // INPUT
        protected InputMB input;
        
        // PHYSICS
        protected Rigidbody rigidBody;
        public Rigidbody RigidBody => rigidBody;
        protected CapsuleCollider collider;
        public CapsuleCollider Collider => collider;
            

        private void Awake()
        {
            m_cachedTransform = transform;
            m_movement = GetComponent<CharacterMovementMB>();
            m_rigidBody = GetComponent<Rigidbody>();
            m_collider = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            m_movement.HandleGrounding();
            HandleMovementInput();
            m_movement.HandleJumping();
        }

        public void JumpStart()
            => m_movement.Jump();

        private void HandleMovementInput()
        {
            Vector3 moveDirection = Vector3.zero;

            float targetAngle = 0;
            
            // Only calculate move direction if there is movement input
            if (m_moveInput != Vector2.zero)
            {
                // Set move direction based on move input
                moveDirection = new Vector3(m_moveInput.x, 0, m_moveInput.y);
                
                // Calculate rotation needed for move direction to be based on camera
                float cameraYEuler = CameraManagerMB.instance.MainCameraTransform.eulerAngles.y;
                targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraYEuler;
                
                // Set move direction based on camera
                moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
            
            m_movement.HandleMovement(moveDirection);

            if (targetAngle != 0)
            {
                m_movement.HandleRotation(targetAngle);
            }
        }
    }
}
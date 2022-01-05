
using Cinemachine;
using Main.Cameras;
using UnityEngine.Serialization;

namespace Main.Characters
{
    using System;
    using UnityEngine;
    using UnityEditor;
    
    using Main.Core.Console;
    
    public class CharacterMB : MonoBehaviour
    {
        // TODO: e.g. speed
        // [SerializeField]
        // protected ScriptableObject someSharedSettings;
        
        protected Transform m_cachedTransform;
        public Transform CachedTransform => m_cachedTransform;
        
        protected CharacterMovementMB m_movement;
        public CharacterMovementMB MovementMB => m_movement;
        
        // Inputs
        private Vector2 m_moveInput;
        private bool m_jumpInput;
        
        // Physics
        protected Rigidbody m_rigidBody;
        public Rigidbody RigidBody => m_rigidBody;
        protected CapsuleCollider m_collider;
        public CapsuleCollider Collider => m_collider;
            

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
        }

        public void Move(Vector2 moveInput)
        {
            m_moveInput = moveInput;
        }

        public void Jump()
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
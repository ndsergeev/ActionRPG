namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Core.Console;
    using Main.Input;
    using Main.Cameras;
    
    public class CharacterMB : MonoBehaviour
    {
        // TRANSFORM
        protected Transform cachedTransform;
        public Transform CachedTransform => cachedTransform;
        
        // INPUT
        protected InputMB Input;
        
        // MOVEMENT
        protected CharacterMovementMB movement;
        public CharacterMovementMB Movement => movement;
        
        // PHYSICS
        protected Rigidbody rigidBody;
        protected CapsuleCollider collider;
        
        public Rigidbody RigidBody => rigidBody;
        public CapsuleCollider Collider => collider;
        
        protected virtual void Awake()
        {
            cachedTransform = transform;

            Input = GetComponent<InputMB>();
            movement = GetComponent<CharacterMovementMB>();
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();
        }
    }
}
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
        protected InputMB input;
        public InputMB Input => input;
        
        // MOVEMENT
        protected CharacterMovementMB movement;
        public CharacterMovementMB Movement => movement;
        
        // PHYSICS
        protected Rigidbody rb;
        protected CapsuleCollider collider;
        
        public Rigidbody RB => rb;
        public CapsuleCollider Collider => collider;
        
        protected virtual void Awake()
        {
            cachedTransform = transform;

            input = GetComponent<InputMB>();
            movement = GetComponent<CharacterMovementMB>();
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();
        }
    }
}
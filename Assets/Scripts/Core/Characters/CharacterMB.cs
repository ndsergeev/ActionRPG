using Main.Characters;

namespace Main.Core
{
    using UnityEngine;

    using Main.Core.StateMachine;
    using Main.Core.Updates;
    using Main.Inputs;

    
    [RequireComponent(typeof(StateMachineMB))]
    public abstract class CharacterMB : Refresh, IRefresh
    {
        // STATE MACHINE
        protected StateMachineMB StateMachine;
        
        // INPUT
        protected InputMB Input;
        public InputMB input => Input;
        
        // MOVEMENT
        protected CharacterMovementMB Movement;
        public CharacterMovementMB movement => Movement;
        
        // PHYSICS
        protected Rigidbody Rb;
        protected CapsuleCollider Col;
        
        public Rigidbody rb => Rb;
        public Collider col => Col;
        
        // ANIMATION
        protected CharacterAnimationsMB Animations;
        public CharacterAnimationsMB animations => Animations;
        
        protected virtual void Awake()
        {
            StateMachine = GetComponent<StateMachineMB>();
            Input = GetComponent<InputMB>();
            Movement = GetComponent<CharacterMovementMB>();
            Rb = GetComponent<Rigidbody>();
            Col = GetComponent<CapsuleCollider>();
            Animations = GetComponent<CharacterAnimationsMB>();
        }
        
        public virtual void Run()
            => StateMachine.UpdateMachine();
    }
}
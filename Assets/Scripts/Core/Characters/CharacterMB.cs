using Main.Characters;

namespace Main.Core
{
    using UnityEngine;

    using Main.Core.StateMachine;
    using Main.Core.Updates;
    using Main.Core.Input;
    
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
        protected Collider Col;
        
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
            Col = GetComponent<Collider>();
            Animations = GetComponent<CharacterAnimationsMB>();
        }
        
        public virtual void Run()
            => StateMachine.UpdateMachine();
    }
}

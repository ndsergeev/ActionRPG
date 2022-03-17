
namespace Main.Core
{
    using UnityEngine;

    using Main.Characters;
    using Main.Core.Input;
    using Main.Core.StateMachine;
    using Main.Core.Updates;
    
    [RequireComponent(typeof(CharacterMovementMB))]
    [RequireComponent(typeof(StateMachineMB))]
    public abstract class CharacterMB : Refresh, IRefresh
    {
        // STATE MACHINE
        protected StateMachineMB m_StateMachine;
        
        // INPUT
        protected InputMB m_Input;
        public InputMB Input => m_Input;
        
        // MOVEMENT
        protected CharacterMovementMB m_Movement;
        public CharacterMovementMB Movement => m_Movement;
        
        // PHYSICS
        protected Rigidbody m_Rb;
        protected Collider m_Col;
        
        public Rigidbody Rb => m_Rb;
        public Collider Col => m_Col;
        
        // ANIMATION
        protected CharacterAnimationsMB m_Animations;
        public CharacterAnimationsMB Animations => m_Animations;
        
        // STATES
        public bool isMoving;
        public bool isWalking;
        public bool isRunning;
        public bool isCrouching;
        public bool isTargeting;
        
        protected virtual void Awake()
        {
            m_StateMachine = GetComponent<StateMachineMB>();
            m_Input = GetComponent<InputMB>();
            m_Movement = GetComponent<CharacterMovementMB>();
            m_Rb = GetComponent<Rigidbody>();
            m_Col = GetComponent<Collider>();
            m_Animations = GetComponent<CharacterAnimationsMB>();

            Input.InputReader.onFocusEvent += ToggleTargeting;
        }
        
        public virtual void Run()
            => m_StateMachine.UpdateMachine();
        
        
        // TODO: Make a separate targeting script
        public void ToggleTargeting()
        {
            isTargeting = !isTargeting;
        }
    }
}

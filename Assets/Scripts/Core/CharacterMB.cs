
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
        protected InputMB input;
        public InputMB Input => input;
        
        // MOVEMENT
        protected MovementMB Movement;
        public MovementMB movement => Movement;
        
        // PHYSICS
        protected Rigidbody Rb;
        protected CapsuleCollider Col;
        
        public Rigidbody rb => Rb;
        public Collider col => Col;
        
        protected virtual void Awake()
        {
            input = GetComponent<InputMB>();
            Movement = GetComponent<MovementMB>();
            Rb = GetComponent<Rigidbody>();
            Col = GetComponent<CapsuleCollider>();
            StateMachine = GetComponent<StateMachineMB>();
        }
        
        public virtual void Run()
            => StateMachine.UpdateMachine();
    }
}

namespace Main.Characters
{
    using UnityEngine;
    
    using Main.Core.Updates;
    using Main.Core.StateMachine;
    using Main.Input;
    
    // public class CharacterMB : Refresh, IRefresh
    // {
    //     // STATE MACHINE
    //     protected StateMachineMB StateMachine;
    //     
    //     // INPUT
    //     protected InputMB input;
    //     public InputMB Input => input;
    //     
    //     // MOVEMENT
    //     protected CharacterMovementMB Movement;
    //     public CharacterMovementMB movement => Movement;
    //     
    //     // PHYSICS
    //     protected Rigidbody rb;
    //     protected CapsuleCollider collider;
    //     
    //     public Rigidbody RB => rb;
    //     public CapsuleCollider Collider => collider;
    //     
    //     protected virtual void Awake()
    //     {
    //         input = GetComponent<InputMB>();
    //         Movement = GetComponent<CharacterMovementMB>();
    //         rb = GetComponent<Rigidbody>();
    //         collider = GetComponent<CapsuleCollider>();
    //         StateMachine = GetComponent<StateMachineMB>();
    //     }
    //     
    //     public virtual void Run()
    //         => StateMachine.UpdateMachine();
    // }
}
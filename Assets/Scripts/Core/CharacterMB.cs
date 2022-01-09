
namespace Main.Core
{
    using UnityEngine;

    using Main.Core.StateMachine;
    using Main.Core.Updates;
    
    [RequireComponent(typeof(StateMachineMB))]
    public abstract class CharacterMB : Refresh, IRefresh
    {
        protected StateMachineMB StateMachine;
    
        protected virtual void Awake()
            => StateMachine = GetComponent<StateMachineMB>();

        public virtual void Run()
            => StateMachine.UpdateMachine();
    }
}
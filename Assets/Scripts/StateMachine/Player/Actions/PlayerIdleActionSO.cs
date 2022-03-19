namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Player_Idle_ActionSO", 
        menuName = "State Machine/Player/New Player_Idle_ActionSO", order = 60)]
    public class PlayerIdleActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        { 
            character.Rb.velocity = Vector3.zero;
        }

        public override void OnUpdate(CharacterMB character)
        { }

        public override void OnExit(CharacterMB character)
        { }
    }
}

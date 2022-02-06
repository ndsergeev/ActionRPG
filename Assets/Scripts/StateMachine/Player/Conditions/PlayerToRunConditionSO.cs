namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToRun_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToRun_ConditionSO", order = 45)]
    public class PlayerToRunConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isMoveInput = player.PlayerInput.moveInput != Vector2.zero;
            var isRunInput = player.PlayerInput.runInput;
            
            return isMoveInput && isRunInput;
        }
    }
}

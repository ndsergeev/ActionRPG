namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToWalkFromRun_ConditionSO", menuName = 
        "State Machine/Player/New Player_ToWalkFromRun_ConditionSO", order = 45)]
    public class PlayerToWalkFromRunConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isMoveInput = player.PlayerInput.moveInput != Vector2.zero;
            var isRunInput = player.PlayerInput.runInput;

            return isMoveInput && !isRunInput;
        }
    }
}

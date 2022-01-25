namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToWalkFromRun_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToWalkFromRun_ConditionSO", order = 45)]
    public class PlayerToWalkFromRunConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isMoveInput = player.PlayerInput.moveInput != Vector2.zero;
            bool isRunInput = player.PlayerInput.runInput;

            if (isMoveInput && !isRunInput)
                return true;

            return false;
        }
    }
}

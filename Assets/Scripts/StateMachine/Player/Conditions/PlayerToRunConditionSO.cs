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
            PlayerMB player = character as PlayerMB;

            bool isMoveInput = player.playerInput.moveInput != Vector2.zero;
            bool isRunInput = player.playerInput.runInput;
            
            if (isMoveInput && isRunInput)
                return true;

            return false;
        }
    }
}

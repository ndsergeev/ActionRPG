namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;

    [CreateAssetMenu(fileName = "Player_ToWalkFromFall_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToWalkFromFall_ConditionSO", order = 45)]
    public class PlayerToWalkFromFallConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isGrounded = player.movement.isGrounded;
            bool isMoveInput = player.playerInput.moveInput != Vector2.zero;
            bool isRunInput = player.playerInput.runInput;

            if (isGrounded && isMoveInput && !isRunInput)
                return true;

            return false;
        }
    }
}

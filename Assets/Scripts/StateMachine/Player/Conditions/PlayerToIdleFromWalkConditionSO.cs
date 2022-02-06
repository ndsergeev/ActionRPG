namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToIdleFromWalk_ConditionSO", menuName =
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToIdleFromWalk_ConditionSO", order = 45)]
    public class PlayerToIdleFromWalkConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isGrounded = player.movement.isGrounded;
            bool isMoveInput = player.playerInput.moveInput != Vector2.zero;

            if (isGrounded && !isMoveInput)
                return true;

            return false;
        }
    }
}

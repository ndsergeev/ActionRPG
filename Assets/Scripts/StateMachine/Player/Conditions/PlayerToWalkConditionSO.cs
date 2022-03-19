namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToWalk_ConditionSO", menuName = 
        "State Machine/Player/New Player_ToWalk_ConditionSO", order = 45)]
    public class PlayerToWalkConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isGrounded = player.Movement.IsGrounded;
            var isMoveInput = player.PlayerInput.moveInput != Vector2.zero;
            
            return isGrounded && isMoveInput;
        }
    }
}

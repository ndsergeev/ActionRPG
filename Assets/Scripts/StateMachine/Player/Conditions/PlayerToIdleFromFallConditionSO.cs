namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToIdleFromFall_ConditionSO", menuName = 
        "State Machine/Player/New Player_ToIdleFromFall_ConditionSO", order = 45)]
    public class PlayerToIdleFromFallConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isGrounded = player.Movement.IsGrounded;
            
            return isGrounded;
        }
    }
}

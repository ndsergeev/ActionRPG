namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToIdleFromFall_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToIdleFromFall_ConditionSO", order = 45)]
    public class PlayerToIdleFromFallConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isGrounded = player.movement.isGrounded;
            
            if (isGrounded)
                return true;

            return false;
        }
    }
}

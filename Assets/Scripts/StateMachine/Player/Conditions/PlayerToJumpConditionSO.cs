namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToJump_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_ToJump_ConditionSO", order = 45)]
    public class PlayerToJumpConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isJumpInput = player.PlayerInput.jumpInput;
            
            return isJumpInput;
        }
    }
}

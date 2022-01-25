namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Fall_ConditionSO", menuName = 
        "Scriptable Objects/State Machine/Player Conditions/New Player_Fall_ConditionSO", order = 45)]
    public class PlayerToFallConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isFalling = player.rb.velocity.y < 0;
            
            if (isFalling)
                return true;

            return false;
        }
    }
}

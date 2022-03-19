namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Fall_ConditionSO", menuName = 
        "State Machine/Player/New Player_Fall_ConditionSO", order = 45)]
    public class PlayerToFallConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            var isFalling = player.Rb.velocity.y < 0;
            return isFalling;
        }
    }
}

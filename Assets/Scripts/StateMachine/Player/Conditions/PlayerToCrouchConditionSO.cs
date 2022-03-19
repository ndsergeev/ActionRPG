
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_ToCrouch_ConditionSO", menuName =
        "State Machine/Player/New Player_ToCrouch_ConditionSO", order = 45)]
    public class PlayerToCrouchConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            var player = character as PlayerMB;
            
            return player.CanCrouch();
        }
    }
}

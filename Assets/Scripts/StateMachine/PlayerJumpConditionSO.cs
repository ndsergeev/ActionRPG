
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "PlayerJumpConditionSO", menuName = "Scriptable Objects/State Machine/New PlayerJumpConditionSO", order = 45)]
    public class PlayerJumpConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            throw new System.NotImplementedException();
        }
    }
}

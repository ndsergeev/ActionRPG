
using Main.Core;

namespace Main
{
    using UnityEngine;
    
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "SecondConditionSO", menuName = "Scriptable Objects/State Machine/New SecondConditionSO", order = 45)]
    public class SecondConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            return ((PlayerMB) character).IsGrounded();
        }
    }
}

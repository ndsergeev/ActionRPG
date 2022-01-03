
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "FirstConditionSO", menuName = "Scriptable Objects/State Machine/New FirstConditionSO", order = 45)]
    public class FirstConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB characterMB)
        {
            return true;
        }
    }
}

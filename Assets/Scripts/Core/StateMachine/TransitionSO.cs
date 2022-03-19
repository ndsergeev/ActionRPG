
namespace Main.Core.StateMachine
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "TransitionSO", menuName = "State Machine/New TransitionSO", order = 30)]
    public class TransitionSO : ScriptableObject
    {
        [Space]
        public ConditionSO condition;
        [Space]
        public StateSO targetState;
    }
}

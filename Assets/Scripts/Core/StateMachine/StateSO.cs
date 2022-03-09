
using System.Collections.Generic;

namespace Main.Core.StateMachine
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "StateSO", menuName = "Scriptable Objects/State Machine/New StateSO", order = 15)]
    public class StateSO : ScriptableObject
    {
        [HideInInspector]
        public bool isFoldout;
        [Space]
        public List<TransitionSO> transitions;
        [Space]
        public List<ActionSO> actions;
    }
}

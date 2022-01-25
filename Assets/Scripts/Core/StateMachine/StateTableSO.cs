
using System.Collections.Generic;

namespace Main.Core.StateMachine
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "StateTableSO", menuName = "Scriptable Objects/State Machine/New StateTableSO", order = 0)]
    public class StateTableSO : ScriptableObject
    {
        // [Space]
        public List<StateSO> states = new List<StateSO>();
// #if UNITY_EDITOR
//         private void OnValidate()
//         {
//             // TODO: check that all targets in transitions are in states List
//         }
// #endif
    }
}


using System.Collections.Generic;

namespace Main.Core.StateMachine
{
    using UnityEngine;

    public sealed class StateMachineMB : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private StateTableSO m_StateTable;
        
        private List<State> m_States;
        private State m_CurrentState; 
        
        private CharacterMB m_Character;

        private void Awake()
        {
            m_Character = GetComponent<CharacterMB>();
            
            // Step creates states
            m_States = new List<State>(m_StateTable.states.Count);
            var statePairs = new Dictionary<StateSO, State>();
            foreach (var stateSO in m_StateTable.states)
            {
                var state = new State(stateSO.transitions.Count, stateSO.actions.Count);
                foreach (var actionSO in stateSO.actions)
                    state.Actions.Add(actionSO);
                
                statePairs.Add(stateSO, state);
                m_States.Add(state);
            }

            // Step connects states
            foreach (var statePair in statePairs)
            {
                var stateSO = statePair.Key;
                var state = statePair.Value;
                foreach (var transitionSO in stateSO.transitions)
                {
                    var condition = transitionSO.condition;
                    var transition = new Transition(transitionSO, condition, statePairs[transitionSO.targetState]);
                    state.Transitions.Add(transition);
                }
            }

            // Finally set up the first state
            m_CurrentState = m_States[0];
            m_CurrentState.OnEnter(m_Character);
        }

        public void UpdateMachine()
        {
            var state = m_CurrentState.CanTransit(m_Character);
            if (state != null) // then transit
            {
                m_CurrentState.OnExit(m_Character);
                m_CurrentState = state;
                m_CurrentState.OnEnter(m_Character);
            }
            
            m_CurrentState.OnUpdate(m_Character);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_States != null && m_States.Count < 1)
                Main.Core.Console.EditorLogger.Log($"<color=yellow>{nameof(m_States)} of {this.name} is empty.</color>");
        }
#endif
    }

    public sealed class State
    {
        public readonly List<Transition> Transitions;
        public readonly List<ActionSO> Actions;

        public State(int transitionsSize, int actionSize)
        {
            Transitions = new List<Transition>(transitionsSize);
            Actions = new List<ActionSO>(actionSize);
        }

        public State CanTransit(CharacterMB character)
        {
            foreach (var transition in Transitions)
            {
                var nextState = transition.CanTransit(character);
                if (nextState != null)
                    return nextState;
            }

            return null;
        }

        public void OnEnter(CharacterMB character)
        {
            foreach (var action in Actions)
                action.OnEnter(character);
        }

        public void OnExit(CharacterMB character)
        {
            foreach (var action in Actions)
                action.OnExit(character);
        }
        
        public void OnUpdate(CharacterMB character)
        {
            foreach (var action in Actions)
                action.OnUpdate(character);
        }
    }

    public sealed class Transition
    {
        /// <summary>
        /// Don't remove, it could have some info for, Idk, delay, loop or anything else
        /// </summary>
        private TransitionSO m_TransitionSO;
        private ConditionSO m_Condition;
        private State m_TargetState;

        public Transition(TransitionSO transitionSO, ConditionSO condition, State targetState)
        {
            m_TransitionSO = transitionSO;
            m_Condition = condition;
            m_TargetState = targetState;
        }

        public State CanTransit(CharacterMB character)
            => m_Condition.CanTransit(character) ? m_TargetState : null;
    }
}


using System.Collections.Generic;

namespace Main.Core.StateMachine
{
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
}
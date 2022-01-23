
namespace Main.Core.StateMachine
{
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
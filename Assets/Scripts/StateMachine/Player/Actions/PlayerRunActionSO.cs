namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Run_ActionSO", 
        menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Run_ActionSO", order = 60)]
    public class PlayerRunActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.Movement.IsRunning = true;
            m_Player.Animations.StartRunning();
        }

        public override void OnUpdate(CharacterMB character)
        { }

        public override void OnExit(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.Movement.IsRunning = false;
            m_Player.Animations.StopRunning();
        }
    }
}

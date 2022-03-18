
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Falling_ActionSO",
        menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Falling_ActionSO", order = 60)]
    public class PlayerFallingActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.Animations.StartFalling();
        }

        public override void OnUpdate(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.Movement.HandleJumping();
            m_Player.Movement.HandleAirVelocity();
        }

        public override void OnExit(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.Animations.StopFalling();
        }
    }
}


using System.Dynamic;

namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Jumping_ActionSO",
        menuName = "State Machine/Player/New Player_Jumping_ActionSO", order = 60)]
    public class PlayerJumpingActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.Movement.Jump();
            m_Player.Animations.StartJumping();
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
            m_Player.Animations.StopJumping();
        }
    }
}

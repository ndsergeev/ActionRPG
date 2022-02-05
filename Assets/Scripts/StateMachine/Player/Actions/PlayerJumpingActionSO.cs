
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Jumping_ActionSO",
        menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Jumping_ActionSO", order = 60)]
    public class PlayerJumpingActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.movement.Jump();
            m_Player.animations.StartJumping();
        }

        public override void OnUpdate(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.movement.HandleJumping();
        }

        public override void OnExit(CharacterMB character)
        {
            m_Player.animations.StopJumping();
        }
    }
}

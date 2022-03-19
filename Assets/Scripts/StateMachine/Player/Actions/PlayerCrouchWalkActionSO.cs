namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;

    
    [CreateAssetMenu(fileName = "Player_CrouchWalk_ActionSO", menuName =
        "State Machine/Player/New Player_CrouchWalk_ActionSO", order = 60)]
    public class PlayerCrouchWalkActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.Animations.StartCrouching();
            m_Player.Animations.StartWalking();
            m_Player.Movement.IsCrouchWalking = true;
        }

        public override void OnUpdate(CharacterMB character)
        { }

        public override void OnExit(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.Animations.StopCrouching();
            m_Player.Animations.StopWalking();
            m_Player.Movement.IsCrouchWalking = false;
        }
    }
}

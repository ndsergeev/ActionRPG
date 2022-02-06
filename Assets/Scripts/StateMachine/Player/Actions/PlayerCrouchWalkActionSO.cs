namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;

    
    [CreateAssetMenu(fileName = "Player_CrouchWalk_ActionSO", menuName =
        "Scriptable Objects/State Machine/Player Actions/New Player_CrouchWalk_ActionSO", order = 60)]
    public class PlayerCrouchWalkActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.animations.StartCrouching();
        }

        public override void OnUpdate(CharacterMB character)
        {
            
        }

        public override void OnExit(CharacterMB character)
        {
            m_Player.animations.StopCrouching();
        }
    }
}

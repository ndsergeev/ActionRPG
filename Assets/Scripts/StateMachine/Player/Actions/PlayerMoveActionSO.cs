namespace Main
{
    using UnityEngine;
    
    using Main.Characters;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Player_Move_ActionSO",
        menuName = "State Machine/Player/New Player_Move_ActionSO", order = 60)]
    public class PlayerMoveActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(Core.CharacterMB character)
        { }

        public override void OnUpdate(Core.CharacterMB character)
        {
            m_Player = character as PlayerMB;
            m_Player.HandleMovementInput();
        }

        public override void OnExit(Core.CharacterMB character)
        { }
    }
}


namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.Console;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Crouch_ActionSO", menuName =
        "State Machine/Player/New Player_Crouch_ActionSO", order = 60)]
    public class PlayerCrouchActionSO : ActionSO
    {
        private PlayerMB m_Player;
        
        public override void OnEnter(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.Animations.StartCrouching();
            m_Player.Rb.velocity = Vector3.zero;
            EditorLogger.Log("<color=yellow>Entered</color> State: Crouch");
        }

        public override void OnUpdate(CharacterMB character)
        { }

        public override void OnExit(CharacterMB character)
        {
            m_Player = character as PlayerMB;
            
            m_Player.Animations.StopCrouching();
            EditorLogger.Log("<color=cyan>Exit</color> State: Crouch");
        }
    }
}

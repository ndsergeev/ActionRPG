namespace Main
{
    using UnityEngine;
    
    using Main.Characters;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Player_Move_ActionSO", menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Move_ActionSO", order = 60)]
    public class PlayerMoveActionSO : ActionSO
    {
        public override void OnEnter(Core.CharacterMB character)
        {
            
        }

        public override void OnUpdate(Core.CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;
            
            player.HandleMovementInput();
        }

        public override void OnExit(Core.CharacterMB character)
        {
            
        }
    }
}

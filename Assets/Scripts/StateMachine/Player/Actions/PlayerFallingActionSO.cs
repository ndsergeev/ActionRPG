namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Falling_ActionSO", menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Falling_ActionSO", order = 60)]
    public class PlayerFallingActionSO : ActionSO
    {
        private PlayerMB player;
        
        public override void OnEnter(CharacterMB character)
        {
            player = character as PlayerMB;
            
            player.animations.StartFalling();
        }

        public override void OnUpdate(CharacterMB character)
        {
            player = character as PlayerMB;
            
            player.movement.HandleJumping();
        }

        public override void OnExit(CharacterMB character)
        {
            player.animations.StopFalling();
        }
    }
}

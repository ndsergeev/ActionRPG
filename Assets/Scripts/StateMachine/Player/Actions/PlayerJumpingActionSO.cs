namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Jumping_ActionSO", menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Jumping_ActionSO", order = 60)]
    public class PlayerJumpingActionSO : ActionSO
    {
        protected PlayerMB player;
        
        public override void OnEnter(CharacterMB character)
        {
            player = character as PlayerMB;
            
            player.movement.Jump();
            player.animations.StartJumping();
        }

        public override void OnUpdate(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;
            
            player.movement.HandleJumping();
        }

        public override void OnExit(CharacterMB character)
        {
            player.animations.StopJumping();
        }
    }
}

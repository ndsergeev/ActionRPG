namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    
    [CreateAssetMenu(fileName = "Player_Run_ActionSO", menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Run_ActionSO", order = 60)]
    public class PlayerRunActionSO : ActionSO
    {
        private PlayerMB player;
        
        public override void OnEnter(CharacterMB character)
        {
            player = character as PlayerMB;

            player.movement.isRunning = true;
            
            player.animations.StartRunning();
        }

        public override void OnUpdate(CharacterMB character)
        {
        }

        public override void OnExit(CharacterMB character)
        {
            player.movement.isRunning = false;
            
            player.animations.StopRunning();
        }
    }
}

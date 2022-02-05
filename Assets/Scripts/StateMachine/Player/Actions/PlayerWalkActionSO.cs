namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Player_Walk_ActionSO",
        menuName = "Scriptable Objects/State Machine/Player Actions/New Player_Walk_ActionSO", order = 60)]
    public class PlayerWalkActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        {
            character.movement.isWalking = true;
            character.animations.StartWalking();
        }

        public override void OnUpdate(CharacterMB character)
        { }

        public override void OnExit(CharacterMB character)
        {
            character.movement.isWalking = false;
            character.animations.StopWalking();
        }
    }
}

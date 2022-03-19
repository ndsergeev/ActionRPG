using ICSharpCode.NRefactory.PrettyPrinter;

namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Player_Walk_ActionSO",
        menuName = "State Machine/Player/New Player_Walk_ActionSO", order = 60)]
    public class PlayerWalkActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        {
            character.Movement.IsWalking = true;
            character.Animations.StartWalking();
        }

        public override void OnUpdate(CharacterMB character)
        {
            
        }

        public override void OnExit(CharacterMB character)
        {
            character.Movement.IsWalking = false;
            character.Animations.StopWalking();
        }
    }
}

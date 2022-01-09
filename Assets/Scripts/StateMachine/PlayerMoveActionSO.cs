
namespace Main
{
    using UnityEngine;
    
    using Main.Characters;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "PlayerMoveActionSO", menuName = "Scriptable Objects/State Machine/New PlayerMoveActionSO", order = 60)]
    public class PlayerMoveActionSO : ActionSO
    {
        public override void OnEnter(Core.CharacterMB character)
        {
            ((PlayerMB) character).JumpStart();
        }

        public override void OnUpdate(Core.CharacterMB character)
        { }

        public override void OnExit(Core.CharacterMB character)
        { }
    }
}

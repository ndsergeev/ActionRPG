
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "Character_Grounding_ActionSO", menuName = "Scriptable Objects/State Machine/Character Actions/New Character_Grounding_ActionSO", order = 60)]
    public class CharacterGroundingActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        {
        }

        public override void OnUpdate(CharacterMB character)
        {
            character.movement.HandleGrounding();
        }

        public override void OnExit(CharacterMB character)
        {
            
        }
    }
}

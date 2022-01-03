
namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.Console;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "FirstActionSO", menuName = "Scriptable Objects/State Machine/New FirstActionSO", order = 60)]
    public class FirstActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnEnter)}: {nameof(FirstActionSO)}</color>");
            ((PlayerMB)character).ChangeColorToRandom();
        }

        public override void OnUpdate(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnUpdate)}: {nameof(FirstActionSO)}</color>");
        }

        public override void OnExit(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnExit)}: {nameof(FirstActionSO)}</color>");
        }
    }
}


namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.Console;
    using Main.Core.StateMachine;
    
    [CreateAssetMenu(fileName = "SecondActionSO", menuName = "Scriptable Objects/State Machine/New SecondActionSO", order = 60)]
    public class SecondActionSO : ActionSO
    {
        public override void OnEnter(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnEnter)}: {nameof(SecondActionSO)}</color>");
            ((PlayerMB)character).ChangeColorToGrey();
        }

        public override void OnUpdate(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnUpdate)}: {nameof(SecondActionSO)}</color>");
        }

        public override void OnExit(CharacterMB character)
        {
            EditorLogger.Log($"<color=cyan>{nameof(OnExit)}: {nameof(SecondActionSO)}</color>");
        }
    }
}

using ICSharpCode.NRefactory.PrettyPrinter;
using UnityEngine.InputSystem;

namespace Main
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Core.StateMachine;
    using Main.Characters;
    using UnityEngine.InputSystem;
    
    [CreateAssetMenu(fileName = "Player_ToCrouch_ConditionSO", menuName =
        "Scriptable Objects/State Machine/Player Conditions/ New Player_ToCrouch_ConditionSO", order = 45)]
    public class PlayerToCrouchConditionSO : ConditionSO
    {
        public override bool CanTransit(CharacterMB character)
        {
            PlayerMB player = character as PlayerMB;

            bool isCrouchInput = player.PlayerInput.inputReader.inputControls.CharacterControl.Crouch.WasPressedThisFrame();
            
            if (isCrouchInput)
                return true;

            return false;
        }
    }
}

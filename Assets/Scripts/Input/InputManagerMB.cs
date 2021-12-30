using Core;

namespace Main.InputControls
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputManagerMB : SingletonMB<InputManagerMB>, InputControls.ICharacterControlActions
    {
        private InputControls inputControls;

        public enum InputDevices
        {
            KeyboardAndMouse,
            Playstation,
            Xbox,
            Switch,
            Mobile,
        }

        private InputDevices currentInputDevice;

        private void Awake()
        {
            // This is the Input Action asset
            // It contains all the information related to the player inputs,
            // is what calls actions (Ex. The 'Move' action calls the 
            // OnMove() function on this script, and passes the InputAction.CallbackContext,
            // which contains the information related to the move input.
            inputControls = new InputControls();

            // 'CharacterControl' is the 'Action Map' on the 'inputControls' Input Action asset 
            // Set the input system to call methods on this script
            inputControls.CharacterControl.SetCallbacks(this);
        }

        void OnEnable()
        {
            EnableInputs();
        }

        void OnDisable()
        {
            DisableInputs();
        }

        public void EnableInputs()
        {
            inputControls.Enable();
        }

        public void DisableInputs()
        {
            inputControls.Disable();
        }

        // Left Joy-Stick // WASD keys // Arrow keys
        public void OnMove(InputAction.CallbackContext context)
        {
            InputEvents.TriggerMoveEvents(context.ReadValue<Vector2>());
        }

        // Right Joy-Stick // Mouse
        public void OnLook(InputAction.CallbackContext context)
        {
            //look = context.ReadValue<Vector2>();
        }

        // Space key // South Button
        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void OnFocus(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }
    }
}
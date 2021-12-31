using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using Main.Input;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReaderSO : ScriptableObject, InputControls.ICharacterControlActions
    {
        private InputControls m_inputControls;

        private void OnEnable()
        {
            m_inputControls = new InputControls();
            m_inputControls.CharacterControl.SetCallbacks(this);
            m_inputControls?.CharacterControl.Enable();
        }

        private void OnDisable()
        {
            m_inputControls?.CharacterControl.Disable();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            lookAtEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Canceled)) jumpEvent?.Invoke();
        }

        public void OnFocus(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Canceled)) focusEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed)) crouchEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    sprintStartEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    sprintEndEvent?.Invoke();
                    break;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public event UnityAction jumpEvent;
        public event UnityAction focusEvent;
        public event UnityAction crouchEvent;
        public event UnityAction sprintStartEvent;
        public event UnityAction sprintEndEvent;
        public event UnityAction<Vector2> moveEvent;
        public event UnityAction<Vector2> lookAtEvent;
    }
}
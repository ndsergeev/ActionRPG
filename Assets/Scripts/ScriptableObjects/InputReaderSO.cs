
namespace Main.Input
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/New Input Reader", order = 0)]
    public class InputReaderSO : ScriptableObject, InputControls.ICharacterControlActions
    {
        public event UnityAction onJumpEvent;
        public event UnityAction onFocusEvent;
        public event UnityAction crouchEvent;
        public event UnityAction sprintStartEvent;
        public event UnityAction sprintEndEvent;
        public event UnityAction<Vector2> onMoveEvent;
        public event UnityAction<Vector2> onLookAroundEvent;
        
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

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Canceled)) onJumpEvent?.Invoke();
        }

        public void OnFocus(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Canceled)) onFocusEvent?.Invoke();
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
            onMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            onLookAroundEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
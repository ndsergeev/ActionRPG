
namespace Main.Input
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/New Input Reader", order = 0)]
    public class InputReaderSO : ScriptableObject, InputControls.ICharacterControlActions
    {
        // EVENTS
        public event UnityAction jumpStartEvent;
        public event UnityAction jumpEndEvent;
        public event UnityAction onFocusEvent;
        public event UnityAction crouchEvent;
        public event UnityAction sprintStartEvent;
        public event UnityAction sprintEndEvent;
        public event UnityAction<Vector2> onMoveEvent;
        public event UnityAction<Vector2> onLookAroundEvent;
         
        // CONTROLS
        private InputControls m_inputControls;

        // STORED LATEST INPUTS
        private Vector2 m_moveInput;
        public Vector2 MoveInput => m_moveInput;
        
        private Vector2 m_lookInput;
        public Vector2 LookInput => m_lookInput;
        
        private bool m_jumpInput;
        public bool JumpInput => m_jumpInput;

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
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_jumpInput = true;
                    jumpStartEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_jumpInput = false;
                    jumpEndEvent?.Invoke();
                    break;
            }
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
            m_moveInput = context.ReadValue<Vector2>();
            onMoveEvent?.Invoke(m_moveInput);
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            m_lookInput = context.ReadValue<Vector2>();
            onLookAroundEvent?.Invoke(m_lookInput);
        }
    }
}
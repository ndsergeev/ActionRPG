
namespace Main.Core.Input
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/New Input Reader", order = 0)]
    public class InputReaderSO : ScriptableObject, InputControls.ICharacterControlActions
    {
        // EVENTS
        public event UnityAction onJumpStartEvent;
        public event UnityAction onJumpEndEvent;
        public event UnityAction onFocusEvent;
        public event UnityAction onRunEvent;
        public event UnityAction onCrouchEvent;
        public event UnityAction<Vector2> onMoveEvent;
        public event UnityAction<Vector2> onLookAroundEvent;
         
        // CONTROLS
        protected InputControls m_InputControls;
        public InputControls inputControls => m_InputControls;

        // STORED LATEST INPUTS
        private Vector2 m_moveInput;
        public Vector2 MoveInput => m_moveInput;
        
        private Vector2 m_lookInput;
        public Vector2 LookInput => m_lookInput;
        
        private bool m_jumpInput;
        public bool JumpInput => m_jumpInput;

        private bool m_runInput;
        public bool RunInput => m_runInput;
        
        protected bool m_CrouchInput;
        protected bool m_CrouchReleased;
        public bool crouchInput => m_CrouchInput;
        public bool crouchReleased => m_CrouchReleased;

        private void OnEnable()
        {
            m_InputControls = new InputControls();
            m_InputControls.CharacterControl.SetCallbacks(this);
            m_InputControls?.CharacterControl.Enable();
            m_InputControls.CharacterControl.Crouch.Enable();
        }

        private void OnDisable()
        {
            m_InputControls?.CharacterControl.Disable();
            m_InputControls?.CharacterControl.Crouch.Disable();
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

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_jumpInput = true;
                    onJumpStartEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_jumpInput = false;
                    onJumpEndEvent?.Invoke();
                    break;
            }
        }

        public void OnFocus(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Canceled))
                onFocusEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_CrouchInput = true;
                    onCrouchEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_CrouchInput = false;
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_runInput = true;
                    onRunEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_runInput = false;
                    onRunEvent?.Invoke();
                    break;
            }
        }
    }
}
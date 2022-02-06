
namespace Main.Core.Input
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
        protected InputControls m_InputControls;
        public InputControls inputControls => m_InputControls;

        // STORED LATEST INPUTS
        protected Vector2 m_MoveInput;
        public Vector2 moveInput => m_MoveInput;
        
        protected Vector2 m_LookInput;
        public Vector2 lookInput => m_LookInput;
        
        protected bool m_JumpInput;
        public bool jumpInput => m_JumpInput;

        protected bool m_RunInput;
        public bool runInput => m_RunInput;
        
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
            m_InputControls.CharacterControl.Crouch.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_JumpInput = true;
                    jumpStartEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_JumpInput = false;
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
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_CrouchInput = true;
                    crouchEvent?.Invoke();
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
                    m_RunInput = true;
                    break;
                case InputActionPhase.Canceled:
                    m_RunInput = false;
                    break;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            m_MoveInput = context.ReadValue<Vector2>();
            onMoveEvent?.Invoke(m_MoveInput);
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            m_LookInput = context.ReadValue<Vector2>();
            onLookAroundEvent?.Invoke(m_LookInput);
        }
    }
}
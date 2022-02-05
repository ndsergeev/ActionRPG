
using NotImplementedException = System.NotImplementedException;

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
        public event UnityAction runEvent;
        public event UnityAction crouchEvent;
        public event UnityAction sprintStartEvent;
        public event UnityAction sprintEndEvent;
        public event UnityAction<Vector2> onMoveEvent;
        public event UnityAction<Vector2> onLookAroundEvent;
         
        // CONTROLS
        private InputControls m_InputControls;

        // STORED LATEST INPUTS
        private Vector2 m_MoveInput;
        public Vector2 moveInput => m_MoveInput;
        
        private Vector2 m_LookInput;
        public Vector2 lookInput => m_LookInput;
        
        private bool m_JumpInput;
        public bool jumpInput => m_JumpInput;

        private bool m_RunInput;
        public bool runInput => m_RunInput;

        private void OnEnable()
        {
            m_InputControls = new InputControls();
            m_InputControls.CharacterControl.SetCallbacks(this);
            m_InputControls?.CharacterControl.Enable();
        }

        private void OnDisable()
        {
            m_InputControls?.CharacterControl.Disable();
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
            if (context.phase.Equals(InputActionPhase.Performed)) crouchEvent?.Invoke();
        }

        // TODO: there is a sprint, see OnSprint, delete it or OnRun and relative variables
        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    m_RunInput = true;
                    runEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    m_RunInput = false;
                    runEvent?.Invoke();
                    break;
            }
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
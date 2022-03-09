namespace Main.Core.Input
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Characters;
    using UnityEngine.InputSystem;

    
    public class PlayerInputMB : InputMB
    {
        protected PlayerMB player => m_Character as PlayerMB;
        
        public Vector2 moveInput => m_InputReader.MoveInput;
        public Vector2 lookInput => m_InputReader.LookInput;
        public bool jumpInput => m_InputReader.JumpInput;

        public bool runInput => m_InputReader.RunInput;

        public bool crouchInput => m_InputReader.crouchInput;
        public bool crouchReleased => m_InputReader.crouchReleased;

        protected void Awake()
            => m_Character = GetComponent<CharacterMB>();
    }
}

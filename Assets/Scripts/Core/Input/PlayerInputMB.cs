namespace Main.Core.Input
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Characters;
    using UnityEngine.InputSystem;

    
    public class PlayerInputMB : InputMB
    {
        protected PlayerMB player => Character as PlayerMB;
        
        public Vector2 moveInput => InputReader.moveInput;
        public Vector2 lookInput => InputReader.lookInput;
        public bool jumpInput => InputReader.jumpInput;

        public bool runInput => InputReader.runInput;

        public bool crouchInput => InputReader.crouchInput;
        public bool crouchReleased => InputReader.crouchReleased;

        protected void Awake()
            => Character = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}

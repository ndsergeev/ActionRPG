
namespace Main.Core.Input
{
    using UnityEngine;
    
    using Main.Core;
    using Main.Characters;
    
    public class PlayerInputMB : InputMB
    {
        protected PlayerMB player => Character as PlayerMB;
        
        public Vector2 moveInput => InputReader.moveInput;
        public bool jumpInput => InputReader.jumpInput;

        public bool runInput => InputReader.runInput;

        protected void Awake()
            => Character = GetComponent<CharacterMB>();
    }
}

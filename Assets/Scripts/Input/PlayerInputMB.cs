
namespace Main.Inputs
{
    using UnityEngine;
    
    using Main.Characters;
    using Main.Core;
    
    public class PlayerInputMB : InputMB
    {
        protected PlayerMB player => Character as PlayerMB;
        
        public Vector2 moveInput => InputReader.moveInput;
        public bool jumpInput => InputReader.jumpInput;

        protected void Awake()
            => Character = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        
            InputReader.jumpStartEvent += player.JumpStart;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        
            InputReader.jumpStartEvent -= player.JumpStart;
        }
    }
}

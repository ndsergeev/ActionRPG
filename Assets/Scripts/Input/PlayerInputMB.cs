
namespace Main.Input
{
    using UnityEngine;
    using Main.Characters;
    
    public class PlayerInputMB : InputMB
    {
        protected PlayerMB player => character as PlayerMB;
        
        public Vector2 MoveInput => inputReaderSO.MoveInput;
        public bool JumpInput => inputReaderSO.JumpInput;

        protected void Awake()
            => character = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        
            inputReaderSO.jumpStartEvent += player.JumpStart;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        
            inputReaderSO.jumpStartEvent -= player.JumpStart;
        }
    }
}

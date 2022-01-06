
namespace Main.Input
{
    using UnityEngine;
    using Main.Characters;
    
    public class PlayerInputMB : InputMB
    {
        public Vector2 MoveInput => inputReaderSO.MoveInput;
        public bool JumpInput => inputReaderSO.JumpInput;

        protected void Awake()
            => character = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        
            inputReaderSO.jumpStartEvent += (character as PlayerMB).JumpStart;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        
            inputReaderSO.jumpStartEvent -= (character as PlayerMB).JumpStart;
        }
    }
}

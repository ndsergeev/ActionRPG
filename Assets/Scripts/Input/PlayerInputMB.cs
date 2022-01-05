
namespace Main.Input
{
    using Main.Characters;
    using UnityEngine;
    
    public class PlayerInputMB : InputMB
    {
        private CharacterMB m_CharacterMB;
        
        public Vector2 MoveInput => inputReaderSO.MoveInput;
        public bool JumpInput => inputReaderSO.JumpInput;

        protected void Awake()
            => m_CharacterMB = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        
            inputReaderSO.onMoveEvent += m_CharacterMB.Move;
            inputReaderSO.jumpStartEvent += m_CharacterMB.JumpStart;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        
            inputReaderSO.onMoveEvent -= m_CharacterMB.Move;
            inputReaderSO.jumpStartEvent -= m_CharacterMB.JumpStart;
        }
    }
}

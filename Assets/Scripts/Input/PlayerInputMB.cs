
namespace Main.Input
{
    using Main.SomethingToBeRenamed;
    
    public class PlayerInputMB : InputMB
    {
        private CharacterMB m_CharacterMB;

        protected void Awake()
            => m_CharacterMB = GetComponent<CharacterMB>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
        
            InputReaderSO.onMoveEvent += m_CharacterMB.Move;
            InputReaderSO.onJumpEvent += m_CharacterMB.Jump;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
        
            InputReaderSO.onMoveEvent -= m_CharacterMB.Move;
            InputReaderSO.onJumpEvent -= m_CharacterMB.Jump;
        }
    }
}

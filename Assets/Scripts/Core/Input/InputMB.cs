
namespace Main.Core.Input
{
    using UnityEngine;
    
    using Main.Core;
    
    public abstract class InputMB : MonoBehaviour
    {
        [SerializeField]
        protected InputReaderSO m_InputReader;
        public InputReaderSO InputReader => m_InputReader;
        
        protected CharacterMB m_Character;
        
        protected virtual void OnEnable()
            => SubscribeToInputManager();

        protected virtual void OnDisable()
            => UnsubscribeFromInputManager();

        protected void SubscribeToInputManager()
            => InputManagerMB.instance.SubscribeInputReader(this);

        protected void UnsubscribeFromInputManager()
            => InputManagerMB.instance.UnsubscribeInputReader(this);
    }
}

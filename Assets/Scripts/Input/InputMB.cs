namespace Main.Input
{
    using UnityEngine;
    
    using Main.Characters;
    
    public abstract class InputMB : MonoBehaviour
    {
        [SerializeField]
        protected InputReaderSO  inputReaderSO;
        public InputReaderSO InputReaderSO => inputReaderSO;
        
        protected CharacterMB character;
        
        
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

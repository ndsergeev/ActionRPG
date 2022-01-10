

namespace Main.Core.Input
{
    using UnityEngine;
    
    using Main.Core;
    
    public abstract class InputMB : MonoBehaviour
    {
        [SerializeField]
        protected InputReaderSO InputReader;
        public InputReaderSO inputReader => InputReader;
        
        protected CharacterMB Character;
        
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

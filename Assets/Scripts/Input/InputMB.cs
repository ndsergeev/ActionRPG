
using UnityEngine.Serialization;

namespace Main.Input
{
    using UnityEngine;
    
    public abstract class InputMB : MonoBehaviour
    {
        [SerializeField]
        protected InputReaderSO  inputReaderSO;
        public InputReaderSO InputReaderSO => inputReaderSO;
        
        
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

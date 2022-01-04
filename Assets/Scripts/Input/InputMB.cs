
namespace Main.Input
{
    using UnityEngine;
    
    public abstract class InputMB : MonoBehaviour
    {
        [SerializeField]
        protected InputReaderSO  InputReaderSO;
        
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

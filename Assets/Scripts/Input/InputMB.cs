using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;

using ScriptableObjects;

namespace Main.Input
{
    public class InputMB : MonoBehaviour
    {
        [SerializeField]
        private InputReaderSO  m_InputReaderSO;
        
        private void OnEnable()
        {
            SubscribeToInputManager();
        }

        public void OnDisable()
        {
            UnsubscribeFromInputManager();
        }

        private void SubscribeToInputManager()
        {
            InputManagerMB.instance.SubscribeInputReader((this));
        }

        private void UnsubscribeFromInputManager()
        {
            InputManagerMB.instance.UnsubscribeInputReader((this));
        }
    }
}


using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.InputControls
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public class InputMB : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Vector2> walkEvent;
        
        private void OnEnable()
        {
            SubscribeToWalkInput();
        }

        public void OnDisable()
        {
            UnsubscribeFromWalkInput();
        }

        private void SubscribeToWalkInput()
        {
            InputEvents.moveEvents.Add(walkEvent);
        }

        private void UnsubscribeFromWalkInput()
        {
            InputEvents.moveEvents.Remove((walkEvent));
        }
    }
}

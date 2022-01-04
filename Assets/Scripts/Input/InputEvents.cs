

namespace Main.Input
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    
    public static class InputEvents
    {
        public static List<UnityEvent<Vector2>> moveEvents = new List<UnityEvent<Vector2>>();
        
        public static void TriggerMoveEvents(Vector2 moveInput)
        {
            for (int i = 0; i < moveEvents.Count; i++)
            {
                moveEvents[i]?.Invoke(moveInput);
            }
        } 
    }
}
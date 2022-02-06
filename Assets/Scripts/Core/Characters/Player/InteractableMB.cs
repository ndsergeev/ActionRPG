
namespace Main
{
    using UnityEngine;
    using UnityEngine.Events;

    using Main.Core.Updates;
    
    public enum Interactions // TODO: these things better to be implemented with interfaces or polymorphism
    {
        Talk,
        Read,
        Open,
        Close,
        Climb,
        Activate,
        Press,
    }
    
    public class InteractableMB : Refresh
    {
        [Space]
        [SerializeField]
        protected Interactions m_Interaction;

        public Interactions Interaction => m_Interaction;

        
        [Space]
        [SerializeField]
        protected UnityEvent m_OnInteractEvent;

        public void Interact()
        {
            m_OnInteractEvent?.Invoke();
        }
    }
}

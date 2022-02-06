namespace Main
{
    using UnityEngine;

    using Main.Core.Updates;
    
    public class PlayerInteractionsMB : Refresh
    {
        protected Interactions m_AvailableInteraction;
        
        public void OnCollisionEnter(Collision collision)
        {
            var interactable = collision.gameObject.GetComponent<InteractableMB>();

            m_AvailableInteraction = interactable.Interaction;

            switch (m_AvailableInteraction)
            {
                case Interactions.Talk:
                    OfferTalkInteraction();
                    break;
            }
        }

        protected void OfferTalkInteraction()
        { }
    }
}

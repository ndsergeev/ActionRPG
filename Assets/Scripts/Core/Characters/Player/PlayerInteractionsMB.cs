namespace Main
{
    using UnityEngine;

    using Main.Core.Updates;
    
    public class PlayerInteractionsMB : Refresh
    {
        protected Interactions availableInteraction;
        
        public void OnCollisionEnter(Collision col)
        {
            InteractableMB interactable = col.gameObject.GetComponent<InteractableMB>();

            availableInteraction = interactable.interaction;

            switch (availableInteraction)
            {
                case Interactions.Talk:
                    OfferTalkInteraction();
                    break;
            }
        }

        protected void OfferTalkInteraction()
        {
              
        }
    }
}

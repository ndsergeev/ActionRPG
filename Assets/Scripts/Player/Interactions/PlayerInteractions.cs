using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{

    bool showingInteraction;

    Interaction currentInteraction;

    private void OnEnable()
    {
        PlayerEvents.InteractEvent += Interact;

        PlayerEvents.EndDialogueEvent += DisplayInteraction;
    }

    private void OnDisable()
    {
        PlayerEvents.InteractEvent -= Interact;

        PlayerEvents.EndDialogueEvent -= DisplayInteraction;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interaction>())
        {
            currentInteraction = other.GetComponent<Interaction>();

            DisplayInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!showingInteraction) return;

        if (other.GetComponent<Interaction>())
        {
            HideInteraction();
        }
    }

    void DisplayInteraction()
    {
        if (!currentInteraction) return;

        print("DISPLAY");
        showingInteraction = true;

        if (currentInteraction.interactionType == Interaction.interactionTypes.talk)
        {
            InteractionUI.Singleton.ShowTalkPopUp();
        }
    }

    void HideInteraction()
    {
        InteractionUI.Singleton.HidePopUp();

        showingInteraction = false;

        currentInteraction = null;
    }

    void Interact()
    {
        if (!showingInteraction) return;

        if (currentInteraction.interactionType == Interaction.interactionTypes.talk)
        {
            StartDialogueInteraction();
        }
    }

    void StartDialogueInteraction()
    {
        currentInteraction.GetComponent<DialogueTrigger>().TriggerDialogue();

        showingInteraction = false;

        //HideInteraction();

    }
}

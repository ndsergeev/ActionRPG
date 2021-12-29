using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    // Event for player jump
    public delegate void Jump();
    public static event Jump JumpEvent;

    public static void TriggerJumpEvent()
    {
        // If event has subscribers, run event
        JumpEvent?.Invoke();
    }

    // Event for player dash
    public delegate void Dash();
    public static event Dash DashEvent;

    public static void TriggerDashEvent()
    {
        // If event has subscribers, run event
        DashEvent?.Invoke();
    }

    // Event for player attack
    public delegate void Attack();
    public static event Attack AttackEvent;

    public static void TriggerAttackEvent()
    {
        // If event has subscribers, run event
        AttackEvent?.Invoke();
    }

    // Event for player interaction
    public delegate void Interact();
    public static event Interact InteractEvent;

    public static void TriggerInteractEvent()
    {
        // If event has subscribers, run event
        InteractEvent?.Invoke();
    }

    // Event for starting dialogue
    public delegate void StartDialogue();
    public static event StartDialogue StartDialogueEvent;

    public static void TriggerStartDialogueEvent()
    {
        // If event has subscribers, run event
        StartDialogueEvent?.Invoke();
    }

    // Event for ending dialogue
    public delegate void EndDialogue();
    public static event EndDialogue EndDialogueEvent;

    public static void TriggerEndDialogueEvent()
    {
        // If event has subscribers, run event
        EndDialogueEvent?.Invoke();
    }

    // Event for next dialogue
    public delegate void NextDialogue();
    public static event NextDialogue NextDialogueEvent;

    public static void TriggerNextDialogueEvent()
    {
        // If event has subscribers, run event
        NextDialogueEvent?.Invoke();
    }

    // Event for input device change
    public delegate void InputDeviceChange();
    public static event InputDeviceChange InputDeviceChangeEvent;

    public static void TriggerInputDeviceChangeEvent()
    {
        // If event has subscribers, run event
        InputDeviceChangeEvent?.Invoke();
    }

    // Event for pause menu button
    public delegate void PauseMenuButton();
    public static event PauseMenuButton PauseMenuButtonEvent;

    public static void TriggerPauseMenuButtonEvent()
    {
        // If event has subscribers, run event
        PauseMenuButtonEvent?.Invoke();
    }

    // Event for unlocking mask
    public delegate void UnlockMask();
    public static event UnlockMask UnlockMaskEvent;

    public static void TriggerUnlockMaskEvent()
    {
        // If event has subscribers, run event
        UnlockMaskEvent?.Invoke();
    }

    // Event for player dying
    public delegate void PlayerDeath();
    public static event UnlockMask PlayerDeathEvent;

    public static void TriggerPlayerDeathEvent()
    {
        // If event has subscribers, run event
        PlayerDeathEvent?.Invoke();
    }

    // Event for player revived
    public delegate void PlayerRevived();
    public static event UnlockMask PlayerRevivedEvent;

    public static void TriggerPlayerRevivedEvent()
    {
        // If event has subscribers, run event
        PlayerRevivedEvent?.Invoke();
    }

    // Event for player targeting toggle
    public delegate void ToggleTargeting();
    public static event ToggleTargeting ToggleTargetingEvent;

    public static void TriggerToggleTargetingEvent()
    {
        // If event has subscribers, run event
        ToggleTargetingEvent?.Invoke();
    }
}

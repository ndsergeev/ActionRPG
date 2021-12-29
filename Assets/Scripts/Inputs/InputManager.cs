using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    [HideInInspector]
    public static InputManager Singleton;

    [HideInInspector]
    //public Player player;

    [SerializeField]
    public PlayerControls playerControls;

    string currentDevice;



    Vector2 move;
    [SerializeField] bool isMoveFrozen;

    Vector2 look;

    bool jump;
    bool isJumpFrozen;

    bool isAttackFrozen;

    bool dash;

    private void Awake()
    {
        // Only one InputManager in the game
        // Make it accessible by all
        Singleton = this;

        // This is the Input Action asset
        // It contains all the information related to the player inputs,
        // is what calls actions (Ex. The 'Move' action calls the 
        // OnMove() function on this script, and passes the InputAction.CallbackContext,
        // which contains the information related to the move input.
        playerControls = new PlayerControls();

        // 'Player' is the 'Action Map' on the 'PlayerControls' Input Action asset 
        // Set the input system to call methods on this script
        playerControls.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        playerControls.Enable();

        ///EventManager.PlayerHasSpawnedEvent += EnableInputs;
        ///

    }

    void OnDisable()
    {
        playerControls.Disable();


        ///EventManager.PlayerHasSpawnedEvent -= EnableInputs;
        ///
    }

    void Start()
    {
        DisableInputs();
    }

    public void EnableInputs()
    {
        playerControls.Enable();
    }

    public void DisableInputs()
    {
        playerControls.Disable();
    }

    public void SetCurrentDevice(string device)
    {
        currentDevice = device;
    }

    public string GetCurrentDevice()
    {
        return currentDevice;
    }


    // Left Joy-Stick // WASD keys // Arrow keys
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput()
    {
        if (isMoveFrozen) move = Vector2.zero;

        return move;
    }

    public void FreezeMoveInput()
    {
        isMoveFrozen = true;
    }

    public void UnFreezeMoveInput()
    {
        isMoveFrozen = false;
    }

    // Right Joy-Stick // Mouse
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public Vector2 GetLookInput()
    {
        return look;
    }

    // Space key
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isJumpFrozen) return;
        
        if (context.started)
        {
            jump = true;
            PlayerEvents.TriggerJumpEvent();
        }
        else if (context.canceled)
        {
            jump = false;
        }
    }

    public bool GetJumpInput()
    {
        if (isJumpFrozen) jump = false;

        return jump;
    }

    public void TurnOffJumpInput()
    {
        jump = false;
    }

    public void FreezeJumpInput()
    {
        isJumpFrozen = true;
    }

    public void UnFreezeJumpInput()
    {
        isJumpFrozen = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dash = true;
            PlayerEvents.TriggerDashEvent();
        }
        else if (context.canceled)
        {
            dash = false;
        }
    }

    // Right Trigger // F key
    public void OnFire(InputAction.CallbackContext context)
    {
        if (isAttackFrozen) return;

        if (context.started)
        {
            PlayerEvents.TriggerAttackEvent();
        }
    }

    public void FreezeAttackInput()
    {
        isAttackFrozen = true;
    }

    public void UnFreezeAttackInput()
    {
        isAttackFrozen = false;
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerEvents.TriggerPauseMenuButtonEvent();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerEvents.TriggerInteractEvent();
        }
    }

    public void OnNextDialogue(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerEvents.TriggerNextDialogueEvent();
        }
    }

    public void OnToggleTargeting(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            PlayerEvents.TriggerToggleTargetingEvent();
        }
    }
}

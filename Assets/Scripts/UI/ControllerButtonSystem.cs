using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControllerButtonSystem : MonoBehaviour
{
    [SerializeField]
    List<Button> buttons = new List<Button>();

    [SerializeField]
    Button currentButton;

    bool isActivated;

    bool isInputLeft;
    bool isInputUp;

    float delayLength = 0.5f;
    float currentTime;
    bool isTimerActive;

    enum HorizontalDirection
    {
        None,
        Left,
        Right,
    }

    HorizontalDirection horDir;
    enum VerticalDirection
    {
        None,
        Up,
        Down,
    }

    VerticalDirection verDir;

    private void OnEnable()
    {
        PlayerEvents.InputDeviceChangeEvent += HandleDeviceChange;
        PlayerEvents.NextDialogueEvent += PressButton;
    }

    private void OnDisable()
    {
        PlayerEvents.InputDeviceChangeEvent -= HandleDeviceChange;
        PlayerEvents.NextDialogueEvent -= PressButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Singleton.EnableInputs();

        currentButton.GetComponent<Animator>().SetTrigger("Highlighted");
        buttons.Remove(currentButton);
    }

    // Update is called once per frame
    void Update()
    {
        HandleDelay();
        //if(Keyboard.current.wKey.wasPressedThisFrame && Keyboard.current.aKey.wasPressedThisFrame
           // && Keyboard.current.sKey.wasPressedThisFrame && Keyboard.current.dKey.wasPressedThisFrame)
        //{ 
        if(!isTimerActive) HandleButtonSelection();
        //}
    }

    void PressButton()
    {
        currentButton.Select();
    }

    void HandleDeviceChange()
    {
        string currentDevice = InputManager.Singleton.GetCurrentDevice();

        if(currentDevice == "keyboard")
        {
            SetUpForKeyboard();
        }

        if (currentDevice == "gamepad")
        {
            SetUpForGamepad();
        }
    }

    void SetUpForKeyboard()
    {
        currentButton.GetComponent<Animator>().SetTrigger("Normal");
        isActivated = false;
    }

    void SetUpForGamepad()
    {
        currentButton.GetComponent<Animator>().SetTrigger("Highlighted");
        isActivated = true;
    }

    void HandleButtonSelection()
    {
        if (!isActivated) return;

        //Getting the direction of the input
        Vector2 moveInput = InputManager.Singleton.GetMoveInput();

        //Don't do anything if there is no directional input
        if (moveInput == Vector2.zero) return;

        //Setting the boolean values based on the direction of input
        //if (moveInput.x < 0) isInputLeft = true;
        //if (moveInput.y > 0) isInputUp = true;

        if (moveInput.x == 0) horDir = HorizontalDirection.None;
        if (moveInput.x < 0) horDir = HorizontalDirection.Left;
        if (moveInput.x > 0) horDir = HorizontalDirection.Right;

        if (moveInput.y == 0) verDir = VerticalDirection.None;
        if (moveInput.y > 0) verDir = VerticalDirection.Up;
        if (moveInput.y < 0) verDir = VerticalDirection.Down;

        //Make a new list to store the buttons in the direction of the input
        List<Button> buttonsInDirection = new List<Button>();

        foreach (Button button in buttons)
        {
            switch (horDir)
            {
                case HorizontalDirection.None:
                    //do nothing
                    /*if (verDir == VerticalDirection.None)
                    {
                        return;
                    }*/
                    break;
                case HorizontalDirection.Left:
                    //Check if button is to the left of the current button
                    if (button.transform.position.x <= currentButton.transform.position.x)
                    {
                        //print("button is to the left");
                        //button is to the left
                    }
                    else
                    {
                        //print(1);
                        //button isn't in input direction so do nothing
                        continue;
                    }
                    break;
                case HorizontalDirection.Right:
                    //Check if button is to the right of the current button
                    if (button.transform.position.x >= currentButton.transform.position.x)
                    {
                        //print("button is to the right");
                        //button is to the right
                    }
                    else
                    {
                        //print(2);
                        //button isn't in input direction so do nothing
                        continue;
                    }
                    break;
            }
            switch (verDir)
            {
                case VerticalDirection.None:
                    //if there is a left input check if button is left of current button
                    if (horDir == HorizontalDirection.Left)
                    {
                        //if button is to the left of the current button, then button is in input direction
                        if(button.transform.position.x < currentButton.transform.position.x)
                        {
                            buttonsInDirection.Add(button);
                        }
                    }
                    //if there is a right input check if button is right of current button
                    if (horDir == HorizontalDirection.Right)
                    {
                        //if button is to the right of the current button, then button is in input direction
                        if (button.transform.position.x > currentButton.transform.position.x)
                        {
                            buttonsInDirection.Add(button);
                        }
                    }
                    break;
                case VerticalDirection.Up:
                    //Check if button is above the current button
                    if (button.transform.position.y >= currentButton.transform.position.y)
                    {
                        //print("button above added");
                        //button is above
                        buttonsInDirection.Add(button);
                    }
                    else
                    {
                        //print(3);
                        //button isn't in input direction so do nothing
                        continue;
                    }
                    break;
                case VerticalDirection.Down:
                    //Check if button is below the current button
                    if (button.transform.position.y <= currentButton.transform.position.y)
                    {
                        //print("button below added");
                        //button is below
                        buttonsInDirection.Add(button);
                    }
                    else
                    {
                        //print(4);
                        //button isn't in input direction so do nothing
                        continue;
                    }
                    break;
            }
        }

        //print("buttons in direction " + buttonsInDirection.Count);

        //If there are no buttons in the input direction then return
        if (buttonsInDirection.Count == 0) return;

        Button currentClosestButton = buttonsInDirection[0];

        //find closest button of all buttons in input direction
        for (int i = 0; i < buttonsInDirection.Count; i++)
        {
            if (Vector2.Distance(currentButton.transform.position, currentClosestButton.transform.position)
                 > Vector2.Distance(currentButton.transform.position, buttonsInDirection[i].transform.position))
            {
                currentClosestButton = buttonsInDirection[i];
            }
        }

        ChangeCurrentButton(currentClosestButton);
        StartDelay();
    }


    void ChangeCurrentButton(Button button)
    {
        //print("changed button");

        //return previously selected button to normal state
        currentButton.GetComponent<Animator>().SetTrigger("Normal");

        buttons.Add(currentButton);

        //set currently selected button to new button
        currentButton = button;

        buttons.Remove(currentButton);

        //set new button to highlighted state
        currentButton.GetComponent<Animator>().SetTrigger("Highlighted");
    }

    void StartDelay()
    {
        isTimerActive = true;
        currentTime = 0f;
    }

    void HandleDelay()
    {
        if (!isTimerActive) return;

        currentTime += Time.deltaTime;

        if (currentTime >= delayLength)
        {
            isTimerActive = false;
        }
    }
}

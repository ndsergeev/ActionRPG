using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RequiredForPlay : MonoBehaviour
{
    // Input Manager
    [SerializeField]
    private GameObject inputManagerPrefab;
    private GameObject inputManagerObject;

    // Dialogue System
    [SerializeField]
    private GameObject dialogueManagerPrefab;
    private GameObject dialogueSystem;

    // Interaction UI
    [SerializeField]
    private GameObject interactionUIPrefab;
    private GameObject interactionUI;

    //BlackScreen
    [SerializeField]
    private GameObject blackScreenUIPrefab;
    private GameObject blackScreenUI;

    //Pause Menu UI
    [SerializeField]
    private GameObject pauseMenuUIPrefab;
    
    // In game HUD UI
    [SerializeField]
    private GameObject hudUIPrefab;
    private GameObject hudUI;

    // Start is called before the first frame update
    private void Awake()
    {
        SpawnRequiredObjects();
    }

    private void SpawnRequiredObjects()
    {
        
    }

    private void SpawnInputManager()
    {
        if (InputManager.Singleton == null)
        {
            
        }
    }

}

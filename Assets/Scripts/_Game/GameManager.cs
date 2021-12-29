using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepareToStartGame()
    {
        Cursor.visible = false;

        BlackScreen.AfterFadeCallback method = StartGame;
        BlackScreen.Singleton.FadeToBlack(method);
    }

    void StartGame()
    {
        InputManager.Singleton.EnableInputs();
        //StartScreenUI.Singleton.CloseStartScreen();

        LevelLoader.Singleton.LoadLevel(1);

        BlackScreen.Singleton.FadeFromBlack();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

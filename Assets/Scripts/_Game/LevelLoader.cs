using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Singleton;

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    public bool isLoading;

    private void Awake()
    {
        Singleton = this;
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));

        isLoading = true;
        
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }

        if (operation.isDone)
        {
            HideLoadingScreen();
            StopCoroutine("LoadAsynchronously");
        }
    }

    void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}

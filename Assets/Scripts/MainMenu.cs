using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private FadeTransition transitionScript;

    private void Awake()
    {
        transitionScript = GetComponent<FadeTransition>();
    }

    public void Play()
    {
        StartSceneTransition("CharacterSelect");
    }

    public void StartGame()
    {
        StartSceneTransition("MainScene");
    }

    public void GoToMenu()
    {
        StartSceneTransition("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

    private void StartSceneTransition(string sceneName)
    {
        StartCoroutine(transitionScript.FadeAndLoadScene(FadeTransition.FadeDirection.In, sceneName));
    }
}

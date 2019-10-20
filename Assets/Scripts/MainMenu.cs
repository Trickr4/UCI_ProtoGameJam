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
        //SceneManager.LoadScene("CharacterSelect");
        StartCoroutine(transitionScript.FadeAndLoadScene(FadeTransition.FadeDirection.In, "CharacterSelect"));
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

    
}

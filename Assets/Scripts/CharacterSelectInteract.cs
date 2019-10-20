using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectInteract : MonoBehaviour
{
    [SerializeField] GameObject playerOneOptions;
    [SerializeField] GameObject playerTwoOptions;

    [SerializeField] AudioClip characterSelectSFX;
    private AudioSource sourceSFX;

    [SerializeField] float readyIndentDistance = 2;

    private int playerOneIndex = 0;
    private int playerTwoIndex = 0;
    private int charCount = 4;

    private bool playerOneSelected = false;
    private bool playerTwoSelected = false;

    private FadeTransition transitionScript;

    void Awake()
    {
        // Get number of characters available
        charCount = playerOneOptions.transform.childCount;

        // Fade in transition
        FadeIn(); 

        // Initially display both player 1 and player 2 brackets 
        // at index = 0. 
        DisplayCurrBracket(true);
        DisplayCurrBracket(false);

        //AudioSource
        sourceSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        MoveSelections();
        CheckReady(); 
    }

    private void FadeIn()
    {
        transitionScript = GetComponent<FadeTransition>();
        StartCoroutine(transitionScript.Fade(FadeTransition.FadeDirection.Out));
    }

    private void MoveSelections()
    {
        if (!playerOneSelected && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)))
        {
            sourceSFX.PlayOneShot(characterSelectSFX, 1f);
            HideCurrBracket(true);

            if (Input.GetKeyDown(KeyCode.W))
                playerOneIndex--;
            else if (Input.GetKeyDown(KeyCode.S))
                playerOneIndex++;

            playerOneIndex = (playerOneIndex + charCount) % charCount;
            DisplayCurrBracket(true);
        }
        else if (!playerTwoSelected && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            sourceSFX.PlayOneShot(characterSelectSFX, 1f);  
            HideCurrBracket(false);

            if (Input.GetKeyDown(KeyCode.UpArrow))
                playerTwoIndex--;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                playerTwoIndex++;

            playerTwoIndex = (playerTwoIndex + charCount) % charCount;
            DisplayCurrBracket(false);
        }
    }

    private void CheckReady()
    {
        // Start the game if both players are ready. 
        if (playerOneSelected && playerTwoSelected)
        {
            PlayerPrefs.SetInt("PlayerOneChar", playerOneIndex);
            PlayerPrefs.SetInt("PlayerTwoChar", playerTwoIndex);

            StartCoroutine(transitionScript.FadeAndLoadScene(FadeTransition.FadeDirection.In, "MainScene"));
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            bool prevReady = playerOneSelected;
            playerOneSelected = Input.GetKeyDown(KeyCode.D);
            ChangeReadyStatus(true, playerOneSelected, prevReady);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool prevReady = playerTwoSelected;
            playerTwoSelected = Input.GetKeyDown(KeyCode.LeftArrow);
            ChangeReadyStatus(false, playerTwoSelected, prevReady);
        }
    }

    // HELPER FUNCTIONS
    // --------------------------------------------------------------------
    private GameObject GetHoveringChar(bool p1)
    {
        if (p1) 
            return playerOneOptions.transform.GetChild(playerOneIndex).gameObject;
        else 
            return playerTwoOptions.transform.GetChild(playerTwoIndex).gameObject;
    }

    private void HideCurrBracket(bool p1)
    {
        GameObject player = GetHoveringChar(p1);
        ChangeCharActive(player, false);
    }

    private void DisplayCurrBracket(bool p1)
    {
        GameObject player = GetHoveringChar(p1);
        ChangeCharActive(player, true);
    }

    private void ChangeCharActive(GameObject player, bool active)
    {
        player.transform.GetChild(0).gameObject.SetActive(active);
    }

    private void ChangeReadyStatus(bool playerOne, bool readied, bool prevReadyStatus)
    {
        GameObject selected = GetHoveringChar(playerOne);
        float indent = readyIndentDistance;
        if ((!playerOne && readied) || (playerOne && !readied))
            indent *= -1;

        if (readied != prevReadyStatus) 
            selected.transform.position = new Vector2(selected.transform.position.x + indent, selected.transform.position.y);

        if (readied)
            HideCurrBracket(playerOne);
        else
            DisplayCurrBracket(playerOne);
    }
}

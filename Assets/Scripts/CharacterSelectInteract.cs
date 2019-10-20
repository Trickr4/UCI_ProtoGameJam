using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectInteract : MonoBehaviour
{
    [SerializeField] GameObject SwordClass;
    [SerializeField] GameObject NunchuckClass;
    [SerializeField] GameObject ShieldClass;
    [SerializeField] GameObject AxeClass;

    private readonly string selectedCharacter = "SelectedCharacter";

    public int playerOneIndex = 0;
    public int playerTwoIndex = 0;

    List<GameObject> Classes = new List<GameObject>();

    void Awake()
    {
        Classes.Add(SwordClass); //0
        Classes.Add(NunchuckClass); //1
        Classes.Add(ShieldClass); //2
        Classes.Add(AxeClass); //3
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScene");
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerOneIndex--;
            playerOneIndex = playerOneIndex % 4;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerOneIndex++;
            playerOneIndex = playerOneIndex % 4;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerTwoIndex--;
            playerTwoIndex = playerTwoIndex % 4;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerTwoIndex--;
            playerTwoIndex = playerTwoIndex % 4;
        }



    }
}

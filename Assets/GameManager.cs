using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int maxRound = 3;

    private int round = 1;
    private int playerOneScore = 0;
    private int playerTwoScore = 0;
    
    [SerializeField] int maxHealth = 100;

    private int playerOneHealth;
    private int playerTwoHealth;

    void Awake()
    {
        playerOneHealth = maxHealth;
        playerTwoHealth = maxHealth;
    }

    private void Update()
    {
        CheckWinner();
    }

    private void CheckGameOver()
    {
        int scoreToWin = (int)Mathf.Ceil(maxRound / 2);
        if (playerOneScore == scoreToWin || playerTwoScore == scoreToWin)
        {
            GameOver();
        }
        else
        {
            StartNewRound();
        }
    }

    private void CheckWinner()
    {
        if (playerOneHealth == 0)
        {
            playerTwoScore++;
            CheckGameOver();
        }
        if (playerTwoHealth == 0)
        {
            playerOneScore++;
            CheckGameOver();
        }
    }

    private void StartNewRound()
    {
        round++;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainScene");
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        //deal with ui stuff lol
    }

    //Getters
    public int getPlayerOneHealth()
    {
        return playerOneHealth;    
    }

    public int getPlayerTwoHealth()
    {
        return playerTwoHealth;
    }
}

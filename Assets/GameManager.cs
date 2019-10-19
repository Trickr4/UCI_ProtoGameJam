using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Round info 
    [SerializeField] int maxRound = 3;

    private int round = 1;
    private int playerOneScore = 0;
    private int playerTwoScore = 0;
    
    // Health info
    [SerializeField] int maxHealth = 100;

    private int playerOneHealth;
    private int playerTwoHealth;

    // Events for decreasing players healths
    private class IntEvent : UnityEvent<int> { };

    private IntEvent playerOneDamagedEvent;
    private IntEvent playerTwoDamagedEvent;

    private void Awake()
    {
        if (playerOneDamagedEvent == null)
            playerOneDamagedEvent = new IntEvent();
        if (playerTwoDamagedEvent == null)
            playerTwoDamagedEvent = new IntEvent();

        playerOneDamagedEvent.AddListener(DecPlayerOneHealth);
        playerTwoDamagedEvent.AddListener(DecPlayerTwoHealth);

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

    private void DecPlayerOneHealth(int damage)
    {
        playerOneHealth -= damage;
    }

    private void DecPlayerTwoHealth(int damage)
    {
        playerTwoHealth -= damage;
    }

    // Setters
    public void SetPlayerOneDamaged(int damage)
    {
        playerOneDamagedEvent.Invoke(damage);
    }

    public void SetPlayerTwoDamaged(int damage)
    {
        playerTwoDamagedEvent.Invoke(damage);
    }

    // Getters
    public int GetPlayerOneHealth()
    {
        return playerOneHealth;    
    }

    public int GetPlayerTwoHealth()
    {
        return playerTwoHealth;
    }
}

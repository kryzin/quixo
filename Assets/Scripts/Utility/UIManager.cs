using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text gameClock;
    public GameManager gameManager;
    public GameObject gameOverPopUp;
    public TMP_Text winner;
    public TMP_Text winMessage;
    public Board board;

    private float timer = 0.0f;
    private bool isTimer = false;

    void Start()
    {
        gameOverPopUp.SetActive(false);
        StartClock();
    }

    void Update()
    {
        if (isTimer )
        {
            timer += Time.deltaTime;
            DisplayClock();
        }
    }

    void DisplayWinner()
    {
        winner.text = "WIN FOR: " + gameManager.winner.ToString(); 
    }

    void DisplayEndMessage(int i)
    {
        if (i == 1)
        {
            winMessage.text = "You got 5 in a row!";
        }
        else
        {
            if (board.lostWin) { winMessage.text = "Next time try to not complete your oponent's line!"; }
            else { winMessage.text = "Wow! Double winner!"; }
        }
    }

    void DisplayClock()
    {
        int minutes = Mathf.FloorToInt(timer / 60.0f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        gameClock.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartClock()
    {
        isTimer = true;
    }

    public void StopClock()
    {
        isTimer = false;
    }

    public void ShowEndPopUp(int i)
    {
        gameOverPopUp.SetActive(true);
        DisplayWinner();
        DisplayEndMessage(i);
    }

    public void ResetGame() // use for new round also
    {
        timer = 0.0f;
        SceneManager.LoadScene(1);
    }

    public void PauseGame()
    {
        isTimer = !isTimer;
        // pause pop up
        // change text on button to play/pause
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

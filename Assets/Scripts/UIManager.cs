using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text playerDisplayText;
    public TMP_Text gameClock;
    public GameManager gameManager;

    private float timer = 0.0f;
    private bool isTimer = false;

    void Start()
    {
        StartClock();
    }

    // Update is called once per frame
    void Update()
    {
        playerDisplayText.text = gameManager.currentPlayer.ToString();

        if (isTimer )
        {
            timer += Time.deltaTime;
            DisplayClock();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameManager gameManager;
    public GameManager.Symbol playerSymbol;

    void Start()
    {
        playerSymbol = Random.Range(0, 2) == 0 ? GameManager.Symbol.O : GameManager.Symbol.X; //generate random symbol
    }

    public bool IsPlayerTurn()
    {
        if (gameManager.currentPlayer == playerSymbol)
        {
            return true;
        }
        return false;
    }
}

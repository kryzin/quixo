using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int k; // for transition between coordinates in board[] and at gameScene
    public static Cube[,] board = new Cube[5, 5];
    public bool playerTurn; // true for O, false for X
    public bool isEnd;
    public GameObject cubePrefab;
    private bool winner;

    void Start()
    {
        // set board to be empty
        playerTurn = true;
        isEnd = false;
        SetUpBoard();
    }

    void Update()
    {
        // maybe limit to checking only after new move
        IsGameOver(); // checking if anyone won
        if (isEnd)
        {
            Debug.Log("GameOver, Winner: " + winner);
        }
    }

    private void IsGameOver()
    {
        // also add maybe a list/dictionary that saves winning f.e. winning = [(row, 0, symbol), (column, 3, symbol)] 
        // means that both row 0 and column 3 are full of the same symbols
        int winning = 0;
        // EDGE CASE: more than one full of different symbols
        // the game is over if any row/column/diagonal is full of the same symbol

        // checking all rows and all columns
        for (int i = 0; i < 5; i++)
        {
            int countRows = 0;
            int countCols = 0;
            for (int j = 0; j < 5; j++)
            {
                if (board[i, j].symbol == 'O') { countRows++; }
                else { countRows--; }

                if (board[j, i].symbol == 'O') { countCols++; }
                else { countCols--; }
            }
            if (countRows == 5 || countRows == -5) { winning++; }
            if (countCols == 5 || countCols == -5) { winning++; }
        }

        // checking diagonal and anti-diagonal
        for (int i = 0; i < 5; i++)
        {
            int j = 4 - i;
            int count = 0;
            int countAnti = 0;

            if (board[i, j].symbol == 'O') { count++; }
            else { count--; }

            if (board[i, i].symbol == 'O') { countAnti++; }
            else { countAnti--; }

            if (count == 5 || count == -5) { winning++; }
            if (countAnti == 5 || countAnti == -5) { winning++; }
        }

        if (winning > 0)
        {
            isEnd = true;
            winner = !playerTurn;
            // add handling the edge case: winning is > 1
            // if all winning lines symbol == currentPlayer symbol -> this player wins
            // but if even one line is of opposite symbol -> the opponent wins
        }
    }

    void SetUpBoard ()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                GameObject cubeObject = Instantiate(cubePrefab, new Vector2(i, j), Quaternion.identity);
                Cube cube = cubeObject.AddComponent<Cube>();

                cube.boardX = i;
                cube.boardY = j;

                board[i, j] = cube;
            }
        }
    }
}

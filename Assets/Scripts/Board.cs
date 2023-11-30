using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
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
        isEnd = false;
    }

    void Update()
    {
        // maybe limit to checking only after new move
        // IsGameOver(); // checking if anyone won
        if (isEnd)
        {
            Debug.Log("GameOver, Winner: " + winner);
        }
    }

    void IsGameOver()
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
                if (board[i, j].symbol == "O") { countRows++; }
                else { countRows--; }

                if (board[j, i].symbol == "O") { countCols++; }
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

            if (board[i, j].symbol == "O") { count++; }
            else { count--; }

            if (board[i, i].symbol == "O") { countAnti++; }
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

    public void SetUpBoard ()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                string cubeName = "Cube" + (char)('A' + i * 5 + j);
                Cube cube = GameObject.Find(cubeName)?.GetComponent<Cube>();

                cube.symbol = "";
                cube.boardX = i;
                cube.boardY = j;

                board[i, j] = cube;
                Debug.Log(cubeName + " set at board[" + i + ", " + j + "]");
            }
        }
    }

    public Cube GetCubeAt(int x, int y)
    {
        if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1))
        {
            return board[x, y];
        }
        else
        {
            Debug.LogError($"Invalid indices: ({x}, {y})");
            return null;
        }
    }

    public void MoveOnBoard(bool isRow, int index, bool goingLeft)
    {
        // or do (bool isRow, int index, bool left)
        // and change all at once, f.e. [2,4] -> [2,0] means (true{cause x=x}, 2, false{cause y1>y2})

        if (isRow)
        {
            if (goingLeft)//cubes [index,1-4] going left(y-1) and cube [index,0] going to [index,4]
            {
                Cube activeCube = board[index, 0];
                for (int i = 0; i < 4; i++)
                {
                    board[index, i] = board[index, i + 1];
                }
                board[index, 4] = activeCube;
            }
            else//cubes [index,3-0] going right(y+1) and cube [index,4] going to [index,0]
            {
                Cube activeCube = board[index, 4];
                for (int i = 4; i > 0; i--)
                {
                    board[index, i] = board[index, i - 1];
                }
                board[index, 0] = activeCube;
            }
        }
        else
        {
            if (goingLeft)//cubes [3-0,index] going down(x+1) and [4,index] goes to [0,index]
            {
                Cube activeCube = board[4, index];
                for (int i = 4; i > 0; i--)
                {
                    board[i, index] = board[i - 1, index];
                }
                board[0, index] = activeCube;
            }
            else//cubes [4-1,index] go up(x-1) and [0,index] goes to [4,index]
            {
                Cube activeCube = board[0, index];
                for (int i = 0; i < 4; i++)
                {
                    board[i, index] = board[i + 1, index];
                }
                board[4, index] = activeCube;
            }
        }
        for (int i = 0; i <= 4; i++) // updating idexes in Cube attributes
        {
            for (int j = 0; j <= 4; j++)
            {
                board[i, j].boardX = i;
                board[i, j].boardY = j;
                board[i, j].cubeRenderer.sortingOrder = i + j;
            }
        }

    }
}

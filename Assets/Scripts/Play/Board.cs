using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int k; // for transition between coordinates in board[] and at gameScene
    public static Cube[,] board = new Cube[5, 5];
    public bool isEnd;
    public GameManager gameManager;
    public GameManager.Symbol winner;
    private bool checkOnce = false;
    private List<GameManager.Symbol> winSymbols = new List<GameManager.Symbol>
        {
            GameManager.Symbol.O,
            GameManager.Symbol.X
        };

    void Start()
    {
        // set board to be empty
        isEnd = false;
    }

    void Update()
    {
        IsGameOver();
        // maybe limit to checking only after new move
        // IsGameOver(); // checking if anyone won
        if (isEnd && !checkOnce)
        {
            Debug.LogError("GameOver, Winner: " + winner);
            checkOnce = true;
        }
    }

    void IsGameOver()
    {
        Dictionary<(string, int), GameManager.Symbol> winningLines = new Dictionary<(string, int), GameManager.Symbol>();
        int winning = 0;
        // EDGE CASE: more than one full of different symbols
        // the game is over if any row/column/diagonal is full of the same symbol
        
        // checking all rows and all columns

        foreach (GameManager.Symbol symbol in winSymbols)
        {
            //check rows and columns
            for (int i = 0; i < 5; i++)
            {
                int countRow = 0;
                int countCol = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (board[i,j].symbol == symbol) { countRow++; }
                    if (board[j,i].symbol == symbol) { countCol++; }
                }
                if (countRow == 5)
                {
                    winning++;
                    winningLines.Add(("row", i), symbol);
                }
                if (countCol == 5)
                {
                    winning++;
                    winningLines.Add(("column", i), symbol);
                }
            }

            // check diagonal
            int countDia = 0;
            for (int i = 0; i < 5; i++)
            {
                if (board[i,i].symbol == symbol) { countDia++; }
            }
            if (countDia == 5)
            {
                winning++;
                winningLines.Add(("diagonal", 1), symbol);
            }

            // check anti-diagonal
            int countAnti = 0;
            for (int i = 0; i < 5; i++)
            {
                int j = 4 - i;
                if (board[i, j].symbol == symbol) { countDia++; }
            }
            if (countAnti == 5)
            {
                winning++;
                winningLines.Add(("diagonal", 0), symbol);
            }
        }

        if (winning == 1)
        {
            isEnd = true;
            if (gameManager.currentPlayer == GameManager.Symbol.X) { winner = GameManager.Symbol.O; }
            else { winner = GameManager.Symbol.X; }
        }
        else if (winning > 1)
        {
            // if all winning lines symbols == currentPlayer symbol -> this player wins
            GameManager.Symbol sampleSymbol = winningLines.Values.FirstOrDefault();
            if (winningLines.Values.All(symbol => symbol == sampleSymbol))
            {
                if (gameManager.currentPlayer == GameManager.Symbol.X) { winner = GameManager.Symbol.O; }
                else { winner = GameManager.Symbol.X; }
            }
            // but if even one line is of opposite symbol -> the opponent wins
            else
            {
                winner = gameManager.currentPlayer;
            }
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

                cube.symbol = GameManager.Symbol.Null;
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

    public void MoveOnBoard(bool isRow, int indexX, int indexY, bool goingLeft)
    {
        // change all at once, f.e. [2,4] -> [2,0] means (true{cause x=x}, 2, false{cause y1>y2})

        if (isRow)
        {
            if (goingLeft)//cubes [index,1-4] going left(y-1) and cube [index,0] going to [index,4]
            {
                Cube activeCube = board[indexX, indexY];
                for (int i = indexY; i < 4; i++)
                {
                    board[indexX, i] = board[indexX, i + 1];
                }
                board[indexX, 4] = activeCube;
            }
            else//cubes [index,3-0] going right(y+1) and cube [index,4] going to [index,0]
            {
                Cube activeCube = board[indexX, indexY];
                for (int i = indexY; i > 0; i--)
                {
                    board[indexX, i] = board[indexX, i - 1];
                }
                board[indexX, 0] = activeCube;
            }
        }
        else
        {
            if (goingLeft)//cubes [3-0,index] going down(x+1) and [4,index] goes to [0,index]
            {
                Cube activeCube = board[indexX, indexY];
                for (int i = indexX; i > 0; i--)
                {
                    board[i, indexY] = board[i - 1, indexY];
                }
                board[0, indexY] = activeCube;
            }
            else//cubes [4-1,index] go up(x-1) and [0,index] goes to [4,index]
            {
                Cube activeCube = board[indexX, indexY];
                for (int i = indexX; i < 4; i++)
                {
                    board[i, indexY] = board[i + 1, indexY];
                }
                board[4, indexY] = activeCube;
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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Symbol { Null, X, O };
    public Symbol currentPlayer;
    public Board board;
    public Symbol winner;
    public UIManager uiManager;
    public RandomEnemy enemy;
    private bool startOnce = false;
    public bool isPaused = false;
    public bool isEnemyMove = false;

    public Cube selectedCube;
    public Cube placeCube;
    public List<int[]> possibleMove;

    public Symbol playerSymbol;

    void Start()
    {
        possibleMove = new List<int[]>();
        Debug.Log("X starts the game");
        currentPlayer = Symbol.X;
        playerSymbol = Random.Range(0, 2) == 0 ? Symbol.O : Symbol.X; //generate random symbol
        enemy.SetEnemySymbol();
        if (enemy.enemySymbol == currentPlayer) { isEnemyMove = true; }
        board.SetUpBoard();
    }

    void Update()
    {
        if (currentPlayer == enemy.enemySymbol && !startOnce && !board.isEnd)
        {
            isEnemyMove = true;
            StartCoroutine(DelayedEnemyMove());
            uiManager.DisablePause();
        }
    }

    IEnumerator DelayedEnemyMove()
    {
        startOnce = true;
        // Wait for 1 second (adjust the time as needed)
        yield return new WaitForSeconds(1f);

        // Now call the method to make the enemy move
        enemy.ChooseCube();
    }

    public bool IsPlayerTurn()
    {
        if (currentPlayer == playerSymbol)
        {
            return true;
        }
        return false;
    }

    public void Win(Symbol player, int i)
    {
        winner = player;
        uiManager.ShowEndPopUp(i);
    }

    public void SelectCube(Cube cube)
    {
        selectedCube = cube;
        cube.isSelected = true;
    }

    public void SetPlaceCube(Cube cube)
    {
        placeCube = cube;
    }

    public void UnsetPlaceCube()
    {
        placeCube.isPossiblePlacement = false;
        placeCube.ResetHighlight();
        placeCube = null;
    }

    public void DeselectCube()
    {
        selectedCube = null;
        ClearPlacementHighlight();
        //possibleMove.Clear();
    }

    public void SwitchTurn()
    {
        if (currentPlayer == Symbol.X)
        {
            currentPlayer = Symbol.O;
        }
        else currentPlayer = Symbol.X;
    }

    public void ShowMovesPlacement(Cube cube)
    {
        possibleMove = new List<int[]>();

        //input: cube
        //output: highlight where we can put the selected cube
        int row = cube.boardX;
        int col = cube.boardY;

        // possible: [x,0], [x,4], [0,y], [4,y]

        if (row == 0) possibleMove.Add(new int[] { 4, col });
        else if (row == 4) possibleMove.Add(new int[] { 0, col });
        else
        {
            possibleMove.Add(new int[] { 4, col });
            possibleMove.Add(new int[] { 0, col });
        }

        if (col == 0) possibleMove.Add(new int[] { row, 4 });
        else if (col == 4) possibleMove.Add(new int[] { row, 0 });
        else
        {
            possibleMove.Add(new int[] { row, 4 });
            possibleMove.Add(new int[] { row, 0 });
        }

        for (int i = 0; i < possibleMove.Count; i++)
        {
            int[] move = possibleMove[i];
            int x = move[0];
            int y = move[1];

            Cube placementCube = board.GetCubeAt(x, y);
            placementCube.HighlightSelection();
            placementCube.isPossiblePlacement = true;
        }
    }

    public void ClearPlacementHighlight()
    {
        foreach (int[] pair in possibleMove)
        {
            Cube highlightedCube = board.GetCubeAt(pair[0], pair[1]);
            highlightedCube.ResetHighlight();
        }
        possibleMove = new List<int[]>();
    }

    public void MakeMove()
    {
        int repeat = 1;
        // set in board and move the cubes around lol
        if (selectedCube.boardX == placeCube.boardX) // placed in the same row
        {
            if (selectedCube.boardY > placeCube.boardY) // placed on the left of the board - count cols down
            {
                int i = selectedCube.boardY;
                while (i > 0)
                {
                    Cube movingCube = board.GetCubeAt(selectedCube.boardX, i - 1);
                    movingCube.Move(1, -1, 1, false);
                    i--;
                }
                repeat = selectedCube.boardY - placeCube.boardY;
                selectedCube.Move(-1, 1, repeat, true);

                board.MoveOnBoard(true, selectedCube.boardX, selectedCube.boardY, false);
            }
            else
            {
                int i = selectedCube.boardY;
                while (i < 4)
                {
                    Cube movingCube = board.GetCubeAt(selectedCube.boardX, i + 1);
                    movingCube.Move(-1, 1, 1, false);
                    i++;
                }
                repeat = placeCube.boardY - selectedCube.boardY;
                selectedCube.Move(1, -1, repeat, true);

                board.MoveOnBoard(true, selectedCube.boardX, selectedCube.boardY, true);
            }
        }
        else if (selectedCube.boardY == placeCube.boardY)
        {
            if (selectedCube.boardX > placeCube.boardX) // placed on the left of the board - count cols down
            {
                int i = selectedCube.boardX;
                while (i > 0)
                {
                    Cube movingCube = board.GetCubeAt(i - 1, selectedCube.boardY);
                    movingCube.Move(-1, -1, 1, false);
                    i--;
                }
                repeat = selectedCube.boardX - placeCube.boardX;
                selectedCube.Move(1, 1, repeat, true);

                board.MoveOnBoard(false, selectedCube.boardX, selectedCube.boardY, true);
            }
            else
            {
                int i = selectedCube.boardX;
                while (i < 4)
                {
                    Cube movingCube = board.GetCubeAt(i + 1, selectedCube.boardY);
                    movingCube.Move(1, 1, 1, false);
                    i++;
                }
                repeat = placeCube.boardX - selectedCube.boardX;
                selectedCube.Move(-1, -1, repeat, true);

                board.MoveOnBoard(false, selectedCube.boardX, selectedCube.boardY, false);
            }
        }
        selectedCube.symbol = currentPlayer;
        selectedCube.isSelected = false;
        ResetAfterMove();
        SwitchTurn();
        startOnce = false;
    }

    public void ResetAfterMove()
    {
        DeselectCube();
        UnsetPlaceCube();
    }
}

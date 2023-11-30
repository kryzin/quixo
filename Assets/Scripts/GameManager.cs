using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Symbol { X, O };
    public Symbol currentPlayer;
    public string currentSymbol = "X";
    public Board board;

    public Cube selectedCube;
    public Cube placeCube;
    public List<int[]> possibleMove;

    void Start()
    {
        possibleMove = new List<int[]>();
        Debug.Log("X starts the game");
        board.SetUpBoard();
        currentPlayer = Symbol.X;
        currentSymbol = currentPlayer.ToString();
    }

    void Update()
    {
        
    }

    public void SelectCube(Cube cube)
    {
        selectedCube = cube;
        cube.isSelected = true;
    }

    public void SetPlaceCube(Cube cube)
    {
        placeCube = cube;
        Debug.LogError("setplacecube " + cube.name);
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
        possibleMove.Clear();
    }

    public void ShowMoves(Cube cube)
    {
        

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
        selectedCube.symbol = currentSymbol;
        // set in board and move the cubes around lol
        if (selectedCube.boardX == placeCube.boardX) // placed in the same row
        {
            if (selectedCube.boardY > placeCube.boardY) // placed on the left of the board - count cols down
            {
                int i = selectedCube.boardY;
                while (i > 0)
                {
                    Cube movingCube = board.GetCubeAt(selectedCube.boardX, i - 1);
                    movingCube.Move(1, -1, 1);
                    i--;
                }
                repeat = selectedCube.boardY - placeCube.boardY;
                selectedCube.Move(-1, 1, repeat);

                board.MoveOnBoard(true, selectedCube.boardX, false);
            }
            else
            {
                int i = selectedCube.boardY;
                while (i < 4)
                {
                    Cube movingCube = board.GetCubeAt(selectedCube.boardX, i + 1);
                    movingCube.Move(-1, 1, 1);
                    i++;
                }
                repeat = placeCube.boardY - selectedCube.boardY;
                selectedCube.Move(1, -1, repeat);

                board.MoveOnBoard(true, selectedCube.boardX, true);
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
                    movingCube.Move(-1, -1, 1);
                    i--;
                }
                repeat = selectedCube.boardX - placeCube.boardX;
                Debug.Log("repeat: " + repeat);
                selectedCube.Move(1, 1, repeat);

                board.MoveOnBoard(false, selectedCube.boardY, true);
            }
            else
            {
                int i = selectedCube.boardX;
                while (i < 4)
                {
                    Cube movingCube = board.GetCubeAt(i + 1, selectedCube.boardY);
                    movingCube.Move(1, 1, 1);
                    i++;
                }
                repeat = placeCube.boardX - selectedCube.boardX;
                selectedCube.Move(-1, -1, repeat);

                board.MoveOnBoard(false, selectedCube.boardY, false);
            }
        }
        ResetAfterMove();
    }

    public void ResetAfterMove()
    {
        DeselectCube();
        UnsetPlaceCube();
    }
}

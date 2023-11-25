using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Symbol { X, O };
    public Symbol currentPlayer;
    public string currentSymbol = "X";
    public Board board;

    public Cube selectedCube;

    void Start()
    {
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
    }

    public void DeselectCube()
    {
        selectedCube = null;
    }
}

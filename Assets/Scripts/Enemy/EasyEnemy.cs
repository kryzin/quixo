using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyEnemy : MonoBehaviour
{
    public GameManager gameManager;
    public Board board;
    public GameManager.Symbol enemySymbol;
    public UIManager uiManager;
    private List<int[]> availableCubes;

    void Start()
    {
        availableCubes = new List<int[]>();
    }

    void Update()
    {

    }

    public void SetEnemySymbol()
    {
        enemySymbol = gameManager.playerSymbol == GameManager.Symbol.X ? GameManager.Symbol.O : GameManager.Symbol.X;
    }

    public void ChooseCube()
    {
        var countingResult = default((int[], int[], int[]));

        //check if opposite symbol has any lines of 4
        if (enemySymbol == GameManager.Symbol.O)
        {
            countingResult = board.CountBoard(GameManager.Symbol.X);
        }
        else
        {
            countingResult = board.CountBoard(GameManager.Symbol.O);
        }

        int[] countRows = countingResult.Item1;
        int[] countCols = countingResult.Item2;
        int[] countDiags = countingResult.Item3;

        //check if any element of those arrays == 4
        //if so - THIS IS REALLY HARD
        //the placement has to be in that f.e. row, so reverse my whole gamelogic...
        availableCubes = new List<int[]>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board.IsMyCube(enemySymbol, i, j))
                {
                    availableCubes.Add(new int[] { i, j });
                }
            }
        }
        int chosen = Random.Range(0, availableCubes.Count - 1);
        int[] pair = availableCubes[chosen];
        int x = pair[0];
        int y = pair[1];

        gameManager.SelectCube(board.GetCubeAt(x, y));
        board.GetCubeAt(x, y).Select(board.GetCubeAt(x, y));

        StartCoroutine(DelayChoosePlacement(board.GetCubeAt(x, y)));
    }

    IEnumerator DelayChoosePlacement(Cube cube)
    {
        yield return new WaitForSeconds(0.6f);
        ChoosePlacement(cube);
    }

    public void ChoosePlacement(Cube cube)
    {
        List<int[]> moves = gameManager.possibleMove;
        int chosen = Random.Range(0, moves.Count - 1);
        int x = moves[chosen][0];
        int y = moves[chosen][1];

        gameManager.SetPlaceCube(board.GetCubeAt(x, y));
        gameManager.MakeMove();
        gameManager.ClearPlacementHighlight();

        uiManager.EnablePause();
        gameManager.isEnemyMove = false;
    }
}

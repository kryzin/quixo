using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
    public GameManager gameManager;
    public Board board;
    public GameManager.Symbol enemySymbol;

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
        availableCubes = new List<int[]>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (board.IsMyCube(enemySymbol, i, j))
                {
                    availableCubes.Add(new int[] {i, j});
                }
            }
        }
        Debug.LogWarning(availableCubes.Count);
        int chosen = Random.Range(0, availableCubes.Count - 1);
        int[] pair = availableCubes[chosen];
        int x = pair[0];
        int y = pair[1];

        gameManager.SelectCube(board.GetCubeAt(x, y));
        board.GetCubeAt(x, y).Select(board.GetCubeAt(x, y));

        ChoosePlacement(board.GetCubeAt(x, y));
    }

    public void ChoosePlacement(Cube cube)
    {
        int chosen = Random.Range(0, gameManager.possibleMove.Count - 1);
        int x = gameManager.possibleMove[chosen][0];
        int y = gameManager.possibleMove[chosen][1];

        gameManager.SetPlaceCube(board.GetCubeAt(x, y));
        gameManager.MakeMove();
        gameManager.ClearPlacementHighlight();
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Board : MonoBehaviour
//{
//    public static GameObject[,] board = new GameObject[5, 5];
//    private bool isSet = false;

//    void Start()
//    {
//        if (!isSet)
//        {
//            int letter = 65;

//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    string cubeName = "Cube" + (char)(letter);
//                    Debug.Log(cubeName);
//                    board[i, j] = GameObject.Find(cubeName);
//                    Debug.Log("added " + board[i, j].name + " to array position " + i + ":i and " + j + ":j");
//                    letter += 1;
//                }
//            }

//            isSet = true;
//        }
//        else
//        {
//            Debug.Log("board is already set :) ");
//        }
//    }
//    public static (int, int) FindObject(string targetObjectName)
//    {
//        for (int i = 0; i < board.GetLength(0); i++)
//        {
//            for (int j = 0; j < board.GetLength(1); j++)
//            {
//                if (board[i, j] != null && board[i, j].name == targetObjectName)
//                {
//                    return (i, j);
//                }
//            }
//        }

//        return (-1, -1);
//    }
//}

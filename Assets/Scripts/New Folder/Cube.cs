using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public char symbol; // symbol is O or X, blank at the begging
    public int boardX;
    public int boardY;


    void Start()
    {
        symbol = (char)32; // set blank symbol
        Debug.Log("Cube with symbol: " +  symbol + " instantiated at board[" + boardX + "," + boardY + "]");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

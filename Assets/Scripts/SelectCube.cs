using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCube : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Board board;
    private Cube cube;

    // for lifting up/down
    private float hoverAmount = 0.6f;
    private float hoverSpeed = 2f;
    private Vector3 initialPosition;
    private bool isHovering = false;

    // for highlighting 
    private Material originalMaterial;
    public Material outlineMaterial;
    private Renderer renderer;

    void Start()
    {
        initialPosition = transform.position;
        cube = GetComponent<Cube>();
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
        board = FindObjectOfType<Board>();
        if (board == null)
        {
            Debug.LogError("Board script not found in the scene.");
        }
    }

    public void OnPointerClick(PointerEventData eventData) // check is object is clicked
    {
        Debug.Log("Pointer Click " + cube.gameObject.name);
        // track Pointer events only if the Cube is interactable
    }

    public void OnPointerEnter(PointerEventData eventData) // when you hover over object
    {
        Debug.Log("Pointer Enter " + cube.gameObject.name + " at ");
        // track Pointer events only if the Cube is interactable
        // only if at y=0 or y=4 or x=0 or x=4 AND symbol = empty or playerTurn
        if (IsAllowedToMove())
        {
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData) // when you stop hovering
    {
        //Debug.Log("Pointer Exit " + gameObject.name);
        // track Pointer events only if the Cube is interactable
        isHovering = false;
    }

    void Update()
    {
        if (isHovering)
        {
            Hover();
        }
        else
        {
            ResetPosition();
        }
    }

    void Hover()
    {
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + hoverAmount, initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        renderer.material = outlineMaterial;
    }

    void ResetPosition()
    {
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        isHovering = false;
        renderer.material = originalMaterial;
    }

    bool IsAllowedToMove()
    {
        bool currentPlayer = board.playerTurn;
        string symbol = currentPlayer ? "O" : "X";

        if (cube.boardX == 0 || cube.boardX == 4 || cube.boardY == 0 || cube.boardY == 4)
        {
            if (cube.symbol == "" || cube.symbol == symbol)
            {
                return true;
            } 
        }
        return false;
    }
}


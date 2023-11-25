using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour
{
    public string symbol = ""; // symbol is O or X, blank at the begging
    public int boardX;
    public int boardY;
    public GameManager gameManager;

    // for lifting up/down
    private float hoverAmount = 0.6f;
    private float hoverSpeed = 2f;
    private Vector3 initialPosition;
    private bool isHovering = false;
    public bool isSelected = false;

    // for highlighting 
    private Material originalMaterial;
    public Material outlineMaterial;
    private Renderer cubeRenderer;

    void Start()
    {
        initialPosition = transform.position;
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null )
        {
            Debug.Log("found renderer");
        }

        originalMaterial = cubeRenderer.material;
    }

    void Update()
    {
        if (!isSelected)
        {
            if (isHovering)
            {
                Hover();
            }
            else
            {
                ResetPosition(this);
            }
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (gameManager.selectedCube != null)
            {
                gameManager.selectedCube.isSelected = false;
                ResetPosition(gameManager.selectedCube);
                gameManager.DeselectCube();
            }
        }
    }

    public bool IsAllowedToMove()
    {
        if (boardX == 0 || boardX == 4 || boardY == 0 || boardY == 4)
        {
            if (symbol == "" || symbol == gameManager.currentSymbol)
            {
                return true;
            }
        }
        return false;
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse Clicked on " + gameObject.name);
        // track only if the Cube is interactable


        if (IsAllowedToMove())
        {
            if (gameManager.selectedCube != null && gameManager.selectedCube != this)
            {
                gameManager.selectedCube.isSelected = false;
                ResetPosition(gameManager.selectedCube);
            }
            gameManager.SelectCube(this);
            isSelected = true;
            Select();
        }
    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Enter " + gameObject.name);
        // track only if the Cube is interactable
        if (IsAllowedToMove())
        {
            isHovering = true;
        }
    }
    public void OnMouseExit() // when you stop hovering
    {
        // track only if the Cube is interactable
        if (!isSelected)
        {
            isHovering = false;
        }
    }

    void Hover()
    {
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + hoverAmount, initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
    }

    void ResetPosition(Cube cube)
    {
        if (cube.transform.position != cube.initialPosition) { }
        StartCoroutine(ResetPositionCoroutine(cube));
        cube.isHovering = false;
        cube.isSelected = false;
        //Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        cube.cubeRenderer.material = originalMaterial;
    }

    public IEnumerator ResetPositionCoroutine(Cube cube)
    {
        Vector3 currentPosition = cube.transform.position;
        Vector3 targetPosition = cube.initialPosition;

        float elapsedTime = 0f;
        float duration = 1f;

        while ( elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = targetPosition;
    }

    void Select()
    {
        gameManager.SelectCube(this);
        Debug.Log("Selected Cube: " + gameObject.name);
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + hoverAmount + 20f, initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        cubeRenderer.material = outlineMaterial;
    }
}

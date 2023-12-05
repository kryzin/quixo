using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Cube : MonoBehaviour
{
    public GameManager.Symbol symbol; // symbol is O or X, blank at the begging
    public int boardX;
    public int boardY;
    public GameManager gameManager;

    // for lifting up/down
    private float hoverAmount = 0.4f;
    private float hoverSpeed = 2f;
    public Vector3 initialPosition;
    public bool isHovering = false;
    public bool isSelected = false;
    public bool isPossiblePlacement = false;
    public bool dontTouch = false;

    // for highlighting 
    //private Material originalMaterial;
    //public Material outlineMaterial;
    private Color hoverColor = Color.red;
    private Color originalColor;
    public SpriteRenderer cubeRenderer;

    private Coroutine selecting;

    void Start()
    {
        initialPosition = transform.position;
        cubeRenderer = GetComponent<SpriteRenderer>();
        if (cubeRenderer != null )
        {
            Debug.Log("found renderer");
        }

        //originalMaterial = cubeRenderer.material;
        originalColor = cubeRenderer.color;
    }

    void Update()
    {
        if (!isSelected)
        {
            if (isHovering)
            {
                Hover(this);
            }
            else
            {
                if(!dontTouch) ResetPosition(this);
            }
        }
        if (Input.GetMouseButtonDown(0)) // dismiss selection if clicked outside Cube objects
        {
            //also reset placement selection-------------------------------------------------------------------------------------
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null || !hit.collider.CompareTag("Piece") && !EventSystem.current.IsPointerOverGameObject())
            {
                if ( selecting != null)
                {
                    StopCoroutine(selecting);
                }
                isSelected = false;
                ResetSelected(this);
                if (gameManager.selectedCube != null)
                {
                    gameManager.DeselectCube();
                    isSelected = false;
                }
            }
        }
    }


    public bool IsAllowedToMove()
    {
        if (boardX == 0 || boardX == 4 || boardY == 0 || boardY == 4)
        {
            if (symbol == GameManager.Symbol.Null || symbol == gameManager.currentPlayer)
            {
                return true;
            }
        }
        return false;
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse Clicked on " + gameObject.name);
        if (isPossiblePlacement) // if chose Cube and chose where to put it
        {
            gameManager.SetPlaceCube(this); 
            gameManager.MakeMove();
            gameManager.ClearPlacementHighlight();
        }
        // track only if the Cube is interactable
        else if (IsAllowedToMove())
        {
            if (gameManager.selectedCube != null && gameManager.selectedCube != this)
            {
                
                gameManager.selectedCube.isSelected = false;
                ResetSelected(gameManager.selectedCube);
                gameManager.DeselectCube();
            }
            gameManager.SelectCube(this);
            isSelected = true;
            Select(this);
        }
    }

    public void OnMouseEnter()
    {
        // track only if the Cube is interactable
        if (IsAllowedToMove() && !isPossiblePlacement && !isSelected)
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

    void Hover(Cube cube)
    {
        Vector3 targetPosition = new Vector3(cube.initialPosition.x, cube.initialPosition.y + hoverAmount, cube.initialPosition.z);
        cube.transform.position = Vector3.Lerp(cube.transform.position, targetPosition, Time.deltaTime * hoverSpeed);
    }

    void ResetPosition(Cube cube)
    {

        cube.isHovering = false;
        cube.isSelected = false;
        Vector3 targetPosition = new Vector3(cube.initialPosition.x, cube.initialPosition.y, cube.initialPosition.z);
        cube.transform.position = Vector3.Lerp(cube.transform.position, targetPosition, Time.deltaTime * hoverSpeed);

    }

    void Select(Cube cube)
    {
        gameManager.SelectCube(cube);
        cube.isHovering = false;
        Debug.Log("Selected Cube: " + cube.name);
        selecting = StartCoroutine(SelectCoroutine(cube));
        //Vector3 targetPosition = new Vector3(cube.initialPosition.x, cube.initialPosition.y + hoverAmount + 1f, cube.initialPosition.z);
        //cube.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        //cube.cubeRenderer.material = cube.outlineMaterial;
        cube.cubeRenderer.color = hoverColor;
        gameManager.ShowMoves(cube);
    }

    public IEnumerator SelectCoroutine(Cube cube)
    {
        Vector3 currentPosition = cube.transform.position;
        Vector3 targetPosition = new Vector3(cube.initialPosition.x, cube.initialPosition.y + hoverAmount + 0.7f, cube.initialPosition.z);

        float elapsedTime = 0f;
        float duration = 0.6f;

        while (elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = targetPosition;
    }

    void ResetSelected(Cube cube)
    {
        gameManager.DeselectCube();
        cube.isSelected = false;
        cube.isHovering = false;

        Vector3 targetPosition = new Vector3(cube.initialPosition.x, cube.initialPosition.y, cube.initialPosition.z);
        cube.transform.position = Vector3.Lerp(cube.transform.position, targetPosition, Time.deltaTime * hoverSpeed);
        //cube.cubeRenderer.material= cube.originalMaterial;
        cube.cubeRenderer.color = originalColor;
    }

    public void HighlightSelection()
    {
        cubeRenderer.color = Color.green;
    }

    public void ResetHighlight()
    {
        cubeRenderer.color = originalColor;
        isPossiblePlacement = false;
    }

    public void Move(int directionX, int directionY, int repeat, bool rotate)
    {
        dontTouch = true;
        if (this == gameManager.selectedCube)
        {
            StartCoroutine(MoveSelectedCoroutine(this, directionX, directionY, repeat, rotate));
        }
        else { StartCoroutine(MoveCoroutine(this, directionX, directionY)); }
        dontTouch = false;
    }

    public IEnumerator MoveCoroutine(Cube cube, int directionX, int directionY)
    {
        // this should be: first move V3(+1.1f,y,z) and at the end move 1.1f=x down
        float speed = 2f;
        Vector3 targetPosition = cube.initialPosition + new Vector3(1.25f * directionX, 0.75f * directionY, 0);
        float duration = 2f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(cube.initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime * speed; // Adjust the speed as needed

            yield return null;
        }
        cube.transform.position = targetPosition;
        cube.initialPosition = cube.transform.position;
    }

    public IEnumerator MoveSelectedCoroutine(Cube cube, int directionX, int directionY, int repeat, bool rotate)
    {
        cube.cubeRenderer.sortingOrder = 8;
        cube.initialPosition = cube.initialPosition + new Vector3(0, 1.1f, 0);

        if (rotate && gameManager.currentPlayer == GameManager.Symbol.X && symbol == GameManager.Symbol.Null)
        {
            Debug.Log("rotating - " + this.name);
            transform.Rotate(0, 0, -120f);
        }
        if (rotate && gameManager.currentPlayer == GameManager.Symbol.O && symbol == GameManager.Symbol.Null)
        {
            Debug.Log("rotating + " + this.name);
            transform.Rotate(0, 0, 120f);
        }

        float speed = 2f;
        Vector3 targetPosition = cube.initialPosition + new Vector3(1.25f * directionX * repeat, 0.75f * directionY * repeat, 0);
        float duration = 2f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(cube.initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }
        cube.transform.position = targetPosition;
        cube.initialPosition = cube.initialPosition + new Vector3(1.25f * directionX * repeat, 0.75f * directionY * repeat, 0);
        StartCoroutine(FinishMoveSelectedCoroutine(cube, directionX, directionY));
    }

    public IEnumerator FinishMoveSelectedCoroutine(Cube cube, int directionX, int directionY)
    {
        cube.cubeRenderer.sortingOrder = cube.boardX + cube.boardY;
        float speed = 2f;
        Vector3 targetPosition = cube.initialPosition + new Vector3( 0, -1.1f, 0);
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(cube.initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }
        cube.transform.position = targetPosition;
        cube.initialPosition = cube.initialPosition + new Vector3( 0, -1.1f, 0);
    }
}

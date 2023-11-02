using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCube : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[,] board;

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
        board = Board.board;
        initialPosition = transform.position;
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    public void SetBoard(GameObject[,] array)
    {
        board = array;
    }

    public void OnPointerClick(PointerEventData eventData) // check is object is clicked
    {
        Debug.Log("Pointer Click " + gameObject.name);
        Debug.Log(Board.FindObject(gameObject.name));
    }

    public void OnPointerEnter(PointerEventData eventData) // when you hover over object
    {
        Debug.Log("Pointer Enter " + gameObject.name);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData) // when you stop hovering
    {
        Debug.Log("Pointer Exit " + gameObject.name);
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
}


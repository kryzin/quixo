using UnityEngine;

public class ResponsiveCam : MonoBehaviour
{
    private Camera mainCamera;
    private float baseOrthographicSize;

    void Start()
    {
        mainCamera = Camera.main;
        baseOrthographicSize = mainCamera.orthographicSize;
        AdjustCameraSize();
    }

    void Update()
    {
        // Check if the current aspect ratio is different from the initial aspect ratio
        if (Screen.width != Mathf.RoundToInt(baseOrthographicSize * 2 * mainCamera.aspect))
        {
            AdjustCameraSize();
        }
    }

    void AdjustCameraSize()
    {
        // Calculate the new orthographic size based on the current screen aspect ratio
        float newOrthographicSize = Screen.width / (2f * mainCamera.aspect);

        // Set the camera's orthographic size to maintain visibility of objects
        mainCamera.orthographicSize = newOrthographicSize;
    }
}

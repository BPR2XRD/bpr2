using UnityEngine;
using UnityEngine.InputSystem;

public class TopViewDropObject : MonoBehaviour
{
    private Camera topViewCamera;
    public GameObject objectToDropPrefab;

    public float spawnHeightOffset = 3f; 

    void Start()
    {
        // Get the camera component attached to the same GameObject
        topViewCamera = GetComponent<Camera>();

        // If the camera component doesn't exist, output an error message
        if (topViewCamera == null)
        {
            Debug.LogError("Camera component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if the left mouse button was clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Create a ray from the camera to the clicked point on the screen
            Ray ray = topViewCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // If the ray hits something, record the point of collision
                Vector3 clickPoint = hit.point;

                // Modify the y-coordinate of clickPoint so the object spawns slightly above the ground
                clickPoint.y += spawnHeightOffset;

                // Instantiate the object at the adjusted position
                Instantiate(objectToDropPrefab, clickPoint, Quaternion.identity);
            }
        }
    }
}

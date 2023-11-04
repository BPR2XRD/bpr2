using UnityEngine;
using UnityEngine.InputSystem;

public class TopViewDropObject : MonoBehaviour
{
    private Camera topViewCamera;
    public GameObject coffinPrefab;
    public GameObject zombiePrefab;

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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = topViewCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 clickPoint = hit.point;
                clickPoint.y += spawnHeightOffset;
                GameObject droppedCoffin = Instantiate(coffinPrefab, clickPoint, Quaternion.identity);
                droppedCoffin.GetComponent<CoffinController>().OnCoffinDropped(zombiePrefab);
            }
        }
    }
}

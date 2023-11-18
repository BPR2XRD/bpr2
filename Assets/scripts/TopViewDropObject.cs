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
        topViewCamera = GetComponent<Camera>();
        if (topViewCamera == null)
        {
            Debug.LogError("Camera component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Check if the number of zombies is less than 50
            if (ObjectSpawner.zombiesCurrentlyOnMap < 50)
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
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TopViewDropObject : MonoBehaviour
{
    private Camera topViewCamera;
    public GameObject coffinPrefab;
    public GameObject zombiePrefab;
    public GameObject barricadePrefab;
    public ToggleGroup toggleGroup;
    private bool isCoffin = true;

    public float spawnHeightOffset = 7f;

    void Start()
    {
        topViewCamera = GetComponent<Camera>();
        if (topViewCamera == null)
        {
            Debug.LogError("Camera component not found on " + gameObject.name);
        }
        if (toggleGroup == null)
        {
            Debug.LogError("ToggleGroup is not assigned!");
            return;
        }

        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Check if the number of zombies is less than 40
            if (ObjectSpawner.zombiesCurrentlyOnMap < 40)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = topViewCamera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                        Vector3 clickPoint = hit.point;
                        clickPoint.y += spawnHeightOffset;
                        if (isCoffin)
                        {
                            GameObject droppedCoffin = Instantiate(coffinPrefab, clickPoint, Quaternion.identity);
                            droppedCoffin.GetComponent<CoffinController>().OnCoffinDropped(zombiePrefab);
                        }
                        else
                        {
                            Instantiate(barricadePrefab, clickPoint, Quaternion.identity);
                        }
              
                }
            }
        }
    }
    private void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            // Execute code based on the selected toggle
            if (changedToggle.CompareTag("Coffin"))
            {
                Debug.Log("Toggle 1 is selected. Execute code for Toggle 1.");
                isCoffin = true;
            }
            else if (changedToggle.CompareTag("Barricade"))
            {
                Debug.Log("Toggle 2 is selected. Execute code for Toggle 2.");
                isCoffin = false;
            }
        }
    }
}

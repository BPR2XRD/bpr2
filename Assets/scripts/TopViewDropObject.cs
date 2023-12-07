using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TopViewDropObject : MonoBehaviour
{
    // References to the camera and prefabs
    private Camera topViewCamera;
    public GameObject coffinPrefab;
    public GameObject zombiePrefab;
    public GameObject barricadePrefab;
    public ToggleGroup toggleGroup; // UI toggle group for selecting objects
    private bool isCoffinSelected = true; // Tracks if coffin is selected
    
    // Offset for spawning objects above the ground
    public float spawnHeightOffset = 7f;

    // UI and cooldown related variables for coffin and barricade
    public Image coffinImage;
    public float cooldownCoffin = 5;
    bool isCooldownCoffin = false;
    public Image barricadeImage;
    public float cooldownBarricade = 5;
    bool isCooldownBarricade = false;
    private EventSystem eventSystem;

    void Start()
    {
        // Initialize the event system and camera
        eventSystem = EventSystem.current;
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

        // Setting up listeners for toggle changes
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }
    }

    void Update()
    {
        // Check for mouse input and if the pointer is not over a UI element
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = topViewCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Calculate the point where the user clicked
                Vector3 clickPoint = hit.point;
                clickPoint.y += spawnHeightOffset;

                // Spawn coffin or barricade based on selection and cooldown
                if (isCoffinSelected && !isCooldownCoffin)
                {
                    isCooldownCoffin = true;
                    coffinImage.fillAmount = 1;
                    GameObject droppedCoffin = Instantiate(coffinPrefab, clickPoint, Quaternion.identity);
                    droppedCoffin.GetComponent<CoffinController>().OnCoffinDropped(zombiePrefab);
                }
                else if (!isCoffinSelected && !isCooldownBarricade)
                {
                    isCooldownBarricade = true;
                    barricadeImage.fillAmount = 1;
                    var tmpBar = Instantiate(barricadePrefab, clickPoint, Quaternion.identity);
                    Destroy(tmpBar, 20f); // Destroy the barricade after 20 seconds
                }
            }
        }

        // Update the cooldown visuals for coffin and barricade
        SetImageCooldown(ref coffinImage, ref cooldownCoffin, ref isCooldownCoffin);
        SetImageCooldown(ref barricadeImage, ref cooldownBarricade, ref isCooldownBarricade);
    }

    private void SetImageCooldown(ref Image image, ref float cooldown, ref bool isCooldown)
    {
        // Reduce the fill amount over time and reset cooldown if it reaches 0
        if (isCooldown)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (image.fillAmount <=0)
            {
                image.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    private void OnToggleValueChanged(Toggle changedToggle)
    {
        // Change the selected object based on the toggle state
        if (changedToggle.isOn)
        {
            if (changedToggle.CompareTag("Coffin"))
            {
                isCoffinSelected = true;
            }
            else if (changedToggle.CompareTag("Barricade"))
            {
                isCoffinSelected = false;
            }
        }
    }

}

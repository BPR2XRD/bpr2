using System.Collections.Generic;
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
    private bool isCoffinSelected = true;
    
    public float spawnHeightOffset = 7f;

    public Image coffinImage;
    public float cooldownCoffin = 5;
    bool isCooldownCoffin = false;
    public Image barricadeImage;
    public float cooldownBarricade = 5;
    bool isCooldownBarricade = false;
    private EventSystem eventSystem;

    void Start()
    {
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

        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !IsPointerOverUIObject())
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = topViewCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                    Vector3 clickPoint = hit.point;
                    clickPoint.y += spawnHeightOffset;
                    if (isCoffinSelected && !isCooldownCoffin)
                    {
                        isCooldownCoffin = true;
                        coffinImage.fillAmount = 1;
                        GameObject droppedCoffin = Instantiate(coffinPrefab, clickPoint, Quaternion.identity);
                        droppedCoffin.GetComponent<CoffinController>().OnCoffinDropped(zombiePrefab);
                    }
                    else if (!isCoffinSelected && !isCooldownBarricade) //execute code for barricade
                    {
                        isCooldownBarricade = true;
                            barricadeImage.fillAmount = 1;
                        var tmpBar =    Instantiate(barricadePrefab, clickPoint, Quaternion.identity);
                    Destroy(tmpBar, 20f);
                    }
              
            }
            
        }
        SetImageCooldown(ref coffinImage, ref cooldownCoffin, ref isCooldownCoffin);
        SetImageCooldown(ref barricadeImage, ref cooldownBarricade, ref isCooldownBarricade);
    }

    private void SetImageCooldown(ref Image image, ref float cooldown, ref bool isCooldown)
    {
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
        if (changedToggle.isOn)
        {
            // Execute code based on the selected toggle
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

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new(eventSystem)
        {
            position = Mouse.current.position.value
        };
        List<RaycastResult> results = new();
        eventSystem.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}

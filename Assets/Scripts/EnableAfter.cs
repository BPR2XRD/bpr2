using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfter : MonoBehaviour
{
    public GameObject objectToEnable;
    public float delayInSeconds = 2f;

    void Start()
    {
        StartCoroutine(EnableObjectAfterDelayCoroutine());
    }

    IEnumerator EnableObjectAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Object to enable is not assigned!");
        }
    }
}

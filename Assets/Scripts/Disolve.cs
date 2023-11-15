using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolve : MonoBehaviour
{
    List<Material> materials = new();

    // Start is called before the first frame update
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
    }
    private void Update()
    {
        DisolveMaterial();
    }

    public void DisolveMaterial()
    {
        var value = Mathf.PingPong(Time.time * 0.5f, 1f);
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
        if (value >= 0.9f) { enabled = false; } //TO DO: needs more work
    }
}

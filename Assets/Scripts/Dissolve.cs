using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolve : MonoBehaviour
{
    List<Material> materials = new();
    public float dissolveDuration = 3f;
    private float elapsedTime = 0f;

    public void DissolveMaterial()
    {
        if (elapsedTime < dissolveDuration)
        {
            float t = elapsedTime / dissolveDuration;
            float value = Mathf.Lerp(0f, 1f, t);

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }

            elapsedTime += Time.deltaTime;
        }
        else
        {
            // Ensure that the dissolve value is exactly 1 at the end
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", 1f);
            }
        }
    }


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
        DissolveMaterial();
    }

    //public void DisolveMaterial()
    //{
    //    var value = Mathf.PingPong(Time.time * 0.5f, 1f);
    //    for (int i = 0; i < materials.Count; i++)
    //    {
    //        materials[i].SetFloat("_Dissolve", value);
    //    }
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float duration = 2;
    public Color color;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float aphaIn,  float aphaOut)
    {
        StartCoroutine(FadeRoutine(aphaIn, aphaOut));
    }

    public IEnumerator FadeRoutine(float aphaIn, float aphaOut)
    {
        float timer = 0;
        while (timer <= duration)
        {
            Color  newColor = color;
            newColor.a = Mathf.Lerp(aphaIn, aphaOut, timer/duration);
            renderer.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = color;
        newColor2.a = aphaOut;
        renderer.material.SetColor("_Color", newColor2);
    }
}

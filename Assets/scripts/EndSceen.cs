using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndSceen : MonoBehaviour
{
    
    public TextMeshProUGUI text;
    public Sprite WinSprite;
    public Sprite LoseSprite;

    public Image image;
    public AudioSource Source;
    public AudioClip LoosingClip;
    public AudioClip WinningClip;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Win(){
        Source.Stop();
        Source.PlayOneShot(WinningClip);

        text.text = "Wow, Ghost Hunter won. Congratulations!!!";
        image.GetComponent<Image> ().sprite = WinSprite;
    }

    public void Lose(){
        Source.Stop();
        Source.PlayOneShot(LoosingClip);

        text.text = "O_O, Ghost Hunter was defeated";
        image.GetComponent<Image> ().sprite = LoseSprite;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

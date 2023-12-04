using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndSceen : MonoBehaviour
{
    
    public TextMeshProUGUI text;
    public TextMeshProUGUI playerText;
    public Sprite WinSprite;
    public Sprite LoseSprite;

    public Image image;
    public Image image2;
    public AudioSource Source;
    public AudioClip LoosingClip;
    public AudioClip WinningClip;


    // Start is called before the first frame update
    void Start()
    {
        if (GameData.isPlayerDead)
        {
            Lose();
        }
        else
        {
            Win();
        }
    }
    public void Win(){
        Source.Stop();
        Source.PlayOneShot(WinningClip);

        text.text = "The Hunter survived";
        playerText.SetText("You survived");
        image.GetComponent<Image> ().sprite = WinSprite;
        image2.GetComponent<Image> ().sprite = WinSprite;
    }

    public void Lose(){
        Source.Stop();
        Source.PlayOneShot(LoosingClip);

        text.text = "The Hunter died";
        playerText.SetText("You Died");
        image.GetComponent<Image> ().sprite = LoseSprite;
        image2.GetComponent<Image> ().sprite = LoseSprite;
    }
}

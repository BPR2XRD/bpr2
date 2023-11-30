using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndSceen : MonoBehaviour
{
    
    public TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Win(){
        text.text = "Wow, Ghost Hunter won. Congratulations!!!";
    }

    public void Lose(){
        text.text = "O_O, Ghost Hunter was defeated";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void PlayAgain(){
        GameData.isPlayerDead = false;
        Debug.Log("button pressed");
        SceneManager.LoadScene("Graveyard");
    }

    public void Quit(){
        Application.Quit();
    }
}

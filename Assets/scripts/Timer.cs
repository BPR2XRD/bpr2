using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private TextMeshPro timerText_pist;
    public TextMeshProUGUI timerText2;
    // public TextMeshProUGUI timerText3;
    public List<TextMeshProUGUI> timerTexts;
    public float initialTime = 360f;
    private float remainingTime;
    private HueLights lights;


    void Start()
    {
        lights = GameObject.FindGameObjectWithTag("Manager").GetComponent<HueLights>();
        timerTexts = new List<TextMeshProUGUI>();

        if (timerText2 == null) 
        {
            Debug.LogError("TimerText is not assigned!");
            return;
        }

        GameObject[] gamepadCanvases = GameObject.FindGameObjectsWithTag("GamepadCanvas");
        foreach (GameObject canvas in gamepadCanvases)
        {
            // Assuming the TextMeshPro component is on the first child of the canvas
            if (canvas.transform.childCount > 0)
            {
                TextMeshProUGUI textComponent = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    timerTexts.Add(textComponent);
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI component not found on child of GameObject: " + canvas.name);
                }
            }
            else
            {
                Debug.LogWarning("GameObject with tag 'GamepadCanvas' has no children: " + canvas.name);
            }
        }

        remainingTime = initialTime; // Set to 6 minutes in seconds
        StartCoroutine(UpdateTimer());
    }
    private void Update()
    {
        try
        {
            //timer on pistol on Update, as to not mess with Coroutine when the controller disconnects
            if (timerText_pist == null)
            {
                timerText_pist = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TextMeshPro>();
                timerText_pist.SetText(FormatTime(remainingTime));
            }
            else
            {
                timerText_pist.SetText(FormatTime(remainingTime));
            }
        }
        catch { }

    }
    private IEnumerator UpdateTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText2.SetText(FormatTime(remainingTime));
            foreach (TextMeshProUGUI text in timerTexts)
            {
                text.SetText(FormatTime(remainingTime));
            }
            yield return null; // Wait until the next frame
        }

        TimerFinished();
    }

    private string FormatTime(float timeInSeconds)
    {
        if (timeInSeconds < 0) timeInSeconds = 0; // To avoid negative display

        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerFinished()
    {
        Debug.Log("Timer finished!");
        if (lights != null)
            lights.PlayerWin();
        SceneManager.LoadScene("EndScene");
    }
}

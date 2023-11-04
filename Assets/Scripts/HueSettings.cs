using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueSettings : MonoBehaviour
{
    [SerializeField]
    string appKey;

    [SerializeField]
    string appName;

    [SerializeField]
    string deviceName;

    public string AppKey { get => appKey; set => appKey = value; }

    public string AppName { get => appName; set => appName = value; }

    public string DeviceName { get => deviceName; set => deviceName = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

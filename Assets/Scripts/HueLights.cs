using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using Light = Q42.HueApi.Light;

public class HueLights : MonoBehaviour
{
    private const string BRIDGE_IP = "192.168.126.44";
    private const string APP_ID = "7556163a989040aa9d5763cbfefef5bf";
    private const string APP_ID2 = "7556163a-9890-40aa-9d57-63cbfefef5bf";

    ILocalHueClient _client;
    Light _light;
    List<string> _lightList;
    bool _isInitialized;

    public async void InitializeHue()
    {
        _isInitialized = false;
        //initialize client with bridge IP and app GUID
        _client = new LocalHueClient(BRIDGE_IP);
        _client.Initialize(APP_ID);

        //only working with light #1 in this demo
        _light = await _client.GetLightAsync("1");
        _lightList = new List<string>() { "1" };

        var hue = _light.State.Hue;
        Debug.Log(hue);
        byte brightness = _light.State.Brightness;

        _isInitialized = true;
    }

    //helper method to convert hex to RGB
    private byte[] HexToRGB(string hex)
    {
        byte[] retvalue = new byte[3];
        if (hex.Contains('#'))
        {
            hex = hex.Remove(0, 1);
        }

        retvalue[0] = Convert.ToByte(hex.Substring(0, 2), 16);
        retvalue[1] = Convert.ToByte(hex.Substring(2, 2), 16);
        retvalue[2] = Convert.ToByte(hex.Substring(4, 2), 16);

        return retvalue;

    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeHue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task ConnectLights()
    {

        //Make sure the user has preSssed the button on the bridge before calling RegisterAsync
        ////It will throw an LinkButtonNotPressedException if the user did not press the button
        //var regResult = await LocalHueClient.RegisterAsync("BRIDGE_IP", "mypersonalappname");

        ////Save the app key for later use and use it to initialize LocalHueApi
        //var appKey = regResult.Username;

    }
}

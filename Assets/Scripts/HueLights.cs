using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using Light = Q42.HueApi.Light;

public class HueLights : MonoBehaviour
{
    private const string BRIDGE_IP = "192.168.126.44";
    [SerializeField]
    HueSettings hueSettings;
    ILocalHueClient client;
    List<Light> lights = new();

    public async void InitializeHue()
    {
        client = new LocalHueClient(BRIDGE_IP);

        if (!string.IsNullOrEmpty(hueSettings.AppKey))
        {
            client.Initialize(hueSettings.AppKey);
        }

        lights = (List<Light>)await client.GetLightsAsync();
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
        //RegisterAppWithHueBridge();
        InitializeHue();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //Used to get AppKey for first time
    public async Task RegisterAppWithHueBridge()
    {
        Debug.Log("Register hue");
        IBridgeLocator locator = new HttpBridgeLocator();
        var timeout = TimeSpan.FromSeconds(5);
        var bridges = await locator.LocateBridgesAsync(timeout);

        // Assuming we have only one bridge
        var bridge = bridges.First();
        Debug.Log(bridge.IpAddress);
        string ipAddressOfTheBridge = bridge.IpAddress;
        var client = new LocalHueClient(ipAddressOfTheBridge);
        Debug.Log("after client");

        // Get the key
        var appKey = await client.RegisterAsync(
            hueSettings.AppName,
            hueSettings.DeviceName);
        Debug.Log(appKey);
        hueSettings.AppKey = appKey;
    }

}
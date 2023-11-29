using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.ColorConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using Light = Q42.HueApi.Light;
using Q42.HueApi.ColorConverters.Original;

public class HueLights : MonoBehaviour
{
    private const string BRIDGE_IP = "192.168.126.44";
    [SerializeField]
    HueSettings hueSettings;
    ILocalHueClient client;
    List<Light> lights = new();

    public async Task InitializeHue()
    {
        try
        {
            client = new LocalHueClient(BRIDGE_IP);

            if (!string.IsNullOrEmpty(hueSettings.AppKey))
            {
                client.Initialize(hueSettings.AppKey);
            }

            lights = (List<Light>)await client.GetLightsAsync();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
       
    }

    public async Task UpdateLights()
    {
        lights = (List<Light>)await client.GetLightsAsync();
    }

    public async Task ChangeLight(string lightId, Color color)
    {
        if (client == null)
        {
            return;
        }

        var lightToChange = lights.FirstOrDefault((l) => l.Id == lightId);
        if (lightToChange != null)
        {
            var command = new LightCommand();
            var lightColor = new RGBColor(color.r, color.g, color.b);
            command.TurnOn().SetColor(lightColor);

            var lightsToAlter = new string[] { lightToChange.Id };
            await client.SendCommandAsync(command, lightsToAlter);
        }
    }
    public async Task ChangeLights(Color color)
    {
        if (client == null || !lights.Any())
        {
            return;
        }
        var command = new LightCommand();
        var lightColor = new RGBColor(color.r, color.g, color.b);
        command.TurnOn().SetColor(lightColor);
        command.Brightness = 254; //Max,  (0-254)
        command.Effect = Effect.None;
        await client.SendCommandAsync(command);

    }

    public async Task TurnOff()
    {
        if (client == null)       
            return;

        var command = new LightCommand();
        command.TurnOff();
        await client.SendCommandAsync(command);

    }

    public async Task InitialScene(Color color)
    {
        if (client == null || !lights.Any())
        {
            return;
        }
        var command = new LightCommand();
        var lightColor = new RGBColor(color.r, color.g, color.b);
        command.TurnOn().SetColor(lightColor);
        command.Brightness = 254;
        command.Effect = Effect.None;
        command.TransitionTime = new TimeSpan(hours: 0, minutes: 0, seconds: 2);
     
        await Task.Delay(1500); //waiting until lights are turned off

        var lightToChange = lights.Find((Light l) => l.Name == "Strip 1");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Ceiling 1");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 2");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 3");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Ceiling 2");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 4");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 5");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Ceiling 3");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 6");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }
        await Task.Delay(1000);

        lightToChange = lights.Find((Light l) => l.Name == "Strip 7");
        if (lightToChange != null)
        {
            await client.SendCommandAsync(command, new string[] { lightToChange.Id });
        }

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
        TryToStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    async void TryToStart()
    {
        await InitializeHue();
        //foreach (var light in lights)
        //{
        //    await ChangeLight(light.Id, Color.green);

        //}
        //await ChangeLights(Color.white);
        await TurnOff();
        await InitialScene(new Color(0.212f, 0.471f, 0)); //0.212f, 0.471f, 0 - Green
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
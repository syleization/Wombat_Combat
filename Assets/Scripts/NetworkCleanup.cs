using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkCleanup : MonoBehaviour
{
    NetworkManager manager;
    bool IsConnecting = false;
    public bool ShowGUI = true;

    void Start()
    {
        //NetworkManager.singleton.StopHost();
        //Network.Disconnect();
        //NetworkServer.Shutdown();
        if(Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
        else
        {
            HudManager hud = FindObjectOfType<HudManager>();
            hud.ToggleHUD();
            manager = FindObjectOfType<NetworkManager>();
        }
    }

    void OnGUI()
    {
        if(!ShowGUI)
        {
            return;
        }
        if(!IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Client"))
        {
            IsConnecting = true;
            manager.StartClient();
        }
        else if (!IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Host"))
        {
            IsConnecting = true;
            manager.StartHost();
        }
        if(IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
        {
            IsConnecting = false;
            manager.StopHost();
        }
        if (!IsConnecting)
        {
            manager.networkAddress = GUI.TextField(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 11.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 5.5f, (float)Screen.height / 10.0f), manager.networkAddress);
        }
    }
}

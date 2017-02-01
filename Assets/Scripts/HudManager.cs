using UnityEngine;
using UnityEngine.Networking;

public class HudManager : MonoBehaviour
{
    NetworkManagerHUD HUD;

    void Awake()
    {
        HUD = this.gameObject.GetComponent<NetworkManagerHUD>();
    }

    public void ToggleHUD()
    {
        HUD.showGUI = !HUD.showGUI;
    }

#if UNITY_ANDROID
    NetworkManager manager;
    bool IsConnecting = false;
    public bool ShowGUI = true;

    void Start()
    {
        ToggleHUD();
        manager = FindObjectOfType<NetworkManager>();
    }

    void OnGUI()
    {
        if (!ShowGUI)
        {
            return;
        }
        if (!IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Client"))
        {
            IsConnecting = true;
            manager.StartClient();
        }
        else if (!IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Host"))
        {
            IsConnecting = true;
            manager.StartHost();
        }
        if (IsConnecting && GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
        {
            IsConnecting = false;
            manager.StopHost();
        }
        if (!IsConnecting)
        {
            manager.networkAddress = GUI.TextField(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 11.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 5.5f, (float)Screen.height / 10.0f), manager.networkAddress);
        }
    }
#endif
}

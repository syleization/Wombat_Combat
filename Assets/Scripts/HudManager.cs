﻿using UnityEngine;
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
    string Password;
    public bool ShowGUI = true;

    void Start()
    {
        ToggleHUD();
        Password = "";
        manager = FindObjectOfType<NetworkManager>();
    }

    void OnGUI()
    {
        if (!ShowGUI)
        {
            return;
        }
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Client"))
            {
                manager.StartClient();
            }
            else if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Host"))
            {
                manager.StartHost();
            }

            manager.networkAddress = GUI.TextField(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 11.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 5.5f, (float)Screen.height / 10.0f), manager.networkAddress);
        }
        if(NetworkServer.active || NetworkClient.active)
        {
            if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
            {
                manager.StopHost();
            }
        }
        if (!NetworkServer.active && !NetworkClient.active)
        {
            float xPos = 0.0f;
            float yPos = 0.0f;
            float spacing = (float)Screen.height / 10.0f;
            if (manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Enable MatchMaker"))
                {
                    manager.StartMatchMaker();
                }
            }
            else
            {
                if (manager.matchInfo == null)
                {
                    if (manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Create Match"))
                        {
                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, Password, "", "", 0, 0, manager.OnMatchCreate);
                        }
                        yPos += spacing;
                        GUI.Label(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Room Name:");
                        manager.matchName = GUI.TextField(new Rect(xPos + (float)Screen.width / 4.0f, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), manager.matchName);
                        yPos += spacing;
                        if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Find Match"))
                        {
                            manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                        }
                    }
                    else
                    {
                        foreach (var match in manager.matches)
                        {
                            if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Join Match:" + match.name))
                            {
                                manager.matchName = match.name;
                                manager.matchSize = (uint)match.currentSize;
                                manager.matchMaker.JoinMatch(match.networkId, Password, "", "", 0, 0, manager.OnMatchJoined);
                            }
                            yPos += spacing;
                        }
                    }
                }
            }
        }
    }
#endif
}

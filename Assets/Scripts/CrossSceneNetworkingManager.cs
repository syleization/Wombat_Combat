using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class CrossSceneNetworkingManager : NetworkBehaviour
{
    NetworkManager manager;
    string Password = "";
    GameType CurrentGameType;

    static UnityEngine.Networking.Types.NetworkID NetworkId;
    static public UnityEngine.Networking.Types.NetworkID MatchId
    {
        get
        {
            return NetworkId;
        }
    }

    private enum NetworkType { None, Client, Host }
    NetworkType CurrentNetworkType;

    private void Start()
    {
        CurrentNetworkType = NetworkType.None;
        SceneManager.sceneLoaded += ConfigureNetworkStuff;
        enabled = false;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= ConfigureNetworkStuff;
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void ConfigureNetworkStuff(Scene scene, LoadSceneMode mode)
    {
        manager = null;
        manager = FindObjectOfType<NetworkManager>();
        
        if (manager != null)
        {
            manager.onlineScene = scene.name;

            switch (CurrentNetworkType)
            {
                case NetworkType.Client:
                    // manager.StartClient();
                    manager.StartMatchMaker();
                    manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                    enabled = true;
                    StartCoroutine(JoinGameTimer(10.0f));
                    break;
                case NetworkType.Host:
                    manager.StartMatchMaker();
                    string matchName;
                    if(manager.matches == null)
                    {
                        matchName = "0";
                    }
                    else
                    {
                        matchName = manager.matches.Count.ToString();
                    }
                    manager.matchMaker.CreateMatch(matchName, manager.matchSize, true, Password, "", "", 0, 0, OnMatchCreate);
                    // manager.StartHost();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("[CrossSceneNetworkingManager::ConfigureNetworkStuff] Network Manager not found");
        }
    }

    public void OnMatchCreate(bool success, string extendedInfo, UnityEngine.Networking.Match.MatchInfo matchInfo)
    {
        if(success)
        {
            manager.OnMatchCreate(success, extendedInfo, matchInfo);
            NetworkId = matchInfo.networkId;
        }
    }

    IEnumerator JoinGameTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // If a match is not found go back to start screen
        enabled = false;
        LoadScene("MainMenu");
    }

    void Update()
    {
        if (manager != null && manager.matches != null)
        {
            foreach (var match in manager.matches)
            {
                if (match.currentSize < (int)CurrentGameType)
                {
                    manager.matchName = match.name;
                    manager.matchSize = (uint)match.currentSize;
                    NetworkId  = match.networkId;
                    manager.matchMaker.JoinMatch(match.networkId, Password, "", "", 0, 0, manager.OnMatchJoined);
                    enabled = false;
                    StopAllCoroutines();
                }
            }
        }
    }
    // 0 for TwoPlayer, 1 for ThreePlayer, 2 for FourPlayer
    public void StartLANClient(string sceneName)
    {
        if (sceneName == "Networking")
            CurrentGameType = GameType.TwoPlayer;
        LoadScene(sceneName);
        CurrentNetworkType = NetworkType.Client;
    }

    public void StartLANHost(string sceneName)
    {
        if (sceneName == "Networking")
            CurrentGameType = GameType.TwoPlayer;
        LoadScene(sceneName);
        CurrentNetworkType = NetworkType.Host;
    }

    public void CancelSearch()
    {
        manager.StopHost();
    }


//#if UNITY_ANDROID
//    public bool ShowGUI = true;

    //    void OnGUI()
    //    {
    //        if (!ShowGUI)
    //        {
    //            return;
    //        }
    //        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
    //        {
    //            if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Client"))
    //            {
    //                manager.StartClient();
    //            }
    //            else if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Start Host"))
    //            {
    //                manager.StartHost();
    //            }

    //            manager.networkAddress = GUI.TextField(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 11.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 5.5f, (float)Screen.height / 10.0f), manager.networkAddress);
    //        }
    //        if(NetworkServer.active || NetworkClient.active)
    //        {
    //            if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)Screen.width / 12.0f, (float)Screen.height / 10.0f * 2.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
    //            {
    //                manager.StopHost();
    //            }
    //        }
    //        if (!NetworkServer.active && !NetworkClient.active)
    //        {
    //            float xPos = 0.0f;
    //            float yPos = 0.0f;
    //            float spacing = (float)Screen.height / 10.0f;
    //            if (manager.matchMaker == null)
    //            {
    //                if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Enable MatchMaker"))
    //                {
    //                    manager.StartMatchMaker();
    //                }
    //            }
    //            else
    //            {
    //                if (manager.matchInfo == null)
    //                {
    //                    if (manager.matches == null)
    //                    {
    //                        if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Create Match"))
    //                        {
    //                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, Password, "", "", 0, 0, manager.OnMatchCreate);
    //                        }
    //                        yPos += spacing;
    //                        GUI.Label(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Room Name:");
    //                        manager.matchName = GUI.TextField(new Rect(xPos + (float)Screen.width / 4.0f, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), manager.matchName);
    //                        yPos += spacing;
    //                        if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Find Match"))
    //                        {
    //                            manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        foreach (var match in manager.matches)
    //                        {
    //                            if (GUI.Button(new Rect(xPos, yPos, (float)Screen.width / 4.0f, (float)Screen.height / 10.0f), "Join Match:" + match.name))
    //                            {
    //                                manager.matchName = match.name;
    //                                manager.matchSize = (uint)match.currentSize;
    //                                manager.matchMaker.JoinMatch(match.networkId, Password, "", "", 0, 0, manager.OnMatchJoined);
    //                            }
    //                            yPos += spacing;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //#endif
}

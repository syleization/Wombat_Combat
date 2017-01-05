using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkCleanup : MonoBehaviour
{

    void Start()
    {
        NetworkManager.singleton.StopHost();
        Network.Disconnect();
        NetworkServer.Shutdown();
    }

}

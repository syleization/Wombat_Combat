using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkCleanup : NetworkBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Disconnect()
    {
        Player player = GlobalSettings.Instance.GetLocalPlayer();
        if (player != null)
        {
            if (player.isServer)
            {
                player.RpcDisconnect();
            }
            else
            {
                player.CmdDisconnect();
            }
        }
        else
        {
            if (isServer)
            {
                NetworkManager.singleton.StopHost();
                NetworkManager manager = FindObjectOfType<NetworkManager>();
                NetworkManager.singleton.StartMatchMaker();
                if (manager.matchMaker != null && manager.matchInfo != null)
                {
                    manager.matchMaker.DestroyMatch(CrossSceneNetworkingManager.MatchId, 0, NetworkManager.singleton.OnDestroyMatch);
                }
                NetworkManager.singleton.StopMatchMaker();
            }
            SceneManager.LoadScene(0);
        }
    }
}

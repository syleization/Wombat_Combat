using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkCleanup : MonoBehaviour
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
    }
}

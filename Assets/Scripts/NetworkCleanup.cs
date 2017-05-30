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
        if(player != null && player.isServer)
        {
            NetworkManager.singleton.StopHost();
        }

        GlobalSettings.Instance.Terminate();
        TurnManager.Instance.Terminate();
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkCleanup : MonoBehaviour
{

    void OnGUI()
    {
        if (GUI.Button(new Rect(0.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
        {
            Terminate(GlobalSettings.Instance.GetLocalPlayer());
        }
    }
    public void Terminate(Player player)
    { 
        if(player.isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }

        GlobalSettings.Instance.Terminate();
        TurnManager.Instance.Terminate();
        SceneManager.LoadScene(0);
    }
}

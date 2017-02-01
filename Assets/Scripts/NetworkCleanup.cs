using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkCleanup : MonoBehaviour
{
    public bool ShowGUI = false;

    void OnGUI()
    {
        if(!ShowGUI)
        {
            return;
        }

        if (GUI.Button(new Rect(0.0f, 0.0f, (float)Screen.width / 6.0f, (float)Screen.height / 10.0f), "Disconnect"))
        {
            Terminate(GlobalSettings.Instance.GetLocalPlayer());
        }
    }

    public void Terminate(Player player)
    { 
        if(player != null && player.isServer)
        {
            NetworkManager.singleton.StopHost();
        }

        GlobalSettings.Instance.Terminate();
        TurnManager.Instance.Terminate();
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using UnityEngine.Networking;

public class HudManager : MonoBehaviour
{
    NetworkManagerHUD HUD;

    void Start()
    {
        HUD = this.gameObject.GetComponent<NetworkManagerHUD>();
    }

    public void ToggleHUD()
    {
        HUD.showGUI = !HUD.showGUI;
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 60, 20), "Toggle"))
    //    {
    //        HUD.showGUI = !HUD.showGUI;
    //    }
    //}
}

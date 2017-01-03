using UnityEngine;
using System.Collections;

public class StorePack : MonoBehaviour
{
    public void PackOnegems()
    {
        GemsUI.CurrentGems += 200;
        Debug.Log("Buying Pack one");
    }

    public void PackTwogems()
    {
        GemsUI.CurrentGems += 500;
        Debug.Log("Buying Pack Two");
    }

    public void PackThreegems()
    {
        GemsUI.CurrentGems += 800;
        Debug.Log("Buying Pack Three");
    }

    public void PackFourgems()
    {
        GemsUI.CurrentGems += 1100;
        Debug.Log("Buying Pack Four");
    }
}

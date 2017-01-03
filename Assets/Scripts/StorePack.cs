using UnityEngine;
using System.Collections;

public class StorePack : MonoBehaviour
{
    public void PackOnegems()
    {
        CurrencyManager.CurrentGems += 200;
        Debug.Log("Buying Pack one");
    }

    public void PackTwogems()
    {
        CurrencyManager.CurrentGems += 500;
        Debug.Log("Buying Pack Two");
    }

    public void PackThreegems()
    {
        CurrencyManager.CurrentGems += 800;
        Debug.Log("Buying Pack Three");
    }

    public void PackFourgems()
    {
        CurrencyManager.CurrentGems += 1100;
        Debug.Log("Buying Pack Four");
    }
}

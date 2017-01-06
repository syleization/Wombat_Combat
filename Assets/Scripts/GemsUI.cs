using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemsUI : MonoBehaviour
{

    public static int CurrentGems;
    public Text GemsText;

    void Start()
    {
        CurrentGems = 0;
    }

    void Update()
    {
        string Gems = ((int)CurrentGems).ToString();

        GemsText.text = Gems;
    }
}

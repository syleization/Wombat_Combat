using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static int CurrentGems;
    public Text GemsText;

    public static int CurrentCoins;
    public Text CoinText;

    void Awake()
    {
        if (SaveLoad.DoesDirExist() == false)
            SaveLoad.CreateDir();
        SaveLoad.LoadClient();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        string Gems = ((int)CurrentGems).ToString();

        GemsText.text = Gems;

        string Coins = ((int)CurrentCoins).ToString();

        CoinText.text = Coins;
    }
}

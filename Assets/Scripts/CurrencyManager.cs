using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static int CurrentGems;
    public Text GemsText;

    public static int CurrentCoins;
    public Text CoinText;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        CurrentGems = 0;
        CurrentCoins = 0;
    }

    void Update()
    {
        string Gems = ((int)CurrentGems).ToString();

        GemsText.text = Gems;

        string Coins = ((int)CurrentCoins).ToString();

        CoinText.text = Coins;


    }

}

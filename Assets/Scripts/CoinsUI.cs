using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour {

    public static int CurrentCoins;
    public Text CoinText;

    void Start()
    {
        CurrentCoins = 0;
    }

    void Update()
    {
        string Coins = ((int)CurrentCoins).ToString();

        CoinText.text = Coins;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTools : MonoBehaviour
{
    public Player player;
    public Hand playerHand;

    private void OnEnable()
    {
        player.enabled = true;
        playerHand.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Card temp = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick);
            temp.owner = player;
            temp.CurrentArea = "Hand";
            playerHand.CardsInHand.Add(temp);
        }
    }
}

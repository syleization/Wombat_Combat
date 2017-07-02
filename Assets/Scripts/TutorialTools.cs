using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTools : MonoBehaviour
{
    private bool init = false;
    public int stage = 0;

    public Player player;
    public Hand playerHand;
    public Field field;
    public GameObject mergeBtn;

    private void Update()
    {
        if (!init)
        {
            player.gameObject.SetActive(true);
            playerHand.gameObject.SetActive(true);
            player.Hand = playerHand;
            field.gameObject.SetActive(true);
            field.localPlayer = player;
            init = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GiveCard(GlobalSettings.Instance.Attack_DonkeyKick);
        }

        switch (stage)
        {
            case 1:
                if (field.IsMergable())
                {
                    mergeBtn.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void GiveCard(Card input)
    {
        Card temp = Instantiate(input);
        temp.owner = player;
        temp.CurrentArea = "Hand";
        playerHand.CardsInHand.Add(temp);
        DeckOfCards.TransformDealtCardToHand(temp, playerHand.CardsInHand.Count-1);
    }
}

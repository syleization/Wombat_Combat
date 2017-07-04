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
                else
                {
                    mergeBtn.SetActive(false);
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

    public void Merge()
    {
        if (Field.Instance.IsMergable())
        {
            Field.Instance.ToggleTwoSquares(false);
            mergeBtn.SetActive(false);
            ++stage;

            Card newCard = Instantiate(GlobalSettings.Instance.GetMergeCard(Field.Instance.GetCard(0).Type, Field.Instance.GetCard(0).Level));
            newCard.gameObject.SetActive(false);

            Effects.Merge(Field.Instance.GetCard(0).SubType, newCard.SubType, Turns.BottomPlayer);

            StartCoroutine(WaitToPlaceMergedCardIntoHand(CardActions.kMergeEffectTime, player, newCard));
        }
    }

    IEnumerator WaitToPlaceMergedCardIntoHand(float waitTime, Player currentPlayer, Card newCard)
    {
        yield return new WaitForSeconds(waitTime);

        newCard.owner = currentPlayer;
        DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
        newCard.CurrentArea = "Hand";
        currentPlayer.Hand.CardsInHand.Add(newCard);

        // Clear field of used cards
        Field.Instance.ClearField();
        
        newCard.gameObject.SetActive(true);
    }
}

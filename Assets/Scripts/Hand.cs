using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public List<Card> CardsInHand = new List<Card>();

    public void ResetHandCardPositions(Card cardMoved, int cardsDealt)
    {
        for (int i = 0; i < cardsDealt - 1; ++i)
        {
            if (CardsInHand[i] == cardMoved)
            {
                // If the current card is the card that is moving then swap its position in the hand to the end
                Card temp = CardsInHand[i];
                CardsInHand[i] = CardsInHand[cardsDealt - 1];
                CardsInHand[cardsDealt - 1] = temp;
            }
            //CardsInHand[i].transform.position = new Vector3(((float)i * 2) - 5, -5, (float)i * -0.01f);
            DeckOfCards.TransformDealtCardToHand(CardsInHand[i], i);
        }
    }

    public int GetCardIndex(GameObject card)
    {
        for(int i = 0; i < CardsInHand.Count; ++i)
        {
            if(CardsInHand[i] == card)
            {
                return i;
            }
        }
        Debug.Log("Card being sent to field that isnt in hand");
        return -1; // error
    }

    public static bool IsYourHand(Card card, GameObject hit)
    {
        return hit == card.owner.Hand.gameObject ? true : false; // if the hand the raycast hit is yours
    }

    public static Player GetOwner(GameObject hit)
    {
        foreach(Player player in GlobalSettings.Players)
        {
            // if the gameobject the card was let go on is the hand of the player
            if(player != null && hit == player.Hand.gameObject)
            {
                return player;
            }
        }
        Debug.Log("[Hand::GetOwner] Invalid Return path");
        return null;
    }

    public bool HasDefenceCards()
    {
        foreach(Card card in CardsInHand)
        {
            if(card.Type == CardType.Defence)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasTrapCards()
    {
        foreach (Card card in CardsInHand)
        {
            if (card.Type == CardType.Trap)
            {
                return true;
            }
        }

        return false;
    }

}

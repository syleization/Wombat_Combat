using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckOfCards : MonoBehaviour
{
    public List<Card> deck = new List<Card>();

    private List<Card> cards = new List<Card>();
    public Player owner;
    private bool showReset = false;

    void ResetDeck()
    {
        for (int i = 0; i < owner.Hand.CardsInHand.Count; i++)
        {
            Destroy(owner.Hand.CardsInHand[i]);
        }
        owner.Hand.CardsInHand.Clear();
        cards.Clear();
        cards.AddRange(deck);
        showReset = false;
    }

    Card DealCard()
    {
        if (cards.Count == 0)
        {
            showReset = true;
            return null;
            //Alternatively to auto reset the deck:
            //ResetDeck();
        }

        int card = Random.Range(0, cards.Count - 1);

        Card go = Instantiate<Card>(cards[card]);

        cards.RemoveAt(card);

        if (cards.Count == 0)
        {
            showReset = true;
        }

        return go;
    }

    void Start()
    {
        ResetDeck();
    }

    void GameOver()
    {
        for (int v = 0; v < owner.Hand.CardsInHand.Count; v++)
        {
            Destroy(owner.Hand.CardsInHand[v]);
        }
        owner.Hand.CardsInHand.Clear();
        cards.Clear();
        cards.AddRange(deck);
    }

    void OnGUI()
    {
        if (!showReset)
        {
            // Deal button
            if (GUI.Button(new Rect(10, 10, 100, 20), "Deal"))
            {
                MoveDealtCard();
            }
        }
        else
        {
            // Reset button
            if (GUI.Button(new Rect(10, 10, 100, 20), "Reset"))
            {
                ResetDeck();
            }
        }
        // GameOver button
        if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 20), "GameOver"))
        {
            GameOver();
        }

        // EndTurn button
        if (GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndTurn"))
        {
            TurnManager.Instance.EndTurn();
        }

        // Merge Button
        if(owner.Field.IsMergable() != CardType.None && GUI.Button(new Rect(10, Screen.height / 2, 50, 20), "Merge"))
        {
            // Add new power card to hand
            Card newCard;
            switch (owner.Field.IsMergable())
            {
                case CardType.Attack:
                    newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_WomboCombo);
                    break;
                case CardType.Defence:
                    newCard = Instantiate<Card>(GlobalSettings.Instance.Defence_GooglyEyes);
                    break;
                case CardType.Trap:
                    newCard = Instantiate<Card>(GlobalSettings.Instance.Trap_WombatCage);
                    break;
                default:
                    Debug.Log("Error in Merge");
                    newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_DonkeyKick);
                    break;
            }
            newCard.transform.position = new Vector3(((float)owner.Hand.CardsInHand.Count * 2) - 5, -5, (float)owner.Hand.CardsInHand.Count * -0.01f); // place card 1/4 up on all axis from last
            newCard.CurrentArea = "Hand";
            owner.Hand.CardsInHand.Add(newCard);

            // Clear field of used cards
            owner.Field.ClearField();
        }
    }

    void MoveDealtCard()
    {
        if (owner.Hand.CardsInHand.Count < owner.CurrentMaxHandSize)
        {
            Card newCard = DealCard();
            // check card is null or not
            if (newCard == null)
            {
                Debug.Log("Out of Cards");
                showReset = true;
                return;
            }

            //newCard.transform.position = Vector3.zero;
            newCard.transform.position = new Vector3(((float)owner.Hand.CardsInHand.Count * 2) - 5, -5, (float)owner.Hand.CardsInHand.Count * -0.01f); // place card 1/4 up on all axis from last
            newCard.CurrentArea = "Hand";
            owner.Hand.CardsInHand.Add(newCard); // add card to hand
        }
    }
}
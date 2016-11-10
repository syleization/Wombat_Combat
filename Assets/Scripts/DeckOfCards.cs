using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckOfCards : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();

    private List<GameObject> cards = new List<GameObject>();
    private List<GameObject> hand = new List<GameObject>();
    private const int maxHandSize = 6;
    private int cardsDealt = 0;
    private bool showReset = false;
    public bool isHoldingCard = false; // should eventually be in a hand manager or game manager

    void ResetDeck()
    {
        cardsDealt = 0;
        for (int i = 0; i < hand.Count; i++)
        {
            Destroy(hand[i]);
        }
        hand.Clear();
        cards.Clear();
        cards.AddRange(deck);
        showReset = false;
    }

    GameObject DealCard()
    {
        if (cards.Count == 0)
        {
            showReset = true;
            return null;
            //Alternatively to auto reset the deck:
            //ResetDeck();
        }

        int card = Random.Range(0, cards.Count - 1);
        GameObject go = GameObject.Instantiate(cards[card]) as GameObject;

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
        cardsDealt = 0;
        for (int v = 0; v < hand.Count; v++)
        {
            Destroy(hand[v]);
        }
        hand.Clear();
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
    }

    void MoveDealtCard()
    {
        if (cardsDealt < maxHandSize)
        {
            GameObject newCard = DealCard();
            // check card is null or not
            if (newCard == null)
            {
                Debug.Log("Out of Cards");
                showReset = true;
                return;
            }

            //newCard.transform.position = Vector3.zero;
            newCard.transform.position = new Vector3(((float)cardsDealt * 2) - 5, -5, (float)cardsDealt * -1); // place card 1/4 up on all axis from last
            hand.Add(newCard); // add card to hand
            cardsDealt++;
        }
    }

    public int GetHandSize()
    {
        return cardsDealt;
    }

    public void ResetHandCardPositions(CardPopUp cardMoved)
    {
        for (int i = 0; i < cardsDealt - 1; ++i)
        {
            if(hand[i] == cardMoved.gameObject)
            {
                // If the current card is the card that is moving then swap its position in the hand to the end
                GameObject temp = hand[i];
                hand[i] = hand[cardsDealt - 1];
                hand[cardsDealt - 1] = temp;
            }
            hand[i].transform.position = new Vector3(((float)i * 2) - 5, -5, (float)i * -1);
        }
    }
}
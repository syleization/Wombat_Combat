using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DeckOfCards : MonoBehaviour
{
    const int DeckSize = 80;
    public List<Card> deck = new List<Card>();

    private List<Card> cards = new List<Card>();

    public Player owner;
   // private bool showReset = false;

    // SHOULD ONLY EVER BE CALLED ONCE BY GLOBALSETTINGS
    public void Initialize()
    {
        for(int i = 0; i < DeckSize; ++i)
        {
            deck.Add(GetRandomBasicCard());
        }
        cards.AddRange(deck);
    }

    //void OnGUI()
    //{
    //    //if (!showReset)
    //    //{
    //    //    // Deal button
    //    //    if (GUI.Button(new Rect(10, 10, 100, 20), "Deal"))
    //    //    {
    //    //        MoveDealtCard();
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    //// Reset button
    //    //    //if (GUI.Button(new Rect(10, 10, 100, 20), "Reset"))
    //    //    //{
    //    //    //    ResetDeck();
    //    //    //}
    //    //}
    //    //// GameOver button
    //    //if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 20), "ClearHand"))
    //    //{
    //    //    GameOver();
    //    //}

    //    // Merge Button
       
    //}

    Card GetRandomBasicCard()
    {
        switch(Random.Range(0, 7))
        {
            case 6:
            case 0:
                return GlobalSettings.Instance.Attack_DonkeyKick;
            case 1:
                return GlobalSettings.Instance.Attack_WombatCharge;
            case 2:
                return GlobalSettings.Instance.Defence_Bark;
            case 3:
                return GlobalSettings.Instance.Defence_Bite;
            case 4:
                return GlobalSettings.Instance.Trap_Sinkhole;
            case 5:
                return GlobalSettings.Instance.Trap_Trampoline;
            default:
                Debug.Log("ERROR[DeckOfCards::GetRandomBasicCard] | Random range isnt returning a valid value");
                break;
        }
        return null;
    }

    //void ResetDeck()
    //{
    //    owner = TurnManager.Instance.GetCurrentPlayer();
    //    if (owner != null)
    //    {
    //        for (int i = 0; i < owner.Hand.CardsInHand.Count; i++)
    //        {
    //            Destroy(owner.Hand.CardsInHand[i]);
    //        }
    //        owner.Hand.CardsInHand.Clear();
    //    }
    //    cards.Clear();
    //    cards.AddRange(deck);
    //    showReset = false;
        
    //}

    Card DealCard()
    {
        if (deck.Count == 0)
        {
            //showReset = true;
            return null;
            //Alternatively to auto reset the deck:
            //ResetDeck();
        }

        int cardIndex = Random.Range(0, cards.Count - 1);

        // Dont instantiate it for non local players
        Card go;

        go = Instantiate<Card>(cards[cardIndex]);

        go.owner = TurnManager.Instance.GetCurrentPlayer();
        deck.Remove(cards[cardIndex]);

        if (cards.Count == 0)
        {
            //showReset = true;
        }

        return go;
    }

    void GameOver()
    {
        owner = TurnManager.Instance.GetCurrentPlayer();
        if (owner != null)
        {
            for (int v = 0; v < owner.Hand.CardsInHand.Count; v++)
            {
                Destroy(owner.Hand.CardsInHand[v].gameObject);
            }
            owner.Hand.CardsInHand.Clear();
        }
        cards.Clear();
        cards.AddRange(deck);
    }

    public void MoveDealtCard()
    {
        owner = TurnManager.Instance.GetCurrentPlayer();
        if (owner.Hand.CardsInHand.Count < owner.CurrentMaxHandSize)
        {
            Card newCard = DealCard();
            // check card is null or not
            if (newCard == null)
            {
                Debug.Log("Out of Cards");
                //showReset = true;
                return;
            }
            TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);

            newCard.CurrentArea = "Hand";
            owner.Hand.CardsInHand.Add(newCard); // add card to hand
        }
    }

    public static void TransformDealtCardToHand(Card newCard, int Spacing)
    {
        newCard.transform.position = new Vector3(((float)Spacing * 2) - 5, newCard.owner.Hand.transform.position.y, (float)Spacing * -0.01f - 0.01f);
        newCard.transform.rotation = new Quaternion(newCard.owner.Hand.transform.rotation.x, newCard.owner.Hand.transform.rotation.y, newCard.owner.Hand.transform.rotation.z, newCard.owner.Hand.transform.rotation.w);
    }
}
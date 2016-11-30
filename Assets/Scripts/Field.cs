﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    // As of now the functionality only supports one field - this may be enough though
    public List<Card> CardsInField = new List<Card>();
    private static int MaxFieldSize;


    void Awake()
    {
        MaxFieldSize = 2;
    }

    public static void ChangeMaxFieldSize(Stage currentStage)
    {
        if(currentStage == Stage.Merge)
        {
            MaxFieldSize = 2;
        }
        else if(currentStage == Stage.Play)
        {
            MaxFieldSize = 1;
        }
    }

    public bool CanBePlaced()
    {
        return CardsInField.Count < MaxFieldSize ? true : false;
    }

    public Card GetCard(int index)
    {
        return index < CardsInField.Count ? CardsInField[index] : null;
    }

    public void ResetFieldCardPositions()
    {
        if (MaxFieldSize == 2)
        {
            if (GetCard(0) != null) // if first card in the list exists
            {
                // Move to the left spot in the field
                CardsInField[0].transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
            }
            if (GetCard(1) != null) // if the second card in the list exists
            {
                CardsInField[1].transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, 0);
            }
        }
        else if(MaxFieldSize == 1) // Happens when a card is played during the play stage
        {
            if (GetCard(0) != null) // if first card in the list exists
            {
                // Move to the middle spot in the field
                CardsInField[0].transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }
    }

    public CardType IsMergable()
    {
        // If there are two cards in the field - their types match - they aren't power cards
        return (GetCard(0) != null && GetCard(1) != null) && (GetCard(0).Type == GetCard(1).Type) && (GetCard(0).IsPowerCard == false && GetCard(1).IsPowerCard == false) ? GetCard(0).Type : CardType.None;
    }

    public void ClearField()
    {
        foreach(Card c in CardsInField)
        {
            Destroy(c.gameObject);
        }
        CardsInField.Clear();
    }

    public static void SendFieldBackToHand(Player currentPlayer)
    {
        // Move cards from field back to hand
        // Needs to be used because the cardsinfield count changes within the loop
        int originalFieldCount = currentPlayer.Field.CardsInField.Count;
        for (int i = 0; i < originalFieldCount; ++i)
        {
            // Since cards are removed each loop the current card will always be the first element in the field
            Card currentCard = currentPlayer.Field.GetCard(0);
            if (currentCard != null)
            {
                CardPopUp popup = currentCard.GetComponent<CardPopUp>();
                currentCard.CurrentArea = "Hand";
                currentCard.IsInHand = true;
                popup.cardIsDown = true;
                currentPlayer.Hand.CardsInHand.Add(currentCard);
                currentPlayer.Field.CardsInField.Remove(currentCard);

                DeckOfCards.TransformDealtCardToHand(currentCard, currentCard.owner.Hand.CardsInHand.Count - 1);
            }
        }
    }
}

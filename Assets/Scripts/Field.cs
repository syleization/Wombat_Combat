using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Field : NetworkBehaviour
{
    // As of now the functionality only supports one field - this may be enough though
    public List<Card> CardsInField = new List<Card>();
    [SyncVar]
    private int MaxFieldSize;
    [SyncVar]
    private int CurrentDamage = 0;
    public int CurrentDamageInField
    {
        set
        {
            CurrentDamage = value;
        }
        get
        {
            return CurrentDamage;
        }
    }
    private static Field TheInstance;
    private Field() { }

    public static Field Instance
    {
        get
        {
            if (TheInstance == null)
            {
                TheInstance = FindObjectOfType<Field>();
            }

            return TheInstance;
        }
    }

    void Awake()
    {
        TheInstance = this;
        MaxFieldSize = 2;
    }

    [ClientRpc]
    public void RpcClearField()
    {
        Instance.ClearField();
    }

    [ClientRpc]
    public void RpcAddCardToField(CardSubType subType)
    {
        if (Instance.CanBePlaced() && Instance.CardsInField.Count == 0)
        {
            Instance.CardsInField.Add(Instantiate<Card>(GlobalSettings.Instance.GetCardOfSubType(subType)));
            Instance.ResetFieldCardPositions();
        }
    }

    [ClientRpc]
    public void RpcRemoveCardFromField(int index)
    {
        if (Instance.GetCard(index) != null)
        {
            Instance.RemoveCard(index);
            Instance.ResetFieldCardPositions();
        }
    }

    public void ChangeMaxFieldSize(Stage currentStage)
    {
        if(currentStage == Stage.Merge || currentStage == Stage.Reaction)
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

    public bool IsMergable()
    {
        // If there are two cards in the field - their types match - they aren't power cards
        return (GetCard(0) != null && GetCard(1) != null) && (GetCard(0).Type == GetCard(1).Type) && (GetCard(0).Level == GetCard(1).Level) && (GetCard(0).Level != CardLevel.Three && GetCard(1).Level != CardLevel.Three) ? true : false;
    }

    public void ClearField()
    {
        foreach(Card c in CardsInField)
        {
            if(c != null)
                Destroy(c.gameObject);
        }
        CardsInField.Clear();
        CurrentDamage = 0;
    }

    public void SendFieldBackToHand(Player currentPlayer)
    {
        // Move cards from field back to hand
        // Needs to be used because the cardsinfield count changes within the loop
        int originalFieldCount = Field.Instance.CardsInField.Count;
        for (int i = 0; i < originalFieldCount; ++i)
        {
            // Since cards are removed each loop the current card will always be the first element in the field
            Card currentCard = Field.Instance.GetCard(0);
            if (currentCard != null)
            {
                CardPopUp popup = currentCard.GetComponent<CardPopUp>();
                currentCard.CurrentArea = "Hand";
                currentCard.IsInHand = true;
                popup.cardIsDown = true;
                currentPlayer.Hand.CardsInHand.Add(currentCard);
                Instance.CardsInField.Remove(currentCard);

                DeckOfCards.TransformDealtCardToHand(currentCard, currentCard.owner.Hand.CardsInHand.Count - 1);
            }
        }
    }

    public void RemoveCard(int index)
    {
        Card card = Instance.GetCard(index);
        Instance.CardsInField.Remove(card);
        Destroy(card.gameObject);
    }

    // Don't put parameters if you want the card hidden forever
    public void HideCards(float timer = 0)
    {
        foreach(Card card in CardsInField)
        {
            card.gameObject.SetActive(false);
        }
        if (timer != 0)
        {
            StartCoroutine(ShowCards(timer));
        }
    }

    IEnumerator ShowCards(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foreach (Card card in CardsInField)
        {
            card.gameObject.SetActive(true);
        }
    }

}

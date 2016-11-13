using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    // As of now the functionality only supports one field - this may be enough though
    public List<Card> CardsInField = new List<Card>();
    private int MaxFieldSize = 2;

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
        if(GetCard(0) != null) // if first card in the list exists
        {
            // Move to the left spot in the field
            CardsInField[0].transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        }
        if(GetCard(1) != null) // if the second card in the list exists
        {
            CardsInField[1].transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, 0);
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
}

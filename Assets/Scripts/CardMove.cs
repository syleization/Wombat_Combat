using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private CardPopUp Card;
    private DeckOfCards Deck;


    void Start()
    {
        Card = GetComponent<CardPopUp>();
        Deck = FindObjectOfType<DeckOfCards>();
    }

    void OnMouseDown()
    {
        // When card is clicked it is no longer in hand
        Deck.isHoldingCard = true;
        screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
        Deck.ResetHandCardPositions(Card);
    }

    void OnMouseDrag()
    {
        // Move the card around with the cursor
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 5);

        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = currentPosition;
    }

    void OnMouseUp()
    {
        // Check if you are releasing the card back to the hand
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // This raycast goes past the card to see the hand
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Card"))))
        {
            Debug.Log(hit.collider.tag.ToString());
            if (hit.collider.tag == "Hand")
            {
                // Card is now in hand
                transform.position = new Vector3((((float)Deck.GetHandSize() - 1) * 2) - 5, -5, ((float)Deck.GetHandSize() - 1) * -1);
                Deck.isHoldingCard = false;
                Card.cardIsDown = true;
            }
        }
    }
}

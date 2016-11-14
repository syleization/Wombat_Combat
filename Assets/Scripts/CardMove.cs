using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private CardPopUp CardPopUp;
    private Card Card;
    
    void Start()
    {
        CardPopUp = GetComponent<CardPopUp>();
        Card = GetComponent<Card>();
    }

    void OnMouseDown()
    {
        if (Card.owner.HasPermission())
        {
            // When card is clicked it is no longer in hand
            Card.owner.IsHoldingCard = true;
            screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
            if (Card.CurrentArea == "Hand")
            {
                Card.owner.Hand.ResetHandCardPositions(Card, Card.owner.Hand.CardsInHand.Count);
            }
        }
    }

    void OnMouseDrag()
    {
        if (Card.owner.HasPermission())
        {
            // Move the card around with the cursor
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 0.1f);

            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = currentPosition;
        }
    }

    void OnMouseUp()
    {
        if (Card.owner.HasPermission())
        {
            // Check if you are releasing the card back to the hand
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // This raycast goes past the card to see the hand
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Card"))))
            {
                if (hit.collider.tag == "Hand" && Hand.IsYourHand(Card, hit.collider.gameObject))
                {
                    if (Card.CurrentArea == "Field")
                    {
                        // Add card to hand
                        Card.owner.Hand.CardsInHand.Add(Card);
                        // Remove card from previous spot
                        Card.owner.Field.CardsInField.Remove(Card);
                    }
                    Card.owner.Field.ResetFieldCardPositions();
                    // Card is now in hand
                    SnapBackToHand();
                    Card.CurrentArea = "Hand";
                    Card.IsInHand = true;
                }
                else if(hit.collider.tag == "Field")
                {
                    if (Card.owner.Field.CanBePlaced())
                    {
                        if (Card.CurrentArea == "Hand")
                        {
                            // Add card to field
                            Card.owner.Field.CardsInField.Add(Card);
                            // Remove card from previous spot
                            Card.owner.Hand.CardsInHand.Remove(Card);
                        }
                        // Move card to the field positions
                        SnapBackToField();
                        Card.CurrentArea = "Field";
                        Card.IsInHand = false;
                    }
                    else
                    {
                        SnapBackToOrigin();
                    }
                }
                else
                {
                    SnapBackToOrigin();
                }
            }
            else // it hit nothing so return the card back to its previous place
            {
                SnapBackToOrigin();
            }
        }
    }

    void SnapBackToOrigin()
    {
        if (Card.CurrentArea == "Hand")
        {
            SnapBackToHand();
        }
        else if (Card.CurrentArea == "Field")
        {
            SnapBackToField();
        }
    }

    void SnapBackToHand()
    {
        //transform.position = new Vector3((((float)Card.owner.Hand.CardsInHand.Count - 1) * 2) - 5, -5, ((float)Card.owner.Hand.CardsInHand.Count - 1) * -0.01f);
        DeckOfCards.TransformDealtCardToHand(Card, Card.owner.Hand.CardsInHand.Count - 1);
        Card.owner.IsHoldingCard = false;
        CardPopUp.cardIsDown = true;
        Card.owner.Hand.ResetHandCardPositions(Card, Card.owner.Hand.CardsInHand.Count);
    }

    void SnapBackToField()
    {
        Card.owner.Field.ResetFieldCardPositions();
        Card.owner.IsHoldingCard = false;
        CardPopUp.cardIsDown = true;
    }
}

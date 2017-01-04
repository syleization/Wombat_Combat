using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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
        if (Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction
            || (TurnManager.Instance.currentStage == Stage.Reaction && Card.owner == CardActions.theReactor))
        {
            if (Card.owner.CurrentActions > 0)
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
    }

    void OnMouseDrag()
    {
        if ((Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction)
            || (TurnManager.Instance.currentStage == Stage.Reaction && Card.owner == CardActions.theReactor))
        {
            if (Card.owner.CurrentActions > 0)
            {
                // Move the card around with the cursor
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 0.1f);

                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                transform.position = currentPosition;
            }
        }
    }

    void OnMouseUp()
    {
        if (Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction)
        {
            if (Card.owner.CurrentActions > 0)
            {
                // Check if you are releasing the card back to the hand
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // This raycast goes past the card to see the hand
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Card"))))
                {
                    if (hit.collider.tag == "Hand")
                    {
                        if (Hand.IsYourHand(Card, hit.collider.gameObject))
                        {
                            if (Card.CurrentArea == "Field")
                            {
                                // Add card to hand
                                Card.owner.Hand.CardsInHand.Add(Card);
                                // Remove card from previous spot
                                Field.Instance.CardsInField.Remove(Card);
                            }
                            Field.Instance.ResetFieldCardPositions();
                            // Card is now in hand
                            SnapBackToHand();
                            Card.CurrentArea = "Hand";
                            Card.IsInHand = true;
                        }
                        else if (TurnManager.Instance.currentStage == Stage.Play)
                        {
                            // Find out whose hand was hit
                            Player target = Hand.GetOwner(hit.collider.gameObject);

                            // If card can target other players
                            if (Card.GetCanTarget())
                            {
                                if (Card.CurrentArea == "Hand")
                                {
                                    // The card is added to the field array, but not displayed in the field 
                                    // This is so we can keep accessing the card from anywhere and as the wombat bounces around
                                    // All players know what type of wombat is bouncing around
                                    Field.Instance.CardsInField.Add(Card);
                                    Card.owner.Hand.CardsInHand.Remove(Card);
                                }
                                // Move card to the field positions
                                SnapBackToField();
                                Card.CurrentArea = "Field";
                                Card.IsInHand = false;

                                PlayCard(target, Card);
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
                    else if (hit.collider.tag == "Field")
                    {
                        if (Field.Instance.CanBePlaced())
                        {
                            if (Card.CurrentArea == "Hand")
                            {
                                // Add card to field
                                Field.Instance.CardsInField.Add(Card);
                                // Remove card from previous spot
                                Card.owner.Hand.CardsInHand.Remove(Card);

                                if (TurnManager.Instance.currentStage == Stage.Play && Card.SubType == CardSubType.DonkeyKick)
                                {
                                    PlayCard(TurnManager.Instance.GetCurrentPlayer(), Field.Instance.GetCard(0));
                                }
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
        // if a player is reacting to a wombat being thrown at them and they are the one moving the card
        else if (TurnManager.Instance.currentStage == Stage.Reaction && Card.owner == CardActions.theReactor)
        {
            if (Card.owner.CurrentActions > 0 && (Card.Type == CardType.Defence || Card.Type == CardType.Trap))
            {
                // Check if you are releasing the card back to the hand
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // This raycast goes past the card to see the hand
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Card"))))
                {
                    if (Field.Instance.CanBePlaced())
                    {
                        if (Card.CurrentArea == "Hand")
                        {
                            // Add card to field
                            Field.Instance.CardsInField.Add(Card);
                            // Remove card from previous spot
                            CardActions.theReactor.Hand.CardsInHand.Remove(Card);
                            // Play the defence card | first parameter is irrelevant
                            PlayCard(TurnManager.Instance.GetCurrentPlayer(), Card);
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
            else
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
        DeckOfCards.TransformDealtCardToHand(Card, Card.owner.Hand.CardsInHand.Count - 1);
        Card.owner.IsHoldingCard = false;
        CardPopUp.cardIsDown = true;
        Card.owner.Hand.ResetHandCardPositions(Card, Card.owner.Hand.CardsInHand.Count);
    }

    void SnapBackToField()
    {
        Field.Instance.ResetFieldCardPositions();
        Card.owner.IsHoldingCard = false;
        CardPopUp.cardIsDown = true;
    }

    void PlayCard(Player target, Card cardUsed)
    {
        CardSubType subTypeOfCard = cardUsed.SubType;
        --cardUsed.owner.CurrentHandSize;
        if (cardUsed.Type == CardType.Attack)
        {
            if(subTypeOfCard == CardSubType.DonkeyKick)
            {
                Field.Instance.CurrentDamageInField = GlobalSettings.Damage_DonkeyKick;
                // Display Card and save damage for everyone else
                if (Card.owner.isServer)
                {
                    Field.Instance.RpcAddCardToField(CardSubType.DonkeyKick);
                }
                else
                {
                    Card.owner.CmdChangeDamageInField(GlobalSettings.Damage_DonkeyKick);
                    Card.owner.CmdAddCardToField(CardSubType.DonkeyKick);
                }

                CardActions.DonkeyKick(TurnManager.Instance.GetCurrentPlayer());
            }
            else if(subTypeOfCard == CardSubType.WombatCharge)
            {
                Field.Instance.CurrentDamageInField = GlobalSettings.Damage_WombatCharge;

                // Display Card for everyone else
                if (Card.owner.isServer)
                {
                    Field.Instance.RpcAddCardToField(CardSubType.WombatCharge);
                }
                else
                {
                    Card.owner.CmdChangeDamageInField(GlobalSettings.Damage_WombatCharge);
                    Card.owner.CmdAddCardToField(CardSubType.WombatCharge);
                }

                CardActions.WombatCharge(TurnManager.Instance.GetCurrentPlayer(), target);
            }
            else if(subTypeOfCard == CardSubType.WomboCombo)
            {
                Field.Instance.CurrentDamageInField = GlobalSettings.Damage_WomboCombo;

                // Display Card for everyone else
                if (Card.owner.isServer)
                {
                    Field.Instance.RpcAddCardToField(CardSubType.WomboCombo);
                }
                else
                {
                    Card.owner.CmdChangeDamageInField(GlobalSettings.Damage_WomboCombo);
                    Card.owner.CmdAddCardToField(CardSubType.WomboCombo);
                }

                CardActions.WomboCombo(TurnManager.Instance.GetCurrentPlayer(), target);
            }
        }
        else if (cardUsed.Type == CardType.Defence)
        {
            if (subTypeOfCard == CardSubType.Bark)
            {
                CardActions.Bark(TurnManager.Instance.GetCurrentPlayer(), CardActions.theReactor);
            }
            else if (subTypeOfCard == CardSubType.Bite)
            {
                CardActions.Bite(CardActions.theReactor);
            }
            else if (subTypeOfCard == CardSubType.GooglyEyes)
            {
                CardActions.GooglyEyes(CardActions.theThrower, CardActions.theReactor);
            }

            // After a defence card is used re check if the player still has defence cards
            Card.owner.HasDefenceCards = Card.owner.Hand.HasDefenceCards();

            if (!Card.owner.isServer)
            {
                Card.owner.CmdChangeHasDefenceCards(Card.owner.HasDefenceCards);
            }

        }
        else if (cardUsed.Type == CardType.Trap)
        {
            if (subTypeOfCard == CardSubType.Sinkhole)
            {
                CardActions.Sinkhole(CardActions.theThrower, CardActions.theReactor);
            }
            else if (subTypeOfCard == CardSubType.Trampoline)
            {
                CardActions.Trampoline(CardActions.theThrower, CardActions.theReactor);
            }
            else if (subTypeOfCard == CardSubType.WombatCage)
            {
                CardActions.WombatCage(CardActions.theThrower, CardActions.theReactor);
            }

            Card.owner.HasDefenceCards = Card.owner.Hand.HasDefenceCards();

            if (!Card.owner.isServer)
            {
                Card.owner.CmdChangeHasDefenceCards(Card.owner.HasDefenceCards);
            }
        }
    }
}

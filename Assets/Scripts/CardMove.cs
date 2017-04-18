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
#if UNITY_ANDROID
    // Touch System
    void Update()
    {
        if(Input.touchCount == 1 && CardPopUp.CardCanMoveNow == true)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            // This raycast goes past the card to see the hand
            if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.collider.gameObject == Card.gameObject)
                //if (transform.GetComponent<Collider>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position))) 
            {
                if (touch.phase == TouchPhase.Stationary && !Card.owner.IsHoldingCard)
                {
                    CardPopUp.ClearInfoBox();
                    MobileOnMouseDown(touch);
                }
                else if (touch.phase == TouchPhase.Ended && Card.owner.IsHoldingCard)
                {
                    MobileOnMouseUp(touch);
                    CardPopUp.CardCanMoveNow = false;
                }
                else if (touch.phase == TouchPhase.Moved && Card.owner.IsHoldingCard)
                {
                    MobileOnMouseDrag(touch);
                }
            }
        }
    }

    void MobileOnMouseDown(Touch touch)
    {
        if(Pause.Instance.IsPaused == false)
        {
            if ((Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
                    || (TurnManager.Instance.currentStage == Stage.Reaction && Card.owner == CardActions.theReactor))
                {
                    if (Card.owner.CurrentActions > 0)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, -3.0f);
                        // When card is clicked it is no longer in hand
                        Card.owner.IsHoldingCard = true;
                
                        if (Card.CurrentArea == "Hand")
                        {
                            Card.owner.Hand.ResetHandCardPositions(Card, Card.owner.Hand.CardsInHand.Count);
                        }
                        else if(Card.CurrentArea == "TrapZone")
                        {
                            Card.owner.Traps.ToggleActive(Card);
                            CanvasManager.Instance.UpdateCanvas("Trap");
                        }
                    }
                }
        }
    }

    void MobileOnMouseDrag(Touch touch)
    {
        if (Pause.Instance.IsPaused == false)
        {
            if ((Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
                || (TurnManager.Instance.currentStage == Stage.Reaction && Card.owner == CardActions.theReactor))
            {
                if (Card.owner.CurrentActions > 0)
                {
                    // Move the card around with the cursor
                    Vector3 curScreenPoint = new Vector3(touch.position.x, touch.position.y, 9.0f);

                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                   // currentPosition.z = -1;
                    transform.position = currentPosition;
                }
            }
        }
    }

    void MobileOnMouseUp(Touch touch)
    {
        if (Pause.Instance.IsPaused == false)
        {
            if (Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
            {
                if (Card.owner.CurrentActions > 0)
                {
                    // Check if you are releasing the card back to the hand
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
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
                                if (TurnManager.Instance.currentStage != Stage.Play || (TurnManager.Instance.currentStage == Stage.Play && Card.Type != CardType.Defence))
                                {
                                    if (Card.CurrentArea == "Hand")
                                    {
                                        // Add card to field
                                        Field.Instance.CardsInField.Add(Card);
                                        // Remove card from previous spot
                                        Card.owner.Hand.CardsInHand.Remove(Card);

                                        if (TurnManager.Instance.currentStage == Stage.Play)
                                        {
                                            if (Card.SubType == CardSubType.DonkeyKick)
                                            {
                                                PlayCard(TurnManager.Instance.GetCurrentPlayer(), Field.Instance.GetCard(0));
                                            }
                                            else if(Card.Type == CardType.Trap)
                                            {
                                                PlayCard(TurnManager.Instance.GetCurrentPlayer(), Field.Instance.GetCard(0));
                                                Card.IsInHand = false;
                                                return;
                                            }
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
                if (Card.owner.CurrentActions > 0 && (Card.Type == CardType.Defence 
                    || (Card.Type == CardType.Trap && Card.CurrentArea == "TrapZone")))
                {
                    // You are trying to play a trap against wombo combo
                    if (Card.Type == CardType.Trap && Field.Instance.CurrentDamageInField == GlobalSettings.Instance.GetDamageAmountOf(CardSubType.WomboCombo))
                    {
                        SnapBackToOrigin();
                        return;
                    }
                    // Check if you are releasing the card back to the hand
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
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
                            else if(Card.CurrentArea == "TrapZone")
                            {
                                // Add card to field
                                Field.Instance.CardsInField.Add(Card);
                                // Remove card from TrapZone
                                Card.owner.Traps.RemoveTrap(Card);
                                // Play the trap card | first parameter is irrelevant
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
    }

#else

    void OnMouseDown()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if ((Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
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
                    else if (Card.CurrentArea == "TrapZone")
                    {
                        Card.owner.Traps.ToggleActive(Card);
                        CanvasManager.Instance.UpdateCanvas("Trap");
                    }
                }
            }
        }
    }

    void OnMouseDrag()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if ((Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
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
    }

    void OnMouseUp()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if (Card.owner.HasPermission() && TurnManager.Instance.currentStage != Stage.Reaction && Card.CurrentArea != "TrapZone")
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
                                if (TurnManager.Instance.currentStage != Stage.Play || (TurnManager.Instance.currentStage == Stage.Play && Card.Type != CardType.Defence))
                                {
                                    if (Card.CurrentArea == "Hand")
                                    {
                                        // Add card to field
                                        Field.Instance.CardsInField.Add(Card);
                                        // Remove card from previous spot
                                        Card.owner.Hand.CardsInHand.Remove(Card);

                                        if (TurnManager.Instance.currentStage == Stage.Play)
                                        {
                                            if (Card.SubType == CardSubType.DonkeyKick)
                                            {
                                                PlayCard(TurnManager.Instance.GetCurrentPlayer(), Field.Instance.GetCard(0));
                                            }
                                            else if (Card.Type == CardType.Trap)
                                            {
                                                PlayCard(TurnManager.Instance.GetCurrentPlayer(), Field.Instance.GetCard(0));
                                                Card.IsInHand = false;
                                                return;
                                            }
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
                if (Card.owner.CurrentActions > 0 && (Card.Type == CardType.Defence
                    || (Card.Type == CardType.Trap && Card.CurrentArea == "TrapZone")))
                {
                    // You are trying to play a trap against wombo combo
                    if (Card.Type == CardType.Trap && Field.Instance.CurrentDamageInField == GlobalSettings.Instance.GetDamageAmountOf(CardSubType.WomboCombo))
                    {
                        SnapBackToOrigin();
                        return;
                    }
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
                            else if (Card.CurrentArea == "TrapZone")
                            {
                                // Add card to field
                                Field.Instance.CardsInField.Add(Card);
                                // Remove card from TrapZone
                                Card.owner.Traps.RemoveTrap(Card);
                                // Play the trap card | first parameter is irrelevant
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
    }
#endif

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
        else if(Card.CurrentArea == "TrapZone")
        {
            SnapBackToTrapZone();
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

    void SnapBackToTrapZone()
    {
        Card.owner.Traps.RemoveTrap(Card);
        Card.owner.Traps.SetTrap(Card);
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
            if(TurnManager.Instance.currentStage == Stage.Play)
            {
                if (Card.owner.Traps.SetTrap(Card) == false)
                {
                    Field.Instance.CardsInField.Remove(Card);
                    Card.owner.Hand.CardsInHand.Add(Card);
                    SnapBackToHand();
                    ++cardUsed.owner.CurrentHandSize;
                }
                else
                {
                    Card.owner.HasTrapCards = Card.owner.Traps.HasTraps();

                    --Card.owner.CurrentActions;
                    if (!Card.owner.isServer)
                    {
                        Card.owner.CmdChangeHasTrapCards(Card.owner.HasTrapCards);
                        Card.owner.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(Card.owner), Card.owner.CurrentActions);
                    }
                }
            }
            else
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

                Card.owner.HasTrapCards = Card.owner.Traps.HasTraps();

                if (!Card.owner.isServer)
                {
                    Card.owner.CmdChangeHasTrapCards(Card.owner.HasTrapCards);
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public enum CardType { Attack, Defence, Trap, None }
// Ordered to determine level
public enum CardSubType { DonkeyKick, Bark, Trampoline, WombatCharge, Bite, Sinkhole, WomboCombo, GooglyEyes, WombatCage, None}
public enum CardLevel { One, Two, Three, None }
public class Card : NetworkBehaviour
{
    public Player owner;
    [Tooltip("Areas are 'Hand', 'Field', 'TrapZone'")]
    public string CurrentArea;
    public CardLevel Level;
    public bool IsInHand;
    public CardType Type;
    public CardSubType SubType;
    private bool CanTarget;
    public Sprite OriginalSprite;
    public bool GetCanTarget() { return CanTarget; }

    void Awake()
    {
        owner = TurnManager.Instance.GetCurrentPlayer();
        IsInHand = true;
        if(Type == CardType.Attack && SubType != CardSubType.DonkeyKick)
        {
            CanTarget = true;
        }
        else
        {
            CanTarget = false;
        }

        // Set the card's level
        int level = (int)SubType;
        const int levelOneCardsCount = 3;
        const int levelTwoCardsCount = 3;
        const int levelThreeCardsCount = 3;
        Level = (level < levelOneCardsCount) ? CardLevel.One : CardLevel.None;

        if (Level == CardLevel.None)
        {
            Level = (level < levelOneCardsCount + levelTwoCardsCount) ? CardLevel.Two : CardLevel.None;
            if (Level == CardLevel.None)
            {
                Level = (level < levelOneCardsCount + levelTwoCardsCount + levelThreeCardsCount) ? CardLevel.Three : CardLevel.None;
            }
            Debug.Assert(Level != CardLevel.None, "[Card::Start] Problem with level initialization");
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        OriginalSprite = spriteRenderer.sprite;
    }

    //public Card(CardType type)
    //{
    //    Type = type;
    //    if(type == CardType.Attack)
    //    {
    //        CanTarget = true;
    //    }
    //    else
    //    {
    //        CanTarget = false;
    //    }
    //}
}

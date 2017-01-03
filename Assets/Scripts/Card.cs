using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public enum CardType { Attack, Defence, Trap, None }
public enum CardSubType { DonkeyKick, WombatCharge, WomboCombo, Bark, Bite, GooglyEyes, Trampoline, Sinkhole, WombatCage, None}
public class Card : NetworkBehaviour
{
    public Player owner;
    public string CurrentArea;
    public bool IsPowerCard;
    public bool IsInHand;
    public CardType Type;
    public CardSubType SubType;
    private bool CanTarget;
    public Sprite OriginalSprite;
    public bool GetCanTarget() { return CanTarget; }

    void Start()
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

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        OriginalSprite = spriteRenderer.sprite;
    }

    public Card(CardType type, bool powerCard = false)
    {
        IsPowerCard = powerCard;
        Type = type;
        if(type == CardType.Attack)
        {
            CanTarget = true;
        }
        else
        {
            CanTarget = false;
        }
    }
}

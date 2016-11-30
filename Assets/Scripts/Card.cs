using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum CardType { Attack, Defence, Trap, None }
public enum CardSubType { DonkeyKick, WombatCharge, WomboCombo, Bark, Bite, GooglyEyes, Trampoline, Sinkhole, WombatCage, None}
public class Card : MonoBehaviour
{
    public Player owner;
    public string CurrentArea;
    public bool IsPowerCard;
    public bool IsInHand;
    public CardType Type;
    public CardSubType SubType;
    private bool CanTarget;

    bool GetCanTarget() { return CanTarget; }

    //public bool CanBePlayed
    //{
    //    get
    //    {
    //        /*
    //        if it is the owners turn
    //        if the owner hasnt played 3 cards this turn

    //        */
    //    }
    //}
    void Start()
    {
        owner = TurnManager.Instance.GetCurrentPlayer();
        IsInHand = true;
        if(Type == CardType.Attack)
        {
            CanTarget = true;
        }
        else
        {
            CanTarget = false;
        }
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

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Hand Hand;
    public int CurrentMaxHandSize;
    public bool IsHoldingCard;
    public bool IsTurn;
    private string PlayersName;
    public string Name
    {
        set
        {
            PlayersName = value;
        }
    }
    private int PlayersCurrentHealth;
    public int CurrentHealth
    {
        get
        {
            return PlayersCurrentHealth;
        }
        set
        {
            PlayersCurrentHealth = value;

            if(PlayersCurrentHealth < 0)
            {
                PlayersCurrentHealth = 0;
            }
        }
    }
    private const int MaxHealth = 15;
    public const int MaxActions = 4;
    public int PlayersCurrentActions;
    public int CurrentActions
    {
        get
        {
            return PlayersCurrentActions;
        }
        set
        {
            PlayersCurrentActions = value;

            if(PlayersCurrentActions < 0)
            {
                PlayersCurrentActions = 0;
            }
            else if(PlayersCurrentActions > MaxActions)
            {
                PlayersCurrentActions = MaxActions;
            }
        }
    }

    void Awake()
    {
        Hand = GetComponentInChildren<Hand>();
        CurrentMaxHandSize = 7;
        CurrentHealth = MaxHealth;
        CurrentActions = MaxActions;
    }
    
    public bool HasPermission()
    {
        return IsTurn ? true : false;
    }

    public override string ToString()
    {
        return PlayersName;
    }
    
}

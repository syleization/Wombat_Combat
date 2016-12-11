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

    void Awake()
    {
        Hand = GetComponentInChildren<Hand>();
        CurrentMaxHandSize = 7;
        CurrentHealth = MaxHealth;
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

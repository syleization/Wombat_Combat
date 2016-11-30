using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Hand Hand;
    public Field Field;
    public int CurrentMaxHandSize;
    public bool IsHoldingCard;
    public bool IsTurn;
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
        Field = FindObjectOfType<Field>();
        Hand = GetComponentInChildren<Hand>();
        CurrentMaxHandSize = 7;
        CurrentHealth = MaxHealth;
    }
    
    public bool HasPermission()
    {
        return IsTurn ? true : false;
    }
}

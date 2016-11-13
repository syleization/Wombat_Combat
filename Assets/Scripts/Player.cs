using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // As of now the functionality only supports one player
    public Hand Hand;
    public Field Field;
    public int CurrentMaxHandSize;
    public bool IsHoldingCard;
    public bool IsTurn;

    void Awake()
    {
        Field = FindObjectOfType<Field>();
        Hand = GetComponent<Hand>();
        CurrentMaxHandSize = 7;
    }

    public bool HasPermission()
    {
        return IsTurn ? true : false;
    }
}

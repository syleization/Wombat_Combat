using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AreaPosition { Left, Top, Right, Bottom }
public enum GameType { TwoPlayer, ThreePlayer, FourPlayer }
public class GlobalSettings : MonoBehaviour
{
    [Header("Game Settings")]
    public GameType TypeOfGame;
    [Header("Players")]
    public Player LeftPlayer;
    public string LeftPlayerName;
    public Player TopPlayer;
    public string TopPlayerName;
    public Player RightPlayer;
    public string RightPlayerName;
    public Player BottomPlayer;
    public string BottomPlayerName;
    [Header("Prefabs and Assets")]
    public Card Attack_WombatCharge;
    public Card Attack_DonkeyKick;
    public Card Attack_WomboCombo;
    public Card Defence_Bite;
    public Card Defence_Bark;
    public Card Defence_GooglyEyes;
    public Card Trap_Trampoline;
    public Card Trap_Sinkhole;
    public Card Trap_WombatCage;
    [Header("Damages")]
    public int Damage_DonkeyKick;
    public int Damage_WombatCharge;
    public int Damage_WomboCombo;
    public static List<Player> Players = new List<Player>();

    // SINGLETON
    private static GlobalSettings TheInstance;

    private GlobalSettings() {}

    public static GlobalSettings Instance
    {
        get
        {
            if(TheInstance == null)
            {
                TheInstance = new GlobalSettings();
            }

            return TheInstance;
        }
    }

    void Awake()
    {
        Players.Add(LeftPlayer);
        Players.Add(TopPlayer);
        Players.Add(RightPlayer);
        Players.Add(BottomPlayer);

        LeftPlayer.Name = LeftPlayerName;
        TopPlayer.Name = TopPlayerName;
        RightPlayer.Name = RightPlayerName;
        BottomPlayer.Name = BottomPlayerName;

        TheInstance = this;

        switch(TypeOfGame)
        {
            case GameType.TwoPlayer:
                Players[0].gameObject.SetActive(false);
                Players[2].gameObject.SetActive(false);
                break;
            case GameType.ThreePlayer:
                Players[2].gameObject.SetActive(false);
                break;
            default: // Do Nothing if the game is starting at four players
                break;
        }
    }

    public int GetDamageAmountOf(CardSubType type)
    {
        switch(type)
        {
            case CardSubType.DonkeyKick:
                return Damage_DonkeyKick;
            case CardSubType.WombatCharge:
                return Damage_WombatCharge;
            case CardSubType.WomboCombo:
                return Damage_WomboCombo;
        }

        Debug.Log("[GlobalSettings::GetDamageAmountOf] Invalid parameter");
        return -1;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AreaPosition { Left, Top, Right, Bottom }
public class GlobalSettings : MonoBehaviour
{
    [Header("Players")]
    public Player LeftPlayer;
    public Player TopPlayer;
    public Player RightPlayer;
    public Player BottomPlayer;
    [Header("Colors")]
    public Color32 CardBodyStandardColor;
    public Color32 CardRibbonsStandardColor;
    public Color32 CardGlowColor;
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
    [Header("Other")]
    //public Button EndTurnButton;
    //public GameObject GameOverCanvas;

    public static List<Player> Players = new List<Player>();

    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Players.Add(LeftPlayer);
        Players.Add(TopPlayer);
        Players.Add(RightPlayer);
        Players.Add(BottomPlayer);
        Instance = this;
    }

    //public bool CanControlThisPlayer(AreaPosition owner)
    //{
    //    bool PlayersTurn = (TurnManager.Instance.whoseTurn == Players[owner]);
    //    bool NotDrawingAnyCards = !Command.CardDrawPending();
    //    return Players[owner].PArea.AllowedToControlThisPlayer && Players[owner].PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    //}

    //public bool CanControlThisPlayer(Player ownerPlayer)
    //{
    //    bool PlayersTurn = (TurnManager.Instance.whoseTurn == ownerPlayer);
    //    bool NotDrawingAnyCards = !Command.CardDrawPending();
    //    return ownerPlayer.PArea.AllowedToControlThisPlayer && ownerPlayer.PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    //}

    //public void EnableEndTurnButtonOnStart(Player P)
    //{
    //    if (P == LowPlayer && CanControlThisPlayer(AreaPosition.Low) ||
    //        P == TopPlayer && CanControlThisPlayer(AreaPosition.Top))
    //        EndTurnButton.interactable = true;
    //    else
    //        EndTurnButton.interactable = false;

    //}
}

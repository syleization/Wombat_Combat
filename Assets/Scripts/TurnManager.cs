using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Turns { LeftPlayer, TopPlayer, RightPlayer, BottomPlayer }
// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour
{
    // for Singleton Pattern
    public static TurnManager Instance;
    private static Turns CurrentTurn; // 0 for left player, 1 for top player, 2 for right player, 3 for bottom player

    void Start()
    {
        switch(Random.Range(0, 4))
        {
            case 0: CurrentTurn = Turns.LeftPlayer;
                break;
            case 1: CurrentTurn = Turns.TopPlayer;
                break;
            case 2: CurrentTurn = Turns.RightPlayer;
                break;
            case 3: CurrentTurn = Turns.BottomPlayer;
                break;
            default:
                break;
        }

        SetTurnBools();
    }

    void SetTurnBools()
    {
        switch (CurrentTurn)
        {
            case Turns.LeftPlayer:
                GlobalSettings.Players[0].IsTurn = false; GlobalSettings.Players[1].IsTurn = true;
                CurrentTurn = Turns.TopPlayer;
                break;
            case Turns.TopPlayer:
                GlobalSettings.Players[1].IsTurn = false; GlobalSettings.Players[2].IsTurn = true;
                CurrentTurn = Turns.RightPlayer;
                break;
            case Turns.RightPlayer:
                GlobalSettings.Players[2].IsTurn = false; GlobalSettings.Players[3].IsTurn = true;
                CurrentTurn = Turns.BottomPlayer;
                break;
            case Turns.BottomPlayer:
                GlobalSettings.Players[3].IsTurn = false; GlobalSettings.Players[0].IsTurn = true;
                CurrentTurn = Turns.LeftPlayer;
                break;
            default:
                break;
        }
        Debug.Log(CurrentTurn.ToString());
    }

    public static Player GetCurrentPlayer()
    {
        foreach(Player p in GlobalSettings.Players)
        {
            if(p.IsTurn == true)
            {
                return p;
            }
        }
        Debug.Log("ERROR[TurnManager::GetCurrentPlayer] | It is currently the turn of no one");
        return null;
    }
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 200, Screen.height - 30, 200, 30), "Turn: " + CurrentTurn.ToString() + " | Health: " + GetCurrentPlayer().CurrentHealth.ToString());
    }
    //private Player _whoseTurn;
    //public Player whoseTurn
    //{
    //    get
    //    {
    //        return _whoseTurn;
    //    }

    //    set
    //    {
    //        _whoseTurn = value;
    //        timer.StartTimer();

    //        GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

    //        TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
    //        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
    //        tm.OnTurnStart();
    //        if (tm is PlayerTurnMaker)
    //        {
    //            whoseTurn.HighlightPlayableCards();
    //        }
    //        // remove highlights for opponent.
    //        whoseTurn.otherPlayer.HighlightPlayableCards(true);

    //    }
    //}

    void Awake()
    {
        Instance = this;
    }

    public void EndTurn()
    {

        Player currentPlayer = GetCurrentPlayer();
        SetTurnBools();
        // Move cards from field back to hand
        // Needs to be used because the cardsinfield count changes within the loop
        int originalFieldCount = currentPlayer.Field.CardsInField.Count;
        for (int i = 0; i < originalFieldCount; ++i) 
        {
            // Since cards are removed each loop the current card will always be the first element in the field
            Card currentCard = currentPlayer.Field.GetCard(0);
            if (currentCard != null)
            {
                CardPopUp popup = currentCard.GetComponent<CardPopUp>();
                currentCard.CurrentArea = "Hand";
                currentCard.IsInHand = true;
                popup.cardIsDown = true;
                currentPlayer.Hand.CardsInHand.Add(currentCard);
                currentPlayer.Field.CardsInField.Remove(currentCard);

                DeckOfCards.TransformDealtCardToHand(currentCard, currentCard.owner.Hand.CardsInHand.Count - 1);
            }
        }

    }


}


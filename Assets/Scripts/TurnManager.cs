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
        int random;
        do
        {
            random = Random.Range(0, 4);

        } while (GlobalSettings.Players[random].gameObject.activeSelf == false);

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
        foreach (Player p in GlobalSettings.Players)
        {
            p.IsTurn = false;
        }

        GetPlayerToTheRightOf(CurrentTurn).IsTurn = true;
        UpdateCurrentTurn();
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

    public static Turns GetCurrentTurn()
    {
        return CurrentTurn;
    }

    private static void UpdateCurrentTurn()
    {
        Player temp = GetCurrentPlayer();

        if(temp == GlobalSettings.Players[0])
        {
            CurrentTurn = Turns.LeftPlayer;
        }
        else if (temp == GlobalSettings.Players[1])
        {
            CurrentTurn = Turns.TopPlayer;
        }
        else if(temp == GlobalSettings.Players[2])
        {
            CurrentTurn = Turns.RightPlayer;
        }
        else if (temp == GlobalSettings.Players[3])
        {
            CurrentTurn = Turns.BottomPlayer;
        }
    }

    // These Gets assume more than one player is active
    public static Player GetPlayerToTheRightOf(Turns player)
    {
        switch (player)
        {
            case Turns.LeftPlayer:
                if(GlobalSettings.Players[3].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[3];
                }
                else if(GlobalSettings.Players[2].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[2];
                }
                else
                {
                    return GlobalSettings.Players[1];
                }

            case Turns.TopPlayer:
                if (GlobalSettings.Players[0].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[0];
                }
                else if (GlobalSettings.Players[3].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[3];
                }
                else
                {
                    return GlobalSettings.Players[2];
                }
            case Turns.RightPlayer:
                if (GlobalSettings.Players[1].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[1];
                }
                else if (GlobalSettings.Players[0].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[0];
                }
                else
                {
                    return GlobalSettings.Players[3];
                }
            case Turns.BottomPlayer:
                if (GlobalSettings.Players[2].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[2];
                }
                else if (GlobalSettings.Players[1].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[1];
                }
                else
                {
                    return GlobalSettings.Players[0];
                }
        }
        Debug.Log("[TurnManager::GetPlayerToTheRightOf] Invalid parameter");
        return null;
    }

    public static Player GetPlayerToTheLeftOf(Turns player)
    {
        switch (player)
        {
            case Turns.LeftPlayer:
                if (GlobalSettings.Players[1].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[1];
                }
                else if (GlobalSettings.Players[2].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[2];
                }
                else
                {
                    return GlobalSettings.Players[3];
                }

            case Turns.TopPlayer:
                if (GlobalSettings.Players[2].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[2];
                }
                else if (GlobalSettings.Players[3].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[3];
                }
                else
                {
                    return GlobalSettings.Players[0];
                }
            case Turns.RightPlayer:
                if (GlobalSettings.Players[3].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[3];
                }
                else if (GlobalSettings.Players[0].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[0];
                }
                else
                {
                    return GlobalSettings.Players[1];
                }
            case Turns.BottomPlayer:
                if (GlobalSettings.Players[0].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[0];
                }
                else if (GlobalSettings.Players[1].gameObject.activeSelf)
                {
                    return GlobalSettings.Players[1];
                }
                else
                {
                    return GlobalSettings.Players[2];
                }
        }
        Debug.Log("[TurnManager::GetPlayerToTheLeftOf] Invalid parameter");
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


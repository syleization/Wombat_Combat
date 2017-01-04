using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum Turns { LeftPlayer, TopPlayer, RightPlayer, BottomPlayer }
public enum Stage { Draw, Merge, Play, Reaction }
// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : NetworkBehaviour
{
    // for Singleton Pattern
    private static TurnManager TheInstance;

    private TurnManager() { }

    public static TurnManager Instance
    {
        get
        {
            if (TheInstance == null)
            {
                TheInstance = FindObjectOfType<TurnManager>();
            }

            return TheInstance;
        }
    }

    [SyncVar]
    private Turns CurrentTurn; // 0 for left player, 1 for top player, 2 for right player, 3 for bottom player

    [SyncVar]
    private Stage CurrentStage = Stage.Draw;
    public Turns currentTurn
    {
        get
        {
            return CurrentTurn;
        }
        set
        {
            CurrentTurn = value;
        }
    }

    public Stage currentStage
    {
        get
        {
            return CurrentStage;
        }
        set
        {
            CurrentStage = value;
        }
    }

    void Awake()
    {
        TheInstance = this;
    }

    // SHOULD ONLY EVER BE CALLED ONCE BY GLOBALSETTINGS
    public void Initialize()
    {
        int random;
        do
        {
            random = Random.Range(0, 4);

        } while (GlobalSettings.Players[random] != null);

        switch(random)
        {
            case 0: CurrentTurn = Turns.LeftPlayer;
                break;
            case 1: CurrentTurn = Turns.TopPlayer;
                break;
            case 2: CurrentTurn = Turns.RightPlayer;
                break;
            case 3: CurrentTurn = Turns.BottomPlayer;
                // Rotate for bottom player is 0.0f which is how it starts off anyway
                break;
            default:
                break;
        }

        SetTurnBools();

        //HideCards.Instance.HideCardsOfOtherPlayers(GetCurrentPlayer());
    }

    //void OnGUI()
    //{
    //    // Creates a box in the bottom right showing whose turn it is and what their health is
    //    GUI.Box(new Rect(Screen.width - 250, Screen.height - 30, 250, 30), 
    //        "Turn: " + GetCurrentPlayer().ToString() + " | Health: " + GetCurrentPlayer().CurrentHealth.ToString() + " | Actions: " + GetCurrentPlayer().CurrentActions.ToString());
    //    // Creates a box in the top middle to show the current stage
    //    GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 30), "Stage: " + CurrentStage.ToString());

    //    // Sifts through the stages and the last stage has an end turn button instead of an end stage button
    //    if (CurrentStage == Stage.Merge && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndStage"))
    //    {
    //        // Do some kind of transition to visually show the stage has changed
    //        CurrentStage = Stage.Play;
    //        Field.Instance.SendFieldBackToHand(GetCurrentPlayer());
    //        Field.Instance.ChangeMaxFieldSize(CurrentStage);
    //    }
    //    else if (CurrentStage == Stage.Play && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndTurn"))
    //    {
    //        // Do some kind of end of turn transition to visually show it
    //        CurrentStage = Stage.Merge;

    //        // If a bark was used against the player put those cards back into their hand
    //        CardActions.PlaceBarkedCards(TurnManager.Instance.GetCurrentPlayer());
    //        Instance.EndTurn();
    //        Field.Instance.ChangeMaxFieldSize(CurrentStage);
    //    }
    //    else if(CurrentStage == Stage.Reaction && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "Don'tReact"))
    //    {
    //        CardActions.DontReact();
    //    }

    //    if (Instance.currentStage == Stage.Merge
    //       && GetCurrentPlayer() != null && Field.Instance.IsMergable() != CardType.None
    //       && GetCurrentPlayer().CurrentActions > 0
    //       && GUI.Button(new Rect(10, Screen.height / 2, 50, 20), "Merge"))
    //    {
    //        // Add new power card to hand
    //        Card newCard;
    //        switch (Field.Instance.IsMergable())
    //        {
    //            case CardType.Attack:
    //                newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_WomboCombo);
    //                break;
    //            case CardType.Defence:
    //                newCard = Instantiate<Card>(GlobalSettings.Instance.Defence_GooglyEyes);
    //                break;
    //            case CardType.Trap:
    //                newCard = Instantiate<Card>(GlobalSettings.Instance.Trap_WombatCage);
    //                break;
    //            default:
    //                Debug.Log("Error in Merge");
    //                newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_DonkeyKick);
    //                break;
    //        }
    //        --GetCurrentPlayer().CurrentActions;
    //        newCard.owner = TurnManager.Instance.GetCurrentPlayer();
    //        DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
    //        newCard.CurrentArea = "Hand";
    //        GetCurrentPlayer().Hand.CardsInHand.Add(newCard);

    //        // Clear field of used cards
    //        Field.Instance.ClearField();
    //    }
    //}

    public void EndTurn()
    {
        Player prevPlayer = GetCurrentPlayer();
        SetTurnBools();
        Field.Instance.SendFieldBackToHand(prevPlayer);
        // HideCards.Instance.HideCardsOfOtherPlayers(GetCurrentPlayer());
        //HideCards.Instance.ShowCardsOfPlayer(GetCurrentPlayer());

        if (!isServer)
        {
            prevPlayer.CmdChangeTurn(CurrentTurn);
            prevPlayer.CmdChangeIsTurn();
        }
    }

    void SetTurnBools()
    {
        foreach (Player p in GlobalSettings.Players)
        {
            if (p != null)
            {
                p.IsTurn = false;
            }
        }
        Player newTurn = GetPlayerToTheRightOf(CurrentTurn);
        newTurn.IsTurn = true;
        UpdateCurrentTurn();
        //if (GlobalSettings.Instance.TypeOfGame == GameType.TwoPlayer)
        //{
        //    Camera main = FindObjectOfType<Camera>();

        //    main.transform.Rotate(0.0f, 0.0f, main.transform.rotation.z + 180.0f);
        //}
    }

    public Player GetCurrentPlayer()
    {
        foreach (Player p in GlobalSettings.Players)
        {
            if (p != null && p.IsTurn == true)
            {
                return p;
            }
        }
        Debug.Log("ERROR[TurnManager::GetCurrentPlayer] | It is currently the turn of no one");
        return null;
    }

    public Player GetPrevCurrentPlayer()
    {
        return GetPlayerToTheLeftOf(CurrentTurn);
    }

    public Player GetNextCurrentPlayer()
    {
        return GetPlayerToTheRightOf(CurrentTurn);
    }

    private void UpdateCurrentTurn()
    {
        Player temp = GetCurrentPlayer();

        if(GlobalSettings.Players[0] != null && temp == GlobalSettings.Players[0])
        {
            CurrentTurn = Turns.LeftPlayer;
        }
        else if (GlobalSettings.Players[1] != null && temp == GlobalSettings.Players[1])
        {
            CurrentTurn = Turns.TopPlayer;
        }
        else if(GlobalSettings.Players[2] != null && temp == GlobalSettings.Players[2])
        {
            CurrentTurn = Turns.RightPlayer;
        }
        else if (GlobalSettings.Players[3] != null && temp == GlobalSettings.Players[3])
        {
            CurrentTurn = Turns.BottomPlayer;
        }
    }

    // These Gets assume more than one player is active
    public Player GetPlayerToTheRightOf(Turns player)
    {
        switch (player)
        {
            case Turns.LeftPlayer:
                if(GlobalSettings.Players[3] != null)
                {
                    return GlobalSettings.Players[3];
                }
                else if(GlobalSettings.Players[2] != null)
                {
                    return GlobalSettings.Players[2];
                }
                else
                {
                    return GlobalSettings.Players[1];
                }

            case Turns.TopPlayer:
                if (GlobalSettings.Players[0] != null)
                {
                    return GlobalSettings.Players[0];
                }
                else if (GlobalSettings.Players[3] != null)
                {
                    return GlobalSettings.Players[3];
                }
                else
                {
                    return GlobalSettings.Players[2];
                }
            case Turns.RightPlayer:
                if (GlobalSettings.Players[1] != null)
                {
                    return GlobalSettings.Players[1];
                }
                else if (GlobalSettings.Players[0] != null)
                {
                    return GlobalSettings.Players[0];
                }
                else
                {
                    return GlobalSettings.Players[3];
                }
            case Turns.BottomPlayer:
                if (GlobalSettings.Players[2] != null)
                {
                    return GlobalSettings.Players[2];
                }
                else if (GlobalSettings.Players[1] != null)
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

    public Player GetPlayerToTheLeftOf(Turns player)
    {
        switch (player)
        {
            case Turns.LeftPlayer:
                if (GlobalSettings.Players[1] != null)
                {
                    return GlobalSettings.Players[1];
                }
                else if (GlobalSettings.Players[2] != null)
                {
                    return GlobalSettings.Players[2];
                }
                else
                {
                    return GlobalSettings.Players[3];
                }

            case Turns.TopPlayer:
                if (GlobalSettings.Players[2] != null)
                {
                    return GlobalSettings.Players[2];
                }
                else if (GlobalSettings.Players[3] != null)
                {
                    return GlobalSettings.Players[3];
                }
                else
                {
                    return GlobalSettings.Players[0];
                }
            case Turns.RightPlayer:
                if (GlobalSettings.Players[3] != null)
                {
                    return GlobalSettings.Players[3];
                }
                else if (GlobalSettings.Players[0] != null)
                {
                    return GlobalSettings.Players[0];
                }
                else
                {
                    return GlobalSettings.Players[1];
                }
            case Turns.BottomPlayer:
                if (GlobalSettings.Players[0] != null)
                {
                    return GlobalSettings.Players[0];
                }
                else if (GlobalSettings.Players[1] != null)
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

    public Player GetPlayerAcrossFrom(Turns turn)
    {
        switch(turn)
        {
            case Turns.LeftPlayer: return GlobalSettings.Instance.RightPlayer;

            case Turns.TopPlayer: return GlobalSettings.Instance.BottomPlayer;

            case Turns.RightPlayer: return GlobalSettings.Instance.LeftPlayer;

            case Turns.BottomPlayer: return GlobalSettings.Instance.TopPlayer;
        }
        Debug.Log("[Turnmanager::GetPlayerAcrossFrom] invalid parameter");
        return null;
    }

    public Player GetPlayerToTheLeftOfWithNull(Turns turn)
    {
        switch (turn)
        {
            case Turns.LeftPlayer: return GlobalSettings.Instance.TopPlayer;

            case Turns.TopPlayer: return GlobalSettings.Instance.RightPlayer;

            case Turns.RightPlayer: return GlobalSettings.Instance.BottomPlayer;

            case Turns.BottomPlayer: return GlobalSettings.Instance.LeftPlayer;
        }
        Debug.Log("[Turnmanager::GetPlayerToTheLeftOfWithNull] invalid parameter");
        return null;
    }

    public Player GetPlayerToTheRightOfWithNull(Turns turn)
    {
        switch (turn)
        {
            case Turns.LeftPlayer: return GlobalSettings.Instance.BottomPlayer;

            case Turns.TopPlayer: return GlobalSettings.Instance.LeftPlayer;

            case Turns.RightPlayer: return GlobalSettings.Instance.TopPlayer;

            case Turns.BottomPlayer: return GlobalSettings.Instance.RightPlayer;
        }
        Debug.Log("[Turnmanager::GetPlayerToTheRightOfWithNull] invalid parameter");
        return null;
    }

    public Turns GetTurnEnumOfPlayer(Player player)
    {
        if(GlobalSettings.Instance.LeftPlayer != null && player == GlobalSettings.Instance.LeftPlayer)
        {
            return Turns.LeftPlayer;
        }
        else if (GlobalSettings.Instance.TopPlayer != null && player == GlobalSettings.Instance.TopPlayer)
        {
            return Turns.TopPlayer;
        }
        else if (GlobalSettings.Instance.RightPlayer != null && player == GlobalSettings.Instance.RightPlayer)
        {
            return Turns.RightPlayer;
        }
        else if (GlobalSettings.Instance.BottomPlayer != null && player == GlobalSettings.Instance.BottomPlayer)
        {
            return Turns.BottomPlayer;
        }

        Debug.Log("[TurnManager::GetTurnEnumOfPlayer] Invalid parameter");
        return Turns.BottomPlayer;
    }

    public Player GetPlayerOfTurnEnum(Turns turn)
    {
        switch(turn)
        {
            case Turns.LeftPlayer: return GlobalSettings.Instance.LeftPlayer;
            case Turns.TopPlayer: return GlobalSettings.Instance.TopPlayer;
            case Turns.RightPlayer: return GlobalSettings.Instance.RightPlayer;
            case Turns.BottomPlayer: return GlobalSettings.Instance.BottomPlayer;
        }

        Debug.Log("[TurnManager::GetPlayerOfTurnEnum] Invalid parameter");
        return null;
    }
}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Turns { LeftPlayer, TopPlayer, RightPlayer, BottomPlayer }
public enum Stage { Merge, Play, Reaction }
// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour
{
    // for Singleton Pattern
    private static TurnManager TheInstance;

    private TurnManager() { }

    public static TurnManager Instance
    {
        get
        {
            if(TheInstance == null)
            {
                TheInstance = new TurnManager();
            }

            return TheInstance;
        }
    }


    private Turns CurrentTurn; // 0 for left player, 1 for top player, 2 for right player, 3 for bottom player
    private Stage CurrentStage;
    public Turns currentTurn
    {
        get
        {
            return CurrentTurn;
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
                // Rotate for bottom player is 0.0f which is how it starts off anyway
                break;
            default:
                break;
        }

        SetTurnBools();

        if (GlobalSettings.Instance.TypeOfGame == GameType.TwoPlayer && CurrentTurn == Turns.BottomPlayer)
        {
            Camera main = FindObjectOfType<Camera>();

            main.transform.Rotate(0.0f, 0.0f, main.transform.rotation.z + 180.0f);
        }

    }

    void OnGUI()
    {
        // Creates a box in the bottom right showing whose turn it is and what their health is
        GUI.Box(new Rect(Screen.width - 250, Screen.height - 30, 250, 30), 
            "Turn: " + GetCurrentPlayer().ToString() + " | Health: " + GetCurrentPlayer().CurrentHealth.ToString() + " | Actions: " + GetCurrentPlayer().CurrentActions.ToString());
        // Creates a box in the top middle to show the current stage
        GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 30), "Stage: " + CurrentStage.ToString());

        // Sifts through the stages and the last stage has an end turn button instead of an end stage button
        if (CurrentStage == Stage.Merge && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndStage"))
        {
            // Do some kind of transition to visually show the stage has changed
            CurrentStage = Stage.Play;
            Field.SendFieldBackToHand(GetCurrentPlayer());
            Field.Instance.ChangeMaxFieldSize(CurrentStage);
        }
        else if (CurrentStage == Stage.Play && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndTurn"))
        {
            // Do some kind of end of turn transition to visually show it
            CurrentStage = Stage.Merge;

            // If a bark was used against the player put those cards back into their hand
            CardActions.PlaceBarkedCards(TurnManager.Instance.GetCurrentPlayer());
            Instance.EndTurn();
            Field.Instance.ChangeMaxFieldSize(CurrentStage);
        }
        else if(CurrentStage == Stage.Reaction && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "Don'tReact"))
        {
            CardActions.DontReact();
        }
    }

    public void EndTurn()
    {
        Player currentPlayer = GetCurrentPlayer();
        SetTurnBools();
        Field.SendFieldBackToHand(currentPlayer);
    }

    void SetTurnBools()
    {
        foreach (Player p in GlobalSettings.Players)
        {
            p.IsTurn = false;
        }
        Player newTurn = GetPlayerToTheRightOf(CurrentTurn);
        newTurn.IsTurn = true;
        newTurn.CurrentActions = Player.MaxActions;
        UpdateCurrentTurn();

        if (GlobalSettings.Instance.TypeOfGame == GameType.TwoPlayer)
        {
            Camera main = FindObjectOfType<Camera>();

            main.transform.Rotate(0.0f, 0.0f, main.transform.rotation.z + 180.0f);
        }
    }

    public Player GetCurrentPlayer()
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

    private void UpdateCurrentTurn()
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
    public Player GetPlayerToTheRightOf(Turns player)
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

    public Player GetPlayerToTheLeftOf(Turns player)
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

    public Turns GetTurnEnumOfPlayer(Player player)
    {
        if(player == GlobalSettings.Instance.LeftPlayer)
        {
            return Turns.LeftPlayer;
        }
        else if (player == GlobalSettings.Instance.TopPlayer)
        {
            return Turns.TopPlayer;
        }
        else if (player == GlobalSettings.Instance.RightPlayer)
        {
            return Turns.RightPlayer;
        }
        else if (player == GlobalSettings.Instance.BottomPlayer)
        {
            return Turns.BottomPlayer;
        }

        Debug.Log("[TurnManager::GetTurnEnumOfPlayer] Invalid parameter");
        return Turns.BottomPlayer;
    }
}


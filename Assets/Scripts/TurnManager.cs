using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum Turns { LeftPlayer, TopPlayer, RightPlayer, BottomPlayer, Null }
public enum Stage { Draw, Merge, Play, Reaction }
// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject CombineBanner;
    [SerializeField]
    private GameObject PlayBanner;
    [SerializeField]
    private GameObject ReactionBanner;

    [SyncVar]
    private bool IsDisplayingBanner = false;
    public bool IsCurrentlyDisplayingBanner
    {
        set
        {
            IsDisplayingBanner = true;
        }
        get
        {
            return IsDisplayingBanner;
        }
    }
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
    [SerializeField]
    [SyncVar]
    private Turns theCurrentTurn = Turns.Null; // 0 for left player, 1 for top player, 2 for right player, 3 for bottom player
    [SerializeField]
    [SyncVar]
    private Stage CurrentStage = Stage.Draw;
    public Turns CurrentTurn
    {
        get
        {
            return theCurrentTurn;
        }
        set
        {
            theCurrentTurn = value;
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
            if(isServer && value != Stage.Draw)
            {
                //RpcDisplayBanner(value);
                ButtonManager.Instance.HideActiveButton();
                GlobalSettings.Instance.GetLocalPlayer().RpcHideActiveButton();
                StartCoroutine(DisplayBanner(value));
            }
        }
    }

    IEnumerator DisplayBanner(Stage stage)
    {
        if (IsDisplayingBanner == false)
        {
            IsDisplayingBanner = true;

            GameObject banner;

            switch (stage)
            {
                case Stage.Merge:
                    banner = Instantiate(CombineBanner);
                    break;
                case Stage.Play:
                    banner = Instantiate(PlayBanner);
                    break;
                case Stage.Reaction:
                    banner = Instantiate(ReactionBanner);
                    break;
                default:
                    banner = new GameObject();
                    break;
            }
            NetworkServer.Spawn(banner);

            yield return new WaitForSeconds(2.0f);

            Destroy(banner);
            NetworkServer.UnSpawn(banner);
            IsDisplayingBanner = false;
            //if (GlobalSettings.Instance.GetLocalPlayer().IsTurn)
            //{
            //    ButtonManager.Instance.ShowActiveButton();
            //}
        }
    }

    [ClientRpc]
    void RpcDisplayBanner(Stage stage)
    {
        StartCoroutine(DisplayBanner(stage));
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

        } while (GlobalSettings.Players[random] == null);

        switch(random)
        {
            case 0: theCurrentTurn = Turns.LeftPlayer;
                break;
            case 1: theCurrentTurn = Turns.TopPlayer;
                break;
            case 2: theCurrentTurn = Turns.RightPlayer;
                break;
            case 3: theCurrentTurn = Turns.BottomPlayer;
                // Rotate for bottom player is 0.0f which is how it starts off anyway
                break;
            default:
                break;
        }

        SetTurnBools();
    }

    public void Terminate()
    {
        CurrentStage = Stage.Draw;
        theCurrentTurn = Turns.Null;
        IsDisplayingBanner = false;
    }

    public void EndTurn()
    {
        Player prevPlayer = GetCurrentPlayer();
        SetTurnBools();
        Field.Instance.SendFieldBackToHand(prevPlayer);

        if (!isServer)
        {
            prevPlayer.CmdChangeTurn(theCurrentTurn);
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
        Player newTurn = GetPlayerToTheRightOf(theCurrentTurn);
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
        return GetPlayerToTheLeftOf(theCurrentTurn);
    }

    public Player GetNextCurrentPlayer()
    {
        return GetPlayerToTheRightOf(theCurrentTurn);
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


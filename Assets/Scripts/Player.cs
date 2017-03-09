using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public bool IsSinkholeActive = false;
    // Variable used for deciding if a player can defend over the network
    [SyncVar]
    public bool HasDefenceCards = false;
    [SyncVar]
    public bool HasTrapCards = false;
    public Sinkhole PlayersSinkhole;
    public Hand Hand;
    public DeckOfCards Deck;
    public TrapZone Traps;
    private int MaxHandSize = 3;
    public int CurrentMaxHandSize
    {
        set
        {
            if(value < 8)
            {
                MaxHandSize = value;
            }
        }
        get
        {
            return MaxHandSize;
        }
    }
    [SyncVar]
    private int HandSize = 0;
    public int CurrentHandSize
    {
        set
        {
            HandSize = value;
            if (!isServer)
            {
                CmdChangeCurrentHandSize(value);
            }
        }
        get
        {
            return HandSize;
        }
    }
    public bool IsHoldingCard;
    [SyncVar]
    public bool IsTurn;
    [SyncVar]
    private string PlayersName = "";
    public string Name
    {
        set
        {
            PlayersName = value;
        }
    }
    [SyncVar]
    public int PlayersCurrentHealth;
    public int CurrentHealth
    {
        get
        {
            return PlayersCurrentHealth;
        }
        set
        {
            PlayersCurrentHealth = value;
            if(value <= 0)
            {
                PlayersCurrentHealth = 0;
                if (GlobalSettings.Instance.TypeOfGame == GameType.TwoPlayer)
                {
                    if (isServer)
                    {
                        TheGUI gui = FindObjectOfType<TheGUI>();
                        gui.GameIsOver = true;
                    }
                    else
                    {
                        GlobalSettings.Instance.GetLocalPlayer().CmdEndGame();
                    }
                }
            }
        }
    }
    private const int MaxHealth = 15;
    public const int MaxActions = 4;
    [SyncVar(hook ="OnAction")]
    public int PlayersCurrentActions;
    public void OnAction(int updatedAction)
    {
        PlayersCurrentActions = updatedAction;
    }
    public int CurrentActions
    {
        get
        {
            return PlayersCurrentActions;
        }
        set
        {
            PlayersCurrentActions = value;

            if(PlayersCurrentActions < 0)
            {
                PlayersCurrentActions = 0;
            }
            else if(PlayersCurrentActions > MaxActions)
            {
                PlayersCurrentActions = MaxActions;
            }
        }
    }

    public override void OnStartClient()
    {
        GlobalSettings.Instance.AddNetworkPlayer(this);
    }

    // Player commands
    [Command]
    public void CmdChangeActions(Turns player, int value)
    {
        TurnManager.Instance.GetPlayerOfTurnEnum(player).CurrentActions = value;
    }

    [Command]
    public void CmdTakeDamage(Turns player, int damage)
    {
        TurnManager.Instance.GetPlayerOfTurnEnum(player).CurrentHealth -= damage;
    }

    [Command]
    public void CmdChangeSinkholeBool(bool sinkhole)
    {
        IsSinkholeActive = sinkhole;
        RpcUpdateSinkhole(TurnManager.Instance.GetTurnEnumOfPlayer(this), sinkhole);
    }

    [Command]
    public void CmdChangeHasDefenceCards(bool hasDefenceCards)
    {
        HasDefenceCards = hasDefenceCards;
    }

    [Command]
    public void CmdChangeHasTrapCards(bool hasTrapCards)
    {
        HasTrapCards = hasTrapCards;
    }

    [Command]
    public void CmdChangeCurrentHandSize(int value)
    {
        HandSize = value;
    }

    // Card Commands
    [Command]
    public void CmdSpawnCard(GameObject cardToSpawn)
    {
        NetworkServer.Spawn(cardToSpawn);
    }

    // Field Commands
    [Command]
    public void CmdChangeFieldSize()
    {
        Field.Instance.ChangeMaxFieldSize(TurnManager.Instance.currentStage);
    }

    [Command]
    public void CmdClearField()
    {
        Field.Instance.RpcClearField();
    }

    [Command]
    public void CmdChangeDamageInField(int damage)
    {
        Field.Instance.CurrentDamageInField = damage;
    }

    [Command]
    public void CmdAddCardToField(CardSubType subType)
    {
        Field.Instance.RpcAddCardToField(subType);
    }

    [Command]
    public void CmdRemoveCardFromField(int index)
    {
        Field.Instance.RpcRemoveCardFromField(index);
    }

    // TurnManager Commands
    [Command]
    public void CmdChangeStage(Stage stage)
    {
        TurnManager.Instance.currentStage = stage;
    }

    [Command]
    public void CmdChangeTurn(Turns turn)
    {
        TurnManager.Instance.currentTurn = turn;
    }

    [Command]
    public void CmdChangeIsTurn()
    {
        foreach (Player p in GlobalSettings.Players)
        {
            if (p != null)
            {
                if (TurnManager.Instance.GetTurnEnumOfPlayer(p) == TurnManager.Instance.currentTurn)
                {
                    p.IsTurn = true;
                }
                else
                {
                    p.IsTurn = false;
                }
            }
        }
    }

    // Card Actions Commands
    [Command]
    public void CmdUpdateThrowerAndReactor(Turns thrower, Turns reactor)
    {
        CardActions.theThrower = TurnManager.Instance.GetPlayerOfTurnEnum(thrower);
        CardActions.theReactor = TurnManager.Instance.GetPlayerOfTurnEnum(reactor);
    }

    [ClientRpc]
    public void RpcUpdateThrowerAndReactor(Turns thrower, Turns reactor)
    {
        CardActions.theThrower = TurnManager.Instance.GetPlayerOfTurnEnum(thrower);
        CardActions.theReactor = TurnManager.Instance.GetPlayerOfTurnEnum(reactor);
    }

    [Command]
    public void CmdUpdateBarkedCards(Turns owner)
    {
        RpcUpdateBarkedCards(owner);
    }

    // GlobalSettings Actions
    [Command]
    public void CmdEndGame()
    {
        GlobalSettings.Instance.RpcEndGame();
    }

    // Effects Commands

    // Client Rpcs
    [ClientRpc]
    public void RpcUpdateBarkedCards(Turns owner)
    {
        if(TurnManager.Instance.GetPlayerOfTurnEnum(owner).isLocalPlayer)
        {
            // Add card to holder array
            CardActions.BarkedCards.Add(Field.Instance.GetCard(0));
            // Remove card from local field so the game object isnt destroyed in server clear field
            Field.Instance.CardsInField.RemoveAt(0);
            // Hide gameobject till it is put back into their hand
            CardActions.BarkedCards[0].gameObject.SetActive(false);
        }
    }

    [ClientRpc]
    public void RpcUpdateSinkhole(Turns playerTurns, bool sinkholeActive)
    {
        Player player = TurnManager.Instance.GetPlayerOfTurnEnum(playerTurns);
        player.IsSinkholeActive = sinkholeActive;

        if(player.IsSinkholeActive)
        {
            Effects.SinkholeOn(player);
        }
        else
        {
            Effects.SinkholeOff(player.PlayersSinkhole);
        }
    }


    void Awake()
    {
        CurrentActions = MaxActions;
        CurrentHealth = MaxHealth;
        Deck = GetComponent<DeckOfCards>();
        Deck.Initialize();
        Deck.owner = this;
        Hand = GetComponent<Hand>();
    }

    Timer WaitToPlayTimer = new Timer();
    bool doneWaiting = true;

    public bool HasPermission()
    {
        return IsTurn ? true : false;
    }

    public override string ToString()
    {
        return PlayersName;
    }


    public void ClearHand()
    {
        foreach (Card c in Hand.CardsInHand)
        {
            Destroy(c.gameObject);
        }
        Hand.CardsInHand.Clear();
        HandSize = 0;
    }
}

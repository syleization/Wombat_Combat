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
    public Hand Hand;
    public DeckOfCards Deck;
    public int CurrentMaxHandSize;
    [SyncVar]
    public int CurrentHandSize;
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
    }

    [Command]
    public void CmdChangeHasDefenceCards(bool hasDefenceCards)
    {
        HasDefenceCards = hasDefenceCards;
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
        Field.Instance.ClearField();
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

    void Awake()
    {
        CurrentMaxHandSize = 7;
        CurrentActions = MaxActions;
        CurrentHealth = MaxHealth;
        Deck = GetComponent<DeckOfCards>();
        Deck.Initialize();
        Deck.owner = this;
        Hand = GetComponent<Hand>();
    }

    void Update()
    {
        if (isLocalPlayer && Hand != null)
        {
            CurrentHandSize = Hand.CardsInHand.Count;
        }
    }
    
    public bool HasPermission()
    {
        return IsTurn ? true : false;
    }

    public override string ToString()
    {
        return PlayersName;
    }

}

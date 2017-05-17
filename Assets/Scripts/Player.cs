using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private int TrapAmount;
    public int CurrentTrapAmount
    {
        set
        {
            TrapAmount = value;
        }
        get
        {
            return TrapAmount;
        }
    }
    [Command]
    public void CmdChangeTrapAmount(int value, Turns player)
    {
        RpcChangeTrapAmount(value, player);
    }
    [ClientRpc]
    public void RpcChangeTrapAmount(int value, Turns player)
    {
        TurnManager.Instance.GetPlayerOfTurnEnum(player).CurrentTrapAmount = value;
        UI_PlayerInfo.Instance.ChangeTrapsText();
    }
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
            else if(value != MaxHealth)
            {
                if (isServer)
                {
                    RpcChangeHealthAmount();
                }
                else
                {
                    GlobalSettings.Instance.GetLocalPlayer().CmdChangeHealthAmount();
                }
            }
        }
    }
    [Command]
    public void CmdChangeHealthAmount()
    {
        RpcChangeHealthAmount();
    }
    [ClientRpc]
    public void RpcChangeHealthAmount()
    {
        UI_PlayerInfo.Instance.ChangeHealthText();
    }
    public const int MaxHealth = 15;
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
    #region Networking
    // Button Manager Commands & RPC
    [Command]
    public void CmdHideActiveButton()
    {
        RpcHideActiveButton();
    }
    [ClientRpc]
    public void RpcHideActiveButton()
    {
        if(isLocalPlayer)
        {
            ButtonManager.Instance.HideActiveButton();
        }
    }
    [Command]
    public void CmdShowActiveButton()
    {
        RpcShowActiveButton();
    }
    [ClientRpc]
    public void RpcShowActiveButton()
    {
        if (isLocalPlayer && IsTurn)
        {
            ButtonManager.Instance.ShowActiveButton();
        }
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
        RpcTakeDamage(player, damage);
    }

    [ClientRpc]
    public void RpcTakeDamage(Turns player, int damage)
    {
        TurnManager.Instance.GetPlayerOfTurnEnum(player).CurrentHealth -= damage;
    }

    [Command]
    public void CmdChangeSinkholeBool(bool sinkhole, Vector3 position, Quaternion rotation)
    {
        IsSinkholeActive = sinkhole;
        RpcUpdateSinkhole(TurnManager.Instance.GetTurnEnumOfPlayer(this), sinkhole, position, rotation);
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

    [Command]
    public void CmdHideCardsFromField(float timer)
    {
        RpcHideCardsFromField(timer);
    }

    [ClientRpc]
    public void RpcHideCardsFromField(float timer)
    {
        Field.Instance.HideCards(timer);
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
        TurnManager.Instance.CurrentTurn = turn;
    }

    [Command]
    public void CmdChangeIsTurn()
    {
        foreach (Player p in GlobalSettings.Players)
        {
            if (p != null)
            {
                if (TurnManager.Instance.GetTurnEnumOfPlayer(p) == TurnManager.Instance.CurrentTurn)
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
        RpcUpdateThrowerAndReactor(thrower, reactor);
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

    // Pause Commands
    [Command]
    public void CmdPauseGame(float waitTime)
    {
        Pause.Instance.RpcPauseGame(waitTime);
    }

    // Effects Commands
    [Command]
    public void CmdEatCard(Turns sinkhole, Vector3 position, Quaternion rotation)
    {
        RpcEatCard(sinkhole, position, rotation);
    }

    [ClientRpc]
    public void RpcEatCard(Turns sinkhole, Vector3 position, Quaternion rotation)
    {
        Sinkhole temp = TurnManager.Instance.GetPlayerOfTurnEnum(sinkhole).PlayersSinkhole;
        temp.EatCard(position, rotation);
    }

    [Command]
    public void CmdBite()
    {
        RpcBite();
    }

    [ClientRpc]
    public void RpcBite()
    {
        Effects.Bite();
    }

    [Command]
    public void CmdBark(Turns defender, Turns attacker)
    {
        RpcBark(defender, attacker);
    }

    [ClientRpc]
    public void RpcBark(Turns defender, Turns attacker)
    {
        Effects.Bark(TurnManager.Instance.GetPlayerOfTurnEnum(defender)
            , TurnManager.Instance.GetPlayerOfTurnEnum(attacker));
    }

    [Command]
    public void CmdAttackDK(CardSubType card, Turns defender, Turns attacker)
    {
        RpcAttackDK(card, defender, attacker);
    }

    [ClientRpc]
    public void RpcAttackDK(CardSubType card, Turns defender, Turns attacker)
    {
        Effects.Attack(Instantiate(GlobalSettings.Instance.GetCardOfSubType(card)).gameObject
            , TurnManager.Instance.GetPlayerOfTurnEnum(defender)
            , TurnManager.Instance.GetPlayerOfTurnEnum(attacker));
    }

    [Command]
    public void CmdAttackWC(CardSubType card, Turns defender, Turns attacker)
    {
        RpcAttackWC(card, defender, attacker);
    }

    [ClientRpc]
    public void RpcAttackWC(CardSubType card, Turns defender, Turns attacker)
    {
        Effects.Charge(Instantiate(GlobalSettings.Instance.GetCardOfSubType(card)).gameObject
            , TurnManager.Instance.GetPlayerOfTurnEnum(defender)
            , TurnManager.Instance.GetPlayerOfTurnEnum(attacker));
    }

    [Command]
    public void CmdAttackWomCom(CardSubType card, Turns defender, Turns attacker)
    {
        RpcAttackWomCom(card, defender, attacker);
    }

    [ClientRpc]
    public void RpcAttackWomCom(CardSubType card, Turns defender, Turns attacker)
    {
        Effects.WomboCombo(Instantiate(GlobalSettings.Instance.GetCardOfSubType(card)).gameObject
            , TurnManager.Instance.GetPlayerOfTurnEnum(defender)
            , TurnManager.Instance.GetPlayerOfTurnEnum(attacker));
    }

    [Command]
    public void CmdAttackEnd()
    {
        RpcAttackEnd();
    }

    [ClientRpc]
    public void RpcAttackEnd()
    {
        Effects.AttackEnd();
    }

    [Command]
    public void CmdCage(Turns playerWhoNowOwnsCard)
    {
        RpcCage(playerWhoNowOwnsCard);
    }

    [ClientRpc]
    public void RpcCage(Turns playerWhoNowOwnsCard)
    {
        Effects.Cage(TurnManager.Instance.GetPlayerOfTurnEnum(playerWhoNowOwnsCard));
    }

    [Command]
    public void CmdTramp(Turns defender, Turns attacker)
    {
        RpcTramp(defender, attacker);
    }

    [ClientRpc]
    public void RpcTramp(Turns defender, Turns attacker)
    {
        Effects.Tramp(TurnManager.Instance.GetPlayerOfTurnEnum(attacker)
            , TurnManager.Instance.GetPlayerOfTurnEnum(defender));
    }

    [Command]
    public void CmdGooglyEyes(Turns targetPlayer)
    {
        RpcGooglyEyes(targetPlayer);
    }

    [ClientRpc]
    public void RpcGooglyEyes(Turns targetPlayer)
    {
        Effects.GooglyEyes(TurnManager.Instance.GetPlayerOfTurnEnum(targetPlayer));
    }

    [Command]
    public void CmdMergeAnimation(CardSubType originalCards, CardSubType finalCard, Turns targetPlayer)
    {
        RpcMergeAnimation(originalCards, finalCard, targetPlayer);
    }

    [ClientRpc]
    public void RpcMergeAnimation(CardSubType originalCards, CardSubType finalCard, Turns targetPlayer)
    {
        Effects.Merge(originalCards, finalCard, targetPlayer);
    }


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
    public void RpcUpdateSinkhole(Turns playerTurns, bool sinkholeActive, Vector3 position, Quaternion rotation)
    {
        Player player = TurnManager.Instance.GetPlayerOfTurnEnum(playerTurns);
        player.IsSinkholeActive = sinkholeActive;

        if(player.IsSinkholeActive)
        {
            Effects.SinkholeOn(player);
            player.PlayersSinkhole.EatCard(position, rotation);
        }
        else
        {
            Effects.SinkholeOff(player.PlayersSinkhole);
        }
    }

    #endregion

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

    void Update()
    {
        // Check Merge Glow
        if (TurnManager.Instance.currentStage == Stage.Merge
                    && this.isLocalPlayer && Field.Instance.GetCard(0) != null
                   && this.CurrentActions > 0
                   && Field.Instance.CardsInField.Count != 2)
        {
            Hand.UpdateGlow(Field.Instance.GetCard(0).SubType);
        }
        else
        // Check react glow
        if (TurnManager.Instance.currentStage == Stage.Reaction
                   && CardActions.theReactor.isLocalPlayer
            )
        {
            Hand.UpdateGlow(CardType.Defence);
        }
        else if (Hand != null)
        {
            Hand.ClearGlow();
        }
    }

}

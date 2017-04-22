using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CardActions : MonoBehaviour
{
    private static CardActions TheInstance;
    void Awake()
    {
        TheInstance = this;
    }
    public const float kEffectTime = 3.0f;
    public const float kMergeEffectTime = 4.0f;
    // Not a singleton, but no instance of this class should ever be constructed
    // It is basically a hub for static functions
    // Thrower will always be the local player
    private static Player Thrower;
    private static Player Reactor;
    public static List<Card> BarkedCards = new List<Card>();
    private CardActions() { }

    public static Player theThrower
    {
        set
        {
            Thrower = value;
        }
        get
        {
            return Thrower;
        }
    }

    public static Player theReactor
    {
        set
        {
            Reactor = value;
        }
        get
        {
            return Reactor;
        }
    }
    #region Attacks
    public static void DonkeyKick(Player thrower)
    {
        --thrower.CurrentActions;
        if (!thrower.isServer)
        {
            thrower.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), thrower.CurrentActions);
        }

        Player leftOfThrowingPlayer = TurnManager.Instance.GetPlayerToTheLeftOf(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
        Player rightOfThrowingPlayer = TurnManager.Instance.GetPlayerToTheRightOf(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));

        Player target;
        if (Random.Range(0, 2) == 0)
        {
            Debug.Log("Wombat donkey kicks towards " + leftOfThrowingPlayer.ToString() + "!");
            target = leftOfThrowingPlayer;
        }
        else
        {
            Debug.Log("Wombat donkey kicks towards " + rightOfThrowingPlayer.ToString() + "!");
            target = rightOfThrowingPlayer;
        }

        if (!thrower.isServer)
        {
            thrower.CmdAttackDK(CardSubType.DonkeyKick, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            thrower.CmdPauseGame(kEffectTime);
        }
        else
        {
            thrower.RpcAttackDK(CardSubType.DonkeyKick, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        TheInstance.StartCoroutine(WaitToReact(kEffectTime, thrower, target));
    }

    public static void WombatCharge(Player thrower, Player target)
    {
        --thrower.CurrentActions;
        if (!thrower.isServer)
        {
            thrower.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), thrower.CurrentActions);
        }

        if (!thrower.isServer)
        {
            thrower.CmdAttackWC(CardSubType.WombatCharge, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            thrower.CmdPauseGame(kEffectTime);
        }
        else
        {
            thrower.RpcAttackWC(CardSubType.WombatCharge, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        Debug.Log("Wombat charges towards " + target.ToString() + "!");
        TheInstance.StartCoroutine(WaitToReact(kEffectTime, thrower, target));
    }

    public static void WomboCombo(Player thrower, Player target)
    {
        --thrower.CurrentActions;
        if (!thrower.isServer)
        {
            thrower.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), thrower.CurrentActions);
        }

        if (!thrower.isServer)
        {
            thrower.CmdAttackWomCom(CardSubType.WomboCombo, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            thrower.CmdPauseGame(kEffectTime);
        }
        else
        {
            thrower.RpcAttackWomCom(CardSubType.WomboCombo, TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        Debug.Log("Two wombats jump at " + target.ToString() + "!");
        TheInstance.StartCoroutine(WaitToReact(kEffectTime, thrower, target));
    }
    #endregion
    #region Defence
    public static void Bark(Player thrower, Player reactor)
    {
        --reactor.CurrentActions;
        if (!reactor.isServer)
        {
            reactor.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), thrower.CurrentActions);
        }

        Debug.Log(reactor.ToString() + "'s dingo scares the wombat back into " + thrower.ToString() + "'s hand at the end of the turn!");

        // Place the card in a holder array and clear the field
        TurnManager.Instance.currentStage = Stage.Play;
        if (!reactor.isServer)
        {
            reactor.CmdChangeStage(Stage.Play);
            reactor.CmdUpdateBarkedCards(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            reactor.CmdBark(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            reactor.CmdClearField();
            reactor.CmdPauseGame(kEffectTime);
        }
        else
        {
            reactor.RpcUpdateBarkedCards(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            reactor.RpcBark(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Field.Instance.RpcClearField();
            Pause.Instance.RpcPauseGame(kEffectTime);
        }
        // HideCards.Instance.HideCardsOfPlayer(reactor);
    }

    public static void PlaceBarkedCards(Player thrower)
    {
        if (BarkedCards.Count > 0)
        {
            foreach (Card card in BarkedCards)
            {
                card.gameObject.SetActive(true);
                thrower.Hand.CardsInHand.Add(card);
                card.CurrentArea = "Hand";
                card.IsInHand = true;
                DeckOfCards.TransformDealtCardToHand(card, thrower.Hand.CardsInHand.Count - 1);
                ++thrower.CurrentHandSize;
            }

            BarkedCards.Clear();
        }
    }

    public static void Bite(Player killer)
    {
        --killer.CurrentActions;
        TurnManager.Instance.currentStage = Stage.Play;
        Field.Instance.ClearField();

        if (!killer.isServer)
        {
            killer.CmdBite();
            killer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(killer), killer.CurrentActions);
            killer.CmdClearField();
            killer.CmdChangeStage(Stage.Play);
            killer.CmdPauseGame(kEffectTime);
        }
        else
        {
            killer.RpcBite();
            Field.Instance.RpcClearField();
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        Debug.Log(killer.ToString() + "'s wolverine bit the wombat and it ran away!");
        // HideCards.Instance.HideCardsOfPlayer(killer);
    }

    public static void GooglyEyes(Player thrower, Player reactor)
    {
        --reactor.CurrentActions;
        Field.Instance.RemoveCard(1);

        if (!reactor.isServer)
        {
            reactor.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), reactor.CurrentActions);
            reactor.CmdGooglyEyes(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            reactor.CmdPauseGame(kEffectTime);
        }
        else
        {
            reactor.RpcGooglyEyes(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        Debug.Log(reactor.ToString() + "'s dingo convinced the wombat to attack " + thrower.ToString());
        //HideCards.Instance.HideCardsOfPlayer(reactor);
        TheInstance.StartCoroutine(WaitToReact(kEffectTime, reactor, thrower));
    }
    #endregion
    #region Traps
    public static void Trampoline(Player thrower, Player reactor)
    {
        Turns reactorTurn = TurnManager.Instance.GetTurnEnumOfPlayer(reactor);

        switch(Random.Range(0, 4))
        {
            case 0:
                Debug.Log(reactor.ToString() + "'s trampoline broke!");
                DealDamage(thrower, reactor);
                break;
            case 1:
                Player left = TurnManager.Instance.GetPlayerToTheLeftOf(reactorTurn);
                Debug.Log(reactor.ToString() + "'s trampoline bounced the wombat towards " + left.ToString());
                TrampolineBounce(reactor, left);
                break;
            case 2:
                Player right = TurnManager.Instance.GetPlayerToTheRightOf(reactorTurn);
                Debug.Log(reactor.ToString() + "'s trampoline bounced the wombat towards " + right.ToString());
                TrampolineBounce(reactor, right);
                break;
            case 3:
                Debug.Log(reactor.ToString() + "'s trampoline bounced the wombat towards " + Thrower.ToString());
                TrampolineBounce(reactor, thrower);
                break;
        }
    }

    private static void TrampolineBounce(Player thrower, Player target)
    {
        // Remove the trampoline from the field
        Field.Instance.RemoveCard(1);

        if (!thrower.isServer)
        {
            thrower.CmdTramp(TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            thrower.CmdPauseGame(kEffectTime);
        }
        else
        {
            thrower.RpcTramp(TurnManager.Instance.GetTurnEnumOfPlayer(target), TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }

        React(thrower, target);
    }

    public static void Sinkhole(Player thrower, Player reactor)
    {
        reactor.IsSinkholeActive = true;
        TurnManager.Instance.currentStage = Stage.Play;

        CardSubType card = Field.Instance.GetCard(0).SubType;
        Vector3 cardPosition = new Vector3(thrower.Hand.transform.position.x, thrower.Hand.transform.position.y, -1.0f);
        Quaternion cardRotation = thrower.Hand.transform.rotation;
        

        if (!reactor.isServer)
        {
            reactor.CmdClearField();
            reactor.CmdChangeStage(Stage.Play);
            reactor.CmdChangeSinkholeBool(true, card, cardPosition, cardRotation);
            reactor.CmdPauseGame(kEffectTime);
        }
        else
        {
            reactor.RpcUpdateSinkhole(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), true, card, cardPosition, cardRotation);
            Field.Instance.RpcClearField();
            Pause.Instance.RpcPauseGame(kEffectTime);
            
        }

        Debug.Log(thrower.ToString() + "'s wombat fell into " + reactor.ToString() + "'s sinkhole!");
    }

    public static void WombatCage(Player thrower, Player reactor)
    {
        Debug.Log(reactor.ToString() + " trapped " + thrower.ToString() + "'s wombat and stole it!");

        // Retrieve the wombat and add it to your hand
        Card thrownCard = Field.Instance.GetCard(0);

        ++reactor.CurrentHandSize;
        reactor.Hand.CardsInHand.Add(thrownCard);
        // Change variables of card so it is now a card in the hand of the other player
        thrownCard.CurrentArea = "Hand";
        thrownCard.IsInHand = true;
        thrownCard.owner = reactor;
        DeckOfCards.TransformDealtCardToHand(thrownCard, reactor.Hand.CardsInHand.Count - 1);

        // Remove the card from the field so its game object isnt destroyed
        Field.Instance.CardsInField.RemoveAt(0);
        if(!reactor.isServer)
        {
            reactor.CmdRemoveCardFromField(0);
            reactor.CmdCage(TurnManager.Instance.GetTurnEnumOfPlayer(reactor));
            reactor.CmdPauseGame(kEffectTime);
        }
        else
        {
            Field.Instance.RpcRemoveCardFromField(0);
            reactor.RpcCage(TurnManager.Instance.GetTurnEnumOfPlayer(reactor));
            Pause.Instance.RpcPauseGame(kEffectTime);
        }
        // Clear the field and reset the stage
        TurnManager.Instance.currentStage = Stage.Play;

        if (!reactor.isServer)
        {
            reactor.CmdClearField();
            reactor.CmdChangeStage(Stage.Play);
        }
        else
        {
            Field.Instance.RpcClearField();
        }
        // HideCards.Instance.HideCardsOfPlayer(reactor);
    }
    #endregion
    #region Other
    static void React(Player thrower, Player reactor)
    {
        if(reactor.IsSinkholeActive == true)
        {
            Debug.Log(thrower.ToString() + "'s wombat fell into " + reactor.ToString() + "'s sinkhole!");
            CardSubType card = Field.Instance.GetCard(0).SubType;
            Vector3 cardPosition = new Vector3(thrower.Hand.transform.position.x, thrower.Hand.transform.position.y, -1.0f);
            Quaternion cardRotation = thrower.Hand.transform.rotation;

            Field.Instance.ClearField();
            if (!thrower.isServer)
            {
                thrower.CmdEatCard(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), card, cardPosition, cardRotation);
                thrower.CmdClearField();
            }
            else
            {
                thrower.RpcEatCard(TurnManager.Instance.GetTurnEnumOfPlayer(reactor), card, cardPosition, cardRotation);
                Field.Instance.RpcClearField();
            }
        }
        else if ((reactor.HasDefenceCards && reactor.CurrentActions > 0) ||
            (reactor.HasTrapCards && Field.Instance.CurrentDamageInField 
            != GlobalSettings.Instance.GetDamageAmountOf(CardSubType.WomboCombo) && reactor.CurrentActions > 0))
        {
            TurnManager.Instance.currentStage = Stage.Reaction;
            Field.Instance.ChangeMaxFieldSize(Stage.Reaction);

            Thrower = thrower;
            Reactor = reactor;
  
            if (thrower.isServer)
            {
                thrower.RpcUpdateThrowerAndReactor(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), TurnManager.Instance.GetTurnEnumOfPlayer(reactor));
            }
            else
            {
                thrower.CmdChangeStage(Stage.Reaction);
                thrower.CmdChangeFieldSize();
                thrower.CmdUpdateThrowerAndReactor(TurnManager.Instance.GetTurnEnumOfPlayer(thrower), TurnManager.Instance.GetTurnEnumOfPlayer(reactor));
            }
            // HideCards.Instance.ShowCardsOfPlayer(reactor);
        }
        else
        {
            DealDamage(thrower, reactor);
        }
    }

    static IEnumerator WaitToReact(float waitTime, Player thrower, Player reactor)
    {
        if (thrower.isServer)
        {
            thrower.RpcHideCardsFromField(0);
        }
        else
        {
            thrower.CmdHideCardsFromField(0);
        }
        yield return new WaitForSeconds(waitTime);
        React(thrower, reactor);
    }

    static void DealDamage(Player thrower, Player victim)
    {
        int damage = Field.Instance.CurrentDamageInField;
        Debug.Log(victim.ToString() + " was hit by " + thrower.ToString() + "'s wombat for " + damage + " damage!");
        victim.CurrentHealth -= damage;

        ///////////////////// ADD EFFECTS.ATTACKEND ////////////////////////////////////
        // Wombat is no longer bouncing around
        TurnManager.Instance.currentStage = Stage.Play;
        Field.Instance.ClearField();
        Player local = victim.isLocalPlayer ? victim : thrower;

        if (!local.isServer)
        {
            local.CmdTakeDamage(TurnManager.Instance.GetTurnEnumOfPlayer(victim), damage);
            local.CmdClearField();
            local.CmdChangeStage(Stage.Play);
        }
        else
        {
            Field.Instance.RpcClearField();
        }
    }

    public static void DontReact()
    {
        //HideCards.Instance.HideCardsOfPlayer(Reactor);
        DealDamage(Thrower, Reactor);
    }
    #endregion
}

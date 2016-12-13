using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardActions : MonoBehaviour
{
    // Not a singleton, but no instance of this class should ever be constructed
    // It is basically a hub for static functions
    static private Player Thrower;
    static private Player Reactor;
    static private List<Card> BarkedCards = new List<Card>();
    private CardActions() { }

    static public Player theThrower
    {
        get
        {
            return Thrower;
        }
    }

    static public Player theReactor
    {
        get
        {
            return Reactor;
        }
    }

    public static void DonkeyKick(Player thrower)
    {
        Player leftOfThrowingPlayer = TurnManager.Instance.GetPlayerToTheLeftOf(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));
        Player rightOfThrowingPlayer = TurnManager.Instance.GetPlayerToTheRightOf(TurnManager.Instance.GetTurnEnumOfPlayer(thrower));

        if (Random.Range(0, 2) == 0)
        {
            Debug.Log("Wombat donkey kicks towards " + leftOfThrowingPlayer.ToString() + "!");
            React(thrower, leftOfThrowingPlayer);
        }
        else
        {
            Debug.Log("Wombat donkey kicks towards " + rightOfThrowingPlayer.ToString() + "!");
            React(thrower, rightOfThrowingPlayer);
        }
        --thrower.CurrentActions;
    }

    public static void WombatCharge(Player thrower, Player target)
    {
        Debug.Log("Wombat charges towards " + target.ToString() + "!");
        React(thrower, target);
        --thrower.CurrentActions;
    }

    public static void WomboCombo(Player thrower, Player target)
    {
        Debug.Log("Two wombats jump at " + target.ToString() + "!");
        React(thrower, target);
        --thrower.CurrentActions;
    }

    public static void Bark(Player thrower, Player reactor)
    {
        Debug.Log(reactor.ToString() + "'s dingo scares the wombat back into " + thrower.ToString() + "'s hand at the end of the turn!");
        Card card = Field.Instance.GetCard(0);
        BarkedCards.Add(card);
        Field.Instance.CardsInField.Remove(card);
        Field.Instance.ClearField();
        card.gameObject.SetActive(false);
        TurnManager.Instance.currentStage = Stage.Play;
        --thrower.CurrentActions;
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
            }
        }
    }

    public static void Bite(Player killer)
    {
        Debug.Log(killer.ToString() + "'s wolverine bit the wombat and it ran away!");
        Field.Instance.ClearField();
        TurnManager.Instance.currentStage = Stage.Play;
        --killer.CurrentActions;
    }

    public static void GooglyEyes(Player thrower, Player reactor)
    {
        Debug.Log(reactor.ToString() + "'s dingo convinced the wombat to attack " + thrower.ToString());
        Card card = Field.Instance.GetCard(1);
        Field.Instance.CardsInField.Remove(card);
        Destroy(card.gameObject);
        React(reactor, thrower);
        --thrower.CurrentActions;
    }

    public static void Trampoline(Player thrower, Player reactor)
    {
        
    }

    public static void Sinkhole(Player thrower, Player reactor)
    {

    }

    public static void WombatCage(Player thrower, Player reactor)
    {

    }

    static void React(Player thrower, Player reactor)
    {
        if(reactor.Hand.HasDefenceCards() && reactor.CurrentActions > 0)
        {
            TurnManager.Instance.currentStage = Stage.Reaction;
            Field.Instance.ChangeMaxFieldSize(Stage.Reaction);

            Thrower = thrower;
            Reactor = reactor;
        }
        else
        {
            DealDamage(thrower, reactor);
        }
    }

    static void DealDamage(Player thrower, Player victim)
    {
        int damage = GlobalSettings.Instance.GetDamageAmountOf(Field.Instance.GetCard(0).SubType);
        Debug.Log(victim.ToString() + " was hit by " + thrower.ToString() + "'s wombat for " + damage + " damage!");
        victim.CurrentHealth -= damage;

        // Wombat is no longer bouncing around
        Field.Instance.ClearField();
        TurnManager.Instance.currentStage = Stage.Play;
    }

    static public void DontReact()
    {
        DealDamage(Thrower, Reactor);
    }
}

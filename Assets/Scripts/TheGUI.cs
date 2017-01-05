using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TheGUI : NetworkBehaviour
{
    Player left, right, across;
    [SyncVar]
    public bool Active = false;
    public bool isActive
    {
        set
        {
            Active = value;
        }
    }
    [SyncVar]
    public bool GameIsOver;

    void Start()
    {
        across = TurnManager.Instance.GetPlayerAcrossFrom(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
        right = TurnManager.Instance.GetPlayerToTheRightOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
        left = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
        GameIsOver = false;
    }

    void OnGUI()
    {
        if (!Active)
        {
            return;
        }

        if (!GameIsOver)
        {
            Player currentPlayer = TurnManager.Instance.GetCurrentPlayer();
            // Creates a box in the top middle to show the current stage
            GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 30), "Stage: " + TurnManager.Instance.currentStage.ToString());

            // Draw card at the start of your turn
            if (TurnManager.Instance.currentStage == Stage.Draw && currentPlayer.isLocalPlayer)
            {
                // if they have a sinkhole active take it away
                if (currentPlayer.IsSinkholeActive)
                {
                    currentPlayer.IsSinkholeActive = false;

                    if (!currentPlayer.isServer)
                    {
                        currentPlayer.CmdChangeSinkholeBool(false);
                    }
                }
                // If a bark was used against the player put those cards back into their hand
                CardActions.PlaceBarkedCards(currentPlayer);

                // Reset action count to max
                currentPlayer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer), Player.MaxActions);

                currentPlayer.CurrentActions = Player.MaxActions;
                TurnManager.Instance.currentStage = Stage.Merge;
                currentPlayer.CmdChangeStage(Stage.Merge);

                // Add cards to hand and update hand sizes
                currentPlayer.CurrentHandSize = currentPlayer.Hand.CardsInHand.Count;

                while (currentPlayer.CurrentHandSize < currentPlayer.CurrentMaxHandSize)
                {
                    currentPlayer.Deck.MoveDealtCard();
                    ++currentPlayer.CurrentHandSize;
                }
                ++currentPlayer.CurrentMaxHandSize;

                Field.Instance.ChangeMaxFieldSize(TurnManager.Instance.currentStage);

                // Check if the player has defence cards
                currentPlayer.HasDefenceCards = currentPlayer.Hand.HasDefenceCards();

                if (!isServer)
                {
                    currentPlayer.CmdChangeFieldSize();
                    currentPlayer.CmdChangeHasDefenceCards(currentPlayer.HasDefenceCards);
                }
            }
            // Sifts through the stages and the last stage has an end turn button instead of an end stage button
            else if (TurnManager.Instance.currentStage == Stage.Merge && currentPlayer.isLocalPlayer && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndStage"))
            {
                // Do some kind of transition to visually show the stage has changed
                TurnManager.Instance.currentStage = Stage.Play;
                currentPlayer.CmdChangeStage(Stage.Play);

                Field.Instance.SendFieldBackToHand(currentPlayer);
                Field.Instance.ChangeMaxFieldSize(TurnManager.Instance.currentStage);
                if (!isServer)
                {
                    currentPlayer.CmdChangeFieldSize();
                }
            }
            else if (TurnManager.Instance.currentStage == Stage.Play && currentPlayer.isLocalPlayer && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "EndTurn"))
            {
                // Do some kind of end of turn transition to visually show it
                TurnManager.Instance.currentStage = Stage.Draw;
                currentPlayer.CmdChangeStage(Stage.Draw);

                TurnManager.Instance.EndTurn();
            }
            else if (TurnManager.Instance.currentStage == Stage.Reaction && CardActions.theReactor.isLocalPlayer && GUI.Button(new Rect(Screen.width - 110, Screen.height / 2, 100, 20), "Don'tReact"))
            {
                CardActions.DontReact();
            }

            if (TurnManager.Instance.currentStage == Stage.Merge
                && currentPlayer.isLocalPlayer
               && currentPlayer != null && Field.Instance.IsMergable() != CardType.None
               && currentPlayer.CurrentActions > 0
               && GUI.Button(new Rect(10, Screen.height / 2, 50, 20), "Merge"))
            {
                // Add new power card to hand
                Card newCard;
                switch (Field.Instance.IsMergable())
                {
                    case CardType.Attack:
                        newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_WomboCombo);
                        break;
                    case CardType.Defence:
                        newCard = Instantiate<Card>(GlobalSettings.Instance.Defence_GooglyEyes);
                        break;
                    case CardType.Trap:
                        newCard = Instantiate<Card>(GlobalSettings.Instance.Trap_WombatCage);
                        break;
                    default:
                        Debug.Log("Error in Merge");
                        newCard = Instantiate<Card>(GlobalSettings.Instance.Attack_DonkeyKick);
                        break;
                }
                if (!isServer)
                {
                    currentPlayer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer), currentPlayer.CurrentActions - 1);
                }
                --currentPlayer.CurrentActions;
                newCard.owner = currentPlayer;
                DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
                newCard.CurrentArea = "Hand";
                currentPlayer.Hand.CardsInHand.Add(newCard);

                // Clear field of used cards
                Field.Instance.ClearField();
            }

            DisplayPlayers();
        }
        else if(GameIsOver && GlobalSettings.Instance.GetLocalPlayer().Hand != null)
        {
            foreach (Player p in GlobalSettings.Players)
            {
                if(p != null)
                {
                    p.ClearHand();
                    Destroy(p.Hand.gameObject);
                }
            }

            Destroy(Field.Instance.gameObject);
        }
        // When the game is over the host gets a play again button which resets
        else if (GameIsOver && isServer && GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Play Again!"))
        {
            GlobalSettings.Instance.RpcChangeScene();
        }
        else if(GameIsOver)
        {
            if (GlobalSettings.Instance.GetLocalPlayer().CurrentHealth == 0)
            {
                GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 45, 100, 30), "Loser!");
            }
            else
            {
                GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 45, 100, 30), "Winner!");
            }
        }
    }

    void DisplayPlayers()
    {
        Player local = GlobalSettings.Instance.GetLocalPlayer();
        GUI.Box(new Rect(Screen.width - 100, Screen.height - 20, 100, 20), "HS:" + local.CurrentHandSize.ToString() + " HP:" + local.CurrentHealth.ToString() + " A:" + local.CurrentActions.ToString());
        if (across != null)
        {
            GUI.Box(new Rect(across.Hand.transform.position.x + (Screen.width * 0.5f) - 50, across.Hand.transform.position.y + 25, 100, 20), "HS:" + across.CurrentHandSize.ToString() + " HP:" + across.CurrentHealth.ToString() + " A:" + across.CurrentActions.ToString());
        }
        if (left != null)
        {
            GUI.Box(new Rect(left.Hand.transform.position.x, left.Hand.transform.position.y - (Screen.height * 0.5f), 50, 20), "HS:" + left.CurrentHandSize.ToString() + " HP:" + left.CurrentHealth.ToString() + " A:" + left.CurrentActions.ToString());
        }
        if (right != null)
        {
            GUI.Box(new Rect(right.Hand.transform.position.x, right.Hand.transform.position.y + (Screen.height * 0.5f), 50, 20), "HS:" + right.CurrentHandSize.ToString() + " HP:" + right.CurrentHealth.ToString() + " A:" + right.CurrentActions.ToString());
        }
    }
}

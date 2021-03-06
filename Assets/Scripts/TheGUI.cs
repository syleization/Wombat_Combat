﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TheGUI : NetworkBehaviour
{
    //Player left, right, across;
    [SyncVar]
    public bool Active = false;
    //bool isMerging = false;
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
        //across = TurnManager.Instance.GetPlayerAcrossFrom(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
       // right = TurnManager.Instance.GetPlayerToTheRightOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
        //left = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer()));
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

            if (Pause.Instance.IsPaused == false)
            {
                // Draw card at the start of your turn
                if (TurnManager.Instance.currentStage == Stage.Draw && currentPlayer.isLocalPlayer)
                {
                    // if they have a sinkhole active take it away
                    if (currentPlayer.IsSinkholeActive)
                    {
                        currentPlayer.IsSinkholeActive = false;

                        if (!currentPlayer.isServer)
                        {
                            currentPlayer.CmdChangeSinkholeBool(false, Vector3.zero, Quaternion.identity);
                        }
                        else
                        {
                            currentPlayer.RpcUpdateSinkhole(TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer), false, Vector3.zero, Quaternion.identity);
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
                //    else if (TurnManager.Instance.currentStage == Stage.Merge
                //        && currentPlayer.isLocalPlayer
                //        && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                //        && GUI.Button(new Rect(Screen.width - Screen.width / 4.5f, Screen.height / 2, Screen.width / 5.0f, Screen.height / 15.0f), "EndStage"))
                //    {
                //        // Do some kind of transition to visually show the stage has changed
                //        TurnManager.Instance.currentStage = Stage.Play;
                //        currentPlayer.CmdChangeStage(Stage.Play);

                //        Field.Instance.SendFieldBackToHand(currentPlayer);
                //        Field.Instance.ChangeMaxFieldSize(TurnManager.Instance.currentStage);
                //        if (!isServer)
                //        {
                //            currentPlayer.CmdChangeFieldSize();
                //        }
                //    }
                //    else if (TurnManager.Instance.currentStage == Stage.Play
                //        && currentPlayer.isLocalPlayer
                //        && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                //        && GUI.Button(new Rect(Screen.width - Screen.width / 4.5f, Screen.height / 2, Screen.width / 5.0f, Screen.height / 15.0f), "EndTurn"))
                //    {
                //        // Do some kind of end of turn transition to visually show it
                //        TurnManager.Instance.currentStage = Stage.Draw;
                //        currentPlayer.CmdChangeStage(Stage.Draw);

                //        TurnManager.Instance.EndTurn();
                //    }
                //    else if (TurnManager.Instance.currentStage == Stage.Reaction
                //        && CardActions.theReactor.isLocalPlayer
                //        && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                //        && GUI.Button(new Rect(Screen.width - Screen.width / 4.5f, Screen.height / 2, Screen.width / 5.0f, Screen.height / 15.0f), "Don'tReact"))
                //    {
                //        CardActions.DontReact();
                //    }

                //    if (TurnManager.Instance.currentStage == Stage.Merge
                //        && currentPlayer.isLocalPlayer
                //       && currentPlayer != null && Field.Instance.IsMergable()
                //       && currentPlayer.CurrentActions > 0
                //       && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                //       && isMerging == false
                //       && GUI.Button(new Rect(Screen.width / 50.0f, Screen.height / 2, Screen.width / 10.0f, Screen.height / 15.0f), "Merge"))
                //    {
                //        // Add new power card to hand
                //        isMerging = true;
                //        ///////////////EDIT FOR MERGE ANIMATION/////////////////////
                //        Card newCard = Instantiate(GlobalSettings.Instance.GetMergeCard(Field.Instance.GetCard(0).Type, Field.Instance.GetCard(0).Level));
                //        newCard.gameObject.SetActive(false);

                //        if (!isServer)
                //        {
                //            currentPlayer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer), currentPlayer.CurrentActions - 1);
                //            currentPlayer.CmdPauseGame(CardActions.kMergeEffectTime);
                //            currentPlayer.CmdMergeAnimation(Field.Instance.GetCard(0).SubType, newCard.SubType, TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer));
                //        }
                //        else
                //        {
                //            Pause.Instance.RpcPauseGame(CardActions.kMergeEffectTime);
                //            currentPlayer.RpcMergeAnimation(Field.Instance.GetCard(0).SubType, newCard.SubType, TurnManager.Instance.GetTurnEnumOfPlayer(currentPlayer));
                //        }
                //        --currentPlayer.CurrentActions;
                //        --currentPlayer.CurrentHandSize;

                //        StartCoroutine(WaitToPlaceMergedCardIntoHand(CardActions.kMergeEffectTime, currentPlayer, newCard));
                //        //newCard.owner = currentPlayer;
                //        //DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
                //        //newCard.CurrentArea = "Hand";
                //        //currentPlayer.Hand.CardsInHand.Add(newCard);

                //        //// Clear field of used cards
                //        //Field.Instance.ClearField();
                //    }
                //}
                //DisplayPlayers();
            }
           
        }
        // When the game is over you can now disconnect from the server or if you are the host shut down the server
        else if (GameIsOver)
        {
            if (GlobalSettings.Instance.GetLocalPlayer().CurrentHealth == 0)
            {
                //GUI.Box(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 - Screen.width / 10.0f, Screen.width / 5.0f, Screen.height / 15.0f), "Loser!");
                UI_PlayerInfo.Instance.WinLoss.SetTrigger("Defeat");
            }
            else
            {
                //GUI.Box(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 - Screen.width / 10.0f, Screen.width / 5.0f, Screen.height / 15.0f), "Winner!");
                UI_PlayerInfo.Instance.WinLoss.SetTrigger("Victory");
            }
            StartCoroutine(WaitToEndGame(3.0f));
        }
    }

    IEnumerator WaitToEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isServer)
        {
            NetworkCleanup endGame = FindObjectOfType<NetworkCleanup>();

            endGame.Disconnect();
        }
    }
}

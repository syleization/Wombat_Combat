using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonManager : NetworkBehaviour
{
    // For in game buttons. Not used for connecting to the network
    public enum ButtonType
    {
        ChangeStage,
        Merge,
        EndTurn,
        StartGame,
        DontReact
    }
    [SerializeField]
    ButtonType CurrentButton;
    public ButtonType CurrentButtonType
    {
        set
        {
            CurrentButton = value;
            SwapButtons(GetButtonOfButtonType(value));
        }
        get
        {
            return CurrentButton;
        }
    }
    [SerializeField]
    Button ChangeStageButton;
    [SerializeField]
    Button MergeButton;
    [SerializeField]
    Button EndTurnButton;
    [SerializeField]
    Button StartGameButton;
    [SerializeField]
    Button DontReactButton;

    Button CurrentActiveButton;
    Player LocalPlayer;

    void Start()
    {
        ChangeStageButton.onClick.AddListener(ChangeStage);
        MergeButton.onClick.AddListener(Merge);
        EndTurnButton.onClick.AddListener(EndTurn);
        StartGameButton.onClick.AddListener(StartGame);
        DontReactButton.onClick.AddListener(DontReact);

        ChangeStageButton.gameObject.SetActive(false);
        MergeButton.gameObject.SetActive(false);
        EndTurnButton.gameObject.SetActive(false);
        StartGameButton.gameObject.SetActive(true);
        DontReactButton.gameObject.SetActive(false);

        CurrentActiveButton = StartGameButton;
    }

    void Update()
    {
        // isLocalPlayer can't work until after all awakes and starts are called
        LocalPlayer = GlobalSettings.Instance.GetLocalPlayer();
        enabled = false;
    }

    void SwapButtons(Button newButton)
    {
        CurrentActiveButton.gameObject.SetActive(false);
        newButton.gameObject.SetActive(true);
    }

    Button GetButtonOfButtonType(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.ChangeStage:
                return ChangeStageButton;
            case ButtonType.Merge:
                return MergeButton;
            case ButtonType.EndTurn:
                return EndTurnButton;
            case ButtonType.StartGame:
                return StartGameButton;
            case ButtonType.DontReact:
                return DontReactButton;
        }

        Debug.Log("[ButtonManager::GetButtonOfButtonType] Invalid parameter");
        return StartGameButton;
    }

    void ChangeStage()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if (TurnManager.Instance.currentStage == Stage.Merge
                    && !TurnManager.Instance.IsCurrentlyDisplayingBanner)
            {
                // Do some kind of transition to visually show the stage has changed
                TurnManager.Instance.currentStage = Stage.Play;
                LocalPlayer.CmdChangeStage(Stage.Play);

                Field.Instance.SendFieldBackToHand(LocalPlayer);
                Field.Instance.ChangeMaxFieldSize(TurnManager.Instance.currentStage);
                if (!isServer)
                {
                    LocalPlayer.CmdChangeFieldSize();
                }
            }
        }
    }

    void Merge()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if (TurnManager.Instance.currentStage == Stage.Merge
                  && LocalPlayer != null && Field.Instance.IsMergable()
                  && LocalPlayer.CurrentActions > 0
                  && !TurnManager.Instance.IsCurrentlyDisplayingBanner)
            {
                // Add new power card to hand

                ///////////////EDIT FOR MERGE ANIMATION/////////////////////
                Card newCard = Instantiate(GlobalSettings.Instance.GetMergeCard(Field.Instance.GetCard(0).Type, Field.Instance.GetCard(0).Level));

                if (!isServer)
                {
                    LocalPlayer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(LocalPlayer), LocalPlayer.CurrentActions - 1);
                }
                --LocalPlayer.CurrentActions;
                --LocalPlayer.CurrentHandSize;
                newCard.owner = LocalPlayer;
                DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
                newCard.CurrentArea = "Hand";
                LocalPlayer.Hand.CardsInHand.Add(newCard);

                // Clear field of used cards
                Field.Instance.ClearField();
            }
        }
    }

    void EndTurn()
    {
        if (Pause.Instance.IsPaused == false)
        {
            if (TurnManager.Instance.currentStage == Stage.Play
                    && !TurnManager.Instance.IsCurrentlyDisplayingBanner)
            {
                // Do some kind of end of turn transition to visually show it
                TurnManager.Instance.currentStage = Stage.Draw;
                LocalPlayer.CmdChangeStage(Stage.Draw);

                TurnManager.Instance.EndTurn();
            }
        }
    }

    void StartGame()
    {
        if (Pause.Instance.IsPaused == false && GlobalSettings.Instance.CanInitialize())
        {
            GlobalSettings.Instance.RequestInitialize();
        }
    }

    void DontReact()
    {
        if (Pause.Instance.IsPaused == false)
        {

        }
    }
}

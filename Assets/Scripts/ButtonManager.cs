using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonManager : NetworkBehaviour
{
    private ButtonManager() { }
    static private ButtonManager TheInstance;
    static public ButtonManager Instance
    {
        get
        {
            return TheInstance;
        }
    }
    // For in game buttons. Not used for connecting to the network
    public enum ButtonType
    {
        ChangeStage,
        Merge,
        EndTurn,
        StartGame,
        DontReact,
        None
    }
    [SerializeField]
    ButtonType CurrentButton;
    public ButtonType CurrentButtonType
    {
        set
        {
            CurrentButton = value;
            if (CurrentButton != ButtonType.None)
            {
                SwapButtons(GetButtonOfButtonType(value));
            }
            else
            {
                HideActiveButton();
            }
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
    public bool CurrentButtonIsActive
    {
        get
        {
            return CurrentActiveButton.IsActive();
        }
    }
    Player LocalPlayer;

    bool isMerging = false;

    void Awake()
    {
        TheInstance = this;
        enabled = false;
    }

    void Start()
    {
        ChangeStageButton.onClick.AddListener(ChangeStage);
        MergeButton.onClick.AddListener(Merge);
        EndTurnButton.onClick.AddListener(EndTurn);
        StartGameButton.onClick.AddListener(StartGame);
        DontReactButton.onClick.AddListener(DontReact);

        ChangeStageButton.gameObject.SetActive(true);
        MergeButton.gameObject.SetActive(false);
        EndTurnButton.gameObject.SetActive(false);
        StartGameButton.gameObject.SetActive(false);
        DontReactButton.gameObject.SetActive(false);

        CurrentActiveButton = ChangeStageButton;
        CurrentButton = ButtonType.ChangeStage;
        if(!isServer)
        {
            HideActiveButton();
        }
    }

    // Should only be called once by globalsettings
    public void Initialize()
    {
        // isLocalPlayer can't work until after all awakes and starts are called
        if (LocalPlayer == null)
        {
            LocalPlayer = GlobalSettings.Instance.GetLocalPlayer();
            InvokeRepeating("CheckDontReact", 1.0f, 2.0f);
            InvokeRepeating("CheckMerge", 1.0f, 0.5f);
            enabled = true;
        }
    }

    void Update()
    {
        if (LocalPlayer.IsTurn)
        {
            ShowActiveButton();
            enabled = false;
        }
        else
        {
            foreach(Player p in GlobalSettings.Players)
            {
                if(p != null && p.IsTurn == true)
                {
                    enabled = false;
                }
            }
        }
    }
    void CheckDontReact()
    {
        if (TurnManager.Instance.currentStage == Stage.Reaction
                    && CardActions.theReactor.isLocalPlayer)
        {
            CurrentButtonType = ButtonType.DontReact;
        }
    }

    void CheckMerge()
    {
        if (LocalPlayer != null && Field.Instance.IsMergable()
                && LocalPlayer.CurrentActions > 0
                && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                && isMerging == false)
        {
            CurrentButtonType = ButtonType.Merge;
        }
    }

    public void HideActiveButton()
    {
        CurrentActiveButton.gameObject.SetActive(false);
    }
    public void ShowActiveButton()
    {
        CurrentActiveButton.gameObject.SetActive(true);
    }

    void SwapButtons(Button newButton)
    {
        HideActiveButton();
        CurrentActiveButton = newButton;
        ShowActiveButton();
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
            case ButtonType.None:
                return null;
        }

        Debug.Log("[ButtonManager::GetButtonOfButtonType] Invalid parameter");
        return null;
    }

    public void ChangeStage()
    {
        if (!TurnManager.Instance.IsCurrentlyDisplayingBanner)
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
            CurrentButtonType = ButtonType.EndTurn;
        }
    }

    public void Merge()
    {
        if (LocalPlayer != null && Field.Instance.IsMergable()
                && LocalPlayer.CurrentActions > 0
                && !TurnManager.Instance.IsCurrentlyDisplayingBanner
                && isMerging == false)
        {
            // Add new power card to hand
            isMerging = true;
            ///////////////EDIT FOR MERGE ANIMATION/////////////////////
            Card newCard = Instantiate(GlobalSettings.Instance.GetMergeCard(Field.Instance.GetCard(0).Type, Field.Instance.GetCard(0).Level));
            newCard.gameObject.SetActive(false);

            if (!isServer)
            {
                LocalPlayer.CmdChangeActions(TurnManager.Instance.GetTurnEnumOfPlayer(LocalPlayer), LocalPlayer.CurrentActions - 1);
                LocalPlayer.CmdPauseGame(CardActions.kMergeEffectTime);
                LocalPlayer.CmdMergeAnimation(Field.Instance.GetCard(0).SubType, newCard.SubType, TurnManager.Instance.GetTurnEnumOfPlayer(LocalPlayer));
            }
            else
            {
                Pause.Instance.RpcPauseGame(CardActions.kMergeEffectTime);
                LocalPlayer.RpcMergeAnimation(Field.Instance.GetCard(0).SubType, newCard.SubType, TurnManager.Instance.GetTurnEnumOfPlayer(LocalPlayer));
            }
            --LocalPlayer.CurrentActions;
            --LocalPlayer.CurrentHandSize;

            StartCoroutine(WaitToPlaceMergedCardIntoHand(CardActions.kMergeEffectTime, LocalPlayer, newCard));
        }
    }

    IEnumerator WaitToPlaceMergedCardIntoHand(float waitTime, Player currentPlayer, Card newCard)
    {
        yield return new WaitForSeconds(waitTime);

        newCard.owner = currentPlayer;
        DeckOfCards.TransformDealtCardToHand(newCard, newCard.owner.Hand.CardsInHand.Count);
        newCard.CurrentArea = "Hand";
        currentPlayer.Hand.CardsInHand.Add(newCard);

        // Clear field of used cards
        Field.Instance.ClearField();

        isMerging = false;
        newCard.gameObject.SetActive(true);
        CurrentButtonType = ButtonType.ChangeStage;
    }

    public void EndTurn()
    {
        if (CurrentButtonType != ButtonType.None && !TurnManager.Instance.IsCurrentlyDisplayingBanner)
        {
            // Do some kind of end of turn transition to visually show it
            TurnManager.Instance.currentStage = Stage.Draw;
            LocalPlayer.CmdChangeStage(Stage.Draw);

            TurnManager.Instance.EndTurn();
            CurrentButtonType = ButtonType.None;
        }
    }

    public void StartGame()
    {
        if (GlobalSettings.Instance.CanInitialize())
        {
            GlobalSettings.Instance.RequestInitialize();
            CurrentButtonType = ButtonType.ChangeStage;
        }
    }

    public void DontReact()
    {
        if (!TurnManager.Instance.IsCurrentlyDisplayingBanner)
        {
            CardActions.DontReact();
        }
    }
}

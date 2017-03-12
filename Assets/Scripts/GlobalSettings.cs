using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum AreaPosition { Left, Top, Right, Bottom }
public enum GameType { TwoPlayer, ThreePlayer, FourPlayer }
public class GlobalSettings : NetworkBehaviour
{
    [Header("Game Settings")]
    public GameType TypeOfGame;
    [Header("Players")]
    public Player LeftPlayer;
    public string LeftPlayerName;
    public Player TopPlayer;
    public string TopPlayerName;
    public Player RightPlayer;
    public string RightPlayerName;
    public Player BottomPlayer;
    public string BottomPlayerName;
    [Header("Prefabs and Assets")]
    public Card Attack_WombatCharge;
    public Card Attack_DonkeyKick;
    public Card Attack_WomboCombo;
    public Card Defence_Bite;
    public Card Defence_Bark;
    public Card Defence_GooglyEyes;
    public Card Trap_Trampoline;
    public Card Trap_Sinkhole;
    public Card Trap_WombatCage;
    [SerializeField]
    private Hand Handzone;
    [SerializeField]
    private Field Fieldzone;
    [SerializeField]
    private TheGUI TheGUI;

    [Header("Damages")]
    public const int Damage_DonkeyKick = 1;
    public const int Damage_WombatCharge = 3;
    public const int Damage_WomboCombo = 5;
    public static List<Player> Players = new List<Player>();
    private List<Card> AllCards = new List<Card>();
    public bool CanStartGame = false;
    private int CurrentPlayerCount = 0;
    // SINGLETON
    private static GlobalSettings TheInstance;

    private GlobalSettings() {}

    public static GlobalSettings Instance
    {
        get
        {
            if (TheInstance == null)
            {
                TheInstance = FindObjectOfType<GlobalSettings>();
            }

            return TheInstance;
        }
    }

    void Awake()
    {
        TheInstance = this;
        AllCards.Add(Attack_DonkeyKick);
        AllCards.Add(Attack_WombatCharge);
        AllCards.Add(Attack_WomboCombo);
        AllCards.Add(Defence_Bark);
        AllCards.Add(Defence_Bite);
        AllCards.Add(Defence_GooglyEyes);
        AllCards.Add(Trap_Trampoline);
        AllCards.Add(Trap_Sinkhole);
        AllCards.Add(Trap_WombatCage);

        // Alex's Testing
        if (GameObject.Find("EffectTester"))
        {
            Players.Add(LeftPlayer);
            Players.Add(TopPlayer);
            Players.Add(RightPlayer);
            Players.Add(BottomPlayer);
        }
        else
        {
            Debug.Log("Don't forget to get rid of the test code");
        }
    }

    void Initialize()
    {
        Debug.Log("Initialize");
        HudManager networkHud = FindObjectOfType<HudManager>();
        NetworkCleanup networkCleanup = FindObjectOfType<NetworkCleanup>();

        networkCleanup.ShowGUI = true;
#if UNITY_ANDROID
        networkHud.ShowGUI = false;
#else
        networkHud.ToggleHUD();
#endif

        Players.Add(LeftPlayer);
        Players.Add(TopPlayer);
        Players.Add(RightPlayer);
        Players.Add(BottomPlayer);

        if (TypeOfGame != GameType.TwoPlayer)
        {
            LeftPlayer.Name = LeftPlayerName;
        }

        if (TypeOfGame == GameType.FourPlayer)
        {
            RightPlayer.Name = RightPlayerName;
        }

        if (TopPlayer)
        {
            TopPlayer.Name = TopPlayerName;
        }
        BottomPlayer.Name = BottomPlayerName;

        SpawnPlayers();

        CanvasManager.Instance.Initialize();

        if (isServer)
        {
            TurnManager.Instance.Initialize();

            Field newField;
            newField = Instantiate<Field>(Fieldzone);
            newField.transform.position = new Vector3(0.0f, 0.0f, 1.0f);
            NetworkServer.Spawn(newField.gameObject);

            TheGUI gui = Instantiate<TheGUI>(TheGUI);
            NetworkServer.Spawn(gui.gameObject);
            gui.isActive = true;
        }
    }

    public void Terminate()
    {
        CanvasManager.Instance.Terminate();
        // Clear Players
        foreach(Player player in Players)
        {
            if(player != null)
            {
                foreach(Card card in player.Hand.CardsInHand)
                {
                    Destroy(card.gameObject);
                }
                player.Hand.CardsInHand.Clear();
                Destroy(player.Hand.gameObject);
                Destroy(player.gameObject);
            }
        }
        Players.Clear();
        LeftPlayer = null;
        TopPlayer = null;
        RightPlayer = null;
        BottomPlayer = null;

        // Reset original values
        CanStartGame = false;
        CurrentPlayerCount = 0;
        LeftPlayerName = "";
        TopPlayerName = "";
        RightPlayerName = "";
        BottomPlayerName = "";
    }

    void SpawnPlayers()
    {
        Hand newHand;
        // Instantiate localplayer's hand
        Player local = GetLocalPlayer();
        newHand = Instantiate<Hand>(Handzone);
        local.Hand = newHand;
        newHand.transform.position = new Vector3(0.0f, -4.5f, 1.0f);
        newHand.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        // Instantiate the player's hand across from the local player
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(TurnManager.Instance.GetTurnEnumOfPlayer(local));
        if (across != null)
        {
            newHand = Instantiate<Hand>(Handzone);
            across.Hand = newHand;
            newHand.transform.position = new Vector3(0.0f, 4.5f, 1.0f);
            newHand.transform.rotation = new Quaternion(0.0f, 0.0f, -180.0f, 0.0f);
        }

        // Instantiate the player's hand to the left of the local player
        Player left = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(local));
        if (left != null)
        {
            newHand = Instantiate<Hand>(Handzone);
            left.Hand = newHand;
            newHand.transform.position = new Vector3(-5.0f, 0.0f, 1.0f);
            newHand.transform.rotation = new Quaternion(0.0f, 0.0f, -90.0f, 0.0f);
        }

        // Instantiate the player's hand to the right of the local player
        Player right = TurnManager.Instance.GetPlayerToTheRightOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(local));

        if (right != null)
        {
            newHand = Instantiate<Hand>(Handzone);
            right.Hand = newHand;
            newHand.transform.position = new Vector3(5.0f, 0.0f, 1.0f);
            newHand.transform.rotation = new Quaternion(0.0f, 0.0f, 90.0f, 0.0f);
        }
    }

    public Player GetLocalPlayer()
    {
        foreach(Player p in Players)
        {
            if(p != null && p.isLocalPlayer)
            {
                return p;
            }
        }
        Debug.Log("[GlobalSettings::GetLocalPlayer] No player is local to this client");
        return null;
    }
    public void AddNetworkPlayer(Player player)
    {
        ++CurrentPlayerCount;
        if (BottomPlayer == null)
        {
            BottomPlayer = player;
        }
        else if (TopPlayer == null)
        {
            TopPlayer = player;
        }
        else if (LeftPlayer == null)
        {
            LeftPlayer = player;
        }
        else if(RightPlayer == null)
        {
            RightPlayer = player;
        }
        else
        {
            --CurrentPlayerCount;
            Debug.Log("Player Not Registered");
        }

       // CanStartGame = true;// A testing shortcut
        if(TypeOfGame == GameType.TwoPlayer && CurrentPlayerCount == 2)
        {
            CanStartGame = true;
        }
        else if(TypeOfGame == GameType.ThreePlayer && CurrentPlayerCount == 3)
        {
            CanStartGame = true;
        }
        else if(TypeOfGame == GameType.FourPlayer && CurrentPlayerCount == 4)
        {
            CanStartGame = true;
        }

    }

    public int GetDamageAmountOf(CardSubType type)
    {
        switch(type)
        {
            case CardSubType.DonkeyKick:
                return Damage_DonkeyKick;
            case CardSubType.WombatCharge:
                return Damage_WombatCharge;
            case CardSubType.WomboCombo:
                return Damage_WomboCombo;
        }

        Debug.Log("[GlobalSettings::GetDamageAmountOf] Invalid parameter");
        return -1;
    }

    public Card GetCardOfSubType(CardSubType type)
    {
        switch(type)
        {
            case CardSubType.DonkeyKick:
                return Attack_DonkeyKick;
            case CardSubType.WombatCharge:
                return Attack_WombatCharge;
            case CardSubType.WomboCombo:
                return Attack_WomboCombo;
            case CardSubType.Bark:
                return Defence_Bark;
            case CardSubType.Bite:
                return Defence_Bite;
            case CardSubType.GooglyEyes:
                return Defence_GooglyEyes;
            case CardSubType.Trampoline:
                return Trap_Trampoline;
            case CardSubType.Sinkhole:
                return Trap_Sinkhole;
            case CardSubType.WombatCage:
                return Trap_WombatCage;
        }

        Debug.Log("[GlobalSettings::GetCardOfSubType] Invalid parameter");
        return null;
    }

    // This is a function so the other clients only need to know about the damage amount the card being thrown at them is doing 
    // instead of having to save the entire card object
    public Card GetAttackCardOfDamageAmount(int damage)
    {
        switch(damage)
        {
            case Damage_DonkeyKick: return Attack_DonkeyKick;

            case Damage_WombatCharge: return Attack_WombatCharge;

            case Damage_WomboCombo: return Attack_WomboCombo;
        }

        Debug.Log("[GlobalSettings::GetAttackCardOfDamageAmount] Invalid parameter");
        return null;
    }

    public Card GetMergeCard(CardType type, CardLevel level)
    {
        int typeIndex = (int)type;
        int levelIndex = (int)level + 1;
        const int amountOfCardTypes = 3;

        return AllCards[typeIndex * amountOfCardTypes  + levelIndex];
    }

    void OnGUI()
    {
        if(isServer && CanStartGame && GUI.Button(new Rect(Screen.width - 130, Screen.height / 2, 120, 20), "Start Game"))
        {
            CanStartGame = false;
            RpcInitialize();
        }
    }

    [ClientRpc]
    void RpcInitialize()
    {
        Initialize();
    }

    [ClientRpc]
    public void RpcEndGame()
    {
        TheGUI.GameIsOver = true;
    }
}

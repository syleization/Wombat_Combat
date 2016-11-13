using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Turns { LeftPlayer, TopPlayer, RightPlayer, BottomPlayer }
// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour
{
    // for Singleton Pattern
    public static TurnManager Instance;
    private Turns CurrentTurn; // 0 for left player, 1 for top player, 2 for right player, 3 for bottom player
    private Player PlayerBottom;

    void Start()
    {
        PlayerBottom = FindObjectOfType<Player>(); // TEMPORARY
        switch(Random.Range(0, 4))
        {
            case 0: CurrentTurn = Turns.LeftPlayer;
                break;
            case 1: CurrentTurn = Turns.TopPlayer;
                break;
            case 2: CurrentTurn = Turns.RightPlayer;
                break;
            case 3: CurrentTurn = Turns.BottomPlayer;
                break;
            default:
                break;
        }

        SetTurnBools();
    }

    void SetTurnBools()
    {
        PlayerBottom.IsTurn = true;        //TEMPORARY
        //switch(CurrentTurn)
        //{
        //    case Turns.LeftPlayer: PlayerLeft.IsTurn = false; PlayerTop.IsTurn = true;
        //        break;
        //    case Turns.TopPlayer: PlayerTop.IsTurn = false; PlayerRight.IsTurn = true;
        //        break;
        //    case Turns.RightPlayer: PlayerRight.IsTurn = false; PlayerBottom.IsTurn = true;
        //        break;
        //    case Turns.BottomPlayer: PlayerBottom.IsTurn = false; PlayerLeft.IsTurn = true;
        //        break;
        //    default:
        //        break;
        //}
    }
    //private Player _whoseTurn;
    //public Player whoseTurn
    //{
    //    get
    //    {
    //        return _whoseTurn;
    //    }

    //    set
    //    {
    //        _whoseTurn = value;
    //        timer.StartTimer();

    //        GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

    //        TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
    //        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
    //        tm.OnTurnStart();
    //        if (tm is PlayerTurnMaker)
    //        {
    //            whoseTurn.HighlightPlayableCards();
    //        }
    //        // remove highlights for opponent.
    //        whoseTurn.otherPlayer.HighlightPlayableCards(true);

    //    }
    //}

    void Awake()
    {
        Instance = this;
    }

    //void Start()
    //{
    //    OnGameStart();
    //}

    //public void OnGameStart()
    //{
    //    //Debug.Log("In TurnManager.OnGameStart()");

    //    CardLogic.CardsCreatedThisGame.Clear();
    //    CreatureLogic.CreaturesCreatedThisGame.Clear();

    //    foreach (Player p in Player.Players)
    //    {
    //        p.ManaThisTurn = 0;
    //        p.ManaLeft = 0;
    //        p.LoadCharacterInfoFromAsset();
    //        p.TransmitInfoAboutPlayerToVisual();
    //        p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
    //        // move both portraits to the center
    //        p.PArea.Portrait.transform.position = p.PArea.handVisual.OtherCardDrawSourceTransform.position;
    //    }

    //    Sequence s = DOTween.Sequence();
    //    s.Append(Player.Players[0].PArea.Portrait.transform.DOMove(Player.Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
    //    s.Insert(0f, Player.Players[1].PArea.Portrait.transform.DOMove(Player.Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
    //    s.PrependInterval(3f);
    //    s.OnComplete(() =>
    //    {
    //        // determine who starts the game.
    //        int rnd = Random.Range(0, 2);  // 2 is exclusive boundary
    //                                       // Debug.Log(Player.Players.Length);
    //        Player whoGoesFirst = Player.Players[rnd];
    //        // Debug.Log(whoGoesFirst);
    //        Player whoGoesSecond = whoGoesFirst.otherPlayer;
    //        // Debug.Log(whoGoesSecond);

    //        // draw 4 cards for first player and 5 for second player
    //        int initDraw = 4;
    //        for (int i = 0; i < initDraw; i++)
    //        {
    //            // second player draws a card
    //            whoGoesSecond.DrawACard(true);
    //            // first player draws a card
    //            whoGoesFirst.DrawACard(true);
    //        }
    //        // add one more card to second player`s hand
    //        whoGoesSecond.DrawACard(true);
    //        //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
    //        whoGoesSecond.DrawACoin();
    //        new StartATurnCommand(whoGoesFirst).AddToQueue();
    //    });
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        EndTurn();
    //}

    public void EndTurn()
    {
        // stop timer
        //timer.StopTimer();
        // send all commands in the end of current player`s turn
        //whoseTurn.OnTurnEnd();

        //new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
        PlayerBottom.IsTurn = !PlayerBottom.IsTurn;
    }

    //public void StopTheTimer()
    //{
    //    timer.StopTimer();
    //}

}


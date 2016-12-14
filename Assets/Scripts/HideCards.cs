using UnityEngine;
using System.Collections;

public class HideCards : MonoBehaviour
{
    private HideCards() { }
    private static HideCards TheInstance;
    public static HideCards Instance
    {
        get
        {
           return TheInstance;
        }
    }
    public Sprite CardBack;

    void Awake()
    {
        TheInstance = this;
    }

    public void HideCardsOfOtherPlayers()
    {
        foreach(Player player in GlobalSettings.Players)
        {
            if(player.IsTurn == false && player.isActiveAndEnabled)
            {
                foreach(Card card in player.Hand.CardsInHand)
                {
                    SpriteRenderer cardSprite = card.gameObject.GetComponent<SpriteRenderer>();
                    cardSprite.sprite = CardBack;
                }
            }
        }
    }

    public void ShowCardsOfCurrentPlayer()
    {
        Player current = TurnManager.Instance.GetCurrentPlayer();

        foreach(Card card in current.Hand.CardsInHand)
        {
            SpriteRenderer cardSprite = card.gameObject.GetComponent<SpriteRenderer>();
            cardSprite.sprite = card.OriginalSprite;
        }
    }
}

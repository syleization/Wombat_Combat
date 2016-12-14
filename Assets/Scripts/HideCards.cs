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

    public void HideCardsOfOtherPlayers(Player notHidden)
    {
        foreach(Player player in GlobalSettings.Players)
        {
            if(player != notHidden && player.isActiveAndEnabled)
            {
                foreach(Card card in player.Hand.CardsInHand)
                {
                    SpriteRenderer cardSprite = card.gameObject.GetComponent<SpriteRenderer>();
                    cardSprite.sprite = CardBack;
                }
            }
        }
    }

    public void ShowCardsOfPlayer(Player player)
    {
        foreach(Card card in player.Hand.CardsInHand)
        {
            SpriteRenderer cardSprite = card.gameObject.GetComponent<SpriteRenderer>();
            cardSprite.sprite = card.OriginalSprite;
        }
    }

    public void HideCardsOfPlayer(Player player)
    {
        foreach (Card card in player.Hand.CardsInHand)
        {
            SpriteRenderer cardSprite = card.gameObject.GetComponent<SpriteRenderer>();
            cardSprite.sprite = CardBack;
        }
    }
}

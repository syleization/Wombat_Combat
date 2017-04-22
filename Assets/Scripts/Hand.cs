using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public List<Card> CardsInHand = new List<Card>();

    #region Glow
    public void ClearGlow()
    {
        foreach (Card card in CardsInHand)
        {
            if(card.transform.childCount > 0)
            {
                Destroy(card.transform.GetChild(0).gameObject);
            }
            card.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void UpdateGlow(CardSubType subtype)
    {
        foreach(Card card in CardsInHand)
        {
            if(card.SubType == subtype && card.transform.childCount == 0)
            {
                GameObject glowParticles = GetMaterialOfSubType(subtype);

                glowParticles.transform.parent = card.transform;
                glowParticles.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                glowParticles.transform.localRotation = glowParticles.transform.localRotation * Quaternion.Euler(Vector3.up * 180.0f);
                glowParticles.transform.localPosition = new Vector3(0.0f, -1.0f, 18.8f);
            }
            else if(card.SubType != subtype)
            {
                card.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
            }
        }
    }

    public void UpdateGlow(CardType type)
    {
        foreach (Card card in CardsInHand)
        {
            if (card.Type == type && card.transform.childCount == 0)
            {
                GameObject glowParticles = GetMaterialOfType(type);

                glowParticles.transform.parent = card.transform;
                glowParticles.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                glowParticles.transform.localRotation = glowParticles.transform.localRotation * Quaternion.Euler(Vector3.up * 180.0f);
                glowParticles.transform.localPosition = new Vector3(0.0f, -1.0f, 18.8f);
            }
            else if(card.Type != type)
            {
                card.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
            }
        }
    }

    GameObject GetMaterialOfSubType(CardSubType subtype)
    {
        switch (subtype)
        {
            case CardSubType.DonkeyKick:
            case CardSubType.WombatCharge:
            case CardSubType.WomboCombo:
                return Instantiate(GlobalSettings.Instance.AttackGlow).gameObject;
            case CardSubType.Bark:
            case CardSubType.Bite:
            case CardSubType.GooglyEyes:
                return Instantiate(GlobalSettings.Instance.DefenceGlow).gameObject;
            case CardSubType.Trampoline:
            case CardSubType.Sinkhole:
            case CardSubType.WombatCage:
                return Instantiate(GlobalSettings.Instance.TrapGlow).gameObject;
        }

        Debug.Log("[Hand::GetMaterialOfSubType] Invalid parameter");
        return null;
    }

    public static GameObject GetMaterialOfType(CardType type)
    {
        switch (type)
        {
            case CardType.Attack:
                return Instantiate(GlobalSettings.Instance.AttackGlow).gameObject;
            case CardType.Defence:
                return Instantiate(GlobalSettings.Instance.DefenceGlow).gameObject;
            case CardType.Trap:
                return Instantiate(GlobalSettings.Instance.TrapGlow).gameObject;
        }

        Debug.Log("[Hand::GetMaterialOfSubType] Invalid parameter");
        return null;
    }
    #endregion

    public void ResetHandCardPositions(Card cardMoved, int cardsDealt)
    {
        for (int i = 0; i < cardsDealt - 1; ++i)
        {
            if (CardsInHand[i] == cardMoved)
            {
                // If the current card is the card that is moving then swap its position in the hand to the end
                Card temp = CardsInHand[i];
                CardsInHand[i] = CardsInHand[cardsDealt - 1];
                CardsInHand[cardsDealt - 1] = temp;
            }
            //CardsInHand[i].transform.position = new Vector3(((float)i * 2) - 5, -5, (float)i * -0.01f);
            DeckOfCards.TransformDealtCardToHand(CardsInHand[i], i);
        }
    }

    public int GetCardIndex(GameObject card)
    {
        for(int i = 0; i < CardsInHand.Count; ++i)
        {
            if(CardsInHand[i] == card)
            {
                return i;
            }
        }
        Debug.Log("Card being sent to field that isnt in hand");
        return -1; // error
    }

    public static bool IsYourHand(Card card, GameObject hit)
    {
        return hit == card.owner.Hand.gameObject ? true : false; // if the hand the raycast hit is yours
    }

    public static Player GetOwner(GameObject hit)
    {
        foreach(Player player in GlobalSettings.Players)
        {
            // if the gameobject the card was let go on is the hand of the player
            if(player != null && hit == player.Hand.gameObject)
            {
                return player;
            }
        }
        Debug.Log("[Hand::GetOwner] Invalid Return path");
        return null;
    }

    public bool HasDefenceCards()
    {
        foreach(Card card in CardsInHand)
        {
            if(card.Type == CardType.Defence)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasTrapCards()
    {
        foreach (Card card in CardsInHand)
        {
            if (card.Type == CardType.Trap)
            {
                return true;
            }
        }

        return false;
    }

}

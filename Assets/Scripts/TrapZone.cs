using UnityEngine;
using System.Collections;

public class TrapZone : MonoBehaviour
{
    const int TrapCardsLength = 2;
    [SerializeField]
    Card[] TrapCards = new Card[TrapCardsLength];

    public void Initialize()
    {
        for(int i = 0; i < TrapCardsLength; ++i)
        {
            TrapCards[i] = null;
        }
        // Add reference to player to access
        if(GlobalSettings.Instance.TutorialScene)
        {
            GlobalSettings.Instance.BottomPlayer.Traps = this;
        }
        else
        {
            GlobalSettings.Instance.GetLocalPlayer().Traps = this;
        }
    }

    public void Terminate()
    {
        for (int i = 0; i < TrapCardsLength; ++i)
        {
            if (TrapCards[i] != null)
            {
                Destroy(TrapCards[i].gameObject);
                TrapCards[i] = null;
            }
        }
    }

    public int GetTrapCardCount()
    {
        int count = 0;

        for (int i = 0; i < TrapCardsLength; ++i)
        {
            if(TrapCards[i] != null)
            {
                ++count;
            }
        }

        return count;
    }

    public bool HasTraps()
    {
        return GetTrapCardCount() > 0 ? true : false;
    }

    public void ToggleActive(Card exception = null)
    {
        foreach(Card card in TrapCards)
        {
            if(card != null && card != exception)
            {
                card.gameObject.SetActive(!card.gameObject.activeSelf);
            }
        }
    }

    public void RemoveTrap(Card removeThis)
    {
        for (int i = 0; i < TrapCardsLength; ++i)
        {
            if (TrapCards[i] == removeThis)
            {
                TrapCards[i] = null;
                ResetCardPositions();
                CanvasManager.Instance.UpdateCanvas("Trap");
                return;
            }
        }
    }
    // If this succeeds the card moves from the field to this
    // If it fails the trap card should go back to hand
    public bool SetTrap(Card card)
    {
        if(card.Type != CardType.Trap)
        {
            return false;
        }

        for (int i = 0; i < TrapCardsLength; ++i)
        {
            if(TrapCards[i] == null)
            {
                if (GlobalSettings.Instance.TutorialScene == false)
                {
                    Field.Instance.CardsInField.Remove(card);
                    Field.Instance.ResetFieldCardPositions();
                }
                card.owner.IsHoldingCard = false;
                TrapCards[i] = card;
                TrapCards[i].CurrentArea = "TrapZone";
                TrapCards[i].gameObject.SetActive(false);
                ResetCardPositions();
                CanvasManager.Instance.UpdateCanvas("Trap");

                return true;
            }
        }

        return false;
    }

    void ResetCardPositions()
    {
        // Hard coded positions for the trap cards to appear at
        if(TrapCards[0] != null)
        {
            if(TrapCards[1] != null)
            {
                // Two Card positions
                TrapCards[0].transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, -1.0f);
                TrapCards[1].transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, -1.0f);
            }
            else
            {
                // One Card positions
                TrapCards[0].transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
            }
        }
    }
}

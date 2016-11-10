using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPopUp : MonoBehaviour
{
    DeckOfCards Deck;

    Ray ray;
    RaycastHit hit;

    private bool waitingForCard = false;
    private float heightChange = 3.0f;
    private float waitTime = 0.1f;

    public bool cardIsDown = true;

    void Start()
    {
        Deck = FindObjectOfType<DeckOfCards>();
    }

    void OnMouseEnter()
    {
        if (cardIsDown && MouseIsWithinBounds() && !Deck.isHoldingCard)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Card")
                {
                    cardIsDown = false;
                    ChangeYPosition(heightChange);
                }
            }
        }
    }

    void OnMouseExit()
    {
        if(!cardIsDown && !waitingForCard && !Deck.isHoldingCard)
        {
            waitingForCard = true;
            StartCoroutine(WaitToPutCardDown(waitTime));
        }
    }

    bool MouseIsWithinBounds()
    {
        return 
            Input.mousePosition.x > 0 && 
            Input.mousePosition.x < Screen.width &&
            Input.mousePosition.y > 0 && 
            Input.mousePosition.y < Screen.height;
    }

    void ChangeYPosition(float amount)
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount);
    }

    IEnumerator WaitToPutCardDown(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeYPosition(-heightChange);
        cardIsDown = true;
        waitingForCard = false;
    }
}

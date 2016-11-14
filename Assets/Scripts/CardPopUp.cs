using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPopUp : MonoBehaviour
{
    Card Card;

    Ray ray;
    RaycastHit hit;

    private bool waitingForCard = false;
    private float heightChange = 3.0f;
    private float waitTime = 0.1f;

    public bool cardIsDown = true;

    void Start()
    {
        Card = GetComponent<Card>();
    }

    void OnMouseEnter()
    {
        if (cardIsDown && MouseIsWithinBounds() && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.tag == "BottomPlayer")
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Card")
                {
                    cardIsDown = false;
                    PopUp(heightChange);
                }
            }
        }
    }

    void OnMouseExit()
    {
        if(!cardIsDown && !waitingForCard && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.tag == "BottomPlayer")
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

    void PopUp(float amount)
    {
        // If it was 4 non AI players
        //if(Card.owner.tag == "LeftPlayer")
        //{
        //    this.transform.position = new Vector3(transform.position.x + amount, transform.position.y, transform.position.z - amount * 0.01f);
        //}
        //else if (Card.owner.tag == "TopPlayer")
        //{
        //    this.transform.position = new Vector3(transform.position.x, transform.position.y - amount, transform.position.z - amount * 0.01f);
        //}
        //else if (Card.owner.tag == "RightPlayer")
        //{
        //    this.transform.position = new Vector3(transform.position.x - amount, transform.position.y, transform.position.z - amount * 0.01f);
        //}
        //else if (Card.owner.tag == "BottomPlayer")
        //{
        //    this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount * 0.01f);
        //}
        //else
        //{
        //    Debug.Log("ERROR | A player isnt tagged correctly");
        //}
          this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount * 0.01f);
    }

    IEnumerator WaitToPutCardDown(float time)
    {
        yield return new WaitForSeconds(time);
        PopUp(-heightChange);
        cardIsDown = true;
        waitingForCard = false;
    }
}

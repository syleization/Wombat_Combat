﻿using UnityEngine;
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

    void Update()
    {
        // Temporary fix for the errors of onMouseExit with andoid
        // Should later be replaced with a better input detection system
        if (Application.platform == RuntimePlatform.Android && Input.touchCount == 0 && !cardIsDown && !waitingForCard && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.isLocalPlayer)
        {
            waitingForCard = true;
            StartCoroutine(WaitToPutCardDown(waitTime));
        }
    }
    
    void OnMouseEnter()
    {
        if (cardIsDown && MouseIsWithinBounds() && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.isLocalPlayer)
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
        if(Application.platform != RuntimePlatform.Android && !cardIsDown && !waitingForCard && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.isLocalPlayer)
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
        this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount * 0.01f);
        // If it was 4 non AI players
        //Turns turn = TurnManager.Instance.GetTurnEnumOfPlayer(Card.owner);
        //if (turn == Turns.LeftPlayer)
        //{
        //    this.transform.position = new Vector3(transform.position.x + amount, transform.position.y, transform.position.z - amount * 0.01f);
        //}
        //else if (turn == Turns.TopPlayer)
        //{
        //    this.transform.position = new Vector3(transform.position.x, transform.position.y - amount, transform.position.z - amount * 0.01f);
        //}
        //else if (turn == Turns.RightPlayer)
        //{
        //    this.transform.position = new Vector3(transform.position.x - amount, transform.position.y, transform.position.z - amount * 0.01f);
        //}
        //else if (turn == Turns.BottomPlayer)
        //{
        //    this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount * 0.01f);
        //}
        //else
        //{
        //    Debug.Log("ERROR[CardPopUp::PopUp] | A player isnt tagged correctly");
        //}
        // this.transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z - amount * 0.01f);
    }

    IEnumerator WaitToPutCardDown(float time)
    {
        yield return new WaitForSeconds(time);
        PopUp(-heightChange);
        cardIsDown = true;
        waitingForCard = false;
    }
}

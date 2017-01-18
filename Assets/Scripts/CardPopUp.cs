using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPopUp : MonoBehaviour
{
    Card TheCard;

    Ray ray;
    RaycastHit hit;

    private bool waitingForCard = false;
    private float heightChange = 3.0f;
    private float waitTime = 0.1f;

    public bool cardIsDown = true;

    void Start()
    {
        TheCard = GetComponent<Card>();
        
    }

    //void Update()
    //{
    //    // Temporary fix for the errors of onMouseExit with andoid
    //    // Should later be replaced with a better input detection system
    //    if (Application.platform == RuntimePlatform.Android && Input.touchCount == 0 && !cardIsDown && !waitingForCard && !Card.owner.IsHoldingCard && Card.IsInHand && Card.owner.isLocalPlayer)
    //    {
    //        waitingForCard = true;
    //        StartCoroutine(WaitToPutCardDown(waitTime));
    //    }
    //}
#if UNITY_ANDROID
    void Update()
    {
        if(Input.touchCount == 1 && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && TheCard.owner.isLocalPlayer)
        {
            Touch touch = Input.GetTouch(0);

            if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                // This raycast goes past the card to see the hand
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == TheCard.gameObject && cardIsDown)
                    {
                        cardIsDown = false;
                        PopUp(heightChange);
                    }
                    else if(hit.collider.gameObject != TheCard.gameObject && touch.phase == TouchPhase.Moved && !cardIsDown && !waitingForCard)
                    {
                        waitingForCard = true;
                        StartCoroutine(WaitToPutCardDown(waitTime));
                    }
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if (!cardIsDown && !waitingForCard && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && TheCard.owner.isLocalPlayer)
                {
                    waitingForCard = true;
                    StartCoroutine(WaitToPutCardDown(waitTime));
                }
            }
        }
    }
#else
    void OnMouseEnter()
    {
        if (cardIsDown && MouseIsWithinBounds() && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && TheCard.owner.isLocalPlayer)
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
        if(!cardIsDown && !waitingForCard && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && TheCard.owner.isLocalPlayer)
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
#endif

    void PopUp(float amount)
    {
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

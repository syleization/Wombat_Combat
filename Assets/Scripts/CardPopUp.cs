﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPopUp : MonoBehaviour
{
    Card TheCard;
    Ray ray;
    RaycastHit hit;
    Timer StationaryTimer = new Timer();

    private bool waitingForCard = false;
    private float heightChange = 3.0f;
    private float waitTime = 0.1f;

    public bool cardIsDown = true;
    public bool CardCanMoveNow = false;

    void Start()
    {
        TheCard = GetComponent<Card>();
        StationaryTimer.Initialize(0.05f);
    }


#if UNITY_ANDROID
    bool MobileStop = false;

    void DisplayCardInfoBox()
    {
        Sprite infoSprite = Resources.Load<Sprite>(TheCard.SubType.ToString() + "_InfoBox");
        CanvasManager.Instance.UpdateCanvas("CardTap", infoSprite);
    }

    void SetCardCanMoveNow()
    {
        CardCanMoveNow = true;
    }

    public void ClearInfoBox()
    {
        CanvasManager.Instance.UpdateCanvas("CardTap", null);
    }

    void FixedUpdate()
    {
        if(Pause.Instance.IsPaused == false && Input.touchCount == 1 && !TheCard.owner.IsHoldingCard && MobileStop == false && CardCanMoveNow == false)
        {
            MobileStop = true;
            Touch touch = Input.GetTouch(0);
            // If someone taps the screen display info box
            if (touch.phase == TouchPhase.Ended && CanvasManager.Instance.IsCardTapCanvasImageNull())
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == TheCard.gameObject)
                    {
                        DisplayCardInfoBox();
                    }
                }
                CardCanMoveNow = false;
            }
            // If someone taps somwhere else that isnt the card and the card info box is currently up
            else if (touch.phase == TouchPhase.Began && !CanvasManager.Instance.IsCardTapCanvasImageNull())
            {
                Debug.Log("Destroy");
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject != TheCard.gameObject)
                    {
                        ClearInfoBox();
                    }
                }
                CardCanMoveNow = false;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == TheCard.gameObject)
                    {
                        StationaryTimer.TimerAction(SetCardCanMoveNow);
                    }
                    else
                    {
                        StationaryTimer.Initialize(0.05f);
                    }
                }
            }
            MobileStop = false;
        //    if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        RaycastHit hit;
        //        // This raycast goes past the card to see the hand
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.collider.gameObject == TheCard.gameObject && cardIsDown)
        //            {
        //                cardIsDown = false;
        //                PopUp(heightChange);
        //            }
        //            else if(hit.collider.gameObject != TheCard.gameObject && touch.phase == TouchPhase.Moved && !cardIsDown && !waitingForCard)
        //            {
        //                waitingForCard = true;
        //                StartCoroutine(WaitToPutCardDown(waitTime));
        //            }
        //        }
        //    }
        //    else if(touch.phase == TouchPhase.Ended)
        //    {
        //        if (!cardIsDown && !waitingForCard && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && TheCard.owner.isLocalPlayer)
        //        {
        //            waitingForCard = true;
        //            StartCoroutine(WaitToPutCardDown(waitTime));
        //        }
        //    }
        }
    }
#else
    void OnMouseEnter()
    {
        if (Pause.Instance.IsPaused && !GlobalSettings.Instance.TutorialHack) return;

        if (cardIsDown && MouseIsWithinBounds() && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && (TheCard.owner.isLocalPlayer || GlobalSettings.Instance.TutorialHack) && TheCard.CurrentArea != "TrapZone")
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
        if (Pause.Instance.IsPaused && !GlobalSettings.Instance.TutorialHack) return;

        if (!cardIsDown && !waitingForCard && !TheCard.owner.IsHoldingCard && TheCard.IsInHand && (TheCard.owner.isLocalPlayer || GlobalSettings.Instance.TutorialHack) && TheCard.CurrentArea != "TrapZone")
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

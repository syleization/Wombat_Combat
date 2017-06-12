using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_PlayerInfo : MonoBehaviour
{
    private UI_PlayerInfo() { }
    static private UI_PlayerInfo TheInstance;
    static public UI_PlayerInfo Instance
    {
        get
        {
            return TheInstance;
        }
    }
    
    [SerializeField]
    Text mLocalPlayerHealthText;
    [SerializeField]
    Text mAcrossPlayerHealthText;
    [SerializeField]
    Text mLeftPlayerHealthText;
    [SerializeField]
    Text mRightPlayerHealthText;

    [SerializeField]
    Text mLocalPlayerTrapsText;
    [SerializeField]
    Text mAcrossPlayerTrapsText;
    [SerializeField]
    Text mLeftPlayerTrapsText;
    [SerializeField]
    Text mRightPlayerTrapsText;

    void Awake()
    {
        TheInstance = this;
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        
        mLocalPlayerHealthText.text = Player.MaxHealth.ToString();
        mAcrossPlayerHealthText.text = Player.MaxHealth.ToString();
        mLeftPlayerHealthText.text = Player.MaxHealth.ToString();
        mRightPlayerHealthText.text = Player.MaxHealth.ToString();

        mLocalPlayerTrapsText.text = "0";
        mAcrossPlayerTrapsText.text = "0";
        mLeftPlayerTrapsText.text = "0";
        mRightPlayerTrapsText.text = "0";

        Turns localPlayer = TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());

        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localPlayer);
        Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(localPlayer);
        Player rightOf = TurnManager.Instance.GetPlayerToTheRightOfWithNull(localPlayer);

        if(across == null)
        {
            Debug.Log("Across");
            mAcrossPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
        if (leftOf == null)
        {
            Debug.Log("Left");
            mLeftPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
        if (rightOf == null)
        {
            Debug.Log("Right");
            mRightPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ChangeHealthText()
    {
        Turns localPlayer = TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());

        mLocalPlayerHealthText.text = GlobalSettings.Instance.GetLocalPlayer().CurrentHealth.ToString();
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localPlayer);
        Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOf(localPlayer);
        Player rightOf = TurnManager.Instance.GetPlayerToTheRightOf(localPlayer);

        if (across != null)
        {
            mAcrossPlayerHealthText.text = across.CurrentHealth.ToString();
        }

        if (leftOf != null)
        {
            mLeftPlayerHealthText.text = leftOf.CurrentHealth.ToString();
        }

        if (rightOf != null)
        {
            mRightPlayerHealthText.text = rightOf.CurrentHealth.ToString();
        }
    }

    public void ChangeTrapsText()
    {
        Turns localPlayer = TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());

        mLocalPlayerTrapsText.text = GlobalSettings.Instance.GetLocalPlayer().CurrentTrapAmount.ToString();
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localPlayer);
        Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOf(localPlayer);
        Player rightOf = TurnManager.Instance.GetPlayerToTheRightOf(localPlayer);

        if (across != null)
        {
            mAcrossPlayerTrapsText.text = across.CurrentTrapAmount.ToString();
        }

        if (leftOf != null)
        {
            mLeftPlayerTrapsText.text = leftOf.CurrentTrapAmount.ToString();
        }

        if (rightOf != null)
        {
            mRightPlayerTrapsText.text = rightOf.CurrentTrapAmount.ToString();
        }
        //if (isLocalPlayer)
        //{
        //    mLocalPlayerTrapsText.text = value.ToString();
        //}
        //else
        //{
        //    mAcrossPlayerTrapsText.text = value.ToString();
        //}
    }
}

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
        mLocalPlayerTrapsText.text = "0";
        mAcrossPlayerTrapsText.text = "0";

        if(GlobalSettings.Instance.TypeOfGame != GameType.TwoPlayer)
        {
            mLeftPlayerHealthText.text = Player.MaxHealth.ToString();
            mLeftPlayerTrapsText.text = "0";
        }

        if (GlobalSettings.Instance.TypeOfGame == GameType.FourPlayer)
        {
            mRightPlayerHealthText.text = Player.MaxHealth.ToString();
            mRightPlayerTrapsText.text = "0";
        }
    }

    public void ChangeHealthText()
    {
        Turns localPlayer = TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());

        mLocalPlayerHealthText.text = GlobalSettings.Instance.GetLocalPlayer().CurrentHealth.ToString();
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localPlayer);
        mAcrossPlayerHealthText.text = across.CurrentHealth.ToString();

        if (GlobalSettings.Instance.TypeOfGame != GameType.TwoPlayer)
        {
            Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOf(localPlayer);
            mAcrossPlayerHealthText.text = leftOf.CurrentHealth.ToString();
        }

        if (GlobalSettings.Instance.TypeOfGame == GameType.FourPlayer)
        {
            Player rightOf = TurnManager.Instance.GetPlayerToTheRightOf(localPlayer);
            mAcrossPlayerHealthText.text = rightOf.CurrentHealth.ToString();
        }
    }

    public void ChangeTrapsText()
    {
        Turns localPlayer = TurnManager.Instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());

        mLocalPlayerTrapsText.text = GlobalSettings.Instance.GetLocalPlayer().CurrentTrapAmount.ToString();
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localPlayer);
        mAcrossPlayerTrapsText.text = across.CurrentTrapAmount.ToString();

        if (GlobalSettings.Instance.TypeOfGame != GameType.TwoPlayer)
        {
            Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOf(localPlayer);
            mAcrossPlayerTrapsText.text = leftOf.CurrentTrapAmount.ToString();
        }

        if (GlobalSettings.Instance.TypeOfGame == GameType.FourPlayer)
        {
            Player rightOf = TurnManager.Instance.GetPlayerToTheRightOf(localPlayer);
            mAcrossPlayerTrapsText.text = rightOf.CurrentTrapAmount.ToString();
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

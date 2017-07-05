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

    public ParticleSystem leftPart;
    public ParticleSystem topPart;
    public ParticleSystem rightPart;
    public ParticleSystem bottomPart;
    private Turns activeTurn = Turns.Null;

    public Animator WinLoss;

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
            mAcrossPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
        if (leftOf == null)
        {
            mLeftPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
        if (rightOf == null)
        {
            mRightPlayerHealthText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void TutorialUpdateHealthText()
    {
        if (GlobalSettings.Instance.TutorialScene)
        {
            Turns localPlayer = Turns.BottomPlayer;

            mLocalPlayerHealthText.text = GlobalSettings.Instance.BottomPlayer.CurrentHealth.ToString();
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

    private void Update()
    {
        ChangeParticles();
    }

    private void ChangeParticles()
    {
        Turns currentTurn = TurnManager.Instance.CurrentTurn;

        if (currentTurn == activeTurn)
        {
            return;
        }

        Player current = TurnManager.Instance.GetPlayerOfTurnEnum(currentTurn);

        Player local = GlobalSettings.Instance.GetLocalPlayer();
        Turns localTurn = TurnManager.Instance.GetTurnEnumOfPlayer(local);

        Player across = TurnManager.Instance.GetPlayerAcrossFrom(localTurn);
        Player leftOf = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(localTurn);
        Player rightOf = TurnManager.Instance.GetPlayerToTheRightOfWithNull(localTurn);

        leftPart.Stop();
        leftPart.Clear();

        topPart.Stop();
        topPart.Clear();

        rightPart.Stop();
        rightPart.Clear();

        bottomPart.Stop();
        bottomPart.Clear();

        activeTurn = currentTurn;

        if (current == local)
        {
            bottomPart.Play();
        }
        else if (current == across)
        {
            topPart.Play();
        }
        else if (current == leftOf)
        {
            leftPart.Play();
        }
        else if (current == rightOf)
        {
            rightPart.Play();
        }
    }
}

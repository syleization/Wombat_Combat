using UnityEngine;
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
    Text mCurrentPlayerHealthText;
    [SerializeField]
    Text mAcrossPlayerHealthText;
    [SerializeField]
    Text mCurrentPlayerTrapsText;
    [SerializeField]
    Text mAcrossPlayerTrapsText;


    void Awake()
    {
        TheInstance = this;
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        mCurrentPlayerHealthText.text = Player.MaxHealth.ToString();
        mAcrossPlayerHealthText.text = Player.MaxHealth.ToString();
        mCurrentPlayerTrapsText.text = "0";
        mAcrossPlayerTrapsText.text = "0";
    }

    public void ChangeHealthText(bool isLocalPlayer, float value)
    {
        if(isLocalPlayer)
        {
            mCurrentPlayerHealthText.text = value.ToString();
        }
        else
        {
            mAcrossPlayerHealthText.text = value.ToString();
        }
    }

    public void ChangeTrapsText(bool isLocalPlayer, float value)
    {
        if (isLocalPlayer)
        {
            mCurrentPlayerTrapsText.text = value.ToString();
        }
        else
        {
            mAcrossPlayerTrapsText.text = value.ToString();
        }
    }
}

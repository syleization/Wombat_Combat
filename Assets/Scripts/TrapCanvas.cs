﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrapCanvas : MonoBehaviour
{
    [SerializeField]
    TrapZone Traps;
    [SerializeField]
    Text AmountOfTraps;

    public void Initialize()
    {
        Traps = Instantiate(Traps);
        Traps.Initialize();
        AmountOfTraps.text = "0";
    }

    public void Terminate()
    {
        Traps.Terminate();
    }

    // Button OnClick function for toggling the cards
    public void ToggleActive()
    {
        if (TurnManager.Instance.currentStage == Stage.Play || TurnManager.Instance.currentStage == Stage.Reaction)
        {
            Traps.ToggleActive();
        }
    }

    public void UpdateTrapAmount()
    {
        AmountOfTraps.text = Traps.GetTrapCardCount().ToString();
        Player localPlayer = GlobalSettings.Instance.GetLocalPlayer();

        if(localPlayer.isServer)
        {
            localPlayer.RpcChangeTrapAmount(Traps.GetTrapCardCount(), TurnManager.Instance.GetTurnEnumOfPlayer(localPlayer));
        }
        else
        {
            localPlayer.CmdChangeTrapAmount(Traps.GetTrapCardCount(), TurnManager.Instance.GetTurnEnumOfPlayer(localPlayer));
        }
    }

    public int GetTrapCardCount()
    {
        return Traps.GetTrapCardCount();
    }

}

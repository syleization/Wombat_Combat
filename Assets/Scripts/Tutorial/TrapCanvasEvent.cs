using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TrapCanvasEvent : TextEvent
    {
        TrapCanvas trapCanvas;
        Card card;

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            CanvasManager.Instance.Initialize();
            trapCanvas = FindObjectOfType<TrapCanvas>();
            GameObject trampoline = Instantiate(GlobalSettings.Instance.Trap_Trampoline).gameObject;
            card = trampoline.GetComponent<Card>();
            card.owner = GlobalSettings.Instance.BottomPlayer;
            trapCanvas.Traps.SetTrap(card);
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            trapCanvas.Traps.RemoveTrap(card);
        }
    }

}


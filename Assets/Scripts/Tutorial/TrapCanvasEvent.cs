using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TrapCanvasEvent : TextEvent
    {
        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            CanvasManager.Instance.Initialize();
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
        }
    }

}


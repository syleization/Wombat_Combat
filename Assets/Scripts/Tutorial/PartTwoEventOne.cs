using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class PartTwoEventOne : TextEvent
    {
        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            UI_PlayerInfo.Instance.topPart.transform.parent.gameObject.SetActive(true);
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
        }
    }

}

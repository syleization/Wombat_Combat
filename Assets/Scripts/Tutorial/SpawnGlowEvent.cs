﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class SpawnGlowEvent : TextEvent
    {

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            UI_PlayerInfo.Instance.bottomPart.Play();
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            UI_PlayerInfo.Instance.bottomPart.Stop();
            UI_PlayerInfo.Instance.bottomPart.Clear();
        }
    }
}
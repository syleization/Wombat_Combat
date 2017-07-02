using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class SpawnGlowEvent : TextEvent
    {
        //Timer endTimer = new Timer();

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            UI_PlayerInfo.Instance.bottomPart.Play();
            //endTimer.Initialize(2.0f);
            
        }

        public override void Tick()
        {
            base.Tick();
            //endTimer.TimerAction(Exit);
        }

        //void Exit()
        //{
        //    TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
        //    tutorialManager.NextButton();
        //}

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            UI_PlayerInfo.Instance.bottomPart.Stop();
            UI_PlayerInfo.Instance.bottomPart.Clear();
        }
    }
}
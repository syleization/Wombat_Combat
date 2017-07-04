using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class MergeEvent : TextEvent
    {
        public CardSubType cardToMerge;
        private TutorialTools tools;
        private Timer endTimer = new Timer();

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            
            tools = TutorialTools.Instance;

            tools.GiveCard(GlobalSettings.Instance.GetCardOfSubType(cardToMerge));
            tools.GiveCard(GlobalSettings.Instance.GetCardOfSubType(cardToMerge));
            tools.field.ToggleTwoSquares(true);
        }

        override public void Tick()
        {
            base.Tick();

            if (tools.field.IsMergable() && tools.mergeFlag == 0)
            {
                tools.mergeBtn.SetActive(true);
            }
            else
            {
                tools.mergeBtn.SetActive(false);
            }

            if (tools.mergeFlag == 2)
            {
                tools.mergeFlag = 3;
                endTimer.Initialize(2.0f);
            }
            else if (tools.mergeFlag == 3)
            {
                endTimer.TimerAction(Exit);
            }
        }

        private void Exit()
        {
            tools.ClearHand();
            tools.mergeFlag = 0;
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.NextButton();
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
        }
    }
}

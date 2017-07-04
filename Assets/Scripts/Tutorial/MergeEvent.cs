using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class MergeEvent : TextEvent
    {
        private TutorialTools tools;

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            
            tools = TutorialTools.Instance;

            tools.GiveCard(GlobalSettings.Instance.Attack_DonkeyKick);
            tools.GiveCard(GlobalSettings.Instance.Attack_DonkeyKick);
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
                Exit();
                tools.mergeFlag = 3;
            }
        }

        private void Exit()
        {
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.NextButton();
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
        }
    }
}

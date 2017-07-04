using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class PlayEffectEvent : TextEvent
    {
        [Tooltip("Current Functionality: Bark, DonkeyKick, Trampoline")]
        [SerializeField]
        CardSubType TheCardSubType;

        Timer endTimer = new Timer();

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            switch(TheCardSubType)
            {
                case CardSubType.DonkeyKick:
                    GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
                    TurnManager instance = TurnManager.Instance;
                    Player attacker = GlobalSettings.Instance.BottomPlayer;
                    Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
                    Effects.Attack(card, defender, attacker);
                    break;
                case CardSubType.Bark:
                    Player atk = GlobalSettings.Instance.TopPlayer;
                    Player def = GlobalSettings.Instance.BottomPlayer;
                    Effects.Bark(def, atk);
                    break;
                case CardSubType.Trampoline:
                    Player oldT = GlobalSettings.Instance.BottomPlayer;
                    Player newT = GlobalSettings.Instance.TopPlayer;
                    Effects.Tramp(oldT, newT);
                    break;
            }
            endTimer.Initialize(CardActions.kEffectTime);

        }

        public override void Tick()
        {
            base.Tick();
            endTimer.TimerAction(Exit);
        }

        void Exit()
        {
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.NextButton();
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            if(TheCardSubType == CardSubType.DonkeyKick)
            {
                Effects.AttackEnd();
            }
        }
    }
}

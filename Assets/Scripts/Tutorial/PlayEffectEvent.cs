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
                case CardSubType.WombatCharge:
                    GameObject carde = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
                    carde.transform.position = Vector3.zero;
                    Player atke = GlobalSettings.Instance.BottomPlayer;
                    Player defe = GlobalSettings.Instance.TopPlayer;
                    Effects.Charge(carde, defe, atke);
                    break;
                case CardSubType.Bark:
                    Player atk = GlobalSettings.Instance.TopPlayer;
                    Player def = GlobalSettings.Instance.BottomPlayer;
                    Effects.Bark(def, atk);
                    break;
                case CardSubType.Bite:
                    Effects.Bite();
                    break;
                case CardSubType.GooglyEyes:
                    Player dir = GlobalSettings.Instance.TopPlayer;
                    Effects.GooglyEyes(dir);
                    break;
                case CardSubType.Trampoline:
                    Player oldT = GlobalSettings.Instance.BottomPlayer;
                    Player newT = GlobalSettings.Instance.TopPlayer;
                    Effects.Tramp(oldT, newT);
                    break;
                case CardSubType.WombatCage:
                    Player owner = GlobalSettings.Instance.BottomPlayer;
                    Effects.Cage(owner);
                    break;
            }
            endTimer.Initialize(CardActions.kEffectTime);
            GlobalSettings.Instance.TutorialHack = false;
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
            if(TheCardSubType == CardSubType.DonkeyKick || TheCardSubType == CardSubType.WombatCharge || TheCardSubType == CardSubType.Trampoline || TheCardSubType == CardSubType.GooglyEyes)
            {
                Effects.AttackEnd();
            }
            GlobalSettings.Instance.TutorialHack = true;
        }
    }
}

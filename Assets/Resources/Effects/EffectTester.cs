using UnityEngine;
using System.Collections;

public class EffectTester : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.TopPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.RightPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.LeftPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WomboCombo).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.WomboCombo(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WomboCombo).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.TopPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.WomboCombo(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WomboCombo).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.RightPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.WomboCombo(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WomboCombo).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.LeftPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.WomboCombo(card, defender, attacker);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(card, defender, attacker);
        }
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Effects.SinkholeOn();
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    Effects.SinkholeOff();
        //}
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            Sinkhole hole = FindObjectOfType<Sinkhole>();
            hole.EatCard(card);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Effects.Bite(card);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Effects.Bite(card);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player owner = GlobalSettings.Instance.TopPlayer;
            Effects.Cage(card, owner);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player owner = GlobalSettings.Instance.BottomPlayer;
            Effects.Cage(card, owner);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player owner = GlobalSettings.Instance.RightPlayer;
            Effects.Cage(card, owner);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player owner = GlobalSettings.Instance.LeftPlayer;
            Effects.Cage(card, owner);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player dir = GlobalSettings.Instance.TopPlayer;
            Effects.GooglyEyes(card, dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player dir = GlobalSettings.Instance.BottomPlayer;
            Effects.GooglyEyes(card, dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player dir = GlobalSettings.Instance.RightPlayer;
            Effects.GooglyEyes(card, dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player dir = GlobalSettings.Instance.LeftPlayer;
            Effects.GooglyEyes(card, dir);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.LeftPlayer;
            Player def = GlobalSettings.Instance.RightPlayer;
            Effects.Bark(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.RightPlayer;
            Player def = GlobalSettings.Instance.LeftPlayer;
            Effects.Bark(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.BottomPlayer;
            Player def = GlobalSettings.Instance.TopPlayer;
            Effects.Bark(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.TopPlayer;
            Player def = GlobalSettings.Instance.BottomPlayer;
            Effects.Bark(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player oldT = GlobalSettings.Instance.BottomPlayer;
            Player newT = GlobalSettings.Instance.TopPlayer;
            Effects.Tramp(card, oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player oldT = GlobalSettings.Instance.TopPlayer;
            Player newT = GlobalSettings.Instance.BottomPlayer;
            Effects.Tramp(card, oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player oldT = GlobalSettings.Instance.RightPlayer;
            Player newT = GlobalSettings.Instance.LeftPlayer;
            Effects.Tramp(card, oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
            Player oldT = GlobalSettings.Instance.LeftPlayer;
            Player newT = GlobalSettings.Instance.RightPlayer;
            Effects.Tramp(card, oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.LeftPlayer;
            Player def = GlobalSettings.Instance.RightPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.RightPlayer;
            Player def = GlobalSettings.Instance.LeftPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.BottomPlayer;
            Player def = GlobalSettings.Instance.TopPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.TopPlayer;
            Player def = GlobalSettings.Instance.BottomPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.LeftPlayer;
            Player def = GlobalSettings.Instance.BottomPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.RightPlayer;
            Player def = GlobalSettings.Instance.TopPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.BottomPlayer;
            Player def = GlobalSettings.Instance.RightPlayer;
            Effects.Charge(card, def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            GameObject card = Instantiate(GlobalSettings.Instance.Attack_WombatCharge).gameObject;
            card.transform.position = Vector3.zero;
            Player atk = GlobalSettings.Instance.TopPlayer;
            Player def = GlobalSettings.Instance.LeftPlayer;
            Effects.Charge(card, def, atk);
        }
    }
}

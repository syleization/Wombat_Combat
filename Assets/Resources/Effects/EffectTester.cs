using UnityEngine;
using System.Collections;

public class EffectTester : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Pause.Instance.Lock();
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (FindObjectOfType<Sinkhole>() == null)
            {
                Instantiate(Resources.Load("Effects/Sinkhole"));
            }
            else
            {
                Destroy(FindObjectOfType<Sinkhole>().gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            string curr = SoundManager.Instance.CurrentSongName();
            if (curr == "Music - Battle")
            {
                SoundManager.Instance.PlaySound("Music - Main");
            }
            else
            {
                SoundManager.Instance.PlaySound("Music - Battle");
            }
        }
            if (Input.GetKeyDown(KeyCode.N))
        {
            Sinkhole hole = FindObjectOfType<Sinkhole>();
            hole.EatCard(Vector3.zero, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Effects.Bite();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Effects.Bite();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Player owner = GlobalSettings.Instance.TopPlayer;
            Effects.Cage(owner);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Player owner = GlobalSettings.Instance.BottomPlayer;
            Effects.Cage(owner);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Player owner = GlobalSettings.Instance.RightPlayer;
            Effects.Cage(owner);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Player owner = GlobalSettings.Instance.LeftPlayer;
            Effects.Cage(owner);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Player dir = GlobalSettings.Instance.TopPlayer;
            Effects.GooglyEyes(dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Player dir = GlobalSettings.Instance.BottomPlayer;
            Effects.GooglyEyes(dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Player dir = GlobalSettings.Instance.RightPlayer;
            Effects.GooglyEyes(dir);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Player dir = GlobalSettings.Instance.LeftPlayer;
            Effects.GooglyEyes(dir);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Player atk = GlobalSettings.Instance.LeftPlayer;
            Player def = GlobalSettings.Instance.RightPlayer;
            Effects.Bark(def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Player atk = GlobalSettings.Instance.RightPlayer;
            Player def = GlobalSettings.Instance.LeftPlayer;
            Effects.Bark(def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Player atk = GlobalSettings.Instance.BottomPlayer;
            Player def = GlobalSettings.Instance.TopPlayer;
            Effects.Bark(def, atk);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Player atk = GlobalSettings.Instance.TopPlayer;
            Player def = GlobalSettings.Instance.BottomPlayer;
            Effects.Bark(def, atk);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Player oldT = GlobalSettings.Instance.BottomPlayer;
            Player newT = GlobalSettings.Instance.TopPlayer;
            Effects.Tramp(oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Player oldT = GlobalSettings.Instance.TopPlayer;
            Player newT = GlobalSettings.Instance.BottomPlayer;
            Effects.Tramp(oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Player oldT = GlobalSettings.Instance.RightPlayer;
            Player newT = GlobalSettings.Instance.LeftPlayer;
            Effects.Tramp(oldT, newT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Player oldT = GlobalSettings.Instance.LeftPlayer;
            Player newT = GlobalSettings.Instance.RightPlayer;
            Effects.Tramp(oldT, newT);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Effects.AttackEnd();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Effects.Merge(CardSubType.DonkeyKick, CardSubType.WombatCharge, Turns.BottomPlayer);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Effects.Merge(CardSubType.Bark, CardSubType.Bite, Turns.BottomPlayer);
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Effects.Merge(CardSubType.Trampoline, CardSubType.Sinkhole, Turns.BottomPlayer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Effects.DamageEffect(Turns.BottomPlayer, "NERB");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Effects.DamageEffect(Turns.TopPlayer, "is");
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Effects.DamageEffect(Turns.LeftPlayer, "Nathan");
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Effects.DamageEffect(Turns.RightPlayer, "a");
        }
    }
}

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
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.TopPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.LeftPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.RightPlayer;
            Player defender = instance.GetPlayerAcrossFrom(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerToTheLeftOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.TopPlayer;
            Player defender = instance.GetPlayerToTheLeftOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.LeftPlayer;
            Player defender = instance.GetPlayerToTheLeftOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.RightPlayer;
            Player defender = instance.GetPlayerToTheLeftOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.BottomPlayer;
            Player defender = instance.GetPlayerToTheRightOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.TopPlayer;
            Player defender = instance.GetPlayerToTheRightOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.LeftPlayer;
            Player defender = instance.GetPlayerToTheRightOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            TurnManager instance = TurnManager.Instance;
            Player attacker = GlobalSettings.Instance.RightPlayer;
            Player defender = instance.GetPlayerToTheRightOf(instance.GetTurnEnumOfPlayer(attacker));
            Effects.Attack(attacker, defender);
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
    }
}

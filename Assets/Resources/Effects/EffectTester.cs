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
    }
}

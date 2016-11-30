using UnityEngine;
using System.Collections;

public class CardActions : MonoBehaviour
{
    // Not a singleton, but no instance of this class should ever be constructed
    // It is basically a hub for static functions
    private CardActions() { }

    public static void DonkeyKick()
    {
        Player leftOfCurrentPlayer = TurnManager.Instance.GetPlayerToTheLeftOf(TurnManager.Instance.currentTurn);
        Player rightOfCurrentPlayer = TurnManager.Instance.GetPlayerToTheRightOf(TurnManager.Instance.currentTurn);

        if (Random.Range(0, 2) == 0)
        {
            leftOfCurrentPlayer.CurrentHealth -= 1;
            Debug.Log("Did damage to the left side of " + TurnManager.Instance.currentTurn.ToString());
        }
        else
        {
            rightOfCurrentPlayer.CurrentHealth -= 1;
            Debug.Log("Did damage to the right side of " + TurnManager.Instance.currentTurn.ToString());
        }
    }

    public static void WombatCharge()
    {

    }

    public static void WomboCombo()
    {

    }

    public static void Bark()
    {

    }

    public static void Bite()
    {

    }

    public static void GooglyEyes()
    {

    }

    public static void Trampoline()
    {

    }

    public static void Sinkhole()
    {

    }

    public static void WombatCage()
    {

    }
}

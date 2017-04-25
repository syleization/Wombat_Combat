using UnityEngine;
using System.Collections;

public class Effects : MonoBehaviour
{
    public static Attack activeAttack;

    public static Attack TheActiveAttack
    {
        set
        {
            activeAttack = value;
        }
        get
        {
            return activeAttack;
        }
    }

    private Effects() { }

    public static void Attack(GameObject card, Player defender, Player attacker)
    {
        GameObject attackAnimation = Instantiate(Resources.Load("Effects/RedAttack")) as GameObject;

        attackAnimation.GetComponent<RedAttack>().Initialize(card, PointBehind(defender), PointBehind(attacker));

        if (TheActiveAttack != null)
        {
            Destroy(TheActiveAttack.target);
            TheActiveAttack.Terminate();
        }

        TheActiveAttack = attackAnimation.GetComponent<Attack>();
    }

    public static float FaceDirection(Player target)
    {
        GlobalSettings instance = GlobalSettings.Instance;
        if (instance.TopPlayer == target)
            return 0;
        else if (instance.RightPlayer == target)
            return 90;
        else if (instance.BottomPlayer == target)
            return 180;
        else if (instance.LeftPlayer == target)
            return -90;
        return 0;
    }

    public static float FaceAwayFromDirection(Player target)
    {
        GlobalSettings instance = GlobalSettings.Instance;
        if (instance.TopPlayer == target)
            return 180;
        else if (instance.RightPlayer == target)
            return -90;
        else if (instance.BottomPlayer == target)
            return 0;
        else if (instance.LeftPlayer == target)
            return 90;
        return 0;
    }

    public static Vector3 PointBehind(Player target)
    {
        // Alex's Testing
        if (GameObject.Find("EffectTester"))
        {
            Camera cam = Camera.main;
            GlobalSettings instance = GlobalSettings.Instance;
            Vector3 result = new Vector3(0, 0, 0);

            if (instance.TopPlayer == target)
                result = cam.ViewportToWorldPoint(new Vector3(0.5f, 1, cam.nearClipPlane));
            else if (instance.RightPlayer == target)
                result = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.nearClipPlane));
            else if (instance.BottomPlayer == target)
                result = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.nearClipPlane));
            else if (instance.LeftPlayer == target)
                result = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.nearClipPlane));
            result.z = -1;
            return result;
        }
        else
        {
            Camera cam = Camera.main;
            TurnManager instance = TurnManager.Instance;
            Vector3 result = new Vector3(0, 0, 0);

            Turns localPlayer = instance.GetTurnEnumOfPlayer(GlobalSettings.Instance.GetLocalPlayer());
            // Top Player on the screen
            if (instance.GetPlayerAcrossFrom(localPlayer) == target)
                result = cam.ViewportToWorldPoint(new Vector3(0.5f, 1, cam.nearClipPlane));
            // Right Player on the screen
            else if (instance.GetPlayerToTheRightOfWithNull(localPlayer) == target)
                result = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.nearClipPlane));
            // Bottom Player on the screen
            else if (instance.GetPlayerOfTurnEnum(localPlayer) == target)
                result = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.nearClipPlane));
            // Left Player on the screen
            else if (instance.GetPlayerToTheLeftOfWithNull(localPlayer) == target)
                result = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.nearClipPlane));
            result.z = -1;
            return result;
        }
    }

    public static void SinkholeOn(Player player)
    {
        GameObject hole = Resources.Load("Effects/Sinkhole") as GameObject;
        hole = Instantiate(hole);

        Player local = GlobalSettings.Instance.GetLocalPlayer();
        Player across = TurnManager.Instance.GetPlayerAcrossFrom(TurnManager.Instance.GetTurnEnumOfPlayer(local));
        Player left = TurnManager.Instance.GetPlayerToTheLeftOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(local));
        Player right = TurnManager.Instance.GetPlayerToTheRightOfWithNull(TurnManager.Instance.GetTurnEnumOfPlayer(local));

        player.PlayersSinkhole = hole.GetComponent<Sinkhole>();

        if (player == local)
        {
            hole.transform.position = new Vector3(0.0f, -2.0f, 0.0f);
            hole.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }
        else if (player == across)
        {
            hole.transform.position = new Vector3(0.0f, 2.0f, 0.0f);
            hole.transform.rotation = new Quaternion(0.0f, 0.0f, 180.0f, 0.0f);
        }
        else if (player == left)
        {
            hole.transform.position = new Vector3(-6.0f, 0.0f, 0.0f);
            hole.transform.rotation = new Quaternion(0.0f, 0.0f, -90.0f, 0.0f);
        }
        else if (player == right)
        {
            hole.transform.position = new Vector3(6.0f, 0.0f, 0.0f);
            hole.transform.rotation = new Quaternion(0.0f, 0.0f, 90.0f, 0.0f);
        }
    }

    public static void SinkholeOff(Sinkhole sinkhole)
    {
        if (sinkhole != null)
            Destroy(sinkhole.gameObject);
    }

    // Send in already instantiated card
    public static void Bite()
    {
        GameObject biteAnimation = Instantiate(Resources.Load("Effects/Bite")) as GameObject;

        GameObject card;
        if (TheActiveAttack != null)
        {
            card = TheActiveAttack.target;
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
        }

        biteAnimation.GetComponent<Bite>().Initialize(card);
    }

    public static void Cage(Player owner)
    {
        GameObject cageAnimation = Instantiate(Resources.Load("Effects/Cage")) as GameObject;

        GameObject card;
        if (TheActiveAttack != null)
        {
            card = TheActiveAttack.target;
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
        }

        cageAnimation.GetComponent<Cage>().Initialize(card, PointBehind(owner));
    }

    public static void GooglyEyes(Player toAttack)
    {
        GameObject eyesAnimation = Instantiate(Resources.Load("Effects/Eyes")) as GameObject;

        GameObject card;
        if (TheActiveAttack != null)
        {
            card = TheActiveAttack.target;
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
        }

        eyesAnimation.GetComponent<GooglyEyes>().Initialize(card, PointBehind(toAttack));
    }

    public static void Bark(Player defender, Player attacker)
    {
        GameObject barkAnimation = Instantiate(Resources.Load("Effects/Bark")) as GameObject;

        GameObject card;
        if (TheActiveAttack != null)
        {
            card = TheActiveAttack.target;
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
        }

        barkAnimation.GetComponent<Bark>().Initialize(card, PointBehind(defender), PointBehind(attacker));
    }

    public static void Tramp(Player oldTarget, Player newTarget)
    {
        GameObject trampAnimation = Instantiate(Resources.Load("Effects/Tramp")) as GameObject;

        GameObject card;
        if (TheActiveAttack != null)
        {
            card = TheActiveAttack.target;
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card.transform.position = Vector3.zero;
        }

        trampAnimation.GetComponent<Tramp>().Initialize(card, PointBehind(oldTarget), PointBehind(newTarget));
    }

    public static void Charge(GameObject card, Player defender, Player attacker)
    {
        GameObject chargeAnimation = Instantiate(Resources.Load("Effects/WombatCharge")) as GameObject;

        chargeAnimation.GetComponent<Charge>().Initialize(card, PointBehind(attacker), PointBehind(defender));

        if (TheActiveAttack != null)
        {
            Destroy(TheActiveAttack.target);
            TheActiveAttack.Terminate();
        }

        TheActiveAttack = chargeAnimation.GetComponent<Attack>();
    }

    public static void WomboCombo(GameObject card, Player defender, Player attacker)
    {
        GameObject chargeAnimation = Instantiate(Resources.Load("Effects/WomboCombo")) as GameObject;

        chargeAnimation.GetComponent<Charge>().Initialize(card, PointBehind(attacker), PointBehind(defender));

        if (TheActiveAttack != null)
        {
            Destroy(TheActiveAttack.target);
            TheActiveAttack.Terminate();
        }

        TheActiveAttack = chargeAnimation.GetComponent<Attack>();
    }

    public static void AttackEnd()
    {
        if (TheActiveAttack != null)
        {
            GameObject endAnimation = Instantiate(Resources.Load("Effects/EndAttack")) as GameObject;
            endAnimation.GetComponent<AtkEnd>().Initialize(TheActiveAttack);
            TheActiveAttack.Terminate();
            TheActiveAttack = null;
        }
    }

    public static void Merge(CardSubType originalCards, CardSubType finalCard, Turns targetPlayer)
    {
        GameObject mergeAnimation = Instantiate(Resources.Load("Effects/MergeObj")) as GameObject;
        mergeAnimation.GetComponent<Merge>().Initialize(originalCards, finalCard, targetPlayer);
    }
}
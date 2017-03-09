using UnityEngine;
using System.Collections;

public class Effects : MonoBehaviour
{

	public static void Attack(Player Sender, Player Reciever)
    {
        //Vector3 start = PointBehind(Sender);
        //Quaternion rot = FaceAwayFromDirection(Sender);
        GameObject temp = Instantiate(Resources.Load("Effects/RedAttack")) as GameObject; //, start, rot
        RedAttack attack = temp.GetComponent<RedAttack>();
        attack.Activate = false;
        attack.transform.Rotate(new Vector3(0, 0, FaceAwayFromDirection(Sender)));
        //attack.transform.Rotate(Vector3.forward, FaceAwayFromDirection(Sender));
        attack.transform.position = PointBehind(Sender);
        attack.TargetDirection = FaceDirection(Reciever);
        attack.Activate = true;
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

    //public static Quaternion FaceDirection(Player target)
    //{
    //    GlobalSettings instance = GlobalSettings.Instance;
    //    if (instance.TopPlayer == target)
    //        return new Quaternion(0, 0, 0, 0);
    //    else if (instance.RightPlayer == target)
    //        return new Quaternion(0, 0, .25f, 0);
    //    else if (instance.BottomPlayer == target)
    //        return new Quaternion(0, 0, .5f, 0);
    //    else if (instance.LeftPlayer == target)
    //        return new Quaternion(0, 0, .75f, 0);
    //    return new Quaternion(0, 0, 0, 0);
    //}

    //public static Quaternion FaceAwayFromDirection(Player target)
    //{
    //    GlobalSettings instance = GlobalSettings.Instance;
    //    if (instance.TopPlayer == target)
    //        return new Quaternion(0, 0, .5f, 0);
    //    else if (instance.RightPlayer == target)
    //        return new Quaternion(0, 0, .75f, 0);
    //    else if (instance.BottomPlayer == target)
    //        return new Quaternion(0, 0, 0, 0);
    //    else if (instance.LeftPlayer == target)
    //        return new Quaternion(0, 0, .25f, 0);
    //    return new Quaternion(0, 0, 0, 0);
    //}

    public static Vector3 PointBehind(Player target)
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
        else if(player == across)
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
}

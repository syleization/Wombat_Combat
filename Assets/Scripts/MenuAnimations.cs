using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public GameObject Stuff;
    public Animator otherAnimator;

    public void PlayButton()
    {
        otherAnimator.SetBool("IsPlay", true);
    }

    public void BackButton()
    {
        if (otherAnimator.GetBool("IsSettings"))
        {
            otherAnimator.SetBool("IsSettings", false);
        }
        else if (otherAnimator.GetBool("IsTwoPlayers"))
        {
            otherAnimator.SetBool("IsTwoPlayers", false);
        }
        else if (otherAnimator.GetBool("IsThreePlayers"))
        {
            otherAnimator.SetBool("IsThreePlayers", false);
        }
        else if (otherAnimator.GetBool("IsFourPlayers"))
        {
            otherAnimator.SetBool("IsFourPlayers", false);
        }
        else
        {
            otherAnimator.SetBool("IsPlay", false);
        }
    }

    public void SettingsButton()
    {
        otherAnimator.SetBool("IsSettings", true);
    }

    public void TwoPlayers()
    {
        otherAnimator.SetBool("IsTwoPlayers", true);
    }

    public void ThreePlayers()
    {
        otherAnimator.SetBool("IsThreePlayers", true);
    }

    public void FourPlayers()
    {
        otherAnimator.SetBool("IsFourPlayers", true);
    }
}

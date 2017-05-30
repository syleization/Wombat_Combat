using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    public GameObject Stuff;
    Animator otherAnimator;

    public void PlayButton()
    {
        otherAnimator.SetBool("IsPlay", true);
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

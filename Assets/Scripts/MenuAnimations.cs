using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuAnimations : MonoBehaviour
{
    public GameObject Stuff;
    public Animator otherAnimator;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectSlider;

    public GameObject PopUp;

    public void PlayButton()
    {
        otherAnimator.SetBool("IsPlay", true);
    }

    public void BackButton()
    {
        if (otherAnimator.GetBool("IsSettings"))
        {
            otherAnimator.SetBool("IsSettings", false);

            string saver = "";
            saver += SoundManager.Instance.MasterVolume;
            saver += ":";
            saver += SoundManager.Instance.MusicVolume;
            saver += ":";
            saver += SoundManager.Instance.EffectVolume;

            SaveLoad.WriteStringToFile(saver, SaveLoad.FolderName + "/" + SaveLoad.Filename);
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

        MasterSlider.value = SoundManager.Instance.MasterVolume;
        MusicSlider.value = SoundManager.Instance.MusicVolume;
        EffectSlider.value = SoundManager.Instance.EffectVolume;
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

    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}

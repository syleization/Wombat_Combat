using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStartUp : MonoBehaviour
{
    public string startMusic;

    private void Start()
    {
        SoundManager.Instance.PlaySound(startMusic);
    }
}

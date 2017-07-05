using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControler : MonoBehaviour
{
    public string Track;

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(Track);
        }
    }
}

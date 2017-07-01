using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControler : MonoBehaviour
{
    public string Track;

    private void OnEnable()
    {
        SoundManager.Instance.PlaySound(Track);
    }
}

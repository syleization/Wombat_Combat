using UnityEngine;
using System.Collections;

using System;
public class Timer
{
    [SerializeField]
    float OriginalTime;
    [SerializeField]
    float TimeLeft;

    public void Initialize(float timeSet)
    {
        OriginalTime = timeSet;
        TimeLeft = OriginalTime;
    }

    public void TimerAction(Action function)
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0)
        {
            function();
            TimeLeft = OriginalTime;
        }
    }
}
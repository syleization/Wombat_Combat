using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pause : NetworkBehaviour
{
    private Pause() { }
    private static Pause TheInstance;
    public static Pause Instance
    {
        get
        {
            if (TheInstance == null)
            {
                TheInstance = FindObjectOfType<Pause>();
            }

            return TheInstance;
        }
    }

    Timer WaitTimer = new Timer();
    bool IsDoneWaiting = true;
    public bool IsPaused
    {
        get
        {
            return !IsDoneWaiting;
        }
    }
    // Use this for initialization
    void Awake()
    {
        TheInstance = this;
    }

    public void StartPauseTimer(float waitTime)
    {
        IsDoneWaiting = false;
        WaitTimer.Initialize(waitTime);
    }

    void Update()
    {
        if(IsDoneWaiting == false)
        {
            WaitTimer.TimerAction(DoneWaiting);
        }
    }

    void DoneWaiting()
    {
        IsDoneWaiting = true;
    }

    // Client Rpcs
    [ClientRpc]
    public void RpcPauseGame(float waitTime)
    {
        StartPauseTimer(waitTime);
    }
    public void Lock()
    {
        IsDoneWaiting = false;
        enabled = false;
    }

}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    TrapCanvas Trap;

    private CanvasManager() { }
    private static CanvasManager TheInstance;
    public static CanvasManager Instance
    {
        get
        {
            return TheInstance;
        }
    }

    void Awake()
    {
        TheInstance = this;
    }

    public void Initialize()
    {
        Trap = Instantiate(Trap);
        Trap.Initialize();
    }

    public void Terminate()
    {
        Trap.Terminate();
        Destroy(Trap.gameObject);
    }

    public void UpdateCanvas(string canvas)
    {
        if(canvas == "Trap")
        {
            Trap.UpdateTrapAmount();
        }
    }
}

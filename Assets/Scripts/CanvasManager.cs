using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    TrapCanvas Trap;
    [SerializeField]
    CardTapCanvas CardTap;

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
        CardTap = Instantiate(CardTap);
        CardTap.Initialize();
        UI_PlayerInfo.Instance.Initialize();
    }

    public void Terminate()
    {
        Trap.Terminate();
        //Destroy(Trap.gameObject);
        //Destroy(CardTap.gameObject);
    }

    // Send null into second parameter if you arent using the cardtap canvas
    public void UpdateCanvas(string canvas, Sprite cardTapCanvasSprite = null)
    {
        if(canvas == "Trap")
        {
            Trap.UpdateTrapAmount();
        }
        else if(canvas == "CardTap")
        {
            CardTap.UpdateCanvasImage(cardTapCanvasSprite);
        }
    }

    public bool IsCardTapCanvasImageNull()
    {
        return CardTap.IsImageNull();
    }
}

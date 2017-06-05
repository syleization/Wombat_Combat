using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    //private SpriteRenderer myRenderer;
    private Animator myAnimator;

    void Start()
    {
        //myRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayDamageEffect(string text, Turns target)
    {
        

        switch (target)
        {
            case Turns.LeftPlayer:
                myAnimator.SetTrigger("Left");
                break;
            case Turns.TopPlayer:
                myAnimator.SetTrigger("Top");
                break;
            case Turns.RightPlayer:
                myAnimator.SetTrigger("Right");
                break;
            case Turns.BottomPlayer:
                myAnimator.SetTrigger("Bottom");
                break;
            case Turns.Null:
                break;
            default:
                break;
        }
    }
}

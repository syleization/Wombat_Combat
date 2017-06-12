using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public float LingerTime = 2.0f;

    public Animator myAnimator;
    public Text myText;

    void Start()
    {
        //myAnimator = GetComponent<Animator>();
        //myText = GetComponentInChildren<Text>();

        enabled = false;
    }

    public void Initialize(string text, Turns target, Vector3 location)
    {
        myText.text = text;

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

        transform.position = location;

        enabled = true;
    }

    private void KillMe()
    {
        Destroy(gameObject);
    }
}

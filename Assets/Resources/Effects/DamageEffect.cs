using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Animator myAnimator;
    public Text myText;

    void Start()
    {
        //myAnimator = GetComponent<Animator>();
        //myText = GetComponentInChildren<Text>();

        enabled = false;
    }

    public void Initialize(string text, int target)
    {
        myText.text = text;

        switch (target)
        {
            case 1:
                myAnimator.SetTrigger("Bottom");
                break;
            case 2:
                myAnimator.SetTrigger("Top");
                break;
            case 3:
                myAnimator.SetTrigger("Left");
                break;
            case 4:
                myAnimator.SetTrigger("Right");
                break;
            default:
                myAnimator.SetTrigger("Bottom");
                break;
        }

        enabled = true;
    }

    private void KillMe()
    {
        Destroy(gameObject);
    }
}

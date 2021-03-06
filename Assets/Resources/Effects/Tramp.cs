﻿using UnityEngine;
using System.Collections;

public class Tramp : Attack {
    
    public float timer = 3;
    public Vector3 bouncePos;
    public RuntimeAnimatorController cardAnim;
    private bool flag = false;
    private bool centered = false;
    private Vector3 rot = new Vector3(0, 0, 10);
    private Vector3 offset = new Vector3(0, 0, -1);

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 bounce, Vector3 hit)
    {
        target = card;
        endPos = hit * 0.5f;
        bouncePos = bounce;

        target.transform.parent = gameObject.transform;

        enabled = true;

        SoundManager.Instance.PlaySound("Trap - Trampoline");
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;

        if (!centered)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, offset, 0.5f);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, offset, 0.5f);
            if (target.transform.position == offset)
                centered = true;
        }
        else if(timer > 2.5f)
        {
            timer -= Time.deltaTime;

            if (!flag)
            {
                target.AddComponent<Animator>();
                Animator temp = target.GetComponent<Animator>();
                temp.runtimeAnimatorController = cardAnim;

                if(bouncePos.x == 0)
                {
                    if(bouncePos.y > 0)
                    {
                        temp.Play("trampolineUp");
                    }
                    else
                    {
                        temp.Play("trampoline");
                    }
                }
                else
                {
                    if (bouncePos.x > 0)
                    {
                        temp.Play("trampolineRight");
                    }
                    else
                    {
                        temp.Play("trampolineLeft");
                    }
                }

                flag = true;
            }            
        }
        else if(timer > 2.0f)
        {
            timer -= Time.deltaTime;
        }
        else if(timer > 1.5f)
        {
            timer -= Time.deltaTime;

            if (flag)
            {
                Destroy(target.GetComponent<Animator>());
                flag = false;
            }

            target.transform.Rotate(rot);
        }
        else if (timer > 1.0f)
        {
            timer -= Time.deltaTime;

            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 1.0f);
            target.transform.Rotate(rot);
        }
        else if (target.transform.rotation.eulerAngles.z >= rot.z)
        {
            target.transform.Rotate(rot);
        }
        else
        {
            target.transform.rotation = Quaternion.identity;
            stop = true;
        }
    }

    public override void Terminate()
    {
        target.transform.parent = null;
        Destroy(gameObject);
    }
}

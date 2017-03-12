﻿using UnityEngine;
using System.Collections;

public class GooglyEyes : MonoBehaviour
{
    public Vector3 endPos;
    public GameObject target;
    public float timer = 3;
    private Vector3 rot = new Vector3(0, 0, 10);

    void Awake()
    {
        enabled = false;
    }

    public void Initialize(GameObject card, Vector3 destination)
    {
        target = card;
        endPos = destination * 2;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -2);
        enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (timer > 2)
        {
            timer -= Time.deltaTime;
        }
        else if (timer > 1)
        {
            timer -= Time.deltaTime;

            transform.Rotate(rot);
            target.transform.Rotate(rot);
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.5f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 0.5f);

            transform.Rotate(rot);
            target.transform.Rotate(rot);
        }
        else
        {
            Destroy(target);
            Destroy(gameObject);
        }
    }
}
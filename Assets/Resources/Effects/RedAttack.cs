﻿using UnityEngine;
using System.Collections;

public class RedAttack : MonoBehaviour
{
    public GameObject target;
    public float timer = 3;
    public Vector3 startPos;
    public Vector3 endPos;
    public float totalSpins = 1080;
    private float currentSpins = 0;
    private Vector3 rot = new Vector3(0, 0, 10);
    private Vector3 offset = new Vector3(0, 0, -1);

    // Use this for initialization
    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 def, Vector3 atk)
    {
        target = card;
        startPos = atk;
        endPos = def * 2;

        transform.position = startPos;
        target.transform.position = startPos;

        

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 2)
        {
            timer -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero + offset, 1.0f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, Vector3.zero, 1.0f);
        }
        else if (currentSpins < totalSpins)
        {
            transform.Rotate(rot);
            target.transform.Rotate(-rot);
            currentSpins += 10;
        }
        else if(timer > 0)
        {
            timer -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, endPos + offset, 2.0f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 2.0f);
        }
        else
        {
            Destroy(gameObject);
            Destroy(target);
        }
    }
}

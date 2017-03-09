﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Sinkhole : MonoBehaviour {

    private class data
    {
        public GameObject card;
        public float time;

        public data(GameObject c, float t)
        {
            card = c;
            time = t;
        }
    }
    
    private List<data> cards = new List<data>();
    public float fallTime = 5.0f;
    private Vector3 target;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        target = new Vector3(transform.position.x, transform.position.y, -2.5f);
        foreach (data item in cards)
        {
            if (item.time > 0)
            {
                item.time -= Time.deltaTime;
                item.card.transform.position = Vector3.MoveTowards(item.card.transform.position, target, 0.5f);
                item.card.transform.localScale = Vector3.MoveTowards(item.card.transform.localScale, Vector3.zero, 0.01f);
            }
            else
            {
                NetworkServer.UnSpawn(item.card);
                Destroy(item.card);
                cards.Remove(item);
            }
        }
	}

    public void EatCard(GameObject input)
    {
        input.transform.position = new Vector3(input.transform.position.x, input.transform.position.y, -2.5f);
        cards.Add(new data(input, fallTime));
    }
}

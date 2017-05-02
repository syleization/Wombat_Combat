using UnityEngine;
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
    private List<data> itemsToDestroy = new List<data>();
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
                itemsToDestroy.Clear();
                itemsToDestroy.Add(item);
            }
        }
        foreach(data item in itemsToDestroy)
        {
            Destroy(item.card);
            cards.Remove(item);
        }
	}

    public void EatCard(Vector3 cardPosition, Quaternion cardRotation)
    {
        GameObject card;
        if (Effects.TheActiveAttack != null)
        {
            card = Effects.TheActiveAttack.target;
            Effects.TheActiveAttack.Terminate();
            Effects.TheActiveAttack = null;
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
        }
        card.transform.rotation = cardRotation;
        card.transform.position = new Vector3(cardPosition.x, cardPosition.y, -2.5f);
        cards.Add(new data(card, fallTime));
    }
}

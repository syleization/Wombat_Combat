using UnityEngine;
using System.Collections;

public class Bite : MonoBehaviour
{
    public GameObject target;
    public float timer = 5.0f;
    private Vector3 dir;

    void Awake()
    {
        enabled = false;
    }

	// Use this for initialization
	public void Initialize(GameObject card)
    {
        target = card;
        dir = new Vector3(Random.Range(1.0f, -1.0f), Random.Range(1.0f, -1.0f), 0);
        dir = dir * 100;
        enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (timer > 4)
        {
            timer -= Time.deltaTime;
        }
        else if(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, dir, 1.0f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, dir, 1.0f);
        }
        else
        {
            Destroy(target);
            Destroy(gameObject);
        }
	}
}

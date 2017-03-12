using UnityEngine;
using System.Collections;

public class Cage : MonoBehaviour {

    public Vector3 endPos;
    public GameObject target;
    public float timer = 3;
    private Vector3 scaleTarget = new Vector3(0.3f, 0.3f, 0.3f);

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 owner)
    {
        target = card;
        endPos = owner * 2;
        transform.position = new Vector3(target.transform.position.x, transform.position.y, -2);
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 2)
        {
            timer -= Time.deltaTime;
            target.transform.localScale = Vector3.MoveTowards(target.transform.localScale, scaleTarget, 0.02f);
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.5f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 0.5f);
        }
        else
        {
            Destroy(target);
            Destroy(gameObject);
        }
    }
}

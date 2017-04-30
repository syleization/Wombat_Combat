using UnityEngine;
using System.Collections;

public class RedAttack : Attack
{
    public float timer = 3;
    public Vector3 atkPos;
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
        atkPos = atk * 2;
        endPos = def * 0.5f;

        transform.position = atkPos;
        target.transform.position = atkPos;

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;

        if (timer > 2)
        {
            timer -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, offset, 1.0f);
            target.transform.position = Vector3.MoveTowards(target.transform.position, offset, 1.0f);
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
            stop = true;
        }
    }

    public override void Terminate()
    {
        Destroy(gameObject);
    }
}

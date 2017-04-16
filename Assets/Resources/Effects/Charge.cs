using UnityEngine;
using System.Collections;

public class Charge : Attack
{
    public Vector3 atkPos;
    public GameObject dust;
    public GameObject mDust;
    private bool flag = false;
    public float timer = 1.5f;
    private float dustSpeed = 0.2f;

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 atk, Vector3 def)
    {
        target = card;
        atkPos = atk * 2;
        endPos = def * 0.5f;

        target.transform.position = atkPos;

        mDust = Instantiate(dust);
        mDust.transform.position = target.transform.position;

        if (atk.x == 0)
            dustSpeed = 0.15f;

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == false)
        if (target.transform.position != Vector3.zero)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, Vector3.zero, 0.25f);
            mDust.transform.position = Vector3.MoveTowards(mDust.transform.position, Vector3.zero, dustSpeed);
            return;
        }
        else
        {
            flag = true;
            mDust.GetComponent<ParticleSystem>().emissionRate = 50;
            return;
        }

        if (timer > 1)
        {
            timer -= Time.deltaTime;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;

            mDust.GetComponent<ParticleSystem>().emissionRate = 100;

            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 1.0f);
            mDust.transform.position = target.transform.position;

            if (target.transform.position == endPos)
                mDust.GetComponent<ParticleSystem>().Pause();
        }
        else
        {
            //Destroy(mDust);
            //Destroy(target);
            //Destroy(gameObject);
        }
    }

    public override void Terminate()
    {
        Destroy(mDust);
        Destroy(gameObject);
    }
}

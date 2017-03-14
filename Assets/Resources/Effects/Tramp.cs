using UnityEngine;
using System.Collections;

public class Tramp : MonoBehaviour {

    public GameObject target;
    public float timer = 3;
    public Vector3 endPos;
    public Vector3 bouncePos;
    public RuntimeAnimatorController cardAnim;
    private bool flag = false;
    private Vector3 rot = new Vector3(0, 0, 10);

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 bounce, Vector3 hit)
    {
        target = card;
        endPos = hit * 2;
        bouncePos = bounce;

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 2.5f)
        {
            timer -= Time.deltaTime;

            if (!flag)
            {
                target.AddComponent<Animator>();
                target.GetComponent<Animator>().runtimeAnimatorController = cardAnim;
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
        else
        {
            Destroy(target);
            Destroy(gameObject);
        }
    }
}

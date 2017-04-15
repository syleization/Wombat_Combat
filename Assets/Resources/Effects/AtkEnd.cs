using UnityEngine;
using System.Collections;

public class AtkEnd : MonoBehaviour
{
    public GameObject target;
    public Vector3 atkPos;

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 atk, Vector3 def)
    {
        target = card;
        atkPos = atk * 2;

        enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
	    if(target.transform.position != atkPos)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, atkPos, 0.25f);
        }
        else
        {
            Destroy(target);
            Destroy(this);
        }
    }
}

using UnityEngine;
using System.Collections;

public class AtkEnd : MonoBehaviour
{
    public GameObject target;
    public Vector3 endPos;

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(Attack input)
    {
        target = input.target;
        endPos = input.endPos * 4;

        enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
	    if(target.transform.position != endPos)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 0.5f);
        }
        else
        {
            Destroy(target);
            Destroy(gameObject);
        }
    }
}

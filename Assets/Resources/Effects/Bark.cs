using UnityEngine;
using System.Collections;

public class Bark : MonoBehaviour
{
    public GameObject target;
    public float timer = 3;
    public Vector3 endPos;
    private GameObject wave;

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 def, Vector3 atk)
    {
        target = card;
        endPos = atk;

        wave = Instantiate(Resources.Load("Effects/Sound")) as GameObject;
        wave.transform.position = def;
        wave.transform.LookAt(target.transform.position);

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

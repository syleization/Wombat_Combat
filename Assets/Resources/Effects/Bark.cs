using UnityEngine;
using System.Collections;

public class Bark : MonoBehaviour
{
    public GameObject target;
    public float timer = 3;
    public Vector3 endPos;
    public RuntimeAnimatorController cardAnim;
    private GameObject wave;
    private Vector3 scaleTarget = new Vector3(0.3f, 0.3f, 0.3f);

    void Awake()
    {
        enabled = false;
    }

    // Use this for initialization
    public void Initialize(GameObject card, Vector3 def, Vector3 atk)
    {
        target = card;
        endPos = atk * 2;

        wave = Instantiate(Resources.Load("Effects/Sound")) as GameObject;
        wave.transform.position = def;
        if (wave.transform.position.x != 0)
            wave.transform.Rotate(0, 0, 90);

        target.AddComponent<Animator>();
        target.GetComponent<Animator>().runtimeAnimatorController = cardAnim;

        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 2)
        {
            timer -= Time.deltaTime;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;

            wave.transform.localScale = Vector3.MoveTowards(wave.transform.localScale, Vector3.zero, 0.05f);
            
            target.transform.position = Vector3.MoveTowards(target.transform.position, endPos, 1.0f);
        }
        else
        {
            Destroy(wave);
            Destroy(target);
            Destroy(gameObject);
        }
    }
}

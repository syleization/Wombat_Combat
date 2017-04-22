using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Merge : MonoBehaviour
{
    public int stage = 0;
    public Card card1, card2;
    public GameObject white;
    public LightningBoltScript[] bolts;
    public RuntimeAnimatorController cardAnim;
    public float mergeSpeed = 0.01f;
    public float fadeSpeed = 0.1f;
    public float smokeSpeed = 0.1f;
    private float lerper = 0;
    private Turns owner;
    private GameObject glowParticles;

    private static Vector3 startPos = new Vector3(4, 0, 0);
    private static Vector3 startScale = new Vector3(0.25f, 0.25f, 0);
    private static Vector3 center = new Vector3(0, 0, -1);
    const float kStartDepth = -30.0f;
    const float kEndDepth = -17.0f;

    void Awake()
    {
        enabled = false;
    }

    public void Initialize(CardSubType card, CardSubType final, Turns player)
    {
        owner = player;

        card1 = Field.Instance.GetCard(0);
        card2 = Field.Instance.GetCard(1);

        if (card1 == null)
        {
            card1 = Instantiate(GlobalSettings.Instance.GetCardOfSubType(card));
            card1.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        }

        if (card2 == null)
        {
            card2 = Instantiate(GlobalSettings.Instance.GetCardOfSubType(card));
            card2.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, 0);
        }

        GameObject temp;
        temp = Instantiate(white);
        temp.transform.parent = card1.transform;
        temp.transform.localPosition = new Vector3(0, 0, 0.5f);
        temp = Instantiate(white);
        temp.transform.parent = card2.transform;
        temp.transform.localPosition = new Vector3(0, 0, 0.5f);

        enabled = true;
    }

    public void Update()
    {
        switch (stage)
        {
            case 0:
                card1.gameObject.transform.position = Vector3.MoveTowards(card1.gameObject.transform.position, -startPos, 0.1f);
                card1.gameObject.transform.localScale = Vector3.MoveTowards(card1.gameObject.transform.localScale, startScale, 0.1f);
                card2.gameObject.transform.position = Vector3.MoveTowards(card2.gameObject.transform.position, startPos, 0.1f);
                card2.gameObject.transform.localScale = Vector3.MoveTowards(card2.gameObject.transform.localScale, startScale, 0.1f);

                if   ( card1.gameObject.transform.position == -startPos
                    && card2.gameObject.transform.position == startPos
                    && card1.gameObject.transform.localScale == startScale
                    && card2.gameObject.transform.localScale == startScale)
                {
                    stage++;
                }
                break;
            case 1:
                for (int i = 0; i < bolts.Length; i++)
                {
                    bolts[i] = Instantiate(bolts[i]);
                    bolts[i].StartObject = card1.gameObject;
                    bolts[i].EndObject = card2.gameObject;
                }

                Animator temp;

                card1.gameObject.AddComponent<Animator>();
                temp = card1.gameObject.GetComponent<Animator>();
                temp.runtimeAnimatorController = cardAnim;

                card2.gameObject.AddComponent<Animator>();
                temp = card2.gameObject.GetComponent<Animator>();
                temp.runtimeAnimatorController = cardAnim;

                stage++;
                break;
            case 2:
                card1.gameObject.transform.position = Vector3.MoveTowards(card1.gameObject.transform.position, center, mergeSpeed);
                card2.gameObject.transform.position = Vector3.MoveTowards(card2.gameObject.transform.position, center, mergeSpeed);

                float alpha =  Mathf.Lerp(1, 0, lerper);
                card1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
                card2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
                lerper += fadeSpeed;

                if (card1.gameObject.transform.position == center)
                    stage++;
                break;
            case 3:
                card1.GetComponent<Animator>().SetBool("End", true);
                card2.GetComponent<Animator>().SetBool("End", true);
                glowParticles = Hand.GetMaterialOfType(card1.Type);
                glowParticles.transform.position = new Vector3(0.0f, 0.0f, kStartDepth);
                lerper = 0;
                stage++;
                break;
            case 4:
                glowParticles.transform.position = new Vector3(0.0f, 0.0f, Mathf.Lerp(kStartDepth, kEndDepth, lerper));
                lerper += smokeSpeed;
                break;
            default:
                break;
        }
    }
}

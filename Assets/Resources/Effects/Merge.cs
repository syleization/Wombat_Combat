using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Merge : MonoBehaviour
{
    public Card card1, card2;
    public LightningBoltScript[] bolts;

    void Awake()
    {
        enabled = false;
    }

    public void Initialize()
    {
        card1 = Field.Instance.GetCard(0);
        card2 = Field.Instance.GetCard(1);

        if (card1 == null)
        {
            //card1 = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card1.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        }

        if (card2 == null)
        {
            //card2 = Instantiate(GlobalSettings.Instance.Attack_DonkeyKick).gameObject;
            card2.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        }

        foreach (LightningBoltScript item in bolts)
        {
            item.StartObject = card1.gameObject;
            item.EndObject = card2.gameObject;
        }

        enabled = true;
    }
}

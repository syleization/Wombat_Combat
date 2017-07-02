using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class SpawnObjectEvent : TextEvent
    {
        [SerializeField]
        GameObject ObjectToSpawn;
        [SerializeField]
        Vector3 PlaceToSpawn;
        GameObject ObjectClone;

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            ObjectClone = Instantiate(ObjectToSpawn, PlaceToSpawn, Quaternion.identity) as GameObject;
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            Destroy(ObjectClone.gameObject);
        }
    }

}

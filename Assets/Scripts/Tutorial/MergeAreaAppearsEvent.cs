using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class MergeAreaAppearsEvent : TextEvent
    {
        [SerializeField]
        GameObject mergeArea;
        GameObject mergeAreaClone;

        override public void Enter(Text textObject)
        {
            base.Enter(textObject);
            mergeAreaClone = Instantiate(mergeArea) as GameObject;
        }

        override public void Exit(Text textObject)
        {
            base.Exit(textObject);
            Destroy(mergeAreaClone.gameObject);
        }
    }

}

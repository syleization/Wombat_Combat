using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TextEvent : MonoBehaviour
    {
        [Tooltip("None of these need to be set if the event does not have text")]
        [SerializeField]
        protected string BoxText;
        [SerializeField]
        protected Color BoxColour;
        [SerializeField]
        protected uint FontSize;

        virtual public void Enter(Text textObject)
        {
            textObject.text = BoxText;
            if (BoxText.Length <= 0)
            {
                textObject.gameObject.SetActive(false);
            }
            else
            {
                textObject.color = BoxColour;
                textObject.fontSize = (int)FontSize;
            }
        }

        virtual public void Tick()
        { }

        virtual public void Exit(Text textObject)
        {
            textObject.gameObject.SetActive(true);
        }
    }
}

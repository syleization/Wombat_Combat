using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DropDownVertical : MonoBehaviour, IPointerClickHandler
{
    public RectTransform container;
    public bool toggle;
    public bool isOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isOpen)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }

    void Start()
    {
        container = transform.FindChild("Container").GetComponent<RectTransform>();
        isOpen = false;
    }

    void Update()
    {
        Vector3 scale = container.localScale;
        scale.y = Mathf.Lerp(scale.y, isOpen ? 1 : 0, Time.deltaTime * 12);
        container.localScale = scale;
    }
}
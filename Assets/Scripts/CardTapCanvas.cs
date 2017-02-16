using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardTapCanvas : MonoBehaviour
{
    [SerializeField]
    Image CardTapImage;

    public void Initialize()
    {
        gameObject.SetActive(false);
        CardTapImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateCanvasImage(Sprite image)
    {
        Debug.Log("Blah");
        Debug.Log(image);
        CardTapImage.sprite = image;
        if(image == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public bool IsImageNull()
    {
        return CardTapImage.sprite == null ? true : false;
    }
}

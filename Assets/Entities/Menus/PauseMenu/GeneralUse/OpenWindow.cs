using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenWindow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ObjectToEnable;
    // Start is called before the first frame update
    public Color HoverColor;
    public Transform TransparencyCard;
    Color DefaultColor;
    void Start()
    {
        DefaultColor = TransparencyCard.gameObject.GetComponent<Image>().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ObjectToEnable.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Color HoverColor;
    public Color ClickColor;
    float transparentValue = .45f;
    public Transform TransparencyCard;
    protected Color DefaultColor;

    // Start is called before the first frame update
    void Start()
    {
        DefaultColor = TransparencyCard.gameObject.GetComponent<Image>().color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = ClickColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = HoverColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }
}

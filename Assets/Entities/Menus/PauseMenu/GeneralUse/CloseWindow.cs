using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ObjectToDisable;
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
        ObjectToDisable.SetActive(false);
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

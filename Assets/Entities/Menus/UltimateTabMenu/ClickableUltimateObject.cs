using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUltimateObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Color HoverColor;
    public Color ClickColor;
    float transparentValue = .45f;
    public UltimateType abilityType;
    GameObject SelectCard;
    public Transform TransparencyCard;
    Color DefaultColor;
    void Start()
    {
        SelectCard = GameObject.Find("SelectedUltimate");
        DefaultColor = TransparencyCard.gameObject.GetComponent<Image>().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // standard select
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FilUltimateHandler.AssignUltimate(abilityType);
            SelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Not Imp but no exception pls");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Not Imp but no exception pls");
    }
}

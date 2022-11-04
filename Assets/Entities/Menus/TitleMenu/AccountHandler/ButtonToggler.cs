using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonToggler : ButtonBase, IPointerClickHandler
{
    public GameObject toDisable;
    public GameObject toEnable;
    public Color SelectedColor;
    public string otherObj;
    public void OnPointerClick(PointerEventData eventData)
    {
        // handle state changes before toDisable is disabled?
        toEnable.active = true;
        GetComponent<Image>().color = SelectedColor;
        toDisable.active = false;
        GameObject.Find(otherObj).GetComponent<Image>().color = Color.white;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(GetComponent<Image>().color != SelectedColor)
            TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (GetComponent<Image>().color != SelectedColor)
            TransparencyCard.gameObject.GetComponent<Image>().color = DefaultColor;
    }
}

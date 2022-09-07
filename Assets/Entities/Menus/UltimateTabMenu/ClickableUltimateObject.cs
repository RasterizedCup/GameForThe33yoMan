using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUltimateObject : ButtonBase, IPointerClickHandler
{
    public UltimateType abilityType;
    GameObject SelectCard;
    void Start()
    {
        SelectCard = GameObject.Find("SelectedUltimate");
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
}

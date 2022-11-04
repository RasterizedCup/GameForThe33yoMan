using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HowToPlayToggle : ButtonBase, IPointerClickHandler
{
    public GameObject HowToPlayCard;
    public void OnPointerClick(PointerEventData eventData)
    {
        HowToPlayCard.active = true;
    }
}

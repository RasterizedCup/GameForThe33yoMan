using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnableDisplayGroup : ButtonBase, IPointerClickHandler
{
    public GameObject activeDisplayGroup;
    public List<GameObject> toDisable;

    public void OnPointerClick(PointerEventData eventData)
    {
        activeDisplayGroup.active = true;
        for(var i=0; i<toDisable.Count; i++)
        {
            toDisable[i].active = false;
        }
    }

}

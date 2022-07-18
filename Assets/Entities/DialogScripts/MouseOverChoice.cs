using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " left");
    }
}

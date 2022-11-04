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
        // refresh existing
        var levelArray = new List<string> {
               "Level1TimeParent",
               "Level2TimeParent",
           };

           // erase existing children
           foreach (var level in levelArray)
           {
               GameObject parent = GameObject.Find(level);
               foreach (Transform child in parent.transform)
               {
                   Destroy(child.gameObject);
               }
           }
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

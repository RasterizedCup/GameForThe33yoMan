using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ClickableAbilityObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Color HoverColor;
    public Color ClickColor;
    float transparentValue = .45f;
    public AbilityType abilityType;
    GameObject PrimarySelectCard, SecondarySelectCard;
    public Transform TransparencyCard;
    Color DefaultColor;
    void Start()
    {
        PrimarySelectCard = GameObject.Find("PrimarySelectedAbility");
        SecondarySelectCard = GameObject.Find("SecondarySelectedAbility");
        DefaultColor = TransparencyCard.gameObject.GetComponent<Image>().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("assigning ability");
        // standard select
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FilAbilityHandler.AssignPrimaryAbility(abilityType);
            PrimarySelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            FilAbilityHandler.AssignSecondaryAbility(abilityType);
            SecondarySelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
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

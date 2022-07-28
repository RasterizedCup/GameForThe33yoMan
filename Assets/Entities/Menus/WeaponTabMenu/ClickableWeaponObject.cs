using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableWeaponObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Color HoverColor;
    public Color ClickColor;
    float transparentValue = .45f;
    public AttackType weapon;
    GameObject PrimarySelectCard, SecondarySelectCard;
    public Transform TransparencyCard;
    Color DefaultColor;
    void Start()
    {
        PrimarySelectCard = GameObject.Find("PrimarySelectedWeapon");
        SecondarySelectCard = GameObject.Find("SecondarySelectedWeapon");
        DefaultColor = TransparencyCard.gameObject.GetComponent<Image>().color;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // standard select
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FilAttackHandler.AssignPrimaryAttack(weapon);
            PrimarySelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            FilAttackHandler.AssignSecondaryAttack(weapon);
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

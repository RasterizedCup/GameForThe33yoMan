using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableWeaponObject : ButtonBase, IPointerClickHandler
{
    public AttackType weapon;
    GameObject PrimarySelectCard, SecondarySelectCard;
    void Start()
    {
        PrimarySelectCard = GameObject.Find("PrimarySelectedWeapon");
        SecondarySelectCard = GameObject.Find("SecondarySelectedWeapon");
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ClickableAbilityObject : ButtonBase, IPointerClickHandler 
{ 
    public GameObject FilAbilities;
    public AbilityType abilityType;
    GameObject PrimarySelectCard, SecondarySelectCard;
    void Start()
    {
        PrimarySelectCard = GameObject.Find("PrimarySelectedAbility");
        SecondarySelectCard = GameObject.Find("SecondarySelectedAbility");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("assigning ability");
        // standard select
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FilAbilities.GetComponent<FilAbilityHandler>().EnsureSnailDisabled();
            FilAbilityHandler.AssignPrimaryAbility(abilityType);
            PrimarySelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            FilAbilityHandler.AssignSecondaryAbility(abilityType);
            SecondarySelectCard.GetComponent<RectTransform>().localPosition = transform.localPosition;
        }
    }
}

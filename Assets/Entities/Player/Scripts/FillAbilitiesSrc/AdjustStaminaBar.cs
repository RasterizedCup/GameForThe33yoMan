using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdjustStaminaBar : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform currStamina;
    public RectTransform maxStamina;
    public RectTransform StaminaDisplayName;

    GameObject PrimaryAbilityDispName;
    GameObject SecondaryAbilityDispName;

    public string StaminaBarTargetName;
    public static float StaminaValue;
    float maxWidth;
    float baseXOffset; // needed to combat any default offset for canvas
    public float fadeRate;
    void Start()
    {
        PrimaryAbilityDispName = GameObject.Find("StaminaPrimaryAbility");
        SecondaryAbilityDispName = GameObject.Find("StaminaSecondaryAbility");
        maxWidth = currStamina.rect.width;
        baseXOffset = currStamina.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        SetStaminaUI();
        setCurrentAbilityDisplay();
        SetStaminaBarFade();
    }

    void SetStaminaUI()
    {
        float percentDecreasedFromBase = GameObject.Find(StaminaBarTargetName).GetComponent<FilAbilities>().getPercentageCurrentStamina();
        float currStaminaWidth = maxWidth;
        float posXoffset;
        currStaminaWidth *= percentDecreasedFromBase;
        posXoffset = (-1 * (maxWidth - currStaminaWidth)) * .5f + baseXOffset;

        currStamina.sizeDelta = new Vector2(currStaminaWidth, currStamina.sizeDelta.y);
        currStamina.localPosition = new Vector3(posXoffset, currStamina.localPosition.y, 0);
    }

    void setCurrentAbilityDisplay()
    {
        PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().text = $"{FilAbilityHandler.GetCurrentPrimaryAbilityName()}";
        SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().text = $"{FilAbilityHandler.GetCurrentSecondaryAbilityName()}";

        if (FilAbilityHandler.ActiveAbility == FilAbilityHandler.PrimaryAbility)
        {
            PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().fontSize = 14;
            SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().fontSize = 11;

            PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color = new Color(
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.r,
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.g,
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.b,
                1);
            SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color = new Color(
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.r,
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.g,
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.b,
                .4f);
        }
        else
        {
            SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().fontSize = 14;
            PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().fontSize = 11;

            PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color = new Color(
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.r,
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.g,
                PrimaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.b,
                .4f);
            SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color = new Color(
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.r,
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.g,
                SecondaryAbilityDispName.GetComponent<TextMeshProUGUI>().color.b,
                1);
        }
    }

    void SetStaminaBarFade()
    {
        // if we have max stamina and the stamina bar is not transparent
        if(GameObject.Find(StaminaBarTargetName).GetComponent<FilAbilities>().getPercentageCurrentStamina() >= .99f &&
            currStamina.gameObject.GetComponent<Image>().color.a > 0)
        {
            // normalize transparency reduction value
            var normalA = (currStamina.gameObject.GetComponent<Image>().color.a - (fadeRate * Time.deltaTime));
            currStamina.gameObject.GetComponent<Image>().color = new Color(
                currStamina.gameObject.GetComponent<Image>().color.r,
                currStamina.gameObject.GetComponent<Image>().color.g,
                currStamina.gameObject.GetComponent<Image>().color.b,
                Mathf.Max(normalA, 0));

            maxStamina.gameObject.GetComponent<Image>().color = new Color(
                maxStamina.gameObject.GetComponent<Image>().color.r,
                maxStamina.gameObject.GetComponent<Image>().color.g,
                maxStamina.gameObject.GetComponent<Image>().color.b,
                Mathf.Max(maxStamina.gameObject.GetComponent<Image>().color.a - (fadeRate * Time.deltaTime), 0));

            StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.r,
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.g,
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.b,
                Mathf.Max(StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.a - (fadeRate * Time.deltaTime), 0));
        }
        else if(currStamina.gameObject.GetComponent<Image>().color.a != 1 &&
            GameObject.Find(StaminaBarTargetName).GetComponent<FilAbilities>().getPercentageCurrentStamina() < .99f)
        {
            currStamina.gameObject.GetComponent<Image>().color = new Color(
                currStamina.gameObject.GetComponent<Image>().color.r,
                currStamina.gameObject.GetComponent<Image>().color.g,
                currStamina.gameObject.GetComponent<Image>().color.b,
                1);

            maxStamina.gameObject.GetComponent<Image>().color = new Color(
                maxStamina.gameObject.GetComponent<Image>().color.r,
                maxStamina.gameObject.GetComponent<Image>().color.g,
                maxStamina.gameObject.GetComponent<Image>().color.b,
                1);

            StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color = new Color(
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.r,
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.g,
                StaminaDisplayName.gameObject.GetComponent<TextMeshProUGUI>().color.b,
                1);
        }
    }
}

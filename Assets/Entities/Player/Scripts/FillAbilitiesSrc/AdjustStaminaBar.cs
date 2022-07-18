using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustStaminaBar : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ActiveStaminaAbilityText;
    public RectTransform currStamina;
    public RectTransform maxStamina;
    public RectTransform StaminaDisplayName;
    public string StaminaBarTargetName;
    public static float StaminaValue;
    float maxWidth;
    float baseXOffset; // needed to combat any default offset for canvas
    public float fadeRate;
    void Start()
    {
        ActiveStaminaAbilityText = GameObject.Find("StaminaMovementAbilityName");
        maxWidth = currStamina.rect.width;
        baseXOffset = currStamina.localPosition.x;
        Debug.Log(StaminaBarTargetName);
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
        ActiveStaminaAbilityText.GetComponent<Text>().text = $"Active Ability: {FilAbilityHandler.GetCurrentPrimaryAbilityName()}";
    }

    void SetStaminaBarFade()
    {
        // if we have max stamina and the stamina bar is not transparent
        if(GameObject.Find(StaminaBarTargetName).GetComponent<FilAbilities>().getPercentageCurrentStamina() >= .99f &&
            currStamina.gameObject.GetComponent<Image>().color.a > 0)
        {
            Debug.Log("FADING STAMINA");
            currStamina.gameObject.GetComponent<Image>().color = new Color(
                currStamina.gameObject.GetComponent<Image>().color.r,
                currStamina.gameObject.GetComponent<Image>().color.g,
                currStamina.gameObject.GetComponent<Image>().color.b,
                Mathf.Max(currStamina.gameObject.GetComponent<Image>().color.a - (fadeRate * Time.deltaTime), 0));
            Debug.Log($"fade value: {Mathf.Max(currStamina.gameObject.GetComponent<Image>().color.a - (fadeRate * Time.deltaTime), 0)}");

            maxStamina.gameObject.GetComponent<Image>().color = new Color(
                maxStamina.gameObject.GetComponent<Image>().color.r,
                maxStamina.gameObject.GetComponent<Image>().color.g,
                maxStamina.gameObject.GetComponent<Image>().color.b,
                Mathf.Max(maxStamina.gameObject.GetComponent<Image>().color.a - (fadeRate * Time.deltaTime), 0));

            StaminaDisplayName.gameObject.GetComponent<Text>().color = new Color(
                StaminaDisplayName.gameObject.GetComponent<Text>().color.r,
                StaminaDisplayName.gameObject.GetComponent<Text>().color.g,
                StaminaDisplayName.gameObject.GetComponent<Text>().color.b,
                Mathf.Max(StaminaDisplayName.gameObject.GetComponent<Text>().color.a - (fadeRate * Time.deltaTime), 0));
        }
        else if(currStamina.gameObject.GetComponent<Image>().color.a != 255 &&
            GameObject.Find(StaminaBarTargetName).GetComponent<FilAbilities>().getPercentageCurrentStamina() < .99f)
        {

            currStamina.gameObject.GetComponent<Image>().color = new Color(
                currStamina.gameObject.GetComponent<Image>().color.r,
                currStamina.gameObject.GetComponent<Image>().color.g,
                currStamina.gameObject.GetComponent<Image>().color.b,
                255);

            maxStamina.gameObject.GetComponent<Image>().color = new Color(
                maxStamina.gameObject.GetComponent<Image>().color.r,
                maxStamina.gameObject.GetComponent<Image>().color.g,
                maxStamina.gameObject.GetComponent<Image>().color.b,
                255);

            StaminaDisplayName.gameObject.GetComponent<Text>().color = new Color(
                StaminaDisplayName.gameObject.GetComponent<Text>().color.r,
                StaminaDisplayName.gameObject.GetComponent<Text>().color.g,
                StaminaDisplayName.gameObject.GetComponent<Text>().color.b,
                255);
        }
    }
}

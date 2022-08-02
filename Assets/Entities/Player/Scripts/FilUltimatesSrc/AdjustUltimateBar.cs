using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdjustUltimateBar : MonoBehaviour
{

    public float expansionRate;
    public float longBarExpansionRate;
    float diagMaxLength;
    float leftRectMaxHeight;
    float rightRectMaxHeight;
    float topRectMaxLength;
    float bottomRectMaxlength;

    public RectTransform diag;
    public RectTransform leftRect;
    public RectTransform rightRect;
    public RectTransform topRect;
    public RectTransform bottomtRect;
    public GameObject UltText;
    public float UltReadyUILingerTime;

    public string UltimateBarTargetName;
    float maxWidth;
    public RectTransform currentUltCharge;
    float baseXOffset;
    bool shouldDisplayReadyText;

    float displayKillCount;
    public static bool killDisplay;
    float displayStartTime;
    TextMeshProUGUI UltReadyTextDisplay;
    // Start is called before the first frame update
    void Start()
    {
        killDisplay = false;
        displayKillCount = 0;
        maxWidth = currentUltCharge.rect.width;
        baseXOffset = currentUltCharge.localPosition.x;

        // get ult name gameObj reference
        UltReadyTextDisplay = GameObject.Find("UltimateReadyTextDisplay").GetComponent<TextMeshProUGUI>();

        // get ult rectangle angles
        diagMaxLength = diag.sizeDelta.x;
        leftRectMaxHeight = leftRect.sizeDelta.y;
        rightRectMaxHeight = rightRect.sizeDelta.y;
        topRectMaxLength = topRect.sizeDelta.x;
        bottomRectMaxlength = bottomtRect.sizeDelta.x;

        diag.sizeDelta = new Vector2(0, diag.sizeDelta.y);
        leftRect.sizeDelta = new Vector2(leftRect.sizeDelta.x, 0);
        rightRect.sizeDelta = new Vector2(rightRect.sizeDelta.x, 0);
        topRect.sizeDelta = new Vector2(0, topRect.sizeDelta.y);
        bottomtRect.sizeDelta = new Vector2(0, bottomtRect.sizeDelta.y);

        UltText.GetComponent<TextMeshProUGUI>().color = new Color(UltText.GetComponent<TextMeshProUGUI>().color.r, UltText.GetComponent<TextMeshProUGUI>().color.g, UltText.GetComponent<TextMeshProUGUI>().color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        interpolateToCompletedUltBar(SetUltimateUI());
    }

    bool SetUltimateUI()
    {
        float percentDecreasedFromBase = GameObject.Find(UltimateBarTargetName).GetComponent<FilUltimates>().getCurrentUltCharge();
        float currStaminaWidth = maxWidth;
        float posXoffset;
        currStaminaWidth *= percentDecreasedFromBase;
        posXoffset = (-1 * (maxWidth - currStaminaWidth)) * .5f + baseXOffset;

        currentUltCharge.sizeDelta = new Vector2(currStaminaWidth, currentUltCharge.sizeDelta.y);
        currentUltCharge.localPosition = new Vector3(posXoffset, currentUltCharge.localPosition.y, 0);

        // update display name if ult has changed
        if (!UltReadyTextDisplay.text.Contains(FilUltimateHandler.GetStringFromActiveUltimateTime()))
        {
            UltReadyTextDisplay.text = $"{FilUltimateHandler.GetStringFromActiveUltimateTime()} Ready";
        }

        return percentDecreasedFromBase > .99f;
    }


    void interpolateToCompletedUltBar(bool ultIsCharged)
    {
        if(Time.time > displayStartTime + UltReadyUILingerTime && shouldDisplayReadyText)
        {
            killDisplay = true;
        }

        if (ultIsCharged && !killDisplay)
        {
            if (!shouldDisplayReadyText)
            {
                displayStartTime = Time.time;
                UltText.GetComponent<TextMeshProUGUI>().color = new Color(UltText.GetComponent<TextMeshProUGUI>().color.r, UltText.GetComponent<TextMeshProUGUI>().color.g, UltText.GetComponent<TextMeshProUGUI>().color.b, 1);
                shouldDisplayReadyText = true;
            }
            if (diag.sizeDelta.x < diagMaxLength)
            {
                diag.sizeDelta = new Vector2(Mathf.Min(diagMaxLength, diag.sizeDelta.x + (longBarExpansionRate * Time.deltaTime)), diag.sizeDelta.y);
            }
            if (leftRect.sizeDelta.y < leftRectMaxHeight)
            {
                leftRect.sizeDelta = new Vector2(leftRect.sizeDelta.x, Mathf.Min(leftRectMaxHeight, leftRect.sizeDelta.y + (expansionRate * Time.deltaTime)));
            }
            if (rightRect.sizeDelta.y < rightRectMaxHeight)
            {
                rightRect.sizeDelta = new Vector2(rightRect.sizeDelta.x, Mathf.Min(rightRectMaxHeight, rightRect.sizeDelta.y + (expansionRate * Time.deltaTime)));
            }
            if (topRect.sizeDelta.x < topRectMaxLength)
            {
                topRect.sizeDelta = new Vector2(Mathf.Min(topRectMaxLength, topRect.sizeDelta.x + (longBarExpansionRate * Time.deltaTime)), topRect.sizeDelta.y);
            }
            if (bottomtRect.sizeDelta.x < bottomRectMaxlength)
            {
                bottomtRect.sizeDelta = new Vector2(Mathf.Min(bottomRectMaxlength, bottomtRect.sizeDelta.x + (longBarExpansionRate * Time.deltaTime)), bottomtRect.sizeDelta.y);
            }
        }
        else if (ultIsCharged && killDisplay)
        {
            if (shouldDisplayReadyText)
            {
                UltText.GetComponent<TextMeshProUGUI>().color = new Color(UltText.GetComponent<TextMeshProUGUI>().color.r, UltText.GetComponent<TextMeshProUGUI>().color.g, UltText.GetComponent<TextMeshProUGUI>().color.b, 0);
                shouldDisplayReadyText = false;
            }
            if (diag.sizeDelta.x > 0)
            {
                diag.sizeDelta = new Vector2(Mathf.Max(0, diag.sizeDelta.x - (longBarExpansionRate * Time.deltaTime)), diag.sizeDelta.y);
            }
            if (leftRect.sizeDelta.y > 0)
            {
                leftRect.sizeDelta = new Vector2(leftRect.sizeDelta.x, Mathf.Max(0, leftRect.sizeDelta.y - (expansionRate * Time.deltaTime)));
            }
            if (rightRect.sizeDelta.y > 0)
            {
                rightRect.sizeDelta = new Vector2(rightRect.sizeDelta.x, Mathf.Max(0, rightRect.sizeDelta.y - (expansionRate * Time.deltaTime)));
            }
            if (topRect.sizeDelta.x > 0)
            {
                topRect.sizeDelta = new Vector2(Mathf.Max(0, topRect.sizeDelta.x - (longBarExpansionRate * Time.deltaTime)), topRect.sizeDelta.y);
            }
            if (bottomtRect.sizeDelta.x > 0)
            {
                bottomtRect.sizeDelta = new Vector2(Mathf.Max(0, bottomtRect.sizeDelta.x - (longBarExpansionRate * Time.deltaTime)), bottomtRect.sizeDelta.y);
            }
        }
        else if(shouldDisplayReadyText || !ultIsCharged) // shortcut conditional to slightly help with efficiency, also base case to remove display
        {
            killDisplay = false;
            UltText.GetComponent<TextMeshProUGUI>().color = new Color(UltText.GetComponent<TextMeshProUGUI>().color.r, UltText.GetComponent<TextMeshProUGUI>().color.g, UltText.GetComponent<TextMeshProUGUI>().color.b, 0);
            shouldDisplayReadyText = false;
            diag.sizeDelta = new Vector2(0, diag.sizeDelta.y);
            leftRect.sizeDelta = new Vector2(leftRect.sizeDelta.x, 0);
            rightRect.sizeDelta = new Vector2(rightRect.sizeDelta.x, 0);
            topRect.sizeDelta = new Vector2(0, topRect.sizeDelta.y);
            bottomtRect.sizeDelta = new Vector2(0, bottomtRect.sizeDelta.y);
        }
    }
}

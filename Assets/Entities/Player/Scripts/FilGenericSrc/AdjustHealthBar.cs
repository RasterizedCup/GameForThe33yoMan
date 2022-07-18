using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform currhealth;
    public string HealthbarTargetName;
    float maxWidth;
    float baseXOffset; // needed to combat any default offset for canvas
    void Start()
    {
        maxWidth = currhealth.rect.width;
        // HealthbarTargetName = transform.parent.name;
        baseXOffset = currhealth.localPosition.x;
        Debug.Log(HealthbarTargetName);
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthUI();
    }

    void SetHealthUI()
    {
        float percentDecreasedFromBase = GameObject.Find(HealthbarTargetName).GetComponent<HealthLogicBase>().getPercentageCurrentHealth();
        float currHealthWidth = maxWidth;
        float posXoffset;
        currHealthWidth *= percentDecreasedFromBase;
        posXoffset = (-1*(maxWidth - currHealthWidth)) * .5f + baseXOffset;

        currhealth.sizeDelta = new Vector2(currHealthWidth, currhealth.sizeDelta.y);
        currhealth.localPosition = new Vector3(posXoffset, currhealth.localPosition.y, 0);


    }
}
